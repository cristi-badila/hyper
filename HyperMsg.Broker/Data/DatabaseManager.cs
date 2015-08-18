using System;
using System.IO;
using System.Linq;
using HyperMsg.Broker.Data.Tables;
using Microsoft.Isam.Esent.Interop;

namespace HyperMsg.Broker.Data
{
    public class DatabaseManager : IConnectionProvider
    {
        private readonly Instance _instance;
        private readonly string _databasePath;
        private bool _disposed;

        public DatabaseManager(IConfigSettings configSettings)
        {
            var instancePath = configSettings.DatabasePath;
            _databasePath = Path.Combine(instancePath, "database.mdb");
            _instance = new Instance(_databasePath);

            _instance.Parameters.CreatePathIfNotExist = true;
            _instance.Parameters.TempDirectory = Path.Combine(instancePath, "temp");
            _instance.Parameters.SystemDirectory = Path.Combine(instancePath, "system");
            _instance.Parameters.LogFileDirectory = Path.Combine(instancePath, "logs");
            _instance.Parameters.Recovery = true;
            _instance.Parameters.CircularLog = true;

            _instance.Init();

            using (var session = new Session(_instance))
            {
                if (!File.Exists(_databasePath))
                {
                    JET_DBID databaseId;
                    Api.JetCreateDatabase(session, _databasePath, null, out databaseId, CreateDatabaseGrbit.None);
                }
            }

            Build();
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }

        public TransactionSession OpenSession()
        {
            return new TransactionSession(new Session(_instance), _databasePath);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _instance.Close();
                _instance.Dispose();
            }

            _disposed = true;
        }

        private void Build()
        {
            using (var session = OpenSession())
            {
                var existingTables = Api.GetTableNames(session.SessionId, session.DatabaseId).ToList();

                if (existingTables.All(t => t != MessagesTable.TableName)) new MessagesTable().Build(session);

                session.Complete();
            }
        }
    }
}
