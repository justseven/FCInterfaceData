using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    internal class SQLDBHelper
    {
        //打开连接
        private SqlConnection con = null;
        //命令
        private SqlCommand cmd = null;
        private static SQLDBHelper dbh = null;
        private SQLDBHelper()
        {
            //连接配置文件,
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["wbstrCon"].ToString());
        }
        //单例实例化DBHelper 节约内存
        public static SQLDBHelper CreateInstance()
        {
            if (dbh == null)
            {
                dbh = new SQLDBHelper();
            }
            return dbh;
        }
        //单数据操作  执行增 删 改
        public bool EditSql(string strsql)
        {
            cmd = new SqlCommand(strsql, con);
            try
            {
                con.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
        //查询 返回DataTable
        public DataTable GetTable(string strsql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
            sda.Fill(ds);
            return ds.Tables[0];
        }


        //非断开式读取  查询
        public SqlDataReader SelectReader(string strsql)
        {
            cmd = new SqlCommand(strsql, con);
            try
            {
                con.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                return null;
            }
        }

        //多数据操作 事务  单向   删除 修改
        public bool EditSqlLst(List<string> sqls)
        {
            //事务声明
            SqlTransaction tran = null;
            //命令对象
            cmd = new SqlCommand();
            cmd.Connection = con;
            try
            {
                con.Open();
                //连接打开后才能开始事物
                tran = con.BeginTransaction();
                //转交事务
                cmd.Transaction = tran;
                //批量处理
                foreach (string sql in sqls)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                //提交
                tran.Commit();
                return true;
            }
            catch
            {
                //回滚
                tran.Rollback();
                return false;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
