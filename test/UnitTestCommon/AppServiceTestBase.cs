using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.ServiceFabric.Data;
using ServiceFabric.Mocks;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Numerics;
using System.Text;
using System.Threading;
using MassTransit;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UnitTestCommon
{
    public abstract class AppServiceTestBase
    {
        protected MockReliableStateManager stateManager =  new MockReliableStateManager();

        protected MockActorStateManager actorStateManager = new MockActorStateManager();

        protected IActorStateManager actorStateManagerFactory(ActorBase actorBase, IActorStateProvider actorStateProvider) 
            => actorStateManager;

        protected static ICodePackageActivationContext codePackageContext = MockCodePackageActivationContext.Default;

        protected static StatefulServiceContext statefulServiceContext = MockStatefulServiceContextFactory.Default;

        protected static StatelessServiceContext statelessServiceContext = MockStatelessServiceContextFactory.Default;

        protected CancellationToken token => CancellationToken.None;

        protected static readonly LoggerFactory TestLoggerFactory
            = new LoggerFactory(new[] { new DebugLoggerProvider() });

        protected static IBusControl CreateBus(Action<IInMemoryReceiveEndpointConfigurator> action)
        {
            var needToDisposeBus = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("UnitTestQueue", action);
            });
            needToDisposeBus.Start();
            return needToDisposeBus;
        }
    }
}
