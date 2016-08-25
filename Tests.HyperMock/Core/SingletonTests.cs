namespace HyperMock.Universal.Tests.Core
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void Instance_Always_ReturnsAnInstanceOfTheSingletonClass()
        {
            var instance = TestClass.Instance;

            Assert.IsInstanceOfType(instance, typeof(TestClass));
        }

        [TestMethod]
        public void Instance_SubsequentCalls_ReturnTheSameInstanceOfTheSingletonClass()
        {
            var instance1 = TestClass.Instance;
            var instance2 = TestClass.Instance;

            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public async Task Instance_Always_IsThreadSafe()
        {
            var tasks = Enumerable.Range(0, 100000).Select(index => Task.Run(() => TestClass.Instance));

            var testInstances = await Task.WhenAll(tasks);

            Assert.IsTrue(testInstances.All(testInstance => testInstance != null));
        }

        private class TestClass : Singleton<TestClass>
        {
        }
    }
}
