namespace HyperMsg.Broker.Data.Tables
{
    internal abstract class TableBase
    {
        internal abstract void Build(TransactionSession session);
    }
}