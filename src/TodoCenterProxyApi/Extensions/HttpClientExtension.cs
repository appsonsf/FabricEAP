using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoCenterProxyApi.Extensions.Internal;

namespace TodoCenterProxyApi.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> PostJsonRpcAsync<T>(this HttpClient client, string requestUri, string methodName, T argument = null)
        where T : class
        {
            var rpcRequest = new JsonRpcRequest<T>()
            {
                JsonRpc = "2.0",
                Method = methodName,
                Params = argument,
                Id = 1
            };
            var httpRequestContent = new StringContent(JsonConvert.SerializeObject(rpcRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, httpRequestContent);
            return response;
        }
    }
}
