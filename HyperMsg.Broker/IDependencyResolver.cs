using System;

namespace HyperMsg.Broker
{
    public interface IDependencyResolver
    {
        TInterface Get<TInterface>(string key = null);
        object Get(Type interfaceType, string key = null);
    }
}
