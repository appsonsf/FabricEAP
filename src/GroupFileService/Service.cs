using AutoMapper;
using GroupFile.Models;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabricContrib;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using static Common.Constants;

namespace GroupFile
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatelessService
    {
        private readonly DiagnosticPipeline _diagnosticPipeline;
        private string _dbConnectString;
        private readonly IMapper _mapper;
        private DbContextOptions<ServiceDbContext> _dbOptions;

        public Service(StatelessServiceContext context,
           DiagnosticPipeline diagnosticPipeline)
            : base(context)
        {
            _diagnosticPipeline = diagnosticPipeline;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = config.CreateMapper();
            this._dbConnectString = Context.GetOption<DBOption>("ConnectionStrings").ServiceDbConnection;
            _dbOptions = CreateDbOptions(_dbConnectString);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => new FabricTransportServiceRemotingListener(context,
                    new ControlAppService(context,_dbOptions,this._mapper)),ListenerName_ControlAppService),
            };
        }
        
        private static DbContextOptions<ServiceDbContext> CreateDbOptions(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<ServiceDbContext>()
                   .UseSqlServer(connectionString);
            return builder.Options;
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
