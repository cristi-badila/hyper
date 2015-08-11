using System;

namespace HyperIoC
{
    /// <summary>
    /// Defines the members of the IFactoryBuilder interface.
    /// </summary>
    public interface IFactoryBuilder
    {
        Item Add<TInterface, TType>(string key = null) where TType : TInterface;
        Item Add(Type interfaceType, Type instanceType, string key = null);
    }
}