using System;
using System.Linq;
using HyperIoC.Universal;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Tests.HyperIoC.Universal.Support;

namespace Tests.HyperIoC.Universal
{
    [TestClass]
    public class FactoryTests
    {
        private Factory _factory;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _factory = new Factory();
        }

        [TestMethod]
        public void AddThrowsExceptionNullInterfaceType()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _factory.Add(null, typeof (TestClass)));
        }

        [TestMethod]
        public void AddThrowsExceptionNullConcreteType()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _factory.Add(typeof(ITestClass), null));
        }

        [TestMethod]
        public void AddThrowsExceptionForInvalidInterfaceType()
        {
            Assert.ThrowsException<ArgumentException>(() => _factory.Add< TestClass, TestClass>());
        }

        [TestMethod]
        public void AddThrowsExceptionForInvalidConcreteType()
        {
            Assert.ThrowsException<ArgumentException>(() => _factory.Add<ITestClass, ITestClass>());
        }

        [TestMethod]
        public void AddRegistersSelf()
        {
            var resolver = _factory.Get<IFactoryResolver>();

            Assert.IsNotNull(resolver);
        }

        [TestMethod]
        public void AddRegistersType()
        {
            var item = _factory.Add<ITestClass, TestClass>();

            Assert.IsTrue(item.InstanceTypes.ContainsKey(typeof(TestClass).FullName));
        }

        [TestMethod]
        public void AddRegistersTypeWithKey()
        {
            var item = _factory.Add<ITestClass, TestClass>("test-class");

            Assert.IsTrue(item.InstanceTypes.ContainsKey("test-class"));
        }

        [TestMethod]
        public void AddRegistersTypeAsSingleton()
        {
            var item = _factory.Add<ITestClass, TestClass>();

            item.AsSingleton();

            var manager = item.InstanceTypes[typeof (TestClass).FullName].LifetimeManager;
            Assert.IsInstanceOfType(manager, typeof(SingletonLifetimeManager));
        }
        
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void GetReturnsInstance(string key)
        {
            _factory.Add<ITestClass, TestClass>(key);

            var instance = _factory.Get<ITestClass>();

            Assert.IsInstanceOfType(instance, typeof(TestClass));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void GetReturnsInstanceFromAbstractDefinition(string key)
        {
            _factory.Add<ITestClass, YetAnotherTestClass>();
            _factory.Add<AbstractTestClass, TestClass2>(key);

            var instance = _factory.Get<ITestClass>();

            Assert.IsInstanceOfType(instance, typeof(YetAnotherTestClass));
        }

        [TestMethod]
        public void GetReturnsInstanceWithKey()
        {
            _factory.Add<ITestClass, TestClass>("test-class");

            var instance = _factory.Get(typeof(ITestClass), "test-class");

            Assert.IsInstanceOfType(instance, typeof(TestClass));
        }

        [TestMethod]
        public void GetReturnsSingletonInstance()
        {
            _factory.Add<ITestClass, TestClass>().AsSingleton();

            var instance1 = _factory.Get(typeof(ITestClass));
            var instance2 = _factory.Get(typeof(ITestClass));

            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public void GetReturnsTransientInstance()
        {
            _factory.Add<ITestClass, TestClass>();

            var instance1 = _factory.Get(typeof(ITestClass));
            var instance2 = _factory.Get(typeof(ITestClass));

            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void GetReturnsNullInstanceWithUnknownKey()
        {
            _factory.Add<ITestClass, TestClass>("test-class");

            var instance = _factory.Get(typeof(ITestClass), "unknown-test-class");

            Assert.IsNull(instance);
        }

        [TestMethod]
        public void GetAllReturnsAllInstances()
        {
            _factory.Add<ITestClass, TestClass>();
            _factory.Add<ITestClass, AnotherTestClass>();

            var instances = _factory.GetAll<ITestClass>();

            Assert.IsTrue(instances.Any(i => i.GetType() == typeof(TestClass)));
            Assert.IsTrue(instances.Any(i => i.GetType() == typeof(AnotherTestClass)));
        }
    }
}
