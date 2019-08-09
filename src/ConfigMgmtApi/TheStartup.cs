using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConfigMgmt
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IBadgeApiClient, BadgeApiClient>();

            services.AddSingleton(RemotingClient.CreateClientAppService());
            services.AddSingleton(RemotingClient.CreateBizSystemAppService());
            services.AddSingleton(RemotingClient.CreateWorkbenchAppService());

            services.AddScoped<Func<Guid, IUserSettingAppService>>(u => RemotingClient.CreateUserSettingAppService);
            services.AddScoped<Func<Guid, IUserFavoriteAppService>>(u => RemotingClient.CreateUserFavoriteAppService);

        }
    }
}
