using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ConfigMgmt
{
    public interface IUserSettingAppService : IService
    {
        Task<InfoVisibility> GetInfoVisibilityAsync(Guid userId);

        Task SetInfoVisibilityAsync(Guid userId, InfoVisibility value);

        /// <summary>
        /// 更新用户的应用入口验证状态
        /// </summary>
        /// <param name="updateAppEntranceAuthStateInput"></param>
        /// <returns></returns>
        Task SetAppEntranceAuthStateAsync(Guid userId, AppEntranceAuthStateInput updateAppEntranceAuthStateInput);
        
        /// <summary>
        /// 得到用户应用入口验证状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceCode"></param>
        /// <param name="appEntranceId"></param>
        /// <returns>true需要验证，false不需要验证</returns>
        Task<bool> GetAppEntranceAuthStateAsync(Guid userId, string deviceCode, Guid appEntranceId);
    }
}
