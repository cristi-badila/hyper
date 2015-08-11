using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace HyperIoC.Mvc
{
    /// <summary>
    /// Extends the IFactoryBuilder to register controllers.
    /// </summary>
    public static class FactoryBuilderExtensions
    {
        /// <summary>
        /// Adds all the IController instances in the assembly.
        /// </summary>
        /// <param name="builder">Builder to add to</param>
        /// <param name="assembly">Assembly to load from</param>
        public static void AddControllers(this IFactoryBuilder builder, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(t => typeof(IController).IsAssignableFrom(t)))
            {
                builder.Add(typeof (IController), type, type.Name);
            }
        }
    }
}