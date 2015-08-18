using System;
using System.IO;
using System.Linq;
using HyperMsg.Controller.Data.Tables;
using Microsoft.Isam.Esent.Interop;

namespace HyperMsg.Controller.Data
{
    public class DatabaseManager : IDatabaseManager
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
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }

        internal MessagesTable Messages { get; private set; }

        public TransactionSession OpenSession()
        {
            return new TransactionSession(new Session(_instance), _databasePath);
        }

        public void Build()
        {
            using (var session = OpenSession())
            {
                var existingTables = Api.GetTableNames(session.SessionId, session.DatabaseId).ToList();

                Messages = new MessagesTable();
                
                if (existingTables.All(t => t != MessagesTable.TableName)) Messages.Build(session);
                
                session.Complete();
            }
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
    }
}
