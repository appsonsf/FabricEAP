using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientEmulator.Config;
using ClientEmulator.Services.Models;
using Common;
using EnterpriseContact.ViewModels;
using InstantMessage.ViewModels;
using Newtonsoft.Json;

namespace ClientEmulator.Services
{
    public partial class ConversationService
    {
        /// <summary>
        /// 创建一个群组,返回一个会话ID
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseData<Guid>> CreateGroupAsync(UserInfo owner, UserInfo partner, string name, string remark)
        {
            this._httpClient.SetBearerToken(owner.AccessToken);
            var param = new GroupEditVm()
            {
                Name = name,
                Remark = remark,
                AddingMemberIds = new HashSet<Guid>() { partner.UserMdmId, owner.UserMdmId },
                AddingMemberNames = new List<string>() { partner.Name, owner.Name }
            };

            var response = await this._httpClient.PostAsync(BizUrlConfig.CreateGroup_POST,
                new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json"));
            var str = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<ResponseData<Guid>>(str);
            return responseModel;
        }

        public async Task<ResponseData> QuitGroup(UserInfo userinfo, Guid groupId)
        {
            this._httpClient.SetBearerToken(userinfo.AccessToken);
            var response = await this._httpClient.PutAsync(string.Format(BizUrlConfig.QuitGroup_PUT, groupId), null);
            var str = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseData>(str);
            return model;
        }

        public async Task<ResponseData> DeleteGroup(UserInfo userinfo, Guid groupId)
        {
            this._httpClient.SetBearerToken(userinfo.AccessToken);
            var response = await this._httpClient.DeleteAsync(string.Format(BizUrlConfig.DeleteGroup_DELETE, groupId));
            var str = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseData>(str);
            return model;
        }
    }
}
