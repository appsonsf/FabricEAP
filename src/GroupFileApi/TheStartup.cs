using Attachment;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupFile
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(RemotingClient.CreateGroupFileControlAppService());
            services.AddTransient<Func<string, IAttachmentAppService>>(provider => Attachment.RemotingClient.CreateAttachmentAppService);
        }
    }
}
