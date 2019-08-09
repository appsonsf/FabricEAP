using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Common;
using ServiceFabricContrib;
using AppsOnSF.Common.Options;

namespace NotifyGatewayWeb
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
                    }))
            };
        }
    }
}
