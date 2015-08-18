using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;

namespace Tests.HyperMsg.Broker
{
    public abstract class TestBase<TSubject>
    {
        private readonly Dictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();
        private readonly ConstructorInfo _ctor;

        protected TestBase()
        {
            _ctor = typeof (TSubject).GetConstructors().First();

            foreach (var ctorParam in _ctor.GetParameters())
            {
                var mockType = typeof(Mock<>);
                Type[] typeArgs = { ctorParam.ParameterType };
                var genericMockType = mockType.MakeGenericType(typeArgs);
                var mock = (Mock)Activator.CreateInstance(genericMockType);
                _mocks.Add(ctorParam.ParameterType, mock);
            }
        }

        protected TSubject Subject { get; private set; }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            Subject = (TSubject)_ctor.Invoke(_mocks.Values.Select(m => m.Object).ToArray());
        }

        protected Mock<TInterface> MockFor<TInterface>() where TInterface : class
        {
            if (_mocks.ContainsKey(typeof(TInterface)))
                return (Mock<TInterface>)_mocks[typeof (TInterface)];

            throw new InvalidOperationException("Cannot find mock for type: " + typeof(TInterface).FullName);
        }
    }
}