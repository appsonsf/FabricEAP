using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public class EmployeeAppService : BaseAppService, IEmployeeAppService
    {
        public EmployeeAppService(StatelessServiceContext serviceContext, DbContextOptions<ServiceDbContext> dbOptions, IMapper mapper)
            : base(serviceContext, dbOptions, mapper)
        {
        }

        public async Task<EmployeeOutput> GetByIdAsync(Guid id)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var dto = await db.Employees.Where(o => o.Id == id)
                    .ProjectTo<EmployeeOutput>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (dto != null)
                {
                    var query = from item in db.EmployeePositions
                                where item.EmployeeId == id && !item.IsPrimary
                                select item.PositionId;
                    dto.ParttimePositionIds = await query.ToListAsync();
                }
                return dto;
            }
        }

        public async Task<EmployeeOutput> GetByUserIdAsync(Guid userId)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var dto = await db.Employees.Where(o => o.UserId == userId)
                    .ProjectTo<EmployeeOutput>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (dto != null)
                {
                    var query = from item in db.EmployeePositions
                                where item.EmployeeId == dto.Id && !item.IsPrimary
                                select item.PositionId;
                    dto.ParttimePositionIds = await query.ToListAsync();
                }
                return dto;
            }
        }

        public async Task<List<EmployeeListOutput>> GetListByDepartmentIdAsync(Guid departmentId)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                return await SelectByDepartmentId(db, departmentId);
            }
        }

        public async Task<List<EmployeeListOutput>> GetListByIdsAsync(Guid[] ids)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Employees
                            where ids.Contains(item.Id)
                            orderby item.Number
                            select item;

                return await query.ProjectTo<EmployeeListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<List<EmployeeCacheOutput>> GetCacheDataAsync()
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Employees
                            select item;
                return await query.ProjectTo<EmployeeCacheOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public async Task<List<EmployeeListOutput>> GetRootListAsync()
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var rootDep = await db.Departments
                    .Where(o => o.ParentId == null)
                    .Select(o => o.Id)
                    .FirstOrDefaultAsync();

                return await SelectByDepartmentId(db, rootDep);
            }
        }

        public async Task<List<EmployeeListOutput>> SearchByKeywordAsync(string keyword)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Employees
                            where item.Name.Contains(keyword)
                            select item;
                return await query.ProjectTo<EmployeeListOutput>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        private Task<List<EmployeeListOutput>> SelectByDepartmentId(ServiceDbContext db, Guid departmentId)
        {
            //兼职人员也返回
            var query = from item in db.Employees
                        from ep in item.Positions
                        join pos in db.Positions.Where(o => o.DepartmentId == departmentId)
                            on ep.PositionId equals pos.Id
                        orderby item.Number
                        select new EmployeeListOutput
                        {
                            Id = item.Id,
                            Name = item.Name,
                            PositionId = pos.Id,
                            PositionName = pos.Name,
                            PrimaryDepartmentId = item.PrimaryDepartmentId,
                            PrimaryPositionId = item.PrimaryPositionId
                        };

            return query.ToListAsync();
        }

        public async Task<List<Guid>> GetUserIdsByDepartmentIdAsync(Guid departmentId)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                var query = from item in db.Employees
                            where item.UserId.HasValue
                            from ep in item.Positions
                            join pos in db.Positions.Where(o => o.DepartmentId == departmentId)
                                on ep.PositionId equals pos.Id
                            orderby item.Number
                            select item.UserId.Value;

                return await query.ToListAsync();
            }
        }
    }
}
