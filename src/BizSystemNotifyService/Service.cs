using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Result;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Base.Eap.Notify.MsgContracts;
using BizSystemNotifyService.EventBus;
using Common;
using EnterpriseContact.Services;
using MassTransit;
using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabricContrib;

namespace BizSystemNotifyService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatelessServiceEapBase
    {
        private readonly IBusControl _bus;

        public Service(StatelessServiceContext context,
          DiagnosticPipeline diagnosticPipeline)
          : base(context, diagnosticPipeline)
        {
            _bus = CreateBus(context);
        }

        //private const string RabbitMqReceiveEndpointName = "Base.Eap.BizSystemNotifyService";

        private IBusControl CreateBus(ServiceContext serviceContext)
        {
            var option = serviceContext.GetOption<RabbitMQOption>("RabbitMQ");

            var cache = new MemoryCache(new MemoryCacheOptions());

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(option.HostAddress), h =>
                {
                    h.Username(option.Username);
                    h.Password(option.Password);
                });
                cfg.ReceiveEndpoint(host, BizSystemNotifyMsg.RabbitMqReceiveEndpointName, c =>
                {
                    c.Consumer(() => new BizSystemNotifyMsgConsumer(
                        ConfigMgmt.RemotingClient.CreateBizSystemAppService(),
                        Notification.RemotingClient.CreateNotifySessionActor,
                        new EmployeeCacheService(cache, EnterpriseContact.RemotingClient.CreateEmployeeAppService())
                        ));
                });

            });
            return bus;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(_ => new MassTransitListener(_bus), "masstransit"),
            };
        }
    }
}
