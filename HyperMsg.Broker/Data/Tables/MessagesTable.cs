using Microsoft.Isam.Esent.Interop;

namespace HyperMsg.Broker.Data.Tables
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

            Api.JetAddColumn(session.SessionId, tableId, "EndPoint",
                new JET_COLUMNDEF
                {
                    coltyp = JET_coltyp.Text,
                    cp = JET_CP.Unicode,
                    grbit = ColumndefGrbit.None
                }, null, 0, out columnid);

            Api.JetAddColumn(session.SessionId, tableId, "Persistent",
                new JET_COLUMNDEF
                {
                    coltyp = JET_coltyp.Bit,
                    cp = JET_CP.Unicode,
                    grbit = ColumndefGrbit.None
                }, null, 0, out columnid);

            const string idIndex = "+Id\0\0";
            Api.JetCreateIndex(session.SessionId, tableId, "IDX_ID",
                CreateIndexGrbit.IndexPrimary, idIndex, idIndex.Length, 100);

            const string msgIdIndex = "+MessageId\0\0";
            Api.JetCreateIndex(session.SessionId, tableId, "UIX_MESSAGEID",
                CreateIndexGrbit.IndexUnique, msgIdIndex, msgIdIndex.Length, 100);

            const string endPointIndex = "+EndPoint\0\0";
            Api.JetCreateIndex(session.SessionId, tableId, "IDX_ENDPOINT",
                CreateIndexGrbit.None, endPointIndex, endPointIndex.Length, 100);
        }
    }
}