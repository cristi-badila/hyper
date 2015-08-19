using System;
using System.Linq;
using HyperMsg;
using HyperMsg.Exceptions;
using HyperMsg.Messages;
using HyperMsg.Providers;
using NUnit.Framework;
using Tests.HyperMsg.Support;

namespace Tests.HyperMsg.Providers
{
    [TestFixture]
    public class InMemoryMessageProviderTests : TestBase<InMemoryMessageProvider>
    {
        [TearDown]
        public void AfterEachTest()
        {
            Subject.Dispose();
        }

        [Test]
        public void SendThrowsExceptionWithNullMessage()
        {
            Assert.That(() => Subject.Send((Message)null),Throws.InstanceOf<ArgumentNullException>());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendThrowsExceptionWithNoMessageEndPoint(string endPoint)
        {
            var message = new BrokeredMessage {EndPoint = endPoint};
            message.SetBody(new User { Forename = "Homer", Surname = "Simpson" });

            var exception = Assert.Throws<MessageException>(() => Subject.Send(new BrokeredMessage()));

            Assert.That(exception.Message, Is.EqualTo("Message does not contain a valid end point."));
        }

        [Test]
        public void ReceiveReturnsEndPointMessages()
        {
            Subject.Send(new BrokeredMessage {EndPoint = "test2"});
            Subject.Send(new BrokeredMessage { EndPoint = "test" });
            Subject.Send(new BrokeredMessage { EndPoint = "test2" });

            var messages = Subject.Receive<BrokeredMessage>("test2", 2).ToList();

            Assert.That(messages.Count, Is.EqualTo(2));
            Assert.That(messages.All(m => m.EndPoint == "test2"), Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ReceiveThrowsExceptionWithInvalidEndPoint(string endPoint)
        {
            Assert.That(() => Subject.Receive<BrokeredMessage>(endPoint), Throws.ArgumentException);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(101)]
        public void ReceiveThrowsExceptionWithInvalidCount(int count)
        {
            Assert.That(() => Subject.Receive<BrokeredMessage>("test", count), 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
