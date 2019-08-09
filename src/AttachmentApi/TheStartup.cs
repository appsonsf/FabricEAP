using Attachment.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attachment
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAttachmentStoreService, MinioAttachmentStoreService>();
            services.AddTransient<Func<string, IAttachmentAppService>>(provider => RemotingClient.CreateAttachmentAppService);
        }
    }
}
