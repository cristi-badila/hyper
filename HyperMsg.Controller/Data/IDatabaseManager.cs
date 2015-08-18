using System;

namespace HyperMsg.Controller.Data
{
    /// <summary>
    /// Defines the members of the IDatabaseManager interface.
    /// </summary>
    public interface IDatabaseManager : IDisposable
    {
        /// <summary>
        /// Builds the database.
        /// </summary>
        void Build();

        /// <summary>
        /// Opens a transactioned session.
        /// </summary>
        /// <returns>Session</returns>
        TransactionSession OpenSession();
    }
}