using HyperMsg;
using HyperMsg.Providers;
using NUnit.Framework;
using Tests.HyperMsg.Support;

namespace Tests.HyperMsg.Providers
{
    [TestFixture]
    public class RemoteMessageProviderTests
    {
        private RemoteMessageProvider _provider;

        [SetUp]
        public void BeforeEachTest()
        {
            _provider = new RemoteMessageProvider();
        }

        [TearDown]
        public void AfterEachTest()
        {
            _provider.Dispose();
        }

        //[Test]
        //public void Todo()
        //{
        //    var message = new BrokeredMessage();
        //    message.SetBody(new User { Forename = "Homer", Surname = "Simpson" });
        //    _provider.Send(message);
        //} 
    }
}
