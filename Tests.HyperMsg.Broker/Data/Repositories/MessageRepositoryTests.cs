using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HyperMsg.Broker;
using HyperMsg.Broker.Config;
using HyperMsg.Broker.Data;
using HyperMsg.Broker.Data.Entities;
using HyperMsg.Broker.Data.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.HyperMsg.Broker.Data.Repositories
{
    [TestFixture]
    public class MessageRepositoryTests
    {
        private MessageRepository _repository;
        private DatabaseFactory _databaseFactory;

        [SetUp]
        public void BeforeEachTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            var configSettings = new Mock<IConfigSettings>();
            configSettings.Setup(s => s.DatabasePath).Returns(path);

            _databaseFactory = new DatabaseFactory(configSettings.Object);
            _databaseFactory.Create();

            _repository = new MessageRepository(_databaseFactory);
        }

        [TearDown]
        public void AfterEachTest()
        {
            _databaseFactory.Dispose();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [Test]
        public void CanSaveAndRetriveMessage()
        {
            var id = Guid.NewGuid();
            var entity = new MessageEntity {MessageId = id, Body = "<Body/>"};

            _repository.Add(entity);
            entity = _repository.Get(id);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Body, Is.EqualTo("<Body/>"));
        }

        [Test]
        public void CanDeleteMessage()
        {
            var id = Guid.NewGuid();
            var entity = new MessageEntity { MessageId = id, Body = "<Body/>" };

            _repository.Add(entity);
            _repository.Remove(id);
            entity = _repository.Get(id);

            Assert.That(entity, Is.Null);
        }

        [Test]
        public void CanGetNextNMessages()
        {
            var idList = new List<Guid>();

            for (var i = 0; i < 20; i++)
            {
                idList.Add(Guid.NewGuid());
            }

            foreach (var id in idList)
            {
                var entity = new MessageEntity { MessageId = id, Body = "<Body/>"};
                _repository.Add(entity);
            }

            var entities = _repository.Get(10).ToList();
            Assert.That(entities.Count, Is.EqualTo(10));
            idList.Take(10).ToList().ForEach(id => Assert.That(entities.Any(e => e.MessageId == id), Is.True));
        }

        [Test]
        public void CanGetNextNMessagesThatExist()
        {
            var idList = new List<Guid>();

            for (var i = 0; i < 5; i++)
            {
                idList.Add(Guid.NewGuid());
            }

            foreach (var id in idList)
            {
                var entity = new MessageEntity { MessageId = id, Body = "<Body/>" };
                _repository.Add(entity);
            }

            var entities = _repository.Get(10).ToList();
            Assert.That(entities.Count, Is.EqualTo(idList.Count));
            idList.ForEach(id => Assert.That(entities.Any(e => e.MessageId == id), Is.True));
        }

        [Test]
        public void CanGetNextNMessagesNoneExist()
        {
            var entities = _repository.Get(10).ToList();
            Assert.That(entities.Count, Is.EqualTo(0));
        }
    }
}
