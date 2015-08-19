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

        public MessageEntity Get(Guid id)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                Api.JetSetCurrentIndex(session.SessionId, table, "UIX_MESSAGEID");
                Api.MakeKey(session.SessionId, table, id, MakeKeyGrbit.NewKey);

                if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                {
                    return Create(session.SessionId, table);
                }

                return null;
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

                updater.Save();
                session.Complete();
            }
        }

        public void Remove(params Guid[] ids)
        {
            using (var session = _databaseFactory.OpenSession())
            using (var table = new Table(session.SessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                foreach (var id in ids)
                {
                    Api.JetSetCurrentIndex(session.SessionId, table, "UIX_MESSAGEID");
                    Api.MakeKey(session.SessionId, table, id, MakeKeyGrbit.NewKey);

                    if (Api.TrySeek(session.SessionId, table, SeekGrbit.SeekEQ))
                    {
                        Api.JetDelete(session.SessionId, table);
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

            return entity;
        }
    }
}
