using AutoMapper;
using Common;
using EnterpriseContact.Consumers;
using MassTransit;
using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using static Common.Constants;

namespace EnterpriseContact
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatelessServiceEapBase
    {
        private readonly DbContextOptions<ServiceDbContext> _dbOptions;
        private readonly IBusControl _busMdm;
        private readonly IBusControl _busMs;
        private readonly IMapper _mapper;
        private readonly RabbitMQOption _rabbitMqOption;

        public Service(StatelessServiceContext context,
             DiagnosticPipeline diagnosticPipeline)
             : base(context, diagnosticPipeline)
        {
            var connectionString = Context.GetOption<DbOption>("ConnectionStrings").ServiceDbConnection;
            _dbOptions = CreateDbOptions(connectionString);
            _busMs = CreateMsBus(Context);
            (_busMdm, _rabbitMqOption) = CreateMdmBus(this.Context);
            _mapper = CreateMapper();
        }

        private (IBusControl, RabbitMQOption) CreateMdmBus(ServiceContext serviceContext)
        {
            var option = serviceContext.GetOption<RabbitMQOption>("RabbitMQ_mdm");

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(option.HostAddress), h =>
                {
                    h.Username(option.Username);
                    h.Password(option.Password);
                });
                cfg.ReceiveEndpoint(host, "EAP_EnterpriseContactService", c =>
                {
                    c.Consumer(() => new MdmDataConsumer(_dbOptions, _mapper));
                    c.Consumer(() => new OrgEventDataConsumer(_mapper, _dbOptions, _busMs));
                });

            });
            return (bus, option);
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

            });
            return bus;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {

            return new[]
            {
                new ServiceInstanceListener(_ => new MassTransitListener(_busMdm), "masstransit_mdm"),
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context,
                    new  DepartmentAppService(context,_dbOptions,_mapper)),ListenerName_DepartmentAppService),
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context,
                    new  PositionAppService(context,_dbOptions,_mapper)),ListenerName_PositionAppService),
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context,
                    new  EmployeeAppService(context,_dbOptions,_mapper)),ListenerName_EmployeeAppService),
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context,
                    new  GroupAppService(context,_dbOptions,_mapper,
                    AppsOnSF.Common.BaseServices.RemotingProxyFactory.CreateSimpleKeyValueService(),
                    _busMs)),
                    ListenerName_GroupAppService)
            };
        }

        private static DbContextOptions<ServiceDbContext> CreateDbOptions(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<ServiceDbContext>()
                //.UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
            return builder.Options;
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMappingProfile>();
            });
            return config.CreateMapper();
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            using (var db = new ServiceDbContext(_dbOptions))
            {
                await db.Database.MigrateAsync();
            }
        }
    }
}
