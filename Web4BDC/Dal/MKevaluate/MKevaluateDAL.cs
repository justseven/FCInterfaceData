using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Dal.MKevaluate
{
    public class MKevaluateDAL
    {
        public static object lockKey = new object();
        public string GetSQRLXFS(string yWBH)
        {
            string sql= @"select nvl(tzryddh,tzrdh) from dj_sjd where slbh = '{0}'";
            sql = string.Format(sql, yWBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o= dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                        return o.ToString();
                    return string.Empty;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        public string GetSQRXM(string yWBH)
        {
            string sql = @"select tzrxm from dj_sjd where slbh = '{0}'";
            sql = string.Format(sql, yWBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                        return o.ToString();
                    return string.Empty;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }
    }
}