using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using static Common.Constants;

namespace ConfigMgmt
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static IClientAppService CreateClientAppService()
        {
            var builder = new ServiceUriBuilder(AppName_ConfigMgmt, ServiceName_SystemConfigStateService);
            return proxyFactory.CreateServiceProxy<IClientAppService>(
                builder.ToUri(), listenerName: ListenerName_ClientAppService);
        }

        public static IBizSystemAppService CreateBizSystemAppService()
        {
            var builder = new ServiceUriBuilder(AppName_ConfigMgmt, ServiceName_SystemConfigStateService);
            return proxyFactory.CreateServiceProxy<IBizSystemAppService>(
                builder.ToUri(), listenerName: ListenerName_TodoCenterAppService);
        }

        public static IWorkbenchAppService CreateWorkbenchAppService()
        {
            var builder = new ServiceUriBuilder(AppName_ConfigMgmt, ServiceName_SystemConfigStateService);
            return proxyFactory.CreateServiceProxy<IWorkbenchAppService>(
                builder.ToUri(), listenerName: ListenerName_WorkbenchAppService);
        }

        public static IUserFavoriteAppService CreateUserFavoriteAppService(Guid userId)
        {
            var builder = new ServiceUriBuilder(AppName_ConfigMgmt, ServiceName_UserConfigStateService);
            return proxyFactory.CreateServiceProxy<IUserFavoriteAppService>(
                builder.ToUri(), new ItemId(userId).GetPartitionKey(), 
                listenerName: ListenerName_UserFavoriteAppService);
        }

        public static IUserSettingAppService CreateUserSettingAppService(Guid userId)
        {
            var builder = new ServiceUriBuilder(AppName_ConfigMgmt, ServiceName_UserConfigStateService);
            return proxyFactory.CreateServiceProxy<IUserSettingAppService>(
                builder.ToUri(), new ItemId(userId).GetPartitionKey(), 
                listenerName: ListenerName_UserSettingAppService);
        }
    }
}
