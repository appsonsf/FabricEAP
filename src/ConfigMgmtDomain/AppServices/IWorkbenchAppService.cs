using ConfigMgmt.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigMgmt
{
    /// <summary>
    /// 工作台AppService接口
    /// </summary>
    public interface IWorkbenchAppService : IService
    {
        /// <summary>
        /// 根据ClientId、ClientPlatform和所需角色获取工作台的应用入口列表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<AppEntranceDto>> GetAppEntrancesAsync(GetAppEntrancesInput input);

        /// <summary>
        /// 根据ClientId获取工作台的应用入口列表数据
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<List<AppEntrance>> GetAppEntrancesRawByClientIdAsync(string clientId);

        /// <summary>
        /// 添加应用入口数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateEntrancesAsync(AddEntrancesInput input);

        /// <summary>
        /// 根据应用入口Id移除
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="entryId"></param>
        /// <returns></returns>
        Task<bool> RemoveEntranceAsync(string clientId, Guid entryId);

        /// <summary>
        /// 根据应用入口Id获取组件数据源配置
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        Task<ComponentConfig> GetComponentConfigAsync(Guid entryId);
    }
}
