using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TodoCenterProxyApi.Extensions
{
    public static class HttpContextExtension
    {
        public static Dictionary<string, string> DecodeUserClaimsFromRequestHeaders(this HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(ApiCommon.Extensions.HttpClientExtension.UserClaimsHeaderName))
                throw new Exception("request has not userClaims header!");

            var raw_userclaims = context.Request.Headers[ApiCommon.Extensions.HttpClientExtension.UserClaimsHeaderName];
            var claims = Encoding.UTF8.GetString(Convert.FromBase64String(raw_userclaims));
            var claims_dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(claims);
            return claims_dic;
        }
    }
}
