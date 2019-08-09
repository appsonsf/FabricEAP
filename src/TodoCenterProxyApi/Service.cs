using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Microsoft.Diagnostics.EventFlow;
using Common;
using ServiceFabricContrib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TodoCenterProxyApi
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Service : StatelessServiceEapBase
    {
        public Service(StatelessServiceContext context,
            DiagnosticPipeline diagnosticPipeline)
            : base(context, diagnosticPipeline)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseCommonConfiguration(serviceContext)
                                    .ConfigureServices(
                                        services => services
                                            .AddLogging()
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .ConfigureLogging((hostingContext, logging)=>{
                                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                        logging.AddConsole();
                                        logging.AddDebug();
                                    })
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    }),"Web")
            };
        }
    }
}
