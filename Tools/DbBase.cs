using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Config;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Configuration;

namespace Config
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
            }
        }
        DbCommand IDbBase.CreateCommand()
        {
            if(dbType.ToLower()=="access")
            {
                return new System.Data.OleDb.OleDbCommand();
            }
            if(dbType.ToLower()=="sql")
            {
                return new SqlCommand();
            }
            return new OracleCommand();
        }
        DbConnection IDbBase.CreateConnection()
        {
            if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbConnection();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlConnection();
            }
            return new OracleConnection();
        }
        DbDataAdapter IDbBase.CreateDataAdapter()
        {
            if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbDataAdapter();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlDataAdapter();
            }
            return new OracleDataAdapter();
        }
        DbParameter IDbBase.CreateParameter()
        {
            if (dbType.ToLower() == "access")
            {
                return new System.Data.OleDb.OleDbParameter();
            }
            if (dbType.ToLower() == "sql")
            {
                return new SqlParameter();
            }
            return new OracleParameter();
        }


    }
}
