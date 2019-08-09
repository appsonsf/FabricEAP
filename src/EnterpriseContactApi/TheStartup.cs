using EnterpriseContact.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EnterpriseContact
{
    public static class TheStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(RemotingClient.CreateDepartmentAppService());
            services.AddSingleton(RemotingClient.CreatePositionAppService());
            services.AddSingleton(RemotingClient.CreateEmployeeAppService());
            services.AddSingleton(RemotingClient.CreateGroupAppService());

            services.AddScoped<IEmployeeCacheService, EmployeeCacheService>();
        }
    }
}
