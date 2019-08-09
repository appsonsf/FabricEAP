using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Data;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public class DepartmentAppService : BaseAppService, IDepartmentAppService
    {
        public DepartmentAppService(StatelessServiceContext serviceContext, DbContextOptions<ServiceDbContext> dbOptions, IMapper mapper)
            : base(serviceContext, dbOptions, mapper)
        {
        }

        public async Task<List<DepartmentListOutput>> GetAllListAsync()
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Departments
                            select item;

                return await query.ProjectTo<DepartmentListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<GroupOutput> GetDepGroupByIdAsync(Guid id)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var dto = await db.Departments
                    .Where(x => x.Id == id)
                    .Select(x => new GroupOutput
                    {
                        Created = DateTimeOffset.UtcNow,
                        CreatedBy = Guid.Empty,
                        Id = x.Id,
                        Name = x.Name,
                        Type = GroupType.DepartmentChat,
                        Updated = DateTimeOffset.UtcNow
                    })
                    .FirstOrDefaultAsync();

                if (dto == null) return null;

                var query = from e in db.Employees
                            from ep in e.Positions
                            join pos in db.Positions.Where(o => o.DepartmentId == id)
                                on ep.PositionId equals pos.Id
                            orderby e.Number
                            select new GroupMemberOutput
                            {
                                EmployeeId = e.Id,
                                EmployeeName = e.Name,
                                Gender = e.Gender,
                                IsOwner = false,
                                Joined = DateTimeOffset.UtcNow,
                                PrimaryDepartmentId = e.PrimaryDepartmentId
                            };

                dto.Members = await query.ToListAsync();

                return dto;
            }
        }

        public async Task<List<DepartmentListOutput>> GetListByIdsAsync(List<Guid> ids)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Departments
                            where ids.Contains(item.Id)
                            select item;

                return await query.ProjectTo<DepartmentListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<List<DepartmentListOutput>> GetListByParentIdAsync(Guid id)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Departments
                            where item.ParentId == id
                            orderby item.Sort, item.Name
                            select item;

                return await query.ProjectTo<DepartmentListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<List<DepartmentListOutput>> GetRootListAsync()
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Departments
                            where item.ParentId == null
                            join child in db.Departments on item.Id equals child.ParentId
                            orderby child.Sort, child.Name
                            select child;

                var result = await query.ProjectTo<DepartmentListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return result;
            }
        }

        public async Task<List<DepartmentListOutput>> SearchByKeywordAsync(string keyword)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Departments
                            where item.Name.Contains(keyword)
                            orderby item.Sort, item.Name
                            select item;

                return await query.ProjectTo<DepartmentListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }
    }
}
