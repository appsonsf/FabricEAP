using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ConversationCtrlStateService.EventBus;
using InstantMessage;
using InstantMessage.Entities;
using MassTransit;
using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Notification;
using ServiceFabricContrib;

namespace ConversationCtrlStateService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class Service : StatefulServiceEapBase
    {
        public const string ConversationKeyToIdDictName = "Conversation/KeyToId";
        public const string ConversationListDictName = "Conversation/List";

        private readonly IBusControl _busMs;


        public Service(StatefulServiceContext context,
             DiagnosticPipeline diagnosticPipeline)
             : base(context, diagnosticPipeline)
        {
            _busMs = CreateMsBus(context);
        }

        private IBusControl CreateMsBus(ServiceContext serviceContext)
        {
            var option = serviceContext.GetOption<RabbitMQOption>("RabbitMQ_ms");

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(option.HostAddress), h =>
                {
                    h.Username(option.Username);
                    h.Password(option.Password);
                });
                cfg.ReceiveEndpoint(host, "EAP_ConversationCtrlStateService", c =>
                {
                    c.Consumer(() => new DepartmentDeletedConsumer(StateManager));
                    c.Consumer(() => new DepartmentMembersUpdatedConsumer(StateManager, EnterpriseContact.RemotingClient.CreateEmployeeAppService()));
                    c.Consumer(() => new GroupAddedConsumer(StateManager, EnterpriseContact.RemotingClient.CreateGroupAppService()));
                    c.Consumer(() => new GroupDeletedConsumer(StateManager));
                    c.Consumer(() => new GroupMembersUpdatedConsumer(StateManager, EnterpriseContact.RemotingClient.CreateGroupAppService()));
                    c.Consumer(() => new GroupNotifiedConsumer(StateManager, Notification.RemotingClient.CreateNotifySessionActor));
                });

            });
            return bus;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,
                        new ConversationCtrlAppService(Context,StateManager,
                        EnterpriseContact.RemotingClient.CreateEmployeeAppService())))
            };
        }

        public static async Task<IReliableDictionary2<string, Guid>> GetKeyToIdDictAsync(IReliableStateManager stateManager)
        {
            return await stateManager.GetOrAddAsync<IReliableDictionary2<string, Guid>>(ConversationKeyToIdDictName);
        }

        public static async Task<IReliableDictionary2<Guid, Conversation>> GetListDictAsync(IReliableStateManager stateManager)
        {
            return await stateManager.GetOrAddAsync<IReliableDictionary2<Guid, Conversation>>(ConversationListDictName);
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                ServiceEventSource.Current.ServiceMessage(Context, "inside RunAsync for ConversationCtrlStateService Service");

                await _busMs.StartAsync();

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Service Fabric wants us to stop
                        await _busMs.StopAsync();

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            catch (OperationCanceledException e)
            {
                ServiceEventSource.Current.ServiceMessage(Context, "RunAsync Stoped, {0}", e);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceMessage(Context, "RunAsync Failed, {0}", e);
                throw;
            }
        }
    }
}
