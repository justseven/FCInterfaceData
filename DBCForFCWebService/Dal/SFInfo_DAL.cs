using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using DBCForFCWebService.Model;
using System.Data;
using DBCForFCWebService.Model.ZZSF;
using Config;

namespace DBCForFCWebService.Dal
{
    public class SFInfo_DAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ConnectionString;

        private static string DAK = ConfigurationManager.ConnectionStrings["bdcdakConnection"].ConnectionString;
        private static string SXK = ConfigurationManager.ConnectionStrings["bdcsxkConnection"].ConnectionString;

        private static readonly object lockKey = new object();
        public bool CheckSFState(string slbh)
        {
            string sql = @"select count(1) from wfm_actinst
left join wfm_activity on wfm_activity.aid=wfm_actinst.mdlid
left join wfm_process on wfm_process.pid=wfm_activity.pid
left join wfm_model on wfm_model.mid=wfm_process.mid
where  wfm_actinst.prjid = :prjid and wfm_actinst.stepname='收费' and wfm_actinst.stepstate<>'已完成' ";// and wfm_actinst.stepstate='已完成'";//wfm_actinst.prjid as slbh,wfm_process.pid as pid,wfm_actinst.stepname as stepName
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":prjid", slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                object o= dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());
                if(null!=o)
                {
                    if (Convert.ToInt32(o) > 0)
                        return true;
                    return false;
                }
                return false;
            }

        }

        internal List<DJ_SFD> GetSFData(string slbh)
        {
            string sql = @"select sfd.SLBH,sfd.JFBH,sfd.XMMC,sfd.JFDW,sfd.TXDZ,sfd.DH,sfdfb.SFXM as JFLX,sfd.JBR,
sfd.JBRQ,sfd.JBYJ,sfd.SHR,sfd.SHYJ,sfd.SHRQ,sfd.YSJE,sfd.SSJE,sfd.SKR,sfd.SKRQ,sfd.SKYJ,sfd.SLR,sfd.DYZT,
sfd.SFZT,sfd.HBDH,sfd.DYR,sfd.DYSJ,sfd.HBR,sfd.HBSJ,sfd.SFBZ,sfd.YYS,sfd.GRSDS,sfd.QS,sfd.TDZZS,sfd.FPH,sfd.ZZSFZT,sfd.ZZSFZFFS 
from dj_sfd sfd
left join dj_sfd_fb sfdfb on sfdfb.slbh=sfd.slbh
where sfd.slbh like '%' || :slbh || '%' and sfd.ysje>0 and sfzt is null";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":slbh", slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt= dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
                if(null!=dt && dt.Rows.Count>0)
                {
                    return ModelHelper<DJ_SFD>.FillModel(dt);
                }
                return null;
            }
        }

        internal void UpDateSfd(DJ_SFD sfd, DbHelper dbhelper,DbTransaction trans)
        {
            string keyStr = "SLBH='" + sfd.SLBH + "'";
            string sql = dbhelper.CreateUpdateStr<DJ_SFD>(sfd, "DJ_SFD", keyStr, MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbhelper.GetParamArray<DJ_SFD>(sfd, MyDBType.Oracle);
            dbhelper.SetProvider(MyDBType.Oracle);
            dbhelper.ExecuteNonQuery(MyDBType.Oracle,trans, System.Data.CommandType.Text, sql, param);
        }

        internal void InsertRequestLog(SF_Submit_Request ssr)
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(SXK);
            if (dbHelper.Conn.State != System.Data.ConnectionState.Open)
            {
                dbHelper.Conn.Open();
            }
            string sql = dbHelper.CreateInsertStr<SF_Submit_Request>(ssr, "SF_Submit_Request", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<SF_Submit_Request>(ssr, MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
        }

        internal void UpDateSDFModel(string token,string userid)
        {
            string sql = "update SFDModel set state='{0}',UserId='{2}' where uuid='{1}'";
            sql = string.Format(sql, "已打印", token, userid);
            DbHelper dbhelper = new DbHelper();
            dbhelper.SetProvider(MyDBType.Oracle);
            dbhelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        internal void UpDateSfd_ModList(List<DJ_SFD> sfd_list,  List<SFDModel> modList)
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            if(null== dbHelper.Conn)
            {
                dbHelper.CreateConn();
            }
            if (dbHelper.Conn.State != System.Data.ConnectionState.Open)
            {
                dbHelper.Conn.Open();
            }
            using (DbTransaction trans = dbHelper.Conn.BeginTransaction())
            {
                try
                {
                    foreach (DJ_SFD sfd in sfd_list)
                    {
                        UpDateSfd(sfd, dbHelper, trans);
                    }
                    foreach (SFDModel mod in modList)
                    {
                        InsertSFDModel(mod, dbHelper,trans);
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }



        private void InsertSFDModel(SFDModel mod,DbHelper dbHelper, DbTransaction trans)
        {
            string sql = dbHelper.CreateInsertStr<SFDModel>(mod, "SFDModel", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<SFDModel>(mod, MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(MyDBType.Oracle,trans, System.Data.CommandType.Text, sql, param);
        }

        private void UpdateSFDModel(SFDModel mod,DbHelper dbHelper,DbTransaction trans)
        {
            string keyStr = "UUID='" + mod.Uuid + "'";
            string sql = dbHelper.CreateUpdateStr<SFDModel>(mod, "DJ_SFD", keyStr, MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<SFDModel>(mod, MyDBType.Oracle);
            dbHelper.SetProvider(MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(MyDBType.Oracle, trans, System.Data.CommandType.Text, sql, param);
        }

        public List<SFDModel> QuerySfdModel(string UUID)
        {
            string sql = "select * from SFDMODEL where UUID=:UUID and state=:state";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":UUID", UUID);
            ListAdd(list, ":state", "查询");
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ModelHelper<SFDModel>.FillModel(dt);
                }
                return null;
            }
        }

    }
}