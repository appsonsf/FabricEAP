using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppComponent.ViewModels;
using ConfigMgmt;
using ConfigMgmt.Entities;

namespace TodoCenterProxyApi.Services
{
    public interface ITodoService
    {
        /// <summary>
        /// 获取代办数量
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns></returns>
        Task<IList<TodoListVM>> GetAsync(Dictionary<string, string> userIdentities);

        /// <summary>
        /// 获取待办的数量
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns></returns>
        Task<int> GetUnReadCountAsync(Dictionary<string, string> userIdentities);

        /// <summary>
        /// 获取已完成代办信息
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns></returns>
        Task<IList<TodoListVM>> GetCompleteAsync(Dictionary<string, string> userIdentities);
    }

    public abstract class TodoServiceBase
    {
        protected readonly IBizSystemAppService _bizSystemAppService;

        protected TodoServiceBase(IBizSystemAppService bizSystemAppService)
        {
            _bizSystemAppService = bizSystemAppService;
        }

        protected async Task<TodoCenterBizSystem> EnsureBizSystemConfigAsync(string bizSystemId)
        {
            var configs = await this._bizSystemAppService.GetAllForTodoCenterAsync();
            var config = configs.FirstOrDefault(u => u.Id == bizSystemId);
            if (config != null)
                return config;
            throw new Exception("requird TodoCenterApp Config:" + bizSystemId);
        }

        protected void EnsureUserIdentity(Dictionary<string, string> userIdentities, ref string idcardno)
        {
            if (userIdentities == null || userIdentities.Count == 0)
                throw new Exception("required userIdentityies!");
            idcardno = userIdentities.ContainsKey("idcard_number") ? userIdentities["idcard_number"] : "";
            if (string.IsNullOrEmpty(idcardno))
                throw new Exception("required user idcardno!");
        }
    }
}
