using System;

namespace HyperMsg.Broker.Data
{
    /// <summary>
    /// Defines the members of the IDatabaseFactory interface.
    /// </summary>
    public interface IDatabaseFactory : IDisposable
    {
        /// <summary>
        /// Creates the database.
        /// </summary>
        void Create();

        /// <summary>
        /// Opens a transactioned session.
        /// </summary>
        /// <returns>Session</returns>
        TransactionSession OpenSession();
    }
}