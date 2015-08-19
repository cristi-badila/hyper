using System.Linq;
using HyperMsg;
using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Broker.Services;
using Moq;
using NUnit.Framework;
using Tests.HyperMsg.Broker.Support;

namespace Tests.HyperMsg.Broker
{
    [TestFixture]
    public class MessageServiceTests : TestBase<MessageService>
    {
        [Test]
        public void PostAddsMessageToDatabase()
        {
            var message = new BrokeredMessage();
            message.SetBody(new User { Forename = "Homer", Surname = "Simpson" });

            Subject.Post(message);

            MockFor<IMessageRepository>().Verify(r => r.Add(
                It.Is<MessageEntity>(e => e.MessageId == message.Id)), Times.Once);
        }

        [TestCase("0")]
        [TestCase("-1")]
        [TestCase("1")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetReturnsSingleMessage(string count)
        {
            MockFor<IMessageRepository>().Setup(r => r.Get("test", 1)).Returns(new[] {new MessageEntity()});

            var entities = Subject.Get("test", count);

            Assert.That(entities.Count(), Is.EqualTo(1));
        }
    }
}
