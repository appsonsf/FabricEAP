using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstantMessage
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(RemotingClient.CreateConversationCtrlAppService());

            services.AddScoped<Func<Guid, IConversationMsgAppService>>(u => RemotingClient.CreateConversationMsgAppService);
            services.AddScoped<Func<Task<IEnumerable<IConversationMsgAppService>>>>(u => RemotingClient.GetAllConversationMsgAppServicesAsync);
        }
    }
}
