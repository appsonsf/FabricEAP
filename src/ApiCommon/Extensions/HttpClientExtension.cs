using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiCommon.Extensions
{
    public static class HttpClientExtension
    {
        public static string UserClaimsHeaderName = "UserClaims";

        public static void AddUserClaimsHeader(this HttpClient client, HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                throw new InvalidOperationException("user do not Authenticated!");

            var claimsDic = new Dictionary<string, string>();
            foreach (var userClaim in context.User.Claims)
            {
                if (!claimsDic.ContainsKey(userClaim.Type))
                    claimsDic.Add(userClaim.Type, userClaim.Value);
            }
            var claimsString = JsonConvert.SerializeObject(claimsDic);
            var claimsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(claimsString));
            if (client.DefaultRequestHeaders.Contains(UserClaimsHeaderName))
                client.DefaultRequestHeaders.Remove(UserClaimsHeaderName);
            client.DefaultRequestHeaders.Add(UserClaimsHeaderName, claimsBase64);
        }
    }
}
