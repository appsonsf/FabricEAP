using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ActorEapBase : Actor
    {
        protected ActorEapBase(
            ActorServiceEapBase actorService,
            ActorId actorId)
            : base(actorService, actorId)
        {
            ActorService = actorService;
        }

        public new ActorServiceEapBase ActorService { get; }
    }
}
