using MassTransit;
using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;

namespace Common
{
    public class ActorServiceEapBase : ActorService, IActorService, IService
    {
        public ActorServiceEapBase(StatefulServiceContext context,
            ActorTypeInformation actorTypeInfo,
            DiagnosticPipeline diagnosticPipeline,
            Func<ActorService, ActorId, ActorBase> actorFactory = null,
            Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
            IActorStateProvider stateProvider = null, ActorServiceSettings settings = null)
            : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
            CreateSerilog(diagnosticPipeline);
        }

        private void CreateSerilog(DiagnosticPipeline diagnosticPipeline)
        {
            var option = diagnosticPipeline == null
                ? new DiagnosticsOption { SerilogEventLevel = Serilog.Events.LogEventLevel.Debug }
                : Context.GetOption<DiagnosticsOption>("Diagnostics");

            var logConfig = new LoggerConfiguration()
                          .MinimumLevel.Is(option.SerilogEventLevel)
                          .WriteTo.Debug();
            if (diagnosticPipeline != null)
                logConfig.WriteTo.EventFlow(diagnosticPipeline);
            Log.Logger = logConfig.CreateLogger();
        }
    }
}
