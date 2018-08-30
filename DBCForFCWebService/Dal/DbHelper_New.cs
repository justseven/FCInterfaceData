using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using Config;

namespace DBCForFCWebService
{
    
    public class DbHelper_New
    {
        
        public void SetTimeoutDefault()
        {
            Timeout = 30;
        }
        public  int Timeout = 30;

        public  IDbBase Provider = new DbBase(MyDBType.Oracle);
         string connstr = string.Empty;
        public  void SetProvider(MyDBType mydbtype)
        {
            Provider = new DbBase(mydbtype);
            
            switch (mydbtype)
            {
                case MyDBType.Access:
                    connstr = DBConfig.CmsAccessConString;
                    break;
                case MyDBType.Sql:
                    connstr = DBConfig.CmsSqlConString;
                    break;
                case MyDBType.Oracle:
                    connstr = DBConfig.CmsOracleConString;
                    break;
                case MyDBType.Other:
                    connstr = DBConfig.CmsConString;
                    break;
            }
        }


        public  string CreateInsertStr<T>(T t,string tableName,MyDBType mydbtype)
         {
             string tStr = "Insert into " + tableName + " ({0}) values({1})";
             string fuhao = "@";
            switch(mydbtype)
            {
                case MyDBType.Oracle:
                    fuhao = ":";
                    break;
            }
             if (t == null)
             {
                 return null;
             }
             System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
             if (properties.Length <= 0)
             {
                 return null;
             }
             string colstr = string.Empty;
             string valuestr = string.Empty;
             for (int i = 0; i < properties.Length; i++)
             {
                 string name = properties[i].Name; //����
                 object value = properties[i].GetValue(t, null);  //ֵ

                 //if (value != null)
                 //{

                     colstr += properties[i].Name + ",";

                     valuestr += fuhao + properties[i].Name + ",";

                     //if (i < properties.Length - 1)
                     //{
                     //    colstr += properties[i].Name + ",";

                     //    valuestr += fuhao + properties[i].Name + ",";
                     //}
                     //else
                     //{
                     //    colstr += properties[i].Name;
                     //    valuestr += fuhao + properties[i].Name;
                     //}
                // }
             }

             colstr = colstr.Substring(0, colstr.Length - 1);
             valuestr = valuestr.Substring(0, valuestr.Length - 1);
             return String.Format(tStr, colstr, valuestr);
                 
             
         }

        internal  void CreateConn()
        {
            Conn = Provider.CreateConnection(connstr);
        }

        public  string CreateInsertStr<T>(T t, MyDBType mydbtype)
        {
            string tStr = "Insert into " + t.GetType().ToString() + " ({0}) values({1})";
            string fuhao = "@";
            switch (mydbtype)
            {
                case MyDBType.Oracle:
                    fuhao = ":";
                    break;
            }
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            string colstr = string.Empty;
            string valuestr = string.Empty;
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ

                //if (value != null)
                //{

                    colstr += properties[i].Name + ",";

                    valuestr += fuhao + properties[i].Name + ",";

                    //if (i < properties.Length - 1)
                    //{
                    //    colstr += properties[i].Name + ",";

                    //    valuestr += fuhao + properties[i].Name + ",";
                    //}
                    //else
                    //{
                    //    colstr += properties[i].Name;
                    //    valuestr += fuhao + properties[i].Name;
                    //}
               // }
            }

            colstr = colstr.Substring(0, colstr.Length - 1);
            valuestr = valuestr.Substring(0, valuestr.Length - 1);
            return String.Format(tStr, colstr, valuestr);


        }

