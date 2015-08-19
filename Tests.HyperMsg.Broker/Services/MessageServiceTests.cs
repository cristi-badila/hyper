using System;
using System.Collections.Generic;
using System.Linq;
using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using HyperMsg.Broker.Services;
using HyperMsg.Messages;
using Moq;
using NUnit.Framework;
using Tests.HyperMsg.Broker.Support;

namespace Tests.HyperMsg.Broker.Services
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

        [Test]
        public void GetReturnsMultipleMessage()
        {
            MockFor<IMessageRepository>().Setup(r => r.Get("test", 2)).Returns(
                new[] { new MessageEntity(), new MessageEntity() });

            var entities = Subject.Get("test", "2");

            Assert.That(entities.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetReturnsMaxMultipleMessages()
        {
            var entities = new List<MessageEntity>();
            for (var i=0; i<100; i++) entities.Add(new MessageEntity());
            MockFor<IMessageRepository>().Setup(r => r.Get("test", 100)).Returns(entities);

            var messages = Subject.Get("test", "101");

            Assert.That(messages.Count(), Is.EqualTo(100));
        }

        [Test]
        public void AcknowledgeRemovesMessages()
        {
            var message = new Acknowledgement {MessageIds = new[] {Guid.NewGuid(), Guid.NewGuid()}};

            Subject.Acknowledge(message);

            MockFor<IMessageRepository>().Verify(r => r.Remove(message.MessageIds), Times.Once);
        }
    }
}
