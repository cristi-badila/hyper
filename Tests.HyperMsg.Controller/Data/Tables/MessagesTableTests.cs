using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HyperMsg.Controller;
using HyperMsg.Controller.Data;
using HyperMsg.Controller.Data.Entities;
using NUnit.Framework;

namespace Tests.HyperMsg.Controller.Data.Tables
{
    [TestFixture]
    public class MessagesTableTests : TestBase<DatabaseManager>
    {
        [SetUp]
        public override void BeforeEachTest()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
            MockFor<IConfigSettings>().Setup(s => s.DatabasePath).Returns(path);

            base.BeforeEachTest();

            Subject.Build();
        }

        [TearDown]
        public void AfterEachTest()
        {
            Subject.Dispose();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        [Test]
        public void CanSaveAndRetriveMessage()
        {
            var id = Guid.NewGuid();

            using (var session = Subject.OpenSession())
            {
                var entity = new MessageEntity {MessageId = id, Body = "<Body/>"};
                Subject.Messages.Add(session, entity);
                session.Complete();
            }

            using (var session = Subject.OpenSession())
            {
                var entity = Subject.Messages.Get(session, id);
                Assert.That(entity, Is.Not.Null);
                Assert.That(entity.Body, Is.EqualTo("<Body/>"));
            }
        }

        [Test]
        public void CanDeleteMessage()
        {
            var id = Guid.NewGuid();

            using (var session = Subject.OpenSession())
            {
                var entity = new MessageEntity { MessageId = id, Body = "<Body/>" };
                Subject.Messages.Add(session, entity);
                session.Complete();
            }

            using (var session = Subject.OpenSession())
            {
                Subject.Messages.Remove(session, id);
                session.Complete();
            }

            using (var session = Subject.OpenSession())
            {
                var entity = Subject.Messages.Get(session, id);
                Assert.That(entity, Is.Null);
            }
        }

        [Test]
        public void CanGetNextNMessages()
        {
            var idList = new List<Guid>();

            for (var i = 0; i < 20; i++)
            {
                idList.Add(Guid.NewGuid());
            }

            using (var session = Subject.OpenSession())
            {
                foreach (var id in idList)
                {
                    var entity = new MessageEntity { MessageId = id, Body = "<Body/>"};
                    Subject.Messages.Add(session, entity);
                }

                session.Complete();
            }

            using (var session = Subject.OpenSession())
            {
                var entities = Subject.Messages.Get(session, 10).ToList();
                Assert.That(entities.Count, Is.EqualTo(10));
                idList.Take(10).ToList().ForEach(id => Assert.That(entities.Any(e => e.MessageId == id), Is.True));
            }
        }

        [Test]
        public void CanGetNextNMessagesThatExist()
        {
            var idList = new List<Guid>();

            for (var i = 0; i < 5; i++)
            {
                idList.Add(Guid.NewGuid());
            }

            using (var session = Subject.OpenSession())
            {
                foreach (var id in idList)
                {
                    var entity = new MessageEntity { MessageId = id, Body = "<Body/>" };
                    Subject.Messages.Add(session, entity);
                }

                session.Complete();
            }

            using (var session = Subject.OpenSession())
            {
                var entities = Subject.Messages.Get(session, 10).ToList();
                Assert.That(entities.Count, Is.EqualTo(idList.Count));
                idList.ForEach(id => Assert.That(entities.Any(e => e.MessageId == id), Is.True));
            }
        }

        [Test]
        public void CanGetNextNMessagesNoneExist()
        {
            using (var session = Subject.OpenSession())
            {
                var entities = Subject.Messages.Get(session, 10).ToList();
                Assert.That(entities.Count, Is.EqualTo(0));
            }
        }
    }
}
