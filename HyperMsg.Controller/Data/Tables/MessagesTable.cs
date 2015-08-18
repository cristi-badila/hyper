using System;
using System.Collections.Generic;
using System.Text;
using HyperMsg.Controller.Data.Entities;
using Microsoft.Isam.Esent.Interop;

namespace HyperMsg.Controller.Data.Tables
{
    internal class MessagesTable : TableBase
    {
        internal const string TableName = "Messages";

        internal override void Build(TransactionSession session)
        {
            JET_TABLEID tableId;

            Api.JetCreateTable(session.SessionId, session.DatabaseId, TableName, 1, 100, out tableId);

            JET_COLUMNID columnid;
            Api.JetAddColumn(session.SessionId, tableId, "Id",
                new JET_COLUMNDEF
                {
                    coltyp = JET_coltyp.Long,
                    grbit = ColumndefGrbit.ColumnFixed | ColumndefGrbit.ColumnAutoincrement | ColumndefGrbit.ColumnNotNULL
                }, null, 0, out columnid);

            Api.JetAddColumn(session.SessionId, tableId, "MessageId",
                new JET_COLUMNDEF
                {
                    cbMax = 16,
                    coltyp = JET_coltyp.Binary,
                    grbit = ColumndefGrbit.ColumnFixed | ColumndefGrbit.ColumnNotNULL
                }, null, 0, out columnid);

            Api.JetAddColumn(session.SessionId, tableId, "Body",
                new JET_COLUMNDEF
                {
                    coltyp = JET_coltyp.LongText,
                    cp = JET_CP.Unicode,
                    grbit = ColumndefGrbit.None
                }, null, 0, out columnid);

            const string idIndex = "+Id\0\0";
            Api.JetCreateIndex(session.SessionId, tableId, "IDX_ID",
                CreateIndexGrbit.IndexPrimary, idIndex, idIndex.Length, 100);

            const string msgIdIndex = "+MessageId\0\0";
            Api.JetCreateIndex(session.SessionId, tableId, "UIX_MESSAGEID",
                CreateIndexGrbit.IndexUnique, msgIdIndex, msgIdIndex.Length, 100);
        }

        public void Add(TransactionSession session, MessageEntity messageEntity)
        {
            var sessionId = session.SessionId;

            using (var table = new Table(sessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            using (var updater = new Update(sessionId, table, JET_prep.Insert))
            {
                var columnDesc = Api.GetTableColumnid(sessionId, table, "MessageId");
                Api.SetColumn(sessionId, table, columnDesc, messageEntity.MessageId);

                columnDesc = Api.GetTableColumnid(sessionId, table, "Body");
                Api.SetColumn(sessionId, table, columnDesc, messageEntity.Body, Encoding.Unicode);

                updater.Save();
            }
        }

        public MessageEntity Get(TransactionSession session, Guid id)
        {
            var sessionId = session.SessionId;

            using (var table = new Table(sessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                Api.JetSetCurrentIndex(sessionId, table, "UIX_MESSAGEID");
                Api.MakeKey(sessionId, table, id, MakeKeyGrbit.NewKey);

                if (Api.TrySeek(sessionId, table, SeekGrbit.SeekEQ))
                {
                    return Create(sessionId, table);
                }

                return null;
            }
        }

        public IEnumerable<MessageEntity> Get(TransactionSession session, int count)
        {
            var sessionId = session.SessionId;

            using (var table = new Table(sessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                var entities = new List<MessageEntity>();

                if (Api.TryMoveFirst(sessionId, table))
                {
                    do
                    {
                        entities.Add(Create(sessionId, table));
                    }
                    while (Api.TryMoveNext(sessionId, table) && entities.Count < count);
                }

                return entities;
            }
        }

        public void Remove(TransactionSession session, Guid id)
        {
            var sessionId = session.SessionId;

            using (var table = new Table(sessionId, session.DatabaseId, TableName, OpenTableGrbit.None))
            {
                Api.JetSetCurrentIndex(sessionId, table, "UIX_MESSAGEID");
                Api.MakeKey(sessionId, table, id, MakeKeyGrbit.NewKey);

                if (Api.TrySeek(sessionId, table, SeekGrbit.SeekEQ))
                {
                    Api.JetDelete(sessionId, table);
                }
            }
        }

        private static MessageEntity Create(Session sessionId, JET_TABLEID table)
        {
            var entity = new MessageEntity();

            var columnId = Api.GetTableColumnid(sessionId, table, "Id");
            entity.Id = Api.RetrieveColumnAsInt32(sessionId, table, columnId) ?? -1;

            var columnMessageId = Api.GetTableColumnid(sessionId, table, "MessageId");
            entity.MessageId = Api.RetrieveColumnAsGuid(sessionId, table, columnMessageId) ?? Guid.Empty;

            var columnDesc = Api.GetTableColumnid(sessionId, table, "Body");
            entity.Body = Api.RetrieveColumnAsString(sessionId, table, columnDesc, Encoding.Unicode);

            return entity;
        }
    }
}