using Common.Services;
using ConfigMgmt.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Serilog.Core;
using ConfigMgmt;
using ApiCommon.Extensions;
using Serilog;

namespace ConfigMgmt
{
    //ref: https://thinkrethink.net/2018/07/25/httpclient-httpclientfactory-asp-net-core/

    /// <summary>
    /// 待办组件Api的类型化HttpClient
    /// </summary>
    public class BadgeApiClient : IBadgeApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public BadgeApiClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = Log.ForContext<BadgeApiClient>();
        }

        public async Task<int?> GetAmountAsync(ComponentConfig config)
        {
            var datasource = config.DataSources.Find(o => o.Key == ComponentDataSourceKeys.BadgeValue);
            if (datasource == null || string.IsNullOrEmpty(datasource.Value))
            {
                _logger.Warning(
                    "{@ComponentConfig} has not config BadgeValue or DataSource Url has not Config", 
                    config);

                return null;
            }

            var url = datasource.Value;
            if (config.AuthType == ComponentDataSourceAuthType.ClientCredential)
            {
                var accessToken = await _identityService.GetClientCredentialAccessTokenAsync(config.AuthUsername, config.AuthPassword, "mp.todo");
                _httpClient.SetBearerToken(accessToken);
            }
            _httpClient.AddUserClaimsHeader(_httpContextAccessor.HttpContext);

            try
            {
                var content = await _httpClient.GetStringAsync(url);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(content);
                return result;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }
    }
}
