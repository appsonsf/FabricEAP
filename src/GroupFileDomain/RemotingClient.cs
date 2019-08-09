using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using static Common.Constants;

namespace GroupFile
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static IControlAppService CreateGroupFileControlAppService()
        {
            var builder = new ServiceUriBuilder(AppName_GroupFile, ServiceName_GroupFileService);
            return proxyFactory.CreateServiceProxy<IControlAppService>(
                builder.ToUri(), listenerName: ListenerName_ControlAppService);
        }
    }
}
