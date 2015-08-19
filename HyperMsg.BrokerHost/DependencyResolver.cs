using System;
using HyperIoC;
using HyperMsg.Broker;

namespace HyperMsg.BrokerHost
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IFactoryResolver _resolver;

        public DependencyResolver(IFactoryResolver resolver)
        {
            _resolver = resolver;
        }

        public TInterface Get<TInterface>(string key = null)
        {
            return _resolver.Get<TInterface>(key);
        }

        public object Get(Type interfaceType, string key = null)
        {
            return _resolver.Get(interfaceType, key);
        }
    }
}
