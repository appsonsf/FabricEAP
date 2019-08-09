using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using static Common.Constants;

namespace InstantMessage
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static IConversationCtrlAppService CreateConversationCtrlAppService()
        {
            var builder = new ServiceUriBuilder(AppName_InstantMessage, ServiceName_ConversationCtrlStateService);
            return proxyFactory.CreateServiceProxy<IConversationCtrlAppService>(
                builder.ToUri());
        }

        public static IConversationMsgAppService CreateConversationMsgAppService(Guid conversationId)
        {
            var builder = new ServiceUriBuilder(AppName_InstantMessage, ServiceName_ConversationMsgStateService);
            return proxyFactory.CreateServiceProxy<IConversationMsgAppService>(builder.ToUri(),
                new ItemId(conversationId).GetPartitionKey());
        }

        public static async Task<IEnumerable<IConversationMsgAppService>> GetAllConversationMsgAppServicesAsync()
        {
            var builder = new ServiceUriBuilder(AppName_InstantMessage, ServiceName_ConversationMsgStateService);
            var serviceUri = builder.ToUri();
            var lst = new List<IConversationMsgAppService>();
            foreach (var p in await fabricClient.QueryManager.GetPartitionListAsync(serviceUri))
            {
                long minKey = (p.PartitionInformation as Int64RangePartitionInformation).LowKey;
                var appService = proxyFactory.CreateServiceProxy<IConversationMsgAppService>(builder.ToUri(),
                    new ServicePartitionKey(minKey));
                lst.Add(appService);
            }
            return lst;
        }
    }
}
