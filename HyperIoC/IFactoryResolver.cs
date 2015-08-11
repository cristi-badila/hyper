using System;

namespace HyperIoC
{
    /// <summary>
    /// Defines the members of the IFactoryResolver interface.
    /// </summary>
    public interface IFactoryResolver
    {
        TInterface Get<TInterface>(string key = null);
        object Get(Type interfaceType, string key = null);
    }
}