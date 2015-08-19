using System;
using System.Collections.Generic;
using System.Text;
using HyperMsg.Broker.Data.Entities;
using Microsoft.Isam.Esent.Interop;

namespace HyperMsg.Broker.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private const string TableName = "Messages";

        public MessageRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public IEnumerable<MessageEntity> Get(params Guid[] messageIds)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                var entities = new List<MessageEntity>();

                foreach (var messageId in messageIds)
                {
                    Api.JetSetCurrentIndex(session.SessionId, table, "UIX_MESSAGEID");
                    Api.MakeKey(session.SessionId, table, messageId, MakeKeyGrbit.NewKey);

                    if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                    {
                        entities.Add(Create(session.SessionId, table));
                    }
                }

                return entities;
            }
        }

        public IEnumerable<MessageEntity> Get(string endpoint, int count)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                var entities = new List<MessageEntity>();

                Api.JetSetCurrentIndex(session.SessionId, table, "IDX_ENDPOINT");
                Api.MakeKey(session.SessionId, table, endpoint, Encoding.Unicode, MakeKeyGrbit.NewKey);

                if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                {
                    do
                    {
                        entities.Add(Create(session.SessionId, table));
                    }
                    while (Api.TryMoveNext(session.SessionId, table) && entities.Count < count);
                }

                return entities;
            }
        }

        public void Add(MessageEntity messageEntity)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            using (var updater = new Update(session.SessionId, table, JET_prep.Insert))
            {
                var columnDesc = Api.GetTableColumnid(session.SessionId, table, "MessageId");
                Api.SetColumn(session.SessionId, table, columnDesc, messageEntity.MessageId);

                columnDesc = Api.GetTableColumnid(session.SessionId, table, "Body");
                Api.SetColumn(session.SessionId, table, columnDesc, messageEntity.Body, Encoding.Unicode);

                columnDesc = Api.GetTableColumnid(session.SessionId, table, "EndPoint");
                Api.SetColumn(session.SessionId, table, columnDesc, messageEntity.EndPoint, Encoding.Unicode);

                columnDesc = Api.GetTableColumnid(session.SessionId, table, "Persistent");
                Api.SetColumn(session.SessionId, table, columnDesc, messageEntity.Persistent);

                columnDesc = Api.GetTableColumnid(session.SessionId, table, "RetryLimit");
                Api.SetColumn(session.SessionId, table, columnDesc, messageEntity.RetryLimit);

                columnDesc = Api.GetTableColumnid(session.SessionId, table, "RetryCount");
                Api.SetColumn(session.SessionId, table, columnDesc, (short)0);

                updater.Save();
                session.Complete();
            }
        }

        public void Remove(params Guid[] messageIds)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                foreach (var messageId in messageIds)
                {
                    Api.JetSetCurrentIndex(session.SessionId, table, "UIX_MESSAGEID");
                    Api.MakeKey(session.SessionId, table, messageId, MakeKeyGrbit.NewKey);

                    if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                    {
                        Api.JetDelete(session.SessionId, table);
                    }
                }

                session.Complete();
            }
        }

        public void Update(params MessageEntity[] entities)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                foreach (var entity in entities)
                {
                    Api.JetSetCurrentIndex(session.SessionId, table, "UIX_MESSAGEID");
                    Api.MakeKey(session.SessionId, table, entity.MessageId, MakeKeyGrbit.NewKey);

                    if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                    {
                        Api.JetPrepareUpdate(session.SessionId, table, JET_prep.Replace);
                        var columnDesc = Api.GetTableColumnid(session.SessionId, table, "RetryCount");
                        Api.SetColumn(session.SessionId, table, columnDesc, entity.RetryCount);
                        Api.JetUpdate(session.SessionId, table);
                    }
                }

                session.Complete();
            }
        }

        private static MessageEntity Create(Session sessionId, JET_TABLEID table)
        {
            var entity = new MessageEntity();

            var columnDesc = Api.GetTableColumnid(sessionId, table, "Id");
            entity.Id = Api.RetrieveColumnAsInt32(sessionId, table, columnDesc) ?? -1;

            columnDesc = Api.GetTableColumnid(sessionId, table, "MessageId");
            entity.MessageId = Api.RetrieveColumnAsGuid(sessionId, table, columnDesc) ?? Guid.Empty;

            columnDesc = Api.GetTableColumnid(sessionId, table, "Body");
            entity.Body = Api.RetrieveColumnAsString(sessionId, table, columnDesc, Encoding.Unicode);

            columnDesc = Api.GetTableColumnid(sessionId, table, "EndPoint");
            entity.EndPoint = Api.RetrieveColumnAsString(sessionId, table, columnDesc, Encoding.Unicode);

            columnDesc = Api.GetTableColumnid(sessionId, table, "Persistent");
            entity.Persistent = Api.RetrieveColumnAsBoolean(sessionId, table, columnDesc) ?? false;

            columnDesc = Api.GetTableColumnid(sessionId, table, "RetryLimit");
            entity.RetryLimit = Api.RetrieveColumnAsInt16(sessionId, table, columnDesc) ?? 5;

            columnDesc = Api.GetTableColumnid(sessionId, table, "RetryCount");
            entity.RetryCount = Api.RetrieveColumnAsInt16(sessionId, table, columnDesc) ?? 0;

            return entity;
        }
    }
}
