using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceFabric.Mocks;
using System.Fabric;
using System.Threading;

namespace UnitTestCommon
{
    public abstract class ManagerTestBase
    {
        protected MockReliableStateManager stateManager = new MockReliableStateManager();

        protected MockActorStateManager actorStateManager = new MockActorStateManager();

        protected IActorStateManager actorStateManagerFactory(ActorBase actorBase, IActorStateProvider actorStateProvider)
            => actorStateManager;

        protected static ICodePackageActivationContext codePackageContext = MockCodePackageActivationContext.Default;

        protected static StatefulServiceContext statefulServiceContext = MockStatefulServiceContextFactory.Default;

        protected static StatelessServiceContext statelessServiceContext = MockStatelessServiceContextFactory.Default;

        protected CancellationToken token => CancellationToken.None;
    }
}
