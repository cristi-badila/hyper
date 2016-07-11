using System;
using System.Reflection;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Entry point for creating proxies of interfaces.
    /// </summary>
    public static class Mock
    {
        private static readonly MethodInfo CreateProxyMethod;

        static Mock()
        {
            CreateProxyMethod = typeof(DispatchProxy).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        ///     Creates a proxy from a template interface.
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <returns>Proxy instance</returns>
        public static T Create<T>()
        {
            return DispatchProxy.Create<T, MockProxyDispatcher>();
        }

        /// <summary>
        ///     Creates a proxy from an interface type.
        /// </summary>
        /// <param name="type">Interface type</param>
        /// <returns>Proxy instance</returns>
        public static object Create(Type type)
        {
            return CreateProxyMethod.MakeGenericMethod(type, typeof(MockProxyDispatcher)).Invoke(null, null);
        }
    }
}