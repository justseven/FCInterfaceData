using Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using Web4BDC.Models.StepPush;

namespace Web4BDC.Dal.StepPushDAL
{
    public class StepPushDAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;
        internal string GetWDBS(string userID)
        {
            string sql = "select department from SEC_EMPLOYEE t where usercode='{0}'";
            sql = string.Format(sql, userID);
            DbHelper dbHelper = new DbHelper();
            
            try
            {
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);

                object o=dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return o.ToString();
                return string.Empty;
            }
            catch (Exception ex) { throw ex; }
            finally { dbHelper.CloseConn(); }
        }

        internal string GetXZQDM(string prjID)
        {
            string sql = "select qxdm from dj_sjd where slbh='{0}' ";
            sql = string.Format(sql, prjID);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            if (null != o)
                return o.ToString();
            return "320301";
        }

        internal void InsertStepLog(StepPushJsonModel spjm)
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            string sql = dbHelper.CreateInsertStr<StepPushJsonModel>(spjm, "StepPushJsonModel", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<StepPushJsonModel>(spjm, MyDBType.Oracle);

            dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);

        }
    }
}