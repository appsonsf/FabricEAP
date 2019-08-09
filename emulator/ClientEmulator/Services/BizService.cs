using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientEmulator.Config;

namespace ClientEmulator.Services
{
    public class BizService
    {
        private readonly HttpClient _httpClient;
        public event Action<HttpResponseMessage> OnErrorOrExceptionEvent;
        public event Action<string> OnSuccessEvent;

        public BizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 获取代办信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public async Task GetPendingList(string accessToken, string componentId)
        {
            this._httpClient.SetBearerToken(accessToken);
            var response = await this._httpClient.GetAsync(BizUrlConfig.PUll_TODO_GET + "?appid=" + componentId);
            if (!response.IsSuccessStatusCode)
            {
                this.OnErrorOrExceptionEvent?.Invoke(response);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            this.OnSuccessEvent?.Invoke(content);
        }

        /// <summary>
        /// 获取已经完成的信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="componentId"></param>
        /// <returns></returns>
        public async Task GetDoneList(string accessToken, string componentId)
        {
            this._httpClient.SetBearerToken(accessToken);
            var response = await this._httpClient.GetAsync(BizUrlConfig.PUll_DONE_GET + "?appid=" + componentId);
            if (!response.IsSuccessStatusCode)
            {
                this.OnErrorOrExceptionEvent?.Invoke(response);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            this.OnSuccessEvent?.Invoke(content);
        }
    }
}
