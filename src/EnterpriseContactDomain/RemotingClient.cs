using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using static Common.Constants;

namespace EnterpriseContact
{
    public static class RemotingClient
    {
        private static ServiceProxyFactory proxyFactory = new ServiceProxyFactory((c) => new FabricTransportServiceRemotingClientFactory());

        private static FabricClient fabricClient = new FabricClient();

        public static IDepartmentAppService CreateDepartmentAppService()
        {
            var builder = new ServiceUriBuilder(AppName_EnterpriseContact, ServiceName_EnterpriseContactService);
            return proxyFactory.CreateServiceProxy<IDepartmentAppService>(
                builder.ToUri(), listenerName:ListenerName_DepartmentAppService);
        }

        public static IEmployeeAppService CreateEmployeeAppService()
        {
            var builder = new ServiceUriBuilder(AppName_EnterpriseContact, ServiceName_EnterpriseContactService);
            return proxyFactory.CreateServiceProxy<IEmployeeAppService>(
                builder.ToUri(), listenerName: ListenerName_EmployeeAppService);
        }

        public static IPositionAppService CreatePositionAppService()
        {
            var builder = new ServiceUriBuilder(AppName_EnterpriseContact, ServiceName_EnterpriseContactService);
            return proxyFactory.CreateServiceProxy<IPositionAppService>(
                builder.ToUri(), listenerName: ListenerName_PositionAppService);
        }

        public static IGroupAppService CreateGroupAppService()
        {
            var builder = new ServiceUriBuilder(AppName_EnterpriseContact, ServiceName_EnterpriseContactService);
            return proxyFactory.CreateServiceProxy<IGroupAppService>(
                builder.ToUri(), listenerName: ListenerName_GroupAppService);
        }
    }
}
