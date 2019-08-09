using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ConfigMgmt;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Common;
using ServiceFabricContrib;
using static Common.Constants;

namespace UserConfigStateService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatefulServiceEapBase
    {
        private const string DictionaryName = "UserConfiges";

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
            return new[]
            {
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,new UserFavoriteAppService(Context,StateManager))
                    ,ListenerName_UserFavoriteAppService),
                new ServiceReplicaListener((c)=>
                    new FabricTransportServiceRemotingListener(c,new UserSettingAppService(Context,StateManager))
                    ,ListenerName_UserSettingAppService)
            };
        }
    }
}
