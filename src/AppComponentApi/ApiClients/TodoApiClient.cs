using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiCommon.Extensions;
using Common.Services;
using AppComponent.ViewModels;
using ConfigMgmt;
using ConfigMgmt.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace AppComponent
{
    public class TodoApiClient : ITodoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public TodoApiClient(HttpClient httpClient, IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = Log.ForContext<TodoApiClient>();
        }

        public async Task<List<TodoListVM>> GetDoneListAsync(ComponentConfig config)
        {
            return await this.GetAsync(config, ComponentDataSourceKeys.TodoDoneList);
        }

        /// <summary>
        /// get pending task 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<List<TodoListVM>> GetPendingListAsync(ComponentConfig config)
        {
            return await this.GetAsync(config, ComponentDataSourceKeys.TodoPendingList);
        }

        private async Task<List<TodoListVM>> GetAsync(ComponentConfig config, string componentDataSouceKey)
        {
            var datasource = config.DataSources.Find(u => u.Key == componentDataSouceKey);
            if (datasource == null || string.IsNullOrEmpty(datasource.Value))
            {
                _logger.Warning(
                    "{@ComponentConfig} has not config PendingList|DoneList key Or DataSource Url has not Config",
                    config);

                return new List<TodoListVM>();
            }

            if (config.AuthType == ComponentDataSourceAuthType.ClientCredential)
            {
                var access_token =
                    await _identityService.GetClientCredentialAccessTokenAsync(config.AuthUsername, config.AuthPassword, "mp.todo");
                _httpClient.SetBearerToken(access_token);
            }
            _httpClient.AddUserClaimsHeader(_httpContextAccessor.HttpContext);

            var response = await _httpClient.GetStringAsync(datasource.Value);
            var vm = JsonConvert.DeserializeObject<List<TodoListVM>>(response);
            return vm;
        }
    }
}
