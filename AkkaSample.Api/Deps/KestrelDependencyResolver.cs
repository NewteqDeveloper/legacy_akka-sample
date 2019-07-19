using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Deps
{
    public class KestrelDependencyResolver : IDependencyResolver
    {
        private IServiceProvider serviceProvider;
        private ConcurrentDictionary<string, Type> typeCache;
        private ActorSystem actorSystem;

        public KestrelDependencyResolver(IServiceProvider serviceProvider, ActorSystem actorSystem)
        {
            this.serviceProvider = serviceProvider;

            this.actorSystem = actorSystem;
            this.actorSystem.AddDependencyResolver(this);

            this.typeCache = new ConcurrentDictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        }

        public Props Create<TActor>() where TActor : ActorBase
        {
            return this.actorSystem.GetExtension<DIExt>().Props(typeof(TActor));
        }

        public Props Create(Type actorType)
        {
            return this.actorSystem.GetExtension<DIExt>().Props(actorType);
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
        {
            return () => (ActorBase)this.serviceProvider.GetRequiredService(actorType);
        }

        public Type GetType(string actorName)
        {
            var value = actorName.GetTypeValue();

            if (value == null)
            {
                value = this.serviceProvider.GetServices<object>()
                .Where(service => service.GetType().Name.ToLower() == actorName.ToLower())
                .Select(service => service.GetType())
                .FirstOrDefault();
            }
            this.typeCache.TryAdd(actorName, value);

            return value;
        }

        public void Release(ActorBase actor)
        {
            // do nothing here I guess
        }
    }
}
