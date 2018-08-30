//#if !DEBUG
//using Geo.Core;
//#endif
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Plug.DataExchange.XZFCPlug.Dal
{

    public class New_DbHelper
    {
        [ThreadStatic] 
        private static ConnectType connectType=ConnectType.SXK;

        public static void SetConnectType(ConnectType t)
        {
            New_DbHelper.connectType=t;
        }
        private static object LockO = new object();
        public static DbConnection Connection
        {
            get
            { 
                    string connectionstring=string.Empty;
                    if(New_DbHelper.connectType==ConnectType.SXK) 
                        connectionstring = ConfigurationManager.ConnectionStrings["bdcsxkConnection"].ToString();
                    else
                        connectionstring = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
                    return  new Oracle.DataAccess.Client.OracleConnection(connectionstring);  
            }
        }
        /// <summary>
        /// 本类中使用新建的connection ，记得要关闭
        /// </summary>
        /// <returns></returns>
        private static DbConnection NewAConnection()
        { 
            DbConnection connection = null;
            if (New_DbHelper.connectType == ConnectType.SXK)
            {
                string connectionstring = ConfigurationManager.ConnectionStrings["bdcsxkConnection"].ToString();
                connection = new Oracle.DataAccess.Client.OracleConnection(connectionstring);
            }
            else
            {
                string connectionstring =  ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
                connection = new Oracle.DataAccess.Client.OracleConnection(connectionstring);
            } 
            return connection;
        } 
        public static int ExecuteTransaction(IList<DbCommand> commands)
        {
            if (!commands.Any())
            {//无数据--擦
                return 0;
            }
            int rowNum = 0;
            using (DbConnection connection = Connection) {
                connection.Open();
                DbTransaction Tran = connection.BeginTransaction(); 
                try
                {
                    foreach (var command in commands)
                    {
                        command.Transaction = Tran;
                        command.ExecuteNonQuery();
                        rowNum++;
                    }
                    Tran.Commit();
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            } 
            return rowNum;
        }

        private static void ExecuteCommand(string sql)
        {
            using (DbConnection connection = NewAConnection())
            {
                connection.Open();
                DbCommand com = connection.CreateCommand();
                com.CommandText = sql;
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
            }
        }

        public static void ExecuteCommand(string sql, ConnectType ConnectType)
        {
            lock (LockO) {
                New_DbHelper.SetConnectType(ConnectType);
                New_DbHelper.ExecuteCommand(sql);
            } 
        }
        /// <summary>
        ///  执行Sql语句,获取DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        private  static DataTable GetDataTable(string sql)
        {
            DataSet dataset = new DataSet();
            using (DbConnection connection = NewAConnection())
            {
                connection.Open();
                DbCommand com = connection.CreateCommand();
                com.CommandText = sql;
                com.CommandType = CommandType.Text;
                DbDataAdapter da = CreateDataAdapter(connection);
                da.SelectCommand = com;
                da.Fill(dataset);
            }
            return dataset.Tables[0];
        }
        public static DataTable GetDataTable(string sql, ConnectType ConnectType)
        {
            lock (LockO)
            {
                New_DbHelper.SetConnectType(ConnectType);
                return DBHelper.GetDataTable(sql);
            }
        }
        private static DataSet GetDataSet(string sql)
        {
            DataSet dataset = new DataSet();
            using (DbConnection connection = NewAConnection())
            {
                connection.Open();
                DbCommand com = connection.CreateCommand();
                com.CommandText = sql;
                com.CommandType = CommandType.Text;
                DbDataAdapter da = CreateDataAdapter(connection);
                da.SelectCommand = com;
                da.Fill(dataset);
            }
            return dataset;
        }
        public static DataSet GetDataSet(string sql, ConnectType ConnectType)
        {
            lock (LockO)
            {
                New_DbHelper.SetConnectType(ConnectType);
                return New_DbHelper.GetDataSet(sql);
            }
        }

        private static object GetScalar(string sql)
        {
            object ret = null;
            using (DbConnection connection = NewAConnection())
            {
                connection.Open();
                DbCommand com = connection.CreateCommand();
                com.CommandText = sql;
                com.CommandType = CommandType.Text;
                ret = com.ExecuteScalar();
            }
            return ret;
        }
        public static object GetScalar(string sql, ConnectType ConnectType)
        {
            lock (LockO)
            {
                New_DbHelper.SetConnectType(ConnectType);
                return DBHelper.GetScalar(sql);
            }
        }
        private static DbDataAdapter CreateDataAdapter(DbConnection connection)
        {
            if (connection is System.Data.SqlClient.SqlConnection)
                return new System.Data.SqlClient.SqlDataAdapter();
            else if (connection is OleDbConnection)
                return new OleDbDataAdapter();
            else if (connection is Oracle.DataAccess.Client.OracleConnection)
                return new Oracle.DataAccess.Client.OracleDataAdapter();
            throw new NotImplementedException();
        }
    }

    public enum ConnectType
    {
       GGK,SXK
    }
}
