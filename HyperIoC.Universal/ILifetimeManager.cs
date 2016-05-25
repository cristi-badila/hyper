using System;

namespace HyperIoC.Universal
{
    /// <summary>
    /// Defines the members of the ILifetimeManager interface.
    /// </summary>
    public interface ILifetimeManager
    {
        object Get(Type type, IFactoryLocator locator, IFactoryResolver resolver);
    }
}