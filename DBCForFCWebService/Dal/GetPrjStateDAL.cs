using Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DBCForFCWebService.Dal
{
    public class GetPrjStateDAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;
        private static readonly object lockKey = new object();
        public JsonModel GetPrjState(string slbh)
        {
            string sql = "select prjid as ywzh,procname as djlx,proposer as sqr,initaddr as zl,prjstate as prjState,ACCEPTTIME as jjrq from WFM_PROCINST where prjid=:prjid";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":prjid", slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<JsonModel>.FillModel(dt.Rows[0]);
                return null;
            }
        }

        public string GetStepName(string slbh)
        {
            string sql = "select stepName from WFM_ACTINST where prjid=:prjid order by accepttime desc";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":prjid", slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                object stepName = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());
                if(null!=stepName)
                {
                    return stepName.ToString();
                }
                return "无此流程";
            }
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        internal string GetZLBySlbh(string ywzh)
        {
            string sql = @"select distinct nvl(sjd.zl,h.zl) as zl from dj_sjd sjd
left join dj_tsgl tsgl on tsgl.slbh = sjd.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
where sjd.slbh = :prjid ";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":prjid", ywzh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object stepName = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
                if (null != stepName)
                {
                    return stepName.ToString();
                }
                return "未查询到坐落";
            }
        }
    }

    public class JsonModel
    {
        public string ywzh { get; set; }
        public string djlx { get; set; }
        public string sqr { get; set; }
        public string zl { get; set; }
        public string prjState { get; set; }
        [XmlIgnore]
        public DateTime jjrq { get; set; }
        [XmlElement("jjrq")]
        public string jjrqString {
            get { return this.jjrq.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.jjrq = DateTime.Parse(value); }
        }
    }
}