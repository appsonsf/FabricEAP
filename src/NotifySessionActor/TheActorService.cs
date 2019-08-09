using Common;
using MassTransit;
using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceFabricContrib;
using System;
using System.Fabric;
using Notification;
using Notification.MsgContracts;

namespace NotifySessionActor
{
    public class TheActorService : ActorServiceEapBase
    {
        public TheActorService(StatefulServiceContext context,
            ActorTypeInformation actorTypeInfo,
            DiagnosticPipeline diagnosticPipeline,
            IBusControl bus = null,
            Func<ActorService, ActorId, ActorBase> actorFactory = null,
            Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
            IActorStateProvider stateProvider = null,
            ActorServiceSettings settings = null)
            : base(context, actorTypeInfo, diagnosticPipeline, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            if (bus == null)
                EventBus = CreateBus();
            else
                EventBus = bus;
        }

        private IBusControl CreateBus()
        {
            var optionRabbitMQ = Context.GetOption<RabbitMQOption>("RabbitMQ_ms");

            var hostAddress = optionRabbitMQ.HostAddress.EndsWith("/") ? optionRabbitMQ.HostAddress : optionRabbitMQ + "/";
            var endpointUri = new Uri(hostAddress + RabbitMqReceiveEndpointNames.NotifyGateway);

            EndpointConvention.Map<SendMessageNotify>(endpointUri);
            EndpointConvention.Map<SendEventNotify>(endpointUri);

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(optionRabbitMQ.HostAddress), h =>
                {
                    h.Username(optionRabbitMQ.Username);
                    h.Password(optionRabbitMQ.Password);
                });

            });

            return bus;
        }

        public IBusControl EventBus { get; }
    }
}
