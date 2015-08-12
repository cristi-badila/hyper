using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace HyperIoC.Wcf
{
    /// <summary>
    /// Extends the IFactoryBuilder to register services.
    /// </summary>
    public static class FactoryBuilderExtensions
    {
        /// <summary>
        /// Adds all the service contract instances in the assembly.
        /// </summary>
        /// <param name="builder">Builder to add to</param>
        /// <param name="assembly">Assembly to load from</param>
        public static void AddServices(this IFactoryBuilder builder, Assembly assembly)
        {
            foreach (var contractType in assembly.GetTypes().Where(IsServiceContractInterface))
            {
                var serviceTypes = assembly.GetTypes().Where(t => ServiceImplementsContract(t, contractType)).ToList();

                if (!serviceTypes.Any()) continue;

                serviceTypes.ForEach(st => builder.Add(contractType, st));
            }
        }

        private static bool IsServiceContractInterface(Type type)
        {
            return type.IsInterface && type.GetCustomAttributes(typeof (ServiceContractAttribute), true).Any();
        }

        private static bool ServiceImplementsContract(Type serviceType, Type contractType)
        {
            return !serviceType.IsInterface && !serviceType.IsAbstract && contractType.IsAssignableFrom(serviceType);
        }
    }
}