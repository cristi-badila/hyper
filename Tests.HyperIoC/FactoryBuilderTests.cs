using HyperIoC;
using NUnit.Framework;
using Tests.HyperIoC.Support;

namespace Tests.HyperIoC
{
    [TestFixture]
    public class FactoryBuilderTests
    {
        [Test]
        public void CreateBuildsFactoryForAnyConfig()
        {
            var factory = FactoryBuilder
                .Build()
                .WithProfile<AnyProfile>()
                .Create();

            var instance = factory.Get<ITestConfig>();

            Assert.That(instance, Is.InstanceOf<TestConfig>());
        }
        
        [Test]
        public void CreateBuildsDebugFactory()
        {
            const string setting = "DEBUG";

            var factory = FactoryBuilder
                .Build()
                .WithProfile<DebugProfile>(() => setting == "DEBUG")
                .WithProfile<ReleaseProfile>(() => setting == "RELEASE")
                .Create();

            var instance = factory.Get<ITestClass>();

            Assert.That(instance,Is.InstanceOf<TestClass>());
        }

        [Test]
        public void CreateBuildsReleaseFactory()
        {
            const string setting = "RELEASE";

            var factory = FactoryBuilder
                .Build()
                .WithProfile<DebugProfile>(() => setting == "DEBUG")
                .WithProfile<ReleaseProfile>(() => setting == "RELEASE")
                .Create();

            var instance = factory.Get<ITestClass>();

            Assert.That(instance, Is.InstanceOf<AnotherTestClass>());
        }

        [Test]
        public void CreateBuildsFactoryFromAnother()
        {
            var originalFactory = FactoryBuilder
                .Build()
                .WithProfile<AnyProfile>()
                .Create();
            var factory = FactoryBuilder
                .Build(originalFactory)
                .WithProfile<DebugProfile>(() => originalFactory.Get<ITestConfig>().Config == "DEBUG")
                .WithProfile<ReleaseProfile>(() => originalFactory.Get<ITestConfig>().Config == "RELEASE")
                .Create();

            var instance = factory.Get<ITestClass>();

            Assert.That(instance, Is.InstanceOf<AnotherTestClass>());
        }
    }
}
