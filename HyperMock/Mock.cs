using System;
using System.Reflection;

namespace HyperMock.Universal
{
    /// <summary>
    /// Entry point for creating proxies of interfaces.
    /// </summary>
    public static class Mock
    {
        /// <summary>
        /// Creates a proxy from a template interface.
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <returns>Proxy instance</returns>
        public static T Create<T>()
        {
            return DispatchProxy.Create<T, MockProxyDispatcher>();
        }

        /// <summary>
        /// Creates a proxy from an interface type.
        /// </summary>
        /// <param name="type">Interface type</param>
        /// <returns>Proxy instance</returns>
        public static object Create(Type type)
        {
            var generatorType = typeof(DispatchProxy).GetTypeInfo().Assembly.GetType("System.Reflection.DispatchProxyGenerator");

            var method = generatorType.GetMethod("CreateProxyInstance", BindingFlags.NonPublic | BindingFlags.Static);

            return method.Invoke(null, new object[] { typeof(MockProxyDispatcher), type });
        }
    }
}
