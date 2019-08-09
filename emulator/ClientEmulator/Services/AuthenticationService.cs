using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using ClientEmulator.Config;
using ClientEmulator.Services.Models;
using Newtonsoft.Json;

namespace ClientEmulator.Services
{
    public class AuthenticationService
    {
        /// <summary>
        /// 登录,返回AccessToken
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserInfo> LoginAsync(string userName, string password)
        {
            var client = new HttpClient();
            var uri = new Uri(URLConfig.Login_Host);
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = URLConfig.Login_Host,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = uri.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase)
                }
            });
            if (disco.IsError) throw new Exception(disco.Error);
            try
            {
                var tokenResponse = await client.RequestPasswordTokenAsync(
                    this.CreatePasswordTokenRequest(disco.TokenEndpoint,
                    userName,
                    password));
                if (tokenResponse.IsError)
                    throw new Exception(tokenResponse.Raw);
                var userinfoResponse = await client.GetUserInfoAsync(new UserInfoRequest()
                {
                    Address = disco.UserInfoEndpoint,
                    Token = tokenResponse.AccessToken
                });
                if (userinfoResponse.IsError)
                    throw new Exception(tokenResponse.Raw);
                var userinfo = ConverToUserInfoModel(userinfoResponse);
                userinfo.AccessToken = tokenResponse.AccessToken;
                return userinfo;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        private UserInfo ConverToUserInfoModel(UserInfoResponse userinfo)
        {
            var username_claim = userinfo.Claims.FirstOrDefault(u => u.Type == "preferred_username");
            var name_claim = userinfo.Claims.FirstOrDefault(u => u.Type == "name");
            var sub_claim = userinfo.Claims.FirstOrDefault(u => u.Type == "sub");
            var mdmId_claim = userinfo.Claims.FirstOrDefault(u => u.Type == "employee_mdmid");
            if (username_claim == null || name_claim == null || sub_claim == null || mdmId_claim == null)
                throw new Exception("UserClaims not full,Calims:" + JsonConvert.SerializeObject(userinfo.Claims));
            return new UserInfo()
            {
                Name = name_claim.Value,
                UserName = username_claim.Value,
                UserId = sub_claim.Value,
                UserMdmId = new Guid(mdmId_claim.Value)
            };
        }

        /// <summary>
        /// 获取用户的UserInfo消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<string> GetUserInfoAsync(string accessToken)
        {
            return "";
        }

        private PasswordTokenRequest CreatePasswordTokenRequest(string address, string userName, string password)
        {
            var request = new PasswordTokenRequest()
            {
                Address = address,
                ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,
                ClientId = ClientConfig.ClientId,
                ClientSecret = ClientConfig.ClientSercet,
                UserName = userName,
                Password = password,
                Scope = string.Join(" ", ClientConfig.Scopes),
            };
            return request;
        }
    }
}
