using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public interface IEmployeeAppService : IService
    {
        Task<List<EmployeeListOutput>> GetRootListAsync();
        Task<List<EmployeeListOutput>> GetListByDepartmentIdAsync(Guid departmentId);
        Task<List<EmployeeListOutput>> SearchByKeywordAsync(string keyword);
        Task<EmployeeOutput> GetByIdAsync(Guid id);
        Task<List<EmployeeListOutput>> GetListByIdsAsync(Guid[] ids);
        /// <summary>
        /// 此方法仅供 EmployeeCacheService 使用
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeCacheOutput>> GetCacheDataAsync();
        Task<EmployeeOutput> GetByUserIdAsync(Guid userId);
        Task<List<Guid>> GetUserIdsByDepartmentIdAsync(Guid departmentId);
    }
}
