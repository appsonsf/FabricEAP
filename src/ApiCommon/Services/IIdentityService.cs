using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IIdentityService
    {
        Task<string> GetClientCredentialAccessTokenAsync(string clientId, string clientSecret, string scope);
    }
}
