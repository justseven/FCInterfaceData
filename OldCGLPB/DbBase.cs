using Oracle.DataAccess.Client;
using System.Data.Common;
using System.Data.SqlClient;

namespace OldCGLPB
{
    public class DbBase : IDbBase
    {



        string dbType = string.Empty;
        public DbBase(MyDBType mydbtype)
        {
            switch (mydbtype)
            {
                case MyDBType.Access:
                    dbType = "access";
                    break;
                case MyDBType.Sql:
                    dbType = "sql";
                    break;
                case MyDBType.Oracle:
                    dbType = "oracle";
                    break;
                case MyDBType.Other:
                    dbType = "other";
                    break;
                default :
                    dbType = "msoracle";
                    break;
            }
        }
        DbCommand IDbBase.CreateCommand()
        {
            if (dbType.ToLower() == "oracle")
            {
                return new Oracle.DataAccess.Client.OracleCommand();
            }
            if (dbType.ToLower()=="access")
            {
                return new System.Data.OleDb.OleDbCommand();
            }
            if(dbType.ToLower()=="sql")
            {
                return new SqlCommand();
            }
            if (dbType.ToLower() == "other")
            {
                return new SqlCommand();
            }
            if (dbType.ToLower() == "msoracle")
            {
                return new OracleCommand();
            }
            return null;
        }
        DbConnection IDbBase.CreateConnection()
        {
            if (dbType.ToLower() == "oracle") {
                return new Oracle.DataAccess.Client.OracleConnection();
            }
            if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbConnection();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlConnection();
            }
            if (dbType.ToLower() == "other")
            {
                return new SqlConnection();
            }
            if (dbType.ToLower() == "msoracle")
            {
                return new OracleConnection();
            }
            return null;
        }

        DbConnection IDbBase.CreateConnection(string connStr)
        {
            if (dbType.ToLower() == "oracle")
            {
                return new Oracle.DataAccess.Client.OracleConnection(connStr);
            }
            if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbConnection(connStr);
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlConnection(connStr);
            }
            if (dbType.ToLower() == "other")
            {
                return new SqlConnection(connStr);
            }
            if (dbType.ToLower() == "msoracle")
            {
                return new OracleConnection(connStr);
            }
            return null;
        }


        DbDataAdapter IDbBase.CreateDataAdapter()
        {
            if (dbType.ToLower() == "oracle") {
                return new Oracle.DataAccess.Client.OracleDataAdapter();
            }
                if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbDataAdapter();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlDataAdapter();
            }
            if (dbType.ToLower() == "other")
            {
                return new SqlDataAdapter();
            }
            if (dbType.ToLower() == "msoracle")
            {
                return new OracleDataAdapter();
            }
            return null;
        }
        DbParameter IDbBase.CreateParameter()
        {
            if (dbType.ToLower() == "oracle") {
                return new Oracle.DataAccess.Client.OracleParameter();
            }
                if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbParameter();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlParameter();
            }
            if (dbType.ToLower() == "other")
            {
                return new SqlParameter();
            }
            if (dbType.ToLower() == "msoracle")
            {
                return new OracleParameter();
            }
            return null;
        }


    }
}
