using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using static Common.Constants;

namespace Attachment
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static IAttachmentAppService CreateAttachmentAppService(string itemId)
        {
            var builder = new ServiceUriBuilder(AppName_Attachment, ServiceName_AttachmentStateService);
            return proxyFactory.CreateServiceProxy<IAttachmentAppService>(builder.ToUri(),
                new ServicePartitionKey(HashUtil.getLongHashCode(itemId)));
        }
    }
}