        public  string CreateSelectStr<T>(T t, string tableName, MyDBType mydbtype)
        {
            string tStr = "select * from  " + tableName + " where 1=1 {0}";
            string fuhao = "@";
            switch (mydbtype)
            {
                case MyDBType.Oracle:
                    fuhao = ":";
                    break;
            }
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            string colstr = string.Empty;
            string valuestr = string.Empty;
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ

                if (value != null)
                {
                    if (CanAdd(name, tableName))
                        colstr += " and " + properties[i].Name + "=" + fuhao + properties[i].Name;
                }
            }
            return String.Format(tStr, colstr);
        }

        public  string CreateSelectStr<T>(T t, MyDBType mydbtype)
        {
            string tStr = "select * from  " + t.GetType().ToString() + " where 1=1 {0}";
            string fuhao = "@";
            switch (mydbtype)
            {
                case MyDBType.Oracle:
                    fuhao = ":";
                    break;
            }
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            string colstr = string.Empty;
            string valuestr = string.Empty;
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ

                if (value != null)
                {
                    if (CanAdd(name, t.GetType().ToString()))
                        colstr += " and " + properties[i].Name + "=" + fuhao + properties[i].Name;
                }
            }
            return String.Format(tStr, colstr);
        }

        private  bool CanAdd(string name,string tablename)
        {
            string[] oteher = new string[] { "oldzcs", "oldszcs","ZCS" };
            string[] pname = new string[] { "Jzmj", "Tnjzmj", "Ftjzmj", "oldzcs", "oldszcs", "ZCS" };
            string[] tab = new string[] { "FC_H_QSDC"};

            if (tablename.ToLower() == "FC_H_QSDC".ToLower())
            {
                foreach (string item in pname)
                {
                    if (item.ToLower() == name.ToLower())
                        return false;
                }
            }
            else
            {
                foreach (string item in oteher)
                {
                    if (item.ToLower() == name.ToLower())
                        return false;
                }
            }
            return true;

        }

        public  DbParameter[] GetSelectParamArray<T>(T t, MyDBType mydbtype)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            System.Collections.Generic.List<DbParameter> list = new System.Collections.Generic.List<DbParameter>();
            DbParameter[] paramArray = null;
            int lenght = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ
                if (value != null)
                {
                    if (CanAdd(name, t.GetType().Name))
                        lenght++;
                }
            }
            switch (mydbtype)
            {
                case MyDBType.Access:
                    paramArray = new System.Data.OleDb.OleDbParameter[lenght];
                    break;
                case MyDBType.Sql:
                    paramArray = new System.Data.SqlClient.SqlParameter[lenght];
                    break;
                case MyDBType.Oracle:
                    paramArray = new Oracle.DataAccess.Client.OracleParameter[lenght];
                    break;
            }

            int index = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ
                string type = properties[i].PropertyType.Name;
                if (value != null )
                {
                    if (CanAdd(name, t.GetType().Name))
                    {
                        switch (mydbtype)
                        {
                            case MyDBType.Access:
                                paramArray[index] = new System.Data.OleDb.OleDbParameter("@" + name, value);
                                break;
                            case MyDBType.Sql:
                                paramArray[index] = new System.Data.SqlClient.SqlParameter("@" + name, value);
                                break;
                            case MyDBType.Oracle:
                                paramArray[index] = new Oracle.DataAccess.Client.OracleParameter(":" + name, value);
                                break;
                        }
                        index++;
                    }
                }
            }

            return paramArray;


        }

         public  DbParameter[] GetParamArray<T>(T t, MyDBType mydbtype)
         {
             string tStr = string.Empty;
             if (t == null)
             {
                 return null;
             }
             System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
             if (properties.Length <= 0)
             {
                 return null;
             }
            System.Collections.Generic.List<DbParameter> list=new System.Collections.Generic.List<DbParameter>();
             DbParameter[] paramArray=null;
             switch(mydbtype)
            {
                case MyDBType.Access:
                    paramArray=new System.Data.OleDb.OleDbParameter[properties.Length];
                    break;
                case MyDBType.Sql:
                    paramArray=new System.Data.SqlClient.SqlParameter[properties.Length];
                    break;
                case MyDBType.Oracle:
                    paramArray=new Oracle.DataAccess.Client.OracleParameter[properties.Length];
                    break;
            }

             for (int i = 0; i < properties.Length; i++)
             {
                 string name = properties[i].Name; //����
                 object value = properties[i].GetValue(t, null);  //ֵ
                 string type = properties[i].PropertyType.Name;
               
                    switch (mydbtype)
                    {
                        case MyDBType.Access:
                            if (null == value)
                            {
                                paramArray[i] = new System.Data.OleDb.OleDbParameter("@" + name, DBNull.Value);
                            }
                            else
                            {
                                paramArray[i] = new System.Data.OleDb.OleDbParameter("@" + name, value);
                            }
                            break;
                        case MyDBType.Sql:
                            if (null == value)
                            {
                                paramArray[i] = new System.Data.SqlClient.SqlParameter("@" + name, DBNull.Value);
                            }
                            else
                            {
                                paramArray[i] = new System.Data.SqlClient.SqlParameter("@" + name, value);
                            }
                            break;
                        case MyDBType.Oracle:
                            if (null==value)
                            {
                                paramArray[i] = new Oracle.DataAccess.Client.OracleParameter(":" + name, DBNull.Value);
                            }
                            else
                            {
                                paramArray[i] = new Oracle.DataAccess.Client.OracleParameter(":" + name, value);
                            }
                            break;
                    }
                
             }

             return paramArray;

            
         }

        public  DbConnection Conn = null;

        public  int ExecuteNonQuery(MyDBType mydbtype,CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            //string connstr=string.Empty;
            //switch(mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr=DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr=DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr=DBConfig.CmsOracleConString;
            //        break;
            //    case MyDBType.Other:
            //        connstr = DBConfig.CmsConString;
            //        break;
            //}
            return ExecuteNonQuery(connstr, cmdType, cmdText, commandParameters);
        }

        
        public  int ExecuteNonQuery(MyDBType mydbtype,DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            //string connstr = string.Empty;
            //switch (mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr = DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr = DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr = DBConfig.CmsOracleConString;
            //        break;
            //}
            //if(null==trans)
            //{
            //    DbConnection conn = Provider.CreateConnection();
            //    conn.ConnectionString = connstr;
            //    trans = conn.BeginTransaction();
            //}
            return ExecuteNonQuery(trans, cmdType, cmdText, commandParameters);
        }

        




        public  void Oracle_UseSqlBulkCopy(DataTable dt)
        {

            using (OracleBulkCopy bulkCopy = new OracleBulkCopy(DBConfig.CmsSqlConString))
            {


                try
                {

                    bulkCopy.DestinationTableName = dt.TableName;//Ҫ����ı��ı���  

                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Close the SqlDataReader. The SqlBulkCopy  
                    // object is automatically closed at the end  
                    // of the using block.  

                }
            }



        }


        /// <summary>
        /// SQLʹ��SqlBulkCopy��������
        /// </summary>
        /// <param name="mydbtype"></param>
        /// <param name="dt"></param>
        public  void SQL_UseSqlBulkCopy(DataTable dt)
        {
            
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(DBConfig.CmsSqlConString))
            {


                try
                {

                    bulkCopy.DestinationTableName = dt.TableName;//Ҫ����ı��ı���  

                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // Close the SqlDataReader. The SqlBulkCopy  
                    // object is automatically closed at the end  
                    // of the using block.  

                }
            }



        }

        //public  int ExecuteNonQuery(int connIndex,CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        //{
        //    if(connIndex==0)
        //    return ExecuteNonQuery(DBConfig.CmsConString, cmdType, cmdText, commandParameters);
        //    return ExecuteNonQuery(DBConfig.CmsConString_Check, cmdType, cmdText, commandParameters);
        //}

        public  int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

            DbCommand cmd = Provider.CreateCommand();

            using (DbConnection conn = Provider.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = connectionString;
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //DbHelper.Conn = conn;
                    return val;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();

                    }
                    if (null != Conn && Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }


                }
            }
        }

        public  int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {

            try
            {
                DbCommand cmd = Provider.CreateCommand();

                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                this.Conn = connection;
                return val;
            }
            catch { throw; }
            finally
            {
                if (null!= Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
                
            }

        }

        public  int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            try
            {
                DbCommand cmd = trans.Connection.CreateCommand();

                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                this.Conn = trans.Connection;
                return val;
            }
            catch { throw; }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        public  DbDataReader ExecuteReader(MyDBType mydbtype, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            //string connstr = string.Empty;
            //switch (mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr = DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr = DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr = DBConfig.CmsOracleConString;
            //        break;
            //}
            return ExecuteReader(connstr, cmdType, cmdText, commandParameters);
        }

        public  DbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = Provider.CreateCommand();
            DbConnection conn = Provider.CreateConnection();
            conn.ConnectionString = connectionString;
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                this.Conn = conn;
                return rdr;
            }
            catch
            {
                
                throw;
            }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        /// <summary>
        /// 执行对默认数据库有自定义排序的分页的查询
        /// </summary>
        /// <param name="connectionString">连接字符�?
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，�?a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，�?students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，�?a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序�?/param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录�?/param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条�?/param>
        /// <param name="PageCount">输出参数，返回查询的总页�?/param>
        /// <returns>返回查询结果</returns>
        public  DbDataReader ExecuteReaderPage(string connectionString, string SqlAllFields, string SqlTablesAndWhere, string IndexField, string GroupClause, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params DbParameter[] commandParameters)
        {
            DbConnection conn = Provider.CreateConnection();
            conn.ConnectionString = connectionString;
            try
            {
                conn.Open();
                DbCommand cmd = Provider.CreateCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, "", commandParameters);
                string Sql = GetPageSql(conn, cmd, SqlAllFields, SqlTablesAndWhere, IndexField, OrderFields, PageIndex, PageSize, out  RecordCount, out  PageCount);
                if (GroupClause != null && GroupClause.Trim() != "")
                {
                    int n = Sql.ToLower().LastIndexOf(" order by ");
                    Sql = Sql.Substring(0, n) + " " + GroupClause + " " + Sql.Substring(n);
                }
                cmd.CommandText = Sql;
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                //DbHelper.Conn = conn;
                return rdr;
            }
            catch
            {
                //if (conn.State == ConnectionState.Open)
                //    conn.Close();
                throw;
            }
            finally
            {
               
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        public  DbDataReader ExecuteReader(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = Provider.CreateCommand();
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            //DbHelper.Conn = connection;
            return rdr;
        }
        public  object ExecuteScalar(MyDBType mydbtype, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            //string connstr = string.Empty;
            //switch (mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr = DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr = DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr = DBConfig.CmsOracleConString;
            //        break;
            //    case MyDBType.Other:
            //        connstr = DBConfig.CmsConString;
            //        break;
            //}
            return ExecuteScalar(connstr, cmdType, cmdText, commandParameters);
        }

        

        public  object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = Provider.CreateCommand();

            using (DbConnection connection = Provider.CreateConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    //DbHelper.Conn = connection;
                    return val;
                }
                catch
                {
                    throw;
                }
                finally
                {
                   
                    if (null != Conn && Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }


                }
            }
        }

        public  object ExecuteScalar(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            try
            {
                DbCommand cmd = Provider.CreateCommand();

                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                this.Conn = connection;
                return val;
            }
            catch { throw; }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        public  object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            try
            {
                DbCommand cmd = Provider.CreateCommand();

                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                this.Conn = trans.Connection;
                return val;
            }
            catch { throw; }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        public  DataTable ExecuteTable(MyDBType mydbtype, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            //string connstr = string.Empty;
            //switch (mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr = DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr = DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr = DBConfig.CmsOracleConString;
            //        break;
            //}
            return ExecuteTable(connstr, cmdType, cmdText, commandParameters);
        }






        public  DataTable ExecuteTable(string connectionString, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = Provider.CreateCommand();

            using (DbConnection connection = Provider.CreateConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    DbDataAdapter ap = Provider.CreateDataAdapter();
                    ap.SelectCommand = cmd;
                    DataSet st = new DataSet();
                    ap.Fill(st, "Result");
                    cmd.Parameters.Clear();
                    //DbHelper.Conn = connection;
                    return st.Tables["Result"];
                }
                catch
                {
                    throw;
                }
                finally
                {
                   
                    if (null != Conn && Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }

                }
            }
        }

        public  DataTable ExecuteTable(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            try
            {
                DbCommand cmd = Provider.CreateCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                DbDataAdapter ap = Provider.CreateDataAdapter();
                ap.SelectCommand = cmd;
                DataSet st = new DataSet();
                ap.Fill(st, "Result");
                cmd.Parameters.Clear();
                this.Conn = connection;
                return st.Tables["Result"];
            }
            catch { throw; }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        /// <summary>
        /// 执行对默认数据库有自定义排序的分页的查询
        /// </summary>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，�?a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，�?students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，�?a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序�?/param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录�?/param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条�?/param>
        /// <param name="PageCount">输出参数，返回查询的总页�?/param>
        /// <returns>返回查询结果</returns>
        public  DataTable ExecutePage(MyDBType mydbtype, string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params DbParameter[] commandParameters)
        {
            //string connstr = string.Empty;
            //switch (mydbtype)
            //{
            //    case MyDBType.Access:
            //        connstr = DBConfig.CmsAccessConString;
            //        break;
            //    case MyDBType.Sql:
            //        connstr = DBConfig.CmsSqlConString;
            //        break;
            //    case MyDBType.Oracle:
            //        connstr = DBConfig.CmsOracleConString;
            //        break;
            //}
            return ExecutePage(connstr, SqlAllFields, SqlTablesAndWhere, IndexField, OrderFields, PageIndex, PageSize, out  RecordCount, out  PageCount, commandParameters);
        }

        /// <summary>
        /// 执行有自定义排序的分页的查询
        /// </summary>
        /// <param name="connectionString">SQL数据库连接字符串</param>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，�?a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，�?students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，�?a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序�?/param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录�?/param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条�?/param>
        /// <param name="PageCount">输出参数，返回查询的总页�?/param>
        /// <returns>返回查询结果</returns>
        public  DataTable ExecutePage(string connectionString, string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params DbParameter[] commandParameters)
        {
            using (DbConnection connection = Provider.CreateConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    //DbHelper.Conn = connection;
                    return ExecutePage(connection, SqlAllFields, SqlTablesAndWhere, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, commandParameters);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    
                    if (null != Conn && Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }


                }
            }
        }

        /// <summary>
        /// 执行有自定义排序的分页的查询
        /// </summary>
        /// <param name="connection">SQL数据库连接对�?/param>
        /// <param name="SqlAllFields">查询字段，如果是多表查询，请将必要的表名或别名加上，�?a.id,a.name,b.score</param>
        /// <param name="SqlTablesAndWhere">查询的表如果包含查询条件，也将条件带上，但不要包含order by子句，也不要包含"from"关键字，�?students a inner join achievement b on a.... where ....</param>
        /// <param name="IndexField">用以分页的不能重复的索引字段名，最好是主表的自增长字段，如果是多表查询，请带上表名或别名，�?a.id</param>
        /// <param name="OrderASC">排序方式,如果为true则按升序排序,false则按降序�?/param>
        /// <param name="OrderFields">排序字段以及方式如：a.OrderID desc,CnName desc</OrderFields>
        /// <param name="PageIndex">当前页的页码</param>
        /// <param name="PageSize">每页记录�?/param>
        /// <param name="RecordCount">输出参数，返回查询的总记录条�?/param>
        /// <param name="PageCount">输出参数，返回查询的总页�?/param>
        /// <returns>返回查询结果</returns>
        public  DataTable ExecutePage(DbConnection connection,string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount, params DbParameter[] commandParameters)
        {
            try
            {
                DbCommand cmd = Provider.CreateCommand();
                PrepareCommand(cmd, connection, null, CommandType.Text, "", commandParameters);
                string Sql = GetPageSql(connection, cmd, SqlAllFields, SqlTablesAndWhere, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount);
                cmd.CommandText = Sql;
                DbDataAdapter ap = Provider.CreateDataAdapter();
                ap.SelectCommand = cmd;
                DataSet st = new DataSet();
                ap.Fill(st, "PageResult");
                cmd.Parameters.Clear();
                this.Conn = connection;
                return st.Tables["PageResult"];
            }
            catch { throw; }
            finally
            {
                
                if (null != Conn && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }


            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public  void CloseConn()
        {
            
            if (this.Conn != null )
            {
                if (this.Conn.State == ConnectionState.Open)
                {
                    this.Conn.Close();
                }
            }
        }
        /// <summary>
        /// 取得分页的SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        /// <param name="SqlAllFields"></param>
        /// <param name="SqlTablesAndWhere"></param>
        /// <param name="IndexField"></param>
        /// <param name="OrderFields"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        private  string GetPageSql(DbConnection connection, DbCommand cmd, string SqlAllFields, string SqlTablesAndWhere, string IndexField, string OrderFields, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            RecordCount = 0;
            PageCount = 0;
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
            string SqlCount = "select count(" + IndexField + ") from " + SqlTablesAndWhere;
            cmd.CommandText = SqlCount;
            RecordCount = (int)cmd.ExecuteScalar();
            if (RecordCount % PageSize == 0)
            {
                PageCount = RecordCount / PageSize;
            }
            else
            {
                PageCount = RecordCount / PageSize + 1;
            }
            if (PageIndex > PageCount)
                PageIndex = PageCount;
            if (PageIndex < 1)
                PageIndex = 1;
            string Sql = null;
            if (PageIndex == 1)
            {
                Sql = "select top " + PageSize + " " + SqlAllFields + " from " + SqlTablesAndWhere + " " + OrderFields;
            }
            else
            {
                Sql = "select top " + PageSize + " " + SqlAllFields + " from ";
                if (SqlTablesAndWhere.ToLower().IndexOf(" where ") > 0)
                {
                    string _where = Regex.Replace(SqlTablesAndWhere, @"\ where\ ", " where (", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Sql += _where + ") and (";
                }
                else
                {
                    Sql += SqlTablesAndWhere + " where (";
                }
                Sql += IndexField + " not in (select top " + (PageIndex - 1) * PageSize + " " + IndexField + " from " + SqlTablesAndWhere + " " + OrderFields;
                Sql += ")) " + OrderFields;
            }
            return Sql;
        }
        private  void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

           
            cmd.Transaction = trans;
            

            cmd.CommandType = cmdType;
            cmd.CommandTimeout = Timeout;
            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                    if (parm != null)
                        cmd.Parameters.Add(parm);
            }
        }


        public  string CreateUpdateStr<T>(T t, string tableName,string keyStr, MyDBType myDBType)
        {
            string tStr = "update  " + tableName + " set {0} where {1}";
            string fuhao = "@";
            switch (myDBType)
            {
                case MyDBType.Oracle:
                    fuhao = ":";
                    break;
            }
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            string colstr = string.Empty;
            string valuestr = string.Empty;
            for (int i = 0; i < properties.Length; i++)
            {
                string name = properties[i].Name; //����
                object value = properties[i].GetValue(t, null);  //ֵ

                //if (value != null)
                //{
                    colstr +=  properties[i].Name + "=" + fuhao + properties[i].Name+",";
                //}
            }
            colstr = colstr.Substring(0, colstr.Length - 1);
            tStr = String.Format(tStr, colstr, keyStr);
            return tStr;
        }
    }
}