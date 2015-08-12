using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace HyperIoC.Wcf
{
    /// <summary>
    /// Defines ContainerServiceHostFactory class.
    /// </summary>
    public class HyperIoCServiceHostFactory : ServiceHostFactoryBase
    {
        private static IFactoryResolver _resolver;
        private static readonly Dictionary<string, Type> _registeredTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Registers a resolver with the factory.
        /// </summary>
        /// <param name="resolver">IoC factory resolver</param>
        public static void Register(IFactoryResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Creates an instance of the service host for creating services from the IoC.
        /// </summary>
        /// <param name="constructorString">Name of service</param>
        /// <param name="baseAddresses">List of addresses for service</param>
        /// <returns>Service host</returns>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            Type serviceType = null;

            // Check the registered types first. If we don't find it here then check all the loaded
            // assemblies.
            if (_registeredTypes.ContainsKey(constructorString))
            { 
                serviceType = _registeredTypes[constructorString];
            }
            else
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    serviceType = asm.GetType(constructorString, false);

                    if (serviceType != null) 
                    {
                        _registeredTypes.Add(constructorString, serviceType);
                        break; 
                    }
                }
            }

            if (serviceType == null)
                throw new InvalidOperationException("Unable to find service type: " + constructorString);

            // Create a service host with the resolver and service type.
            var host = new HyperIoCServiceHost(_resolver, serviceType, baseAddresses);
            return host;
        }
    }
}
