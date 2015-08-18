using System;

namespace HyperMsg.Broker.Data
{
    /// <summary>
    /// Defines the members of the IConnectionProvider interface.
    /// </summary>
    public interface IConnectionProvider : IDisposable
    {
        /// <summary>
        /// Opens a transactioned session.
        /// </summary>
        /// <returns>Session</returns>
        TransactionSession OpenSession();
    }
}