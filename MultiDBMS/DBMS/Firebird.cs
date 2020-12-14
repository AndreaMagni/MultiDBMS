using System;
using FirebirdSql.Data.FirebirdClient;

namespace MultiDBMS.DBMS
{
    public class Firebird
    {
        private string connectionString = null;
        private static FbConnection connection = null;
        private static FbTransaction transaction = null;

        public Firebird(string conn)
        {
            this.connectionString = conn;
        }

        public bool OpenConnection()
        {
            try
            {
                connection = new FbConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                connection.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
            transaction.Dispose();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
            transaction.Dispose();
        }

    }
}
