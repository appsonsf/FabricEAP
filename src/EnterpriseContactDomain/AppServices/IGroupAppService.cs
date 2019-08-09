using Microsoft.ServiceFabric.Services.Remoting;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    public interface IGroupAppService : IService
    {
        /// <summary>
        /// 检查两个员工是否同处一个白名单
        /// </summary>
        /// <param name="currentEmployeeId"></param>
        /// <param name="targetEmployeeId"></param>
        /// <returns></returns>
        Task<bool> CheckSameWhiteListGroupAsync(Guid currentEmployeeId, Guid targetEmployeeId);

        /// <summary>
        /// 得到某个员工加入的自定义群组列表
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<List<GroupListOutput>> GetListByEmployeeIdAsync(Guid employeeId);

        /// <summary>
        /// 创建自定义群组列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Guid> CreateCustomAsync(GroupInput input);

        /// <summary>
        /// 得到群组的所有成员的UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Guid>> GetUserIdsByIdAsync(Guid id);

        /// <summary>
        /// 更新自定义群组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RemotingResult> UpdateAsync(GroupInput input);

        /// <summary>
        /// 删除自定义群组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RemotingResult> DeleteAsync(GroupInput input);

        //TODO 支持部门群组的情况
        /// <summary>
        /// 得到自定义/部门群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GroupOutput> GetByIdAsync(Guid id);

        /// <summary>
        /// 基于Id数组得到群组列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<GroupListOutput>> GetListByIdsAsync(List<Guid> ids);

        /// <summary>
        /// 退出自定义群组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RemotingResult> QuitAsync(GroupInput input);
        
        /// <summary>
        /// 生成扫码加群的代码
        /// </summary>
        /// <param name="id">群组Id</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns>FailedCode=1：不存在此群组；FailedCode=2：当前用户不是创建者</returns>
        Task<RemotingResult<string>> CreateScanJoinCodeAsync(Guid id, Guid userId);

        /// <summary>
        /// 处理扫码加群
        /// </summary>
        /// <param name="code">二维码内容</param>
        /// <param name="employeeId">当前用户的员工Id</param>
        /// <param name="employeeName">当前用户的员工姓名</param>
        /// <returns>FailedCode=1：code格式或内容不对；FailedCode=2：群组不存在；FailedCode=3：此成员已经在群组中</returns>
        Task<RemotingResult> ScanJoinAsync(string code, Guid employeeId, string employeeName);
    }
}
