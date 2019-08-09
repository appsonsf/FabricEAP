using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TodoCenterProxyApi.Extensions.Internal
{
    internal class JsonRpcRequest<T> where T : class
    {

        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public object Params { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
