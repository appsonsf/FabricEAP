using EnterpriseContact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact.Services
{
    public interface IEmployeeCacheService
    {
        Task<List<EmployeeCacheOutput>> ByNumberAsync(params string[] keys);

        Task<List<EmployeeCacheOutput>> ByUserIdAsync(params Guid[] keys);

        Task<List<EmployeeCacheOutput>> ByIdAsync(params Guid[] keys);

        /// <summary>
        /// 清除映射数据的缓存，在EC同步到MDM的数据之后，需要通过事件消费者来调用此方法
        /// </summary>
        void ClearCache();
    }
}
