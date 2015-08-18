using HyperMsg.Broker;
using NUnit.Framework;

namespace Tests.HyperMsg.Broker
{
    [TestFixture]
    public class BrokerManagerTests : TestBase<BrokerManager>
    {
        [Test]
        public void InitReturnsSelf()
        {
            var broker = Subject.Init();
            Assert.That(broker, Is.SameAs(Subject));
        }
    }
}
