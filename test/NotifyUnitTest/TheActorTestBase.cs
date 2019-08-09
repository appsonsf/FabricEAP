using MassTransit;
using Microsoft.Diagnostics.EventFlow;
using Microsoft.Diagnostics.EventFlow.ServiceFabric;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotifySessionActor;
using System;
using UnitTestCommon;

namespace NotifySessionActorTest
{
    [TestClass]
    public class TheActorTestBase : AppServiceTestBase
    {
        protected TheActorService CreateActorService(IBusControl bus)
        {
            //var actorName = typeof(TheActor).FullName;
            //var assembly = typeof(TheActor).Assembly;
            //var actorType = assembly.GetType(actorName);
            var actorTypeInformation = ActorTypeInformation.Get(typeof(TheActor));
            var service = new TheActorService(
                statefulServiceContext,
                actorTypeInformation,
                null,
                bus,
                null,
                actorStateManagerFactory
                );
            return service;
        }
    }
}
