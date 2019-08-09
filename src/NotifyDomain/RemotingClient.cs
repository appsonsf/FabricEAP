using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using static Common.Constants;

namespace Notification
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static INotifySessionActor CreateNotifySessionActor(Guid userId)
        {
            var builder = new ServiceUriBuilder(AppName_Notify, ServiceName_NotifySessionActorService);
            return ActorProxy.Create<INotifySessionActor>(new ActorId(userId), builder.ToUri());
        }
    }
}
