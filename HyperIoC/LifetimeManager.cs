using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperIoC
{
    /// <summary>
    /// Lifetime manager base class.
    /// </summary>
    public abstract class LifetimeManager : ILifetimeManager
    {
        public abstract object Get(Type type, IFactoryLocator locator, IFactoryResolver resolver);

        protected object CreateInstance(Type type, IFactoryLocator locator, IFactoryResolver resolver)
        {
            var ctors = type.GetConstructors().ToList();

            foreach (var ctor in ctors)
            {
                var ctorParams = new List<object>();

                foreach (var paramInfo in ctor.GetParameters())
                {
                    if (!paramInfo.ParameterType.IsInterface)
                    {
                        ctorParams.Clear();
                        break;
                    }

                    var item = locator.FindItem(paramInfo.ParameterType);

                    if (item == null)
                    {
                        ctorParams.Clear();
                        break;
                    }

                    var instance = resolver.Get(paramInfo.ParameterType);
                    ctorParams.Add(instance);
                }

                if (ctorParams.Count > 0)
                {
                    return Activator.CreateInstance(type, ctorParams.ToArray());
                }
            }

            if (ctors.Any(ctor => ctor.GetParameters().Length == 0)) return Activator.CreateInstance(type);

            throw new InvalidOperationException(BuildErrorMessage(type));
        }

        private static string BuildErrorMessage(Type type)
        {
            var ctors = type.GetConstructors().ToList();

            var message = new StringBuilder();
            message.AppendFormat("Cannot construct type '{0}':", type.FullName);
            message.AppendLine();

            foreach (var ctorInfo in ctors)
            {
                message.AppendLine(" Constructor...");

                foreach (var paramInfo in ctorInfo.GetParameters())
                {
                    message.AppendLine("  " + paramInfo.ParameterType.FullName);
                }
            }

            return message.ToString();
        }
    }
}