using AppsOnSF.Common.Options;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;
using ServiceFabricContrib;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;

namespace Common
{
    public abstract class StatelessServiceEapBase : StatelessService
    {
        public StatelessServiceEapBase(StatelessServiceContext context,
            DiagnosticPipeline diagnosticPipeline)
            : base(context)
        {
            CreateSerilog(diagnosticPipeline);
        }

        private void CreateSerilog(DiagnosticPipeline diagnosticPipeline)
        {
            var option = Context.GetOption<DiagnosticsOption>("Diagnostics");

            Log.Logger = new LoggerConfiguration()
                          .MinimumLevel.Is(option.SerilogEventLevel)
                          .WriteTo.Debug()
                          .WriteTo.EventFlow(diagnosticPipeline)
                          .CreateLogger();
        }
    }
}
