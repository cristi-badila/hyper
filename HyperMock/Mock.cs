using System;
using System.Reflection;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Entry point for creating proxies of interfaces.
    /// </summary>
    public class Mock<T>
        where T : class
    {
        public T Object { get; }

        public MockProxyDispatcher Dispatcher { get; }

        public Mock()
            : this(DispatchProxy.Create<T, MockProxyDispatcher>() as MockProxyDispatcher)
        {
        }

        public Mock(MockProxyDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            Object = dispatcher as T;
        }
    }

    public static class Mock
    {
        /// <summary>
        ///     Creates a proxy from an interface type.
        /// </summary>
        /// <param name="type">Interface type</param>
        /// <returns>Proxy instance</returns>
        public static object Create(Type type)
        {
            var constructedType = typeof(Mock<>).MakeGenericType(type);
            return Activator.CreateInstance(constructedType, new object[] { });
        }

        /// <summary>
        ///     Creates a proxy from a template interface.
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <returns>Proxy instance</returns>
        public static Mock<T> Create<T>() where T : class
        {
            return new Mock<T>();
        }
    }
}