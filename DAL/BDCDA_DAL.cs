using Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using XZFCDA.Models;
using XZFCDA.Models.BDCModel;
using XZFCDA.Tools;

namespace XZFCDA.Dal
{
    public class BDCDA_DAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["ZtgeoGGK"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["ZtgeoWDK"].ConnectionString;

        internal static Models.BDCModel.DOC_BINFILE GetDoc_binfile(string p)
        {
            string sql = "select t.* from DOC_BINFILE t where  CID ='" + p + "'";
            DbHelper.Conn = new OracleConnection(WDK);
            
            try
            {
                DataTable dt = DbHelper.ExecuteTable(DbHelper.Conn, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.DOC_BINFILE>.FillModel(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static DataTable GetCID(PageParams pageParams)
        {
            string sql = string.Concat(new string[]
            {"select lst.* from wfm_attachlst lst ",
"left join WFM_ACTIVITY b on b.pid=lst.pnode ",
"left join wfm_actinst c on c.mdlid=b.aid ",
"where c.prjid='",pageParams.PrjId,"' and c.stepname='受理'"
        });
            DbHelper.Conn = new OracleConnection(GGK);

            try
            {
                return  DbHelper.ExecuteTable(DbHelper.Conn, System.Data.CommandType.Text, sql, null);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static List<Models.BDCModel.DJ_QLR> GetQLR(string p)
        {
            string sql = "select qlr.* from dj_qlr qlr left join dj_qlrgl gl on qlr.qlrid=gl.qlrid where gl.slbh='{0}'";
            sql = string.Format(sql, p);
            DbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            return ModelHelper<Models.BDCModel.DJ_QLR>.FillModel(dt);
        }

        internal static DJ_QLRGL GetQlrType(string qlrid, string slbh)
        {
            string sql = "select * from dj_qlrgl gl where gl.qlrid='{0}' and gl.slbh='{1}'";
            sql = string.Format(sql, qlrid, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
                return ModelHelper<Models.BDCModel.DJ_QLRGL>.FillModel(dt.Rows[0]);
            return null;
        }

        internal static Models.BDCModel.DJ_TSGL GetTSGL(string slbh)
        {
            string sql = "select * from dj_tsgl where slbh='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            return ModelHelper<Models.BDCModel.DJ_TSGL>.FillModel(dt.Rows[0]);
        }

        internal static DJ_XGDJGL GetDJ_XGDJGL(string slbh)
        {
            //string sql = "select bdczh as 证号 from dj_djb where slbh='{0}' union select bdczmh as 证号 from dj_yg where slbh='{0}' union select bdczmh as 证号 from dj_dy where slbh='{0}' union select bdczmh as 证号 from dj_yy where slbh='{0}'";
            string sql = "select * from dj_xgdjgl where zslbh='{0}'";

            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
                return ModelHelper<Models.BDCModel.DJ_XGDJGL>.FillModel(dt.Rows[0]);
            return null;
        }

        internal static Models.BDCModel.FC_H_QSDC GetFC_H_QSDC(string slbh)
        {
            string sql = " select h.* from fc_h_qsdc h left join dj_tsgl t on t.tstybm=h.tstybm where t.slbh='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            return ModelHelper<Models.BDCModel.FC_H_QSDC>.FillModel(dt.Rows[0]);
        }

        internal static bool IsFW(string slbh)
        {
            string sql = "select bdclx from dj_tsgl where slbh = '{0}'";
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            object o=DbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != o)
            {
                string res = o.ToString();
                if (res.Equals("宗地"))
                    return false;
                return true;
            }
            return false;

        }

        internal static List<Models.BDCModel.WFM_ATTACHLST> GetWFM_ATTACHLST(string slbh)
        {
            string sql = "select * from wfm_attachlst t where t.pnode='{0}' ";
            sql = string.Format(sql, slbh);
            DbHelper.Conn = new OracleConnection(GGK);

            try
            {
                DataTable dt = DbHelper.ExecuteTable(DbHelper.Conn, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.WFM_ATTACHLST>.FillModel(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void InsertLog(Models.BDCModel.FC_DA_TAG tag)
        {
            string sql = "insert into FC_DA_TAG (ID,SLBH,PUSHUSER,PUSHDATE,ISSUCCESS,MESSAGE) values(:ID,:SLBH,:PUSHUSER,:PUSHDATE,:ISSUCCESS,:MESSAGE)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":ID", tag.ID);
            ListAdd(list, ":SLBH", tag.SLBH);
            ListAdd(list, ":PUSHUSER", tag.PUSHUSER);
            ListAdd(list, ":PUSHDATE", tag.PUSHDATE);
            ListAdd(list, ":ISSUCCESS", tag.ISSUCCESS);
            ListAdd(list, ":MESSAGE", tag.MESSAGE);

            DbHelper.SetProvider(MyDBType.Oracle);
            DbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());

        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new OracleParameter(paraName, value));
        }

        internal static string GetBDCZH(string slbh)
        {
            string sql = "select bdczh as 证号 from dj_djb where slbh='{0}' union select bdczmh as 证号 from dj_yg where slbh='{0}' union select bdczmh as 证号 from dj_dy where slbh='{0}' union select bdczmh as 证号 from dj_yy where slbh='{0}'";
            
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Oracle);
            return DbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null).ToString();
            
        }

       
    }
}