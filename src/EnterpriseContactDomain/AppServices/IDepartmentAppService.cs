using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public interface IDepartmentAppService : IService
    {
        Task<List<DepartmentListOutput>> GetRootListAsync();
        Task<List<DepartmentListOutput>> GetListByParentIdAsync(Guid id);
        Task<List<DepartmentListOutput>> SearchByKeywordAsync(string keyword);
        Task<List<DepartmentListOutput>> GetAllListAsync();

        /// <summary>
        /// 得到部门虚拟群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GroupOutput> GetDepGroupByIdAsync(Guid id);

        /// <summary>
        /// 通过Id集合得到部门列表信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<DepartmentListOutput>> GetListByIdsAsync(List<Guid> ids);
    }
}
