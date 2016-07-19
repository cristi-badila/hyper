using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HyperMock.Universal
{
    /// <summary>
    ///     Helper base class that provides automatic initialization of mock dependencies.
    /// </summary>
    /// <typeparam name="TSubject">Class under test</typeparam>
    public abstract class TestBase<TSubject>
    {
        private readonly Dictionary<Type, IMock> _mocks = new Dictionary<Type, IMock>();

        protected TestBase()
        {
            var ctor = typeof(TSubject).GetConstructors().First();

            foreach (var ctorParam in ctor.GetParameters())
            {
                var mock = Mock.Create(ctorParam.ParameterType);
                _mocks.Add(ctorParam.ParameterType, mock);
            }

            Subject = (TSubject) ctor.Invoke(_mocks.Values.Select(mock => mock.Object).ToArray());
        }

        protected TSubject Subject { get; private set; }

        protected Mock<TInterface> MockFor<TInterface>() where TInterface : class
        {
            if (_mocks.ContainsKey(typeof(TInterface)))
                return (Mock<TInterface>) _mocks[typeof(TInterface)];

            throw new InvalidOperationException("Cannot find mock for type: " + typeof(TInterface).FullName);
        }
    }
}