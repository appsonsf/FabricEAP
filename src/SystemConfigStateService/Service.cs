using System.Collections.Generic;
using System.Fabric;
using AutoMapper;
using ConfigMgmt;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Common;
using static Common.Constants;

namespace SystemConfigStateService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public sealed class Service : StatefulServiceEapBase
    {
        public const string DictionaryName_Client = "ClientList";
        public const string DictionaryName_TodoCenterBizSystem = "TodoCenterBizSystemList";
        public const string DictionaryName_MsgNotifyBizSystem = "MsgNotifyBizSystemList";
        public const string DictionaryName_AppEntrance = "AppEntrancList";

        public Service(StatefulServiceContext context,
            DiagnosticPipeline diagnosticPipeline)
            : base(context, diagnosticPipeline)
        {
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
            var mapper = CreateMapper();
            return new[]
            {
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,new ClientAppService(Context,StateManager))
                    ,ListenerName_ClientAppService),
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,
                    new WorkbenchAppService(Context,StateManager,mapper))
                    ,ListenerName_WorkbenchAppService),
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,new BizSystemAppService(Context,StateManager))
                    ,ListenerName_TodoCenterAppService)
            };
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            return config.CreateMapper();
        }
    }
}
