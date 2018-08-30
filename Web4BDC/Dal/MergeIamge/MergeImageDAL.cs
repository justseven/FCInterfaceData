using Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Tools;

namespace Web4BDC.Dal.MergeIamge
{
    public class MergeImageDAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ConnectionString;
        internal List<string> GetTstybms(string slbh)
        {
            List<string> list = new List<string>();
            string sql = "select tstybm from dj_tsgl where slbh='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper dbhelper = new DbHelper();
            dbhelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbhelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
            if(null!=dt && dt.Rows.Count>0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list;
        }

        internal List<string> GetFileIds(List<string> tstybm)
        {
            List<string> list = new List<string>();
            string sql = "select Fileid from PUB_ATTACHLST t where t.parentnode in ({0})";
            string param = CreateParam(tstybm);
            sql = string.Format(sql, param);
            DbHelper dbhelper = new DbHelper();
            dbhelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
            dbhelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbhelper.ExecuteTable(dbhelper.Conn, System.Data.CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list;


        }

        internal List<string> GetImagePath(List<string> fileIds)
        {
            List<string> list = new List<string>();
            string sql = "select ftpath from DOC_BINFILE t where t.fileid in ({0})";
            string param = CreateParam(fileIds);
            sql = string.Format(sql, param);
            DbHelper dbhelper = new DbHelper();
            dbhelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);
            dbhelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbhelper.ExecuteTable(dbhelper.Conn, System.Data.CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item[0].ToString());
                }
            }
            return list;
        }

        private string CreateParam(List<string> fileIds)
        {
            string res = string.Empty;
            if(null!=fileIds&& fileIds.Count>0)
            {
                foreach (string item in fileIds)
                {
                    if(string.IsNullOrEmpty(res))
                    {
                        res = "'" + item + "'";
                    }
                    else
                    {
                        res += ",'" + item + "'";
                    }
                }
            }
            return res;
        }
    }
}