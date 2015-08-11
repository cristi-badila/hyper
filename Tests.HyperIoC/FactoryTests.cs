using System;
using System.Linq;
using HyperIoC;
using NUnit.Framework;
using Tests.HyperIoC.Support;

namespace Tests.HyperIoC
{
    [TestFixture]
    public class FactoryTests
    {
        private Factory _factory;

        [SetUp]
        public void BeforeEachTest()
        {
            _factory = new Factory();
        }

        [Test]
        public void AddThrowsExceptionNullInterfaceType()
        {
            Assert.That(() => _factory.Add(null, typeof(TestClass)), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddThrowsExceptionNullConcreteType()
        {
            Assert.That(() => _factory.Add(typeof(ITestClass), null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void AddThrowsExceptionForInvalidInterfaceType()
        {
            Assert.That(() => _factory.Add<TestClass, TestClass>(), Throws.ArgumentException);
        }

        [Test]
        public void AddThrowsExceptionForInvalidConcreteType()
        {
            Assert.That(() => _factory.Add<ITestClass, ITestClass>(), Throws.ArgumentException);
        }

        [Test]
        public void AddRegistersType()
        {
            var item = _factory.Add<ITestClass, TestClass>();

            Assert.That(item.InstanceTypes.ContainsKey(typeof(TestClass).FullName), Is.Not.Null);
        }

        [Test]
        public void AddRegistersTypeWithKey()
        {
            var item = _factory.Add<ITestClass, TestClass>("test-class");

            Assert.That(item.InstanceTypes.ContainsKey("test-class"), Is.Not.Null);
        }

        [Test]
        public void AddRegistersTypeAsSingleton()
        {
            var item = _factory.Add<ITestClass, TestClass>();

            item.AsSingleton();

            Assert.That(item.InstanceTypes[typeof(TestClass).FullName].LifetimeManager, Is.InstanceOf<SingletonLifetimeManager>());
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetReturnsInstance(string key)
        {
            _factory.Add<ITestClass, TestClass>(key);

            var instance = _factory.Get<ITestClass>();

            Assert.That(instance, Is.InstanceOf<TestClass>());
        }

        [Test]
        public void GetReturnsInstanceWithKey()
        {
            _factory.Add<ITestClass, TestClass>("test-class");

            var instance = _factory.Get(typeof(ITestClass), "test-class");

            Assert.That(instance, Is.InstanceOf<TestClass>());
        }

        [Test]
        public void GetReturnsSingletonInstance()
        {
            _factory.Add<ITestClass, TestClass>().AsSingleton();

            var instance1 = _factory.Get(typeof(ITestClass));
            var instance2 = _factory.Get(typeof(ITestClass));

            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void GetReturnsTransientInstance()
        {
            _factory.Add<ITestClass, TestClass>();

            var instance1 = _factory.Get(typeof(ITestClass));
            var instance2 = _factory.Get(typeof(ITestClass));

            Assert.That(instance1, Is.Not.SameAs(instance2));
        }

        [Test]
        public void GetReturnsNullInstanceWithUnknownKey()
        {
            _factory.Add<ITestClass, TestClass>("test-class");

            var instance = _factory.Get(typeof(ITestClass), "unknown-test-class");

            Assert.That(instance, Is.Null);
        }

        [Test]
        public void GetAllReturnsAllInstances()
        {
            _factory.Add<ITestClass, TestClass>();
            _factory.Add<ITestClass, AnotherTestClass>();

            var instances = _factory.GetAll<ITestClass>();

            Assert.That(instances.Any(i => i.GetType() == typeof(TestClass)), Is.True);
            Assert.That(instances.Any(i => i.GetType() == typeof(AnotherTestClass)), Is.True);
        }
    }
}
