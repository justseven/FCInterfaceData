 
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace H2FTP
{
    public class DBHelper
    {
        private static DbConnection connection;
        // private string connectionstring = @"Provider=OraOLEDB.Oracle;Data Source=192.168.0.103/orcl;User ID=bdcsxk;Password=bdcsxk;Unicode=True";

        public static DbConnection Connection
        {
            get
            {

                if (connection == null)
                {
#if DEBUG
                    // string connectionstring = @"Provider=OraOLEDB.Oracle;Data Source=10.3.10.21/oradata;User ID=bdcsxk;Password=sn_bdcsxk;Unicode=True";
                    //string connectionstring = @"Provider=OraOLEDB.Oracle;Data Source=localhost/orcl;User ID=bdcsxk;Password=bdcsxk;Unicode=True";
                    string connectionstring = ConfigurationManager.ConnectionStrings["oracleConnection"].ToString();
                    connection = new OleDbConnection(connectionstring);
#else 
                    XConnNode node = XConnNode.CreateNode();
                    string connectionstring = node.ConnString["属性数据"];
                    DbProviderFactory provider = DbProviderFactories.GetFactory(node.ProviderName["属性数据"]);
                    connection = provider.CreateConnection();
                    connection.ConnectionString = connectionstring;
#endif
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();

                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        /// <summary>
        /// 本类中使用新建的connection ，记得要关闭
        /// </summary>
        /// <returns></returns>
        private static DbConnection NewAConnection()
        {
#if DEBUG
            string connectionstring = ConfigurationManager.ConnectionStrings["oracleConnection"].ToString();
            DbConnection connection = new OleDbConnection(connectionstring);
#else 
                    XConnNode node = XConnNode.CreateNode();
                    string connectionstring = node.ConnString["属性数据"];
                    DbProviderFactory provider = DbProviderFactories.GetFactory(node.ProviderName["属性数据"]);
                    DbConnection connection = provider.CreateConnection();
                    connection.ConnectionString = connectionstring;
#endif
            return connection;
        }
        public static int ExecuteTransaction(IList<DbCommand> commands)
        {
            if (!commands.Any())
            {//无数据--擦
                return 0;
            }
            DbTransaction Tran = Connection.BeginTransaction();
            int rowNum = 0;
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
#if DEBUG
                throw new Exception(ex.Message);
#else
                ILog log = new ErrorLog(typeof(DBHelper));
                log.WriteLog(ex);
                throw new Exception(ex.Message);
#endif
                Tran.Rollback();
            }
            finally
            {
                Connection.Close();
            }
            return rowNum;
        }

        /// <summary>
        ///  执行Sql语句,获取DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {
            DataSet dataset = new DataSet();
            using (DbConnection connection = NewAConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand com = connection.CreateCommand();
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    DbDataAdapter da = CreateDataAdapter(connection);
                    da.SelectCommand = com;
                    da.Fill(dataset);
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw new Exception(ex.Message);
#else
                    ILog log = new ErrorLog(typeof(DBHelper));
                    log.WriteLog(ex);
                    throw new Exception(ex.Message);
#endif
                }
                finally
                {
                    connection.Close();
                }
            }
            return dataset.Tables[0];
        }
        public static object GetScalar(string sql)
        {
            object ret = null;
            using (DbConnection connection = NewAConnection())
            {
                try
                {
                    connection.Open();
                    DbCommand com = connection.CreateCommand();
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    ret = com.ExecuteScalar();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw new Exception(ex.Message);
#else
                    ILog log = new ErrorLog(typeof(DBHelper));
                    log.WriteLog(ex);
                    throw new Exception(ex.Message);
#endif
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }
        private static DbDataAdapter CreateDataAdapter(DbConnection connection)
        {
            if (connection is System.Data.SqlClient.SqlConnection)
                return new System.Data.SqlClient.SqlDataAdapter();
            else if (connection is OleDbConnection)
                return new OleDbDataAdapter();
            throw new NotImplementedException();
        }

        public static int ExecuteNonQuery(string sql)
        {
            int ret = 0;
            using (DbConnection connection = NewAConnection())
            {

                try
                {
                    connection.Open();
                    DbCommand com = connection.CreateCommand();
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    ret = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw new Exception(ex.Message);
#else
                    ILog log = new ErrorLog(typeof(DBHelper));
                    log.WriteLog(ex);
                    throw new Exception(ex.Message);
#endif
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }
        public static int ExecuteNonQuery(string sql, DbConnection _connection)
        {
            int ret = 0;
            using (DbConnection connection = _connection)
            {

                try
                {
                    connection.Open();
                    DbCommand com = connection.CreateCommand();
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    ret = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw new Exception(ex.Message);
#else
                    ILog log = new ErrorLog(typeof(DBHelper));
                    log.WriteLog(ex);
                    throw new Exception(ex.Message);
#endif
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }
        public static int ExecuteNonQuery(string sql, DbCommand com)
        {
            int ret = 0;
            using (DbConnection connection = com.Connection)
            {

                try
                {
                    connection.Open();
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;
                    ret = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw new Exception(ex.Message);
#else
                    ILog log = new ErrorLog(typeof(DBHelper));
                    log.WriteLog(ex);
                    throw new Exception(ex.Message);
#endif
                }
                finally
                {
                    connection.Close();
                }
            }
            return ret;
        }
    }
}
