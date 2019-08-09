using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Base.Mdm.Org.MsgContracts;
using EFCore.BulkExtensions;
using EnterpriseContact.Entities;
using EnterpriseContactService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseContact.Consumers
{
    //TODO 在更新数据之后，需要发送事件，让其他地方处理诸如清除缓存等操作
    public class MdmDataConsumer : IConsumer<FullOrgData>
    {
        protected readonly IMapper _mapper;
        protected readonly DbContextOptions<ServiceDbContext> _dbOptions;

        public MdmDataConsumer(DbContextOptions<ServiceDbContext> dbOptions, IMapper mapper)
        {
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<FullOrgData> context)
        {
            //sort: department,position,employee
            var message = context.Message;
            if (message.Contacts == null ||
                message.Contacts.Count == 0 ||
                message.OrgUnits == null ||
                message.OrgUnits.Count == 0)
            {
                return;
            }

            if (!await this.EnsureHistoryChangedAsync(message.DataVersion, message.OrgUnits.Count, message.Contacts.Count))
            {
                return;
            }
            var departments = this._mapper.Map<List<Department>>(message.OrgUnits);
            var positions = departments.SelectMany(u => u.Positions).ToList();
            var employees = this._mapper.Map<List<Employee>>(message.Contacts);
            var employeePositions = employees.SelectMany(u => u.Positions).ToList();
            employeePositions = this.RemoveRepeatAndNull(employeePositions);
            using (var db = new ServiceDbContext(_dbOptions))
            {
                try
                {
                    await this.FillInManualDataAsync(db, departments, positions, employees, employeePositions);
                    await db.BulkInsertOrUpdateOrDeleteAsync(departments);
                    await db.BulkInsertOrUpdateOrDeleteAsync(positions);
                    await db.BulkInsertOrUpdateOrDeleteAsync(employees);
                    await db.BulkInsertOrUpdateOrDeleteAsync(employeePositions);
                    await db.SaveChangesAsync();

                    await db.MdmDataHistories.AddAsync(new MdmDataHistory()
                    {
                        HistoryVersion = message.DataVersion,
                        SyncTime = DateTimeOffset.Now,
                        Description = $"Departments:{message.OrgUnits.Count},Employeees:{message.Contacts.Count}"
                    });
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message(ex.ToString());
                }

            }
        }

        private async Task FillInManualDataAsync(ServiceDbContext db, List<Department> departments, List<Position> positions, List<Employee> employees, List<EmployeePosition> employeePositions)
        {
            var manualDepartments = await db.Departments.Where(u => u.DataSourceType == DataSourceType.Manual).ToListAsync();
            var manualPositions = await db.Positions.Where(u => u.DataSourceType == DataSourceType.Manual).ToListAsync();
            var manualEmployees = await db.Employees.Where(u => u.DataSourceType == DataSourceType.Manual).ToListAsync();
            var manualEmployeePositions = await db.EmployeePositions.Where(u => u.DataSourceType == DataSourceType.Manual).ToListAsync();
            departments.AddRange(manualDepartments);
            positions.AddRange(manualPositions);
            employees.AddRange(manualEmployees);
            employeePositions.AddRange(manualEmployeePositions);

        }

        private List<EmployeePosition> RemoveRepeatAndNull(List<EmployeePosition> employeePositions)
        {
            var dic = new Dictionary<string, EmployeePosition>();
            foreach (var employeePosition in employeePositions)
            {
                if (employeePosition.PositionId == default) continue;
                var key = employeePosition.EmployeeId.ToString() + employeePosition.PositionId.ToString();
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, employeePosition);
                }
            }
            return dic.Select(u => u.Value).ToList();
        }

        public async Task<bool> EnsureHistoryChangedAsync(string currentVersion, int departmentCount, int employeeCount)
        {
            using (var db = new ServiceDbContext(this._dbOptions))
            {
                var latestHistory = await db.MdmDataHistories.OrderByDescending(u => u.SyncTime).FirstOrDefaultAsync();
                if (latestHistory == null || latestHistory.HistoryVersion != currentVersion)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
