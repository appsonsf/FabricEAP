using Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppComponent
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ITodoApiClient, TodoApiClient>();
        }
    }
}
