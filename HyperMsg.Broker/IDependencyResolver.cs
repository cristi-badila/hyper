using System;

namespace HyperMsg.Broker
{
    /// <summary>
    /// Defines the members of the IDependencyResolver interface. Required by the WCF infrastructure.
    /// </summary>
    /// <remarks>
    /// You will need to provide an implementation using whatever IoC you are using.
    /// </remarks>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Gets an instance for the interface (and key if required).
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <param name="key">Optional key for the type to resolve.</param>
        /// <returns>Instance of interface type</returns>
        TInterface Get<TInterface>(string key = null);

        /// <summary>
        /// Gets an instance for the interface (and key if required).
        /// </summary>
        /// <param name="interfaceType">Interface type</param>
        /// <param name="key">Optional key for the type to resolve.</param>
        /// <returns>Instance of interface type</returns>
        object Get(Type interfaceType, string key = null);
    }
}
