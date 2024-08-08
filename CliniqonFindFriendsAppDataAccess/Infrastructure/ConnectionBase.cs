
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Infrastructure
{
    public abstract class ConnectionBase : IDbConnection
    {
        readonly string injectedConnectionString;
        public ConnectionBase(string connectionString)
        {
            this.injectedConnectionString = connectionString;
        }

        protected ConnectionBase(IDbConnection connection)
        {
            Connection = connection;
        }

        private IDbConnection Connection { get; set; }

        // Verbose but necessary implementation of IDbConnection:
        #region "IDbConnection implementation"

        public string ConnectionString
        {
            get
            {
                return Connection.ConnectionString;
            }

            set
            {
                Connection.ConnectionString = value;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return Connection.ConnectionTimeout;
            }
        }

        public string Database
        {
            get
            {
                return Connection.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return Connection.State;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Connection.BeginTransaction(isolationLevel);
        }

        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            Connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return Connection.CreateCommand();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public void Open()
        {
            Connection.Open();
        }

        #endregion

        protected IDbConnection GetConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("conneciton string is null or empty");

            return new SqlConnection(connectionString);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                return GetConnection(Connection.ConnectionString);
            }
            else
            {
                if (Connection == null)
                    throw new ArgumentNullException("conneciton unable to inject");

                return Connection;

            }



        }

    }
}
