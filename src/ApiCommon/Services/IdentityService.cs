using IdentityModel.Client;
using AppsOnSF.Common.Options;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdSvrOption _option;
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient, IOptions<IdSvrOption> optionAccessor)
        {
            _option = optionAccessor.Value;
            _httpClient = httpClient;
        }

        public async Task<string> GetClientCredentialAccessTokenAsync(string clientId, string clientSecret, string scope)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _option.IssuerUri,
                Policy =
                {
                    RequireHttps=_option.RequireHttps
                }
            });
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            });
            if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);

            return tokenResponse.AccessToken;
        }
    }
}
