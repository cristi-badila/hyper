namespace HyperMock.Universal
{
    using System;
    using System.Reflection;

    public static class Mock
    {
        /// <summary>
        ///     Creates a proxy from an interface type.
        /// </summary>
        /// <param name="type">Interface type</param>
        /// <returns>Proxy instance</returns>
        public static IMock Create(Type type)
        {
            var constructedType = typeof(Mock<>).MakeGenericType(type);
            return Activator.CreateInstance(constructedType, new object[] { }) as IMock;
        }

        /// <summary>
        ///     Creates a proxy from a template interface.
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <returns>Proxy instance</returns>
        public static Mock<T> Create<T>()
            where T : class
        {
            return new Mock<T>();
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    /// <summary>
    ///     Entry point for creating proxies of interfaces.
    /// </summary>
    public class Mock<T> : IMock
#pragma warning restore SA1402 // File may only contain a single class
        where T : class
    {
        public Mock()
            : this(DispatchProxy.Create<T, MockProxyDispatcher>() as MockProxyDispatcher)
        {
        }

        public Mock(MockProxyDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            Object = dispatcher as T;
        }

        public T Object { get; }

        public MockProxyDispatcher Dispatcher { get; }

        object IMock.Object => Object;
    }
}