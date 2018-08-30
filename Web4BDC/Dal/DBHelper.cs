using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb; 
using System.Linq;
using System.Text;

namespace Web4BDC.Dal
{
    public class DBHelper
    { 
        /// <summary>
        /// 获得一个唯一的CONNECTION 实例
        /// </summary>
        public static OracleConnection Connection
        {
            get
            {  
                    //string connectionstring = @"Provider=OraOLEDB.Oracle.1;Data Source=orcl;User ID=bdcsxkcs;Password=bdcsxkcs;Unicode=True"; 
                    string connectionstring = ConfigurationManager.ConnectionStrings["bdcsxkConnection"].ToString();
                    return new  OracleConnection(connectionstring); 
                
            }
        } 
        /// <summary>
        /// 返回执行SQL 语句所影响数据的行数
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <returns>影响行数</returns>
        public static int ExecuteCommand(string sql)
        {
            using (OracleConnection connection = Connection) {
                connection.Open();
                OracleCommand com = new OracleCommand(sql, connection);
                int result = com.ExecuteNonQuery();
                connection.Close();
                return result;
            } 
        }
        /// <summary>
        /// 获取结果集的第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetScalar(string sql)
        {
            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleCommand com = new OracleCommand(sql, connection);
                int result = int.Parse(com.ExecuteScalar().ToString());
                connection.Close();
                return result;
            }
        }

        public static object GetScalar_Object(string sql)
        {
            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleCommand com = new OracleCommand(sql, connection);
                object result = com.ExecuteScalar();
                connection.Close();
                return result;
            }
        }
        public static int GetScalar(string connectionStr,string sql)
        {
            using (OracleConnection Connection = new OracleConnection(connectionStr))
            {
                Connection.Open();
                OracleCommand com = new OracleCommand(sql, Connection);
                int result = int.Parse(com.ExecuteScalar().ToString());
                return result; 
            } 
        }
        /// <summary>
        ///  执行Sql语句,获取DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {

            using (OracleConnection connection = Connection)
            {
                connection.Open();
                DataSet dataset = new DataSet();
                OracleCommand com = new OracleCommand(sql, connection);
                OracleDataAdapter da = new OracleDataAdapter(com);
                da.Fill(dataset);
                connection.Close();
                return dataset.Tables[0];
            }
        }

        /// <summary>
        ///  执行Sql语句,获取DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string ConnectionStr,string sql)
        {
            using (OracleConnection Connection = new OracleConnection(ConnectionStr))
            {
                Connection.Open();
                DataSet dataset = new DataSet();
                OracleCommand com = new OracleCommand(sql, Connection);
                OracleDataAdapter da = new OracleDataAdapter(com);
                da.Fill(dataset);
                return dataset.Tables[0];
            }
        }
        /// <summary>
        /// 执行Sql语句,获取DataSet结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql)
        { 
            using (OracleConnection connection = Connection)
            {
                connection.Open();
                DataSet dataset = new DataSet();
                OracleCommand com = new OracleCommand(sql, connection);
                OracleDataAdapter da = new OracleDataAdapter(com);
                da.Fill(dataset);
                connection.Close();
                return dataset;
            }
        }

        public static DataSet GetDataSet(string ConnectionStr,string sql)
        {
            using (OracleConnection Connection = new OracleConnection(ConnectionStr))
            {
                Connection.Open();
                DataSet dataset = new DataSet();
                OracleCommand com = new OracleCommand(sql, Connection);
                OracleDataAdapter da = new OracleDataAdapter(com);
                da.Fill(dataset);
                return dataset;
            } 
        }
        /// <summary>
        /// 获取OleDbDataReader结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OracleDataReader GetReader(string sql)
        {

            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleCommand com = new OracleCommand(sql, connection);
                OracleDataReader reader = com.ExecuteReader();
                connection.Close();
                return reader;

            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>影响行数</returns>
        public static int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {

            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleCommand com = new OracleCommand(SQLString, connection);
                PrepareCommand(com, connection, null, SQLString, cmdParms);
                int rows = com.ExecuteNonQuery();
                com.Parameters.Clear();
                connection.Close();
                return rows;
            }
        }
        public static int ExecuteSql(string connectStr,string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection Connection = new OracleConnection(connectStr)) {
                Connection.Open();
              OracleCommand com = new OracleCommand(SQLString, Connection);
              PrepareCommand(com, Connection, null, SQLString, cmdParms);
              int rows = com.ExecuteNonQuery();
              com.Parameters.Clear();
              return rows;
            }
        }
        public static int ExecuteTransaction(IList<OracleCommand> commands)
        {
            if (!commands.Any()) {//无数据--擦
                return 0;
            }
            int rowNum = 0;
            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleTransaction Tran = connection.BeginTransaction();
                
                try
                {

                    foreach (var command in commands)
                    {
                        command.Transaction = Tran;
                        command.Connection = connection;
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
        /// <summary>
        /// 获取DataSet结果集
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string SQLString, params OracleParameter[] cmdParms)
        {

            using (OracleConnection connection = Connection)
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand(SQLString, connection);
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    da.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
                return ds;
            } 
        }
        /// <summary>
        /// 新加的方法，可以将多个sql放在一个dateset中
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(IDictionary<string,string> sqls) {
            using (OracleConnection connection = Connection)
            {
                connection.Open();
                DataSet ds = new DataSet();
                foreach (var sql in sqls)
                {
                    if (!string.IsNullOrEmpty(sql.Value))
                    {
                        OracleDataAdapter ordAdapter = new OracleDataAdapter(sql.Value, connection);
                        ordAdapter.Fill(ds, sql.Key);
                    }
                }
                return ds;
            }
        }
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
