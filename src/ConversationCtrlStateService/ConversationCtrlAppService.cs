using InstantMessage;
using InstantMessage.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using Notification;
using EnterpriseContact;
using ConversationCtrlStateService.DomainServices;

namespace ConversationCtrlStateService
{
    public class ConversationCtrlAppService : StatefulRemotingService, IConversationCtrlAppService
    {
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ConversationManager _manager;

        public ConversationCtrlAppService(StatefulServiceContext serviceContext,
            IReliableStateManager stateManager,
            IEmployeeAppService employeeAppService) : base(serviceContext, stateManager)
        {
            _employeeAppService = employeeAppService;
            _manager = new ConversationManager(stateManager);
        }

        public async Task DeleteAsync(Guid id) => await _manager.DeleteAsync(id);

        public async Task<Conversation> GetOrAddP2PAsync(Guid senderId, Guid receiverId)
        {
            var dictKeyToId = await Service.GetKeyToIdDictAsync(StateManager);
            var dictIdToEntity = await Service.GetListDictAsync(StateManager);
            var key = GenerateConversationKey(senderId, receiverId);
            using (var tx = StateManager.CreateTransaction())
            {
                var newId = Guid.NewGuid();
                var id = await dictKeyToId.GetOrAddAsync(tx, key, newId);
                var newEntity = new Conversation
                {
                    Id = id,
                    Key = key,
                    Type = ConversationType.P2P,
                    Participants = new List<Guid> { senderId, receiverId },
                    Updated = DateTimeOffset.UtcNow
                };
                var entity = await dictIdToEntity.GetOrAddAsync(tx, id, newEntity);
                await tx.CommitAsync();
                return entity;
            }
        }

        public async Task<Conversation?> GetByIdAsync(Guid id) => await _manager.GetByIdAsync(id);

        public static string GenerateConversationKey(Guid senderId, Guid receiverId)
        {
            var lst = new List<string>
            {
                senderId.ToString("N"),
                receiverId.ToString("N")
            };
            lst.Sort();
            var source = string.Join(",", lst);
            return source.ComputeMd5();
        }

        public async Task<RemotingResult> AddDepAsync(AddDepConversationInput input)
        {
            var userIds = await _employeeAppService.GetUserIdsByDepartmentIdAsync(input.DepartmentId);

            if (!userIds.Contains(input.UserId))
                return RemotingResult.Fail(1);
            if (userIds.Count < 2)
                return RemotingResult.Fail(2);

            await _manager.AddOrUpdateAsync(input.DepartmentId, userIds, ConversationType.DepartmentGroup);
            return RemotingResult.Success();
        }

        public async Task<List<Conversation>> GetByIdsAsync(List<Guid> ids)
        {
            var result = new List<Conversation>();
            var dictIdToEntity = await Service.GetListDictAsync(StateManager);
            using (var tx = StateManager.CreateTransaction())
            {
                foreach (var id in ids)
                {
                    var entityWrap = await dictIdToEntity.TryGetValueAsync(tx, id);
                    if (entityWrap.HasValue)
                        result.Add(entityWrap.Value);
                }
            }
            return result;
        }
    }
}
