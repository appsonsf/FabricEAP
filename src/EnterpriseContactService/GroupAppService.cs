using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseContact.Entities;
using EnterpriseContact.MsgConstracts;
using InstantMessage;
using MassTransit;
using AppsOnSF.Common.BaseServices;
using Microsoft.EntityFrameworkCore;
using Nest;
using Notification;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    //TODO 以后要把对IConversationCtrlAppService的rpc调用改为EventBus调用

    public class GroupAppService : BaseAppService, IGroupAppService
    {
        private const string SKVContainer_ScanJoinCode = "EnterpriseContact_ScanJoinCode";
        private readonly ILogger _logger;
        private readonly ISimpleKeyValueService _simpleKeyValueService;
        private readonly IBusControl _busMs;

        public GroupAppService(StatelessServiceContext serviceContext, DbContextOptions<ServiceDbContext> dbOptions,
            IMapper mapper,
            ISimpleKeyValueService simpleKeyValueService,
            IBusControl busMs)
            : base(serviceContext, dbOptions, mapper)
        {
            _logger = Log.ForContext<GroupAppService>();
            _simpleKeyValueService = simpleKeyValueService;
            _busMs = busMs;
        }

        public async Task<bool> CheckSameWhiteListGroupAsync(Guid currentEmployeeId, Guid targetEmployeeId)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var queryTargetGroup = from g in db.Groups
                                       from m in g.Members
                                       where g.Type == GroupType.WhiteListChat && m.EmployeeId == targetEmployeeId
                                       select g;
                if (await queryTargetGroup.CountAsync() == 0) return true;

                var query = from m in db.GroupMembers
                            where m.EmployeeId == currentEmployeeId
                            join tg in queryTargetGroup
                                on m.GroupId equals tg.Id
                            select m;

                var count = await query.CountAsync();
                return count > 0;
            }
        }

        public async Task<Guid> CreateCustomAsync(GroupInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new ArgumentNullException("must not be null", nameof(input.Name));

            if (input.CurrentUserId == Guid.Empty)
                throw new ArgumentNullException("must not be empty", nameof(input.CurrentUserId));

            if (!input.CurrentEmployeeId.HasValue)
                throw new ArgumentNullException("must not be null", nameof(input.CurrentEmployeeId));

            using (var db = new ServiceDbContext(_dbOptions))
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = _mapper.Map<Group>(input);
                        entity.Type = GroupType.CustomChat;
                        db.Groups.Add(entity);

                        await db.SaveChangesAsync();

                        await _busMs.Publish(new GroupAdded
                        {
                            Id = entity.Id
                        });

                        tx.Commit();
                        return entity.Id;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _logger.Error(ex, "CreateCustomAsync Transaction rollback");
                        throw;
                    }
                }
            }
        }

        public async Task<List<Guid>> GetUserIdsByIdAsync(Guid id)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from member in db.GroupMembers
                            where member.GroupId == id
                            join emp in db.Employees
                               on member.EmployeeId equals emp.Id
                            where emp.UserId.HasValue
                            select emp.UserId.Value;
                return await query.ToListAsync();
            }
        }

        public async Task<List<GroupListOutput>> GetListByEmployeeIdAsync(Guid employeeId)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from g in db.Groups
                            where g.Type != GroupType.WhiteListChat
                            from m in g.Members
                            where m.EmployeeId == employeeId
                            select g;

                return await query.ProjectTo<GroupListOutput>(_mapper.ConfigurationProvider)
                     .ToListAsync();
            }
        }

        public async Task<RemotingResult> UpdateAsync(GroupInput input)
        {
            if (!input.Id.HasValue)
                throw new ArgumentNullException("must not be null", nameof(input.Id));

            if (input.CurrentUserId == Guid.Empty)
                throw new ArgumentNullException("must not be empty", nameof(input.CurrentUserId));

            if (input.RemovingMemberIds?.Count > 0)
            {
                //如果需要删除成员，那么就需要传递当前员工Id
                if (!input.CurrentEmployeeId.HasValue)
                    throw new ArgumentNullException("must not be null", nameof(input.CurrentEmployeeId));

                if (input.RemovingMemberIds.Contains(input.CurrentEmployeeId.Value))
                    return RemotingResult.Fail(FailedCodes.Group_CannotRemoveOwner);
            }

            using (var db = new ServiceDbContext(_dbOptions))
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = await db.Groups.FirstOrDefaultAsync(o => o.Id == input.Id);

                        if (entity == null)
                            return RemotingResult.Fail();
                        if (entity.CreatedBy != input.CurrentUserId)
                            return RemotingResult.Fail(FailedCodes.Group_NotCreatedBy);

                        var nameUpdated = false;
                        if (!string.IsNullOrWhiteSpace(input.Name) && entity.Name != input.Name)
                        {
                            entity.Name = input.Name;
                            nameUpdated = true;
                        }

                        if (input.Remark != null)
                            entity.Remark = input.Remark;

                        var membersUpdated = false;
                        if (input.AddingMemberIds?.Count > 0
                            || input.RemovingMemberIds?.Count > 0)
                        {
                            //需要编辑成员，就显式加载成员集合
                            await db.Entry(entity).Collection(o => o.Members).LoadAsync();
                            membersUpdated = true;
                        }

                        if (input.AddingMemberIds != null)
                        {
                            foreach (var employeeId in input.AddingMemberIds)
                            {
                                if (!entity.Members.Exists(o => o.EmployeeId == employeeId))
                                    entity.Members.Add(new GroupMember
                                    {
                                        EmployeeId = employeeId,
                                        GroupId = input.Id.Value,
                                        Joined = DateTimeOffset.UtcNow,
                                    });
                            }
                        }

                        if (input.RemovingMemberIds != null)
                        {
                            var willRemoving = new List<GroupMember>();
                            foreach (var employeeId in input.RemovingMemberIds)
                            {
                                var m = entity.Members.Find(o => o.EmployeeId == employeeId);
                                willRemoving.Add(m);
                            }
                            willRemoving.ForEach(o => entity.Members.Remove(o));
                        }

                        await db.SaveChangesAsync();

                        if (membersUpdated)
                            await _busMs.Publish(new GroupMembersUpdated { Id = entity.Id });

                        await SendEventNotifyForUpdateAsync(nameUpdated, input);

                        tx.Commit();
                        return RemotingResult.Success();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _logger.Error(ex, "UpdateAsync Transaction rollback");
                        return RemotingResult.Fail();
                    }
                }

            }
        }

        private async Task SendEventNotifyForUpdateAsync(bool nameUpdated, GroupInput input)
        {
            var groupNameUpdatedNotify = nameUpdated
                ? new EventNotifyDto
                {
                    Target = NotifyTargetType.Conversation,
                    TargetId = input.Id.ToString(),
                    Created = DateTimeOffset.UtcNow,
                    Text = input.Name,
                    TargetCategory = (int)GroupEventNotifyType.GroupNameUpdated
                }
                : null;

            var participantAddedNotify = (input.AddingMemberNames?.Count > 0)
                ? new EventNotifyDto
                {
                    Target = NotifyTargetType.Conversation,
                    TargetId = input.Id.ToString(),
                    Created = DateTimeOffset.UtcNow,
                    Text = string.Join(",", input.AddingMemberNames),
                    TargetCategory = (int)GroupEventNotifyType.ParticipantAdded
                }
                : null;

            var participantRemovedNotify = (input.RemovingMemberNames?.Count > 0)
                ? new EventNotifyDto
                {
                    Target = NotifyTargetType.Conversation,
                    TargetId = input.Id.ToString(),
                    Created = DateTimeOffset.UtcNow,
                    Text = string.Join(",", input.RemovingMemberNames),
                    TargetCategory = (int)GroupEventNotifyType.ParticipantRemoved
                }
                : null;

            var notifies = new List<EventNotifyDto>();

            if (groupNameUpdatedNotify != null)
                notifies.Add(groupNameUpdatedNotify);
            if (participantAddedNotify != null)
                notifies.Add(participantAddedNotify);
            if (participantRemovedNotify != null)
                notifies.Add(participantAddedNotify);

            if (notifies.Count > 0)
                await _busMs.Publish(new GroupNotified { Id = input.Id.Value, Notifies = notifies });
        }

        public async Task<RemotingResult> DeleteAsync(GroupInput input)
        {
            if (!input.Id.HasValue)
                throw new ArgumentNullException("must not be null", nameof(input.Id));

            if (input.CurrentUserId == Guid.Empty)
                throw new ArgumentNullException("must not be empty", nameof(input.CurrentUserId));

            using (var db = new ServiceDbContext(_dbOptions))
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = await db.Groups.FirstOrDefaultAsync(o => o.Id == input.Id);

                        if (entity == null)
                            return RemotingResult.Fail();
                        if (entity.CreatedBy != input.CurrentUserId)
                            return RemotingResult.Fail(FailedCodes.Group_NotCreatedBy);

                        db.Groups.Remove(entity);

                        await db.SaveChangesAsync();

                        await _busMs.Publish(new GroupDeleted { Id = entity.Id });

                        await _busMs.Publish(new GroupNotified
                        {
                            Id = entity.Id,
                            Notifies = new List<EventNotifyDto>
                            {
                                new EventNotifyDto
                                {
                                    Target = NotifyTargetType.Conversation,
                                    TargetId = entity.Id.ToString(),
                                    Created = DateTimeOffset.UtcNow,
                                    Text = entity.Name,
                                    TargetCategory = (int)GroupEventNotifyType.GroupDismissed
                                }
                            }
                        });

                        tx.Commit();
                        return RemotingResult.Success();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _logger.Error(ex, "DeleteAsync Transaction rollback");
                        return RemotingResult.Fail();
                    }
                }
            }
        }

        public async Task<GroupOutput> GetByIdAsync(Guid id)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var dto = await db.Groups.ProjectTo<GroupOutput>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (dto == null) return null;
                var query = from gm in db.GroupMembers
                            where gm.GroupId == id
                            join e in db.Employees
                              on gm.EmployeeId equals e.Id
                            select new GroupMemberOutput
                            {
                                EmployeeId = e.Id,
                                EmployeeName = e.Name,
                                Gender = e.Gender,
                                IsOwner = gm.IsOwner,
                                Joined = gm.Joined,
                                PrimaryDepartmentId = e.PrimaryDepartmentId
                            };

                dto.Members = await query.ToListAsync();

                return dto;
            }
        }

        public async Task<List<GroupListOutput>> GetListByIdsAsync(List<Guid> ids)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Groups
                            where ids.Contains(item.Id)
                            select item;

                return await query.ProjectTo<GroupListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<RemotingResult> QuitAsync(GroupInput input)
        {
            if (!input.Id.HasValue)
                throw new ArgumentNullException("must not be null", nameof(input.Id));

            if (!input.CurrentEmployeeId.HasValue)
                throw new ArgumentNullException("must not be null", nameof(input.CurrentEmployeeId));

            using (var db = new ServiceDbContext(_dbOptions))
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = await db.Groups
                          .Include(o => o.Members)
                          .FirstOrDefaultAsync(o => o.Id == input.Id);

                        if (entity == null)
                            return RemotingResult.Fail();

                        var member = entity.Members.Find(o => o.EmployeeId == input.CurrentEmployeeId.Value);
                        if (member == null)
                            return RemotingResult.Fail();
                        if (member.IsOwner)
                            return RemotingResult.Fail(FailedCodes.Group_OwnerCannotQuit);
                        entity.Members.Remove(member);

                        await db.SaveChangesAsync();

                        await _busMs.Publish(new GroupMembersUpdated { Id = entity.Id });

                        await _busMs.Publish(new GroupNotified
                        {
                            Id = entity.Id,
                            Notifies = new List<EventNotifyDto>
                            {
                                new EventNotifyDto
                                {
                                    Target = NotifyTargetType.Conversation,
                                    TargetId = input.Id.ToString(),
                                    Created = DateTimeOffset.UtcNow,
                                    Text = input.CurrentEmployeeName,
                                    TargetCategory = (int)GroupEventNotifyType.ParticipantQuited
                                }
                            }
                        });

                        tx.Commit();
                        return RemotingResult.Success();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _logger.Error(ex, "QuitAsync Transaction rollback");
                        return RemotingResult.Fail();
                    }
                }
            }
        }

        public async Task<RemotingResult<string>> CreateScanJoinCodeAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty) throw new ArgumentException("must not be empty", nameof(id));
            if (userId == Guid.Empty) throw new ArgumentException("must not be empty", nameof(userId));

            var createdBy = Guid.Empty;
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from entity in db.Groups
                            where entity.Id == id && entity.Type == GroupType.CustomChat
                            select entity.CreatedBy;
                createdBy = await query.FirstOrDefaultAsync();
                if (createdBy == Guid.Empty) return RemotingResult<string>.Fail(1);
                if (createdBy != userId) return RemotingResult<string>.Fail(2);
            }
            var fingerprint = Guid.NewGuid().ToString("N");
            await _simpleKeyValueService.AddOrUpdate(SKVContainer_ScanJoinCode,
                id.ToString("N"), fingerprint);
            return RemotingResult<string>.Success(id.ToString("N") + "," + fingerprint);
        }

        public async Task<RemotingResult> ScanJoinAsync(string code, Guid employeeId, string employeeName)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (employeeId == Guid.Empty) throw new ArgumentException("must not be empty", nameof(employeeId));

            var splited = code.Split(',');
            if (splited.Length != 2) return RemotingResult.Fail(1);
            if (!Guid.TryParse(splited[0], out Guid id)) return RemotingResult.Fail(1);
            if (!Guid.TryParse(splited[1], out Guid _)) return RemotingResult.Fail(1);

            var originalFingerprint = await _simpleKeyValueService.CheckAndGet(SKVContainer_ScanJoinCode,
                id.ToString("N"), TimeSpan.FromDays(1));
            if (splited[1] != originalFingerprint) return RemotingResult.Fail(1);

            using (var db = new ServiceDbContext(_dbOptions))
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var entity = await db.Groups.Include(o => o.Members)
                            .Where(o => o.Type == GroupType.CustomChat)
                            .FirstOrDefaultAsync(o => o.Id == id);

                        if (entity == null)
                            return RemotingResult.Fail(2);

                        if (entity.Members.Exists(o => o.EmployeeId == employeeId))
                            return RemotingResult.Fail(3);

                        entity.Members.Add(new GroupMember
                        {
                            EmployeeId = employeeId,
                            GroupId = id,
                            Joined = DateTimeOffset.UtcNow,
                        });

                        await db.SaveChangesAsync();

                        await _busMs.Publish(new GroupMembersUpdated { Id = entity.Id });

                        await _busMs.Publish(new GroupNotified
                        {
                            Id = entity.Id,
                            Notifies = new List<EventNotifyDto>
                            {
                                new EventNotifyDto
                                {
                                    Target = NotifyTargetType.Conversation,
                                    TargetId = entity.Id.ToString(),
                                    Created = DateTimeOffset.UtcNow,
                                    Text = employeeName,
                                    TargetCategory = (int)GroupEventNotifyType.ParticipantAdded
                                }
                            }
                        });

                        tx.Commit();
                        return RemotingResult.Success();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        _logger.Error(ex, "UpdateAsync Transaction rollback");
                        return RemotingResult.Fail();
                    }
                }
            }
        }
    }
}
