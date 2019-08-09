using Microsoft.Extensions.DependencyInjection;
using Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway
{
    public static class NotifyStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Func<Guid, INotifySessionActor>>(u => RemotingClient.CreateNotifySessionActor);
        }
    }
}
