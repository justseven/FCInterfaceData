/****************************************************************************************
 *                              2017.7.17
 *                                 by seven
 * 
 * 
 * 
 * 
 * 
 * 
 * ***************************************************************************************/
using Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using Web4BDC.FC.Models;
using Web4BDC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.FCDAModel;
using Web4BDC.Tools;

namespace Web4BDC.Dal
{
    public class BDCDA_DAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ConnectionString;

        private static string DAK = ConfigurationManager.ConnectionStrings["bdcdakConnection"].ConnectionString;

        private static readonly object lockKey = new object();
        internal static Models.BDCModel.DOC_BINFILE GetDoc_binfile(string p)
        {
            string sql = "select t.* from DOC_BINFILE t where  CID ='" + p + "'";
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    dbHelper.Conn.Close();
                    return ModelHelper<Models.BDCModel.DOC_BINFILE>.FillModel(dt.Rows[0]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        internal static DataTable GetRecSLBHS()
        {
            string sql = "select distinct slbh from FC_DA_SLBH where state='0'";
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex) { throw ex; }
        }

        internal static DataTable GetRecSLBHZX()
        {
            string sql = "select distinct slbh from FC_DA_SLBH_ZX where state='0'";
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 插入归档
        /// </summary>
        /// <param name="gd"></param>
        internal static void Insert_Into_GD(ARCH_GLDAXX gd,List<ARCH_FWYBDJ> ybList,List<ARCH_BDCDYDJ> dyList,List<ARCH_BDCCFDJ> cfList, List<ARCH_BDCZXDJ> zxList,List<WFM_ATTACHLST> wfm_ATTACHLST_list)
        {
            if (null != gd)
            {
                InsertGLDAXX(gd);
            }
            if (null!= ybList)
            {
                //foreach (ARCH_FWYBDJ yb in ybList)
                //{
                if(ybList.Count>1)
                {
                    ybList[0].BDCDYH += "等" + ybList.Count + "户";
                    ybList[0].ZL+= "等" + ybList.Count + "户";
                    ybList[0].BDCZSH+= "等" + ybList.Count + "户";
                }
                    InsertFWYBDJ(ybList[0]);
                //}
                
            }
            if(null!= dyList)
            {
                //foreach (ARCH_BDCDYDJ dy in dyList)
                //{
                if (dyList.Count > 1)
                {
                    dyList[0].BDCDYH += "等" + dyList.Count + "户";
                    dyList[0].ZJJZWZL += "等" + dyList.Count + "户";
                }
                InsertDYDJ(dyList[0]);
                //}
            }
            if(null!=cfList)
            {
                //foreach (ARCH_BDCCFDJ cf in cfList)
                //{
                if (cfList.Count > 1)
                {
                    cfList[0].BDCDYH += "等" + cfList.Count + "户";
                    cfList[0].ZL += "等" + cfList.Count + "户";
                }
                InsertCFDJ(cfList[0]);
                //}
                
            }
            if(null!=zxList)
            {
                InsertZXDJ(zxList[0]);
            }

            if(null!=wfm_ATTACHLST_list)
            {
                foreach (WFM_ATTACHLST item in wfm_ATTACHLST_list)
                {
                    try
                    {
                        Insertwfm_ATTACHLST_list(item);
                    }
                    catch
                    {
                        continue;
                    }
                }
                
            }
         }

        internal static bool CheckProcInDA(string slbh)
        {
            string sql = string.Format("select count(1) from ARCH_GLDAXX where slbh='{0}'", slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);


                dbHelper.SetProvider(MyDBType.Oracle);
                
                object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return Convert.ToInt32(o) > 0;

                }

                return false;



            }
        }

        internal static void UpdateBDCDAState(string slbh, int v)
        {
            FC_DA_SLBH da = InFC_DA_SLBH(slbh);
            FC_DA_SLBH_ZX dazx = InFC_DA_SLBH_ZX(slbh);
            if(null!=da)
            {
                UpdateBDCDAQSState(da.SLBH, v);
            }
            if(null!=dazx)
            {
                UpdateBDCDAZXState(dazx.SLBH, v);
            }

        }

        internal static void UpdateBDCDAQSState(string slbh, int v)
        {
            string sql = string.Format("update FC_DA_SLBH set STATE='{0}' where slbh='{1}'",v,slbh);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                
            }
            catch (Exception ex) { throw ex; }

        }

        internal static void UpdateBDCDAZXState(string slbh, int v)
        {
            string sql = string.Format("update FC_DA_SLBH_ZX set STATE='{0}' where slbh='{1}'", v, slbh);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex) { throw ex; }

        }



        internal static bool CheckProcState(string slbh)
        {
            string sql = "select count(1) from WFM_PROCINST t where t.prjid like '{0}%' and prjstate='已完成'";
            sql = string.Format(sql, slbh);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);

                object o=dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if(null!=o)
                {
                    return Convert.ToInt32(o) > 0;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        internal static void InsertBDCLog(BDCDALog log)
        {
            DbHelper bDCHelper = new DbHelper();
            bDCHelper.SetProvider(MyDBType.Oracle);
            lock (lockKey)
            {
                string sql = bDCHelper.CreateInsertStr<BDCDALog>(log, "BDCDALog", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = bDCHelper.GetParamArray<BDCDALog>(log, MyDBType.Oracle);

                bDCHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
            }
        }

        internal static List<DJ_XGDJGL> GetBDC_XGDJGL(string busiNO,string ywlx)
        {
            string sql = string.Empty;
            string sql1 = "select * from dj_xgdjgl where zslbh =  '{0}' and xgzlx<>'土地证'";
            string sql2= "select * from dj_xgdjgl where zslbh like  '{0}%' and xgzlx<>'土地证'";
            if(ywlx.ToUpper().Equals("Z"))
            {
                sql = sql2;
            }
            else
            {
                sql = sql1;
            }
            sql = string.Format(sql, busiNO);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                sql = string.Empty;
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.DJ_XGDJGL>.FillModel(dt);
                return null;
            }
        }

        private static void InsertZXDJ(ARCH_BDCZXDJ zx)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_BDCZXDJ>(zx, "ARCH_BDCZXDJ", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_BDCZXDJ>(zx, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
               
            }
        }

        private static void Insertwfm_ATTACHLST_list(WFM_ATTACHLST item)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<WFM_ATTACHLST>(item, "WFM_ATTACHLST", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<WFM_ATTACHLST>(item, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
                
            }
        }

        internal static DataTable GetSLBHByProName(string proName)
        {

            //string sql = "select slbh from FC_SPFHX_TAG where SFTS<>1";

            string sql = "select prjid slbh from WFM_PROCINST t where t.procname like '%{0}%' and prjstate='已完成'";
            sql = string.Format(sql, proName);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);

                return dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex) { throw ex; }

        }

        private static void InsertCFDJ(ARCH_BDCCFDJ cf)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_BDCCFDJ>(cf, "ARCH_BDCCFDJ", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_BDCCFDJ>(cf, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
               
            }
        }

        internal static List<ARCH_BDCZXDJ> GetARCH_BDCZXDJ(string busiNO)
        {
            string sql = @"
select djzx.SLBH,djzx.slbh as DAH,SQR,SQRQ,SQNR,SQBZ,djrq,DBR,djzx.slbh as DABH,qsdc.zl as zl,XGZH  from dj_xgdjzx djzx
left join dj_tsgl tsgl on tsgl.slbh=djzx.slbh
left join fc_h_qsdc qsdc on qsdc.tstybm=tsgl.tstybm
where djzx.slbh='{0}'";
            sql = string.Format(sql, busiNO);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_BDCZXDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        private static void InsertDYDJ(ARCH_BDCDYDJ dy)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_BDCDYDJ>(dy, "ARCH_BDCDYDJ", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_BDCDYDJ>(dy, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
               
            }
        }

        internal static void InsertPushDALog(FC_DA_NewLog log, DbHelper bDCHelper, DbTransaction bdctrans)
        {
            lock(lockKey)
            {
                string sql = bDCHelper.CreateInsertStr<FC_DA_NewLog>(log, "FC_DA_NewLog", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = bDCHelper.GetParamArray<FC_DA_NewLog>(log, MyDBType.Oracle);

                bDCHelper.ExecuteNonQuery(MyDBType.Oracle, bdctrans, System.Data.CommandType.Text, sql, param);
            }
        }
        internal static FC_DA_SLBH InFC_DA_SLBH(string slbh)
        {
            string sql = string.Format("select * from fc_da_slbh where slbh='{0}'",slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();

                dbHelper.SetProvider(MyDBType.Oracle);
                
                DataTable dt=dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql,null);
                if(null!=dt && dt.Rows.Count>0)
                {
                    return ModelHelper<FC_DA_SLBH>.FillModel(dt.Rows[0]);
                }
                return null;


            }
        }

        internal static void UpdateFC_DA_SLBH(FC_DA_SLBH da)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();

                dbHelper.SetProvider(MyDBType.Oracle);
                string sql = dbHelper.CreateUpdateStr<FC_DA_SLBH>(da, "FC_DA_SLBH","SLBH='"+da.SLBH+"'", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<FC_DA_SLBH>(da, MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);

            }
        }
        

        internal static void InsertFC_DA_SLBH(string slbh,string prjName)
        {
            lock (lockKey)
            {
                if (!ISDYZX(slbh))
                {

                    if (IsZXYW(slbh))
                    {
                        InsertZX_FC_DA_SLBH(slbh, prjName);
                    }
                    else
                    {
                        InsertQS_FC_DA_SLBH(slbh, prjName);
                    }
                }
            }
        }

        private static bool ISDYZX(string slbh)
        {
            string sql = string.Format("select count(1) from dj_tsgl where slbh='{0}' and djzl like '%抵押注销%'",slbh);
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return Convert.ToInt32(o) > 0;
                }
                return false;
            }
        }

        private static bool IsZXYW(string slbh)
        {
            string sql = "select distinct djzl from dj_tsgl where slbh like '{0}%' and bdclx<>'宗地'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=o)
                {
                    return IsContains(o.ToString());//.Contains("权属注销");
                }
                return false;
            }
        }

        private static bool IsContains(string str)
        {
            string[] zxzl = new string[] { "权属注销", "预告注销" };
            if (null!=zxzl)
            {
                foreach (var item in zxzl)
                {
                    if (str.Contains(item))
                        return true;
                }
            }
            return false;
        }

        private static void InsertQS_FC_DA_SLBH(string slbh, string prjName)
        {
            lock (lockKey)
            {
                FC_DA_SLBH fds = InFC_DA_SLBH(slbh);
                if (null == fds)
                {
                    fds = new FC_DA_SLBH();
                    fds.ID = Guid.NewGuid().ToString("N");
                    fds.CREATETIME = DateTime.Now;
                    fds.LCLX = prjName;
                    fds.SLBH = slbh;
                    fds.STATE = 0;

                    DbHelper dbHelper = new DbHelper();

                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<FC_DA_SLBH>(fds, "FC_DA_SLBH", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<FC_DA_SLBH>(fds, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);


                }
                else
                {
                    fds.CREATETIME = DateTime.Now;
                    fds.LCLX = prjName;
                    fds.SLBH = slbh;
                    fds.STATE = 0;
                    UpdateFC_DA_SLBH(fds);

                }
            }

        }

        private static void InsertZX_FC_DA_SLBH(string slbh, string prjName)
        {
            FC_DA_SLBH_ZX fds = InFC_DA_SLBH_ZX(slbh);
            if (null == fds)
            {
                fds = new FC_DA_SLBH_ZX();
                fds.ID = Guid.NewGuid().ToString("N");
                fds.CREATETIME = DateTime.Now;
                fds.LCLX = prjName;
                fds.SLBH = slbh;
                fds.STATE = 0;
                lock (lockKey)
                {
                    DbHelper dbHelper = new DbHelper();

                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<FC_DA_SLBH_ZX>(fds, "FC_DA_SLBH_ZX", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<FC_DA_SLBH_ZX>(fds, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);

                }
            }
            else
            {
                fds.CREATETIME = DateTime.Now;
                fds.LCLX = prjName;
                fds.SLBH = slbh;
                fds.STATE = 0;
                UpdateFC_DA_SLBH_ZX(fds);

            }

        }

        private static void UpdateFC_DA_SLBH_ZX(FC_DA_SLBH_ZX da)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();

                dbHelper.SetProvider(MyDBType.Oracle);
                string sql = dbHelper.CreateUpdateStr<FC_DA_SLBH_ZX>(da, "FC_DA_SLBH_ZX", "SLBH='"+da.SLBH+"'", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<FC_DA_SLBH_ZX>(da, MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);

            }
        }

        private static FC_DA_SLBH_ZX InFC_DA_SLBH_ZX(string slbh)
        {
            string sql = string.Format("select * from FC_DA_SLBH_ZX where slbh='{0}'",slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();

                dbHelper.SetProvider(MyDBType.Oracle);

                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                {
                    return ModelHelper<FC_DA_SLBH_ZX>.FillModel(dt.Rows[0]);
                }
                return null;


            }
        }

        private static void InsertFWYBDJ(ARCH_FWYBDJ yb)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_FWYBDJ>(yb, "ARCH_FWYBDJ", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_FWYBDJ>(yb, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
               
            }
        }

        internal static List<ARCH_BDCCFDJ> GetARCH_BDCCFDJ(object slbh,string dah)
        {
            string sql = @"select '{1}' as DAH,h.bdcdyh as BDCDYH,cf.xgzh as BDCZSXLH,'' as DABH,CF.SLBH AS SLBH,qlr.qlrmc as QLRMC,qlr.zjhm as ZJHM,cf.cflx as DJLX,
'' as ZXSLBH,'' as ZJZL,cf.fj as FJ,cf.dbr as DBR,cf.djsj as DJSJ,cf.cfqssj as QSRQ,cf.cfqx as SYQX,tsgl.bdclx as BDCLX,cf.xgzh as YBDCZSH,
cf.cfjg as CFJG,cf.cflx as CFLX,cf.cfwj as CFWJ,cf.cfwh as CFWH,cf.cfqssj as CFQSSJ,cf.cfjssj as CFJSSJ,cf.cffw as CFFW,'' as JFJG,'' as JFWJ,
'' as JFWH,'' as JFDJSJ,'' as JFDBR,h.zl as ZL,'' as XGWJ,'' as XGWH,cf.cfbh as CFBH
 from dj_cf cf
left join dj_tsgl tsgl on tsgl.slbh=cf.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on h.lsztybm=z.tstybm
left join dj_qlrgl qlrgl on qlrgl.slbh=cf.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where cf.slbh = '{0}' and qlrgl.qlrlx='义务人'";
            sql = string.Format(sql, slbh,dah);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_BDCCFDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        internal static List<ARCH_FWYBDJ> GetARCH_FWYBDJ_ShouCi(string busiNO, string dah)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量业务获取归档人员名称
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        internal static string GetUserNameInPL(string slbh)
        {
            string userName = "guidangren";

            string sql = "select fjr from dj_fjd where slbh like '{0}%'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                    {
                        userName = o.ToString();
                    }
                    else
                    {
                        userName = "guidangren";
                    }
                    if (null != dbHelper.Conn)
                        dbHelper.Conn.Close();

                }
                catch (Exception ex)
                {
                    userName = "guidangren";
                    throw ex;
                }
            }
            return userName;
        }

            internal static string GetUserNameBySlbh(string slbh)
        {
            string userName = "guidangren";
            
            string sql = "select fjr from dj_fjd where slbh like '{0}%'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if(null!=o)
                    {
                        userName = o.ToString();
                    }
                    else
                    {
                        userName = "guidangren";
                    }
                    if(null!= dbHelper.Conn)
                        dbHelper.Conn.Close();

                }
                catch (Exception ex)
                {
                    userName = "guidangren";
                    throw ex;
                }
            }

            if (userName.Equals("guidangren"))
            {
                sql = @"select dbr as name
  from dj_djb
 where slbh = '{0}'
union
select dbr as name
  from dj_yg
 where slbh = '{0}'
union
select dbr as name
  from dj_dy
 where slbh = '{0}'
union
select dbr as name
  from dj_yy
 where slbh = '{0}'
 union
select dbr as name
  from dj_cf 
 where slbh = '{0}' 
union
select dbr as name
  from dj_xgdjzx
 where slbh = '{0}'";
                sql = string.Format(sql, slbh);
                lock (lockKey)
                {
                    //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                    try
                    {
                        DbHelper dbHelper = new DbHelper();
                        dbHelper.SetProvider(MyDBType.Oracle);
                        object ob = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                        if (null != dbHelper.Conn)
                            dbHelper.Conn.Close();
                        if (null != ob)
                        {
                            userName = ob.ToString();
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return userName;
        }




        internal static DataTable GetLogFail()
        {
            string sql = "select slbh from fc_da_tag t where t.issuccess='0' and message not like '%未找到业务宗号或非房产业务%'";
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                return dt;
            }
        }

        internal static string GetUserID(string userName)
        {
            string sql = "select usercode from SEC_EMPLOYEE t where usercode='{0}' or name='{0}'";
            sql = string.Format(sql, userName);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                try
                {
                    
                    dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                    {
                        return o.ToString();

                    }
                    return "";
                }
                catch (Exception ex) { throw ex; }
                finally { dbHelper.Conn.Close(); }
            }

        }

        internal static List<ARCH_FWYBDJ> GetARCH_FWYBDJ(ArchiveIndex arch,string dah)
        {
            string slbh = arch.BusiNO;
            List<ARCH_FWYBDJ> list = null;
            if (arch.ReqType.Equals("首次登记"))
            {
                list = GetARCH_QSDJ_ShouCi(slbh, dah);
                
            }
            if (null == list || list.Count == 0)
            { 
                list = GetARCH_QSDJ(slbh, dah);
            }

            
            if(null==list || list.Count==0)
            {
                list = GetARCH_YGDJ(slbh,dah);
            }
            if(null!=list && list.Count>0)
            {
                return list;
            }
            return null;
        }

        private static List<ARCH_FWYBDJ> GetARCH_QSDJ_ShouCi(string slbh, string dah)
        {
            string sql = @"select  '{1}' as DAH,  h.bdcdyh as BDCDYH,h.qllx as QLLX,h.qlxz as QLXZ,djb.bdczh as BDCZSH,djb.zsxlh as BDCZSXLH,'' as DABH,
djb.slbh as SLBH,qlr.qlrmc as QLRMC,qlr.zjhm as ZJHM,djb.fzrq as FZRQ,djb.fzjg as FZJG,djb.djlx as DJLX,djb.djyy as DJYY,
qlr.qlrmc as SQR,djb.sqrq as SQRQ,djb.sqnr as SQNR,djb.sqbz as SQBZ,'' as ZXSLBH,'' as ZJZL,djb.gyfs as GYQK,qlrgl.qlrlx as QLRLX,
h.fttdmj as FTTDMJ,h.tdzzrq as TDZZRQ,h.dytdmj as DYTDMJ,z.xmmc as XMMC,z.zh as ZH,h.tdsyqr as TDSYQR,h.sjc as SJC,z.zcs as ZCS,
h.fwxz as FWXZ,h.jzmj as JZMJ,h.ftjzmj as FTJZMJ,z.jgrq as JGSJ,z.zts as ZTS,h.ghyt as GHYT,h.qdjg as QDJG,djb.fj as FJ,
djb.dbr as DBR,z.fwjg as FWJG,djb.djrq as DJSJ,h.tdsyqr as SYQX,'' as ZSQT,tsgl.bdclx as BDCLX,h.zl as ZL 
 from dj_djb djb
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on h.lsztybm=z.tstybm
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where djb.slbh like '{0}-%' and qlrgl.qlrlx='权利人' and bdclx='房屋'";
            sql = string.Format(sql, slbh, dah);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_FWYBDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        internal static List<DJ_QLRGL> GetQLRGL(string slbh)
        {
            //string sql = "select qlr.* from dj_qlr qlr left join dj_qlrgl gl on qlr.qlrid=gl.qlrid where gl.slbh like '{0}%'";

            //string sql = "select qlrGL.* from dj_qlr qlr where qlrid in (select distinct qlrid from dj_qlrgl where slbh= '{0}')";

            string sql = "select * from dj_qlrgl where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.DJ_QLRGL>.FillModel(dt);
            }
        }

        private static List<ARCH_FWYBDJ> GetARCH_QSDJ(string slbh,string dah)
        {
            string sql = @"select  '{1}' as DAH,  h.bdcdyh as BDCDYH,h.qllx as QLLX,h.qlxz as QLXZ,djb.bdczh as BDCZSH,djb.zsxlh as BDCZSXLH,'' as DABH,
djb.slbh as SLBH,qlr.qlrmc as QLRMC,qlr.zjhm as ZJHM,djb.fzrq as FZRQ,djb.fzjg as FZJG,djb.djlx as DJLX,djb.djyy as DJYY,
qlr.qlrmc as SQR,djb.sqrq as SQRQ,djb.sqnr as SQNR,djb.sqbz as SQBZ,'' as ZXSLBH,'' as ZJZL,djb.gyfs as GYQK,qlrgl.qlrlx as QLRLX,
h.fttdmj as FTTDMJ,h.tdzzrq as TDZZRQ,h.dytdmj as DYTDMJ,z.xmmc as XMMC,z.zh as ZH,h.tdsyqr as TDSYQR,h.sjc as SJC,z.zcs as ZCS,
h.fwxz as FWXZ,h.jzmj as JZMJ,h.ftjzmj as FTJZMJ,z.jgrq as JGSJ,z.zts as ZTS,h.ghyt as GHYT,h.qdjg as QDJG,djb.fj as FJ,
djb.dbr as DBR,z.fwjg as FWJG,djb.djrq as DJSJ,h.tdsyqr as SYQX,'' as ZSQT,tsgl.bdclx as BDCLX,h.zl as ZL 
 from dj_djb djb
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on h.lsztybm=z.tstybm
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where djb.slbh like '{0}%' and qlrgl.qlrlx='权利人' and bdclx='房屋' ";
            sql = string.Format(sql, slbh,dah);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_FWYBDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        internal static string GetZDPH(ARCH_GLDAXX gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "select zdph from ARCH_GLDAXX where DAH='{0}'";
                    sql = string.Format(sql, gd.DAH);
                    object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                    {
                        return o.ToString();
                        
                    }
                    
                    return "";

               
                
            }
        }

        private static List<ARCH_FWYBDJ> GetARCH_YGDJ(string slbh,string dah)
        {
            string sql = @"select '{1}' as DAH,  h.bdcdyh as BDCDYH,h.qllx as QLLX,h.qlxz as QLXZ,yg.bdczmh as BDCZSH,yg.zsxlh as BDCZSXLH,'' as DABH,
yg.slbh as SLBH,WM_CONCAT(to_char(qlr.qlrmc))as QLRMC,WM_CONCAT(to_char(qlr.zjhm)) as ZJHM,yg.fzrq as FZRQ,yg.fzjg as FZJG,yg.djlx as DJLX,yg.djyy as DJYY,
WM_CONCAT(to_char(qlr.qlrmc))as SQR,yg.sqrq as SQRQ,yg.sqnr as SQNR,yg.sqbz as SQBZ,'' as ZXSLBH,'' as ZJZL,yg.gyfs as GYQK,qlrgl.qlrlx as QLRLX,
h.fttdmj as FTTDMJ,h.tdzzrq as TDZZRQ,h.dytdmj as DYTDMJ,z.xmmc as XMMC,z.zh as ZH,h.tdsyqr as TDSYQR,h.sjc as SJC,z.zcs as ZCS,
h.fwxz as FWXZ,h.jzmj as JZMJ,h.ftjzmj as FTJZMJ,z.jgrq as JGSJ,z.zts as ZTS,h.ghyt as GHYT,h.qdjg as QDJG,yg.fj as FJ,
yg.dbr as DBR,z.fwjg as FWJG,yg.djrq as DJSJ,h.tdsyqr as SYQX,'' as ZSQT,tsgl.bdclx as BDCLX,h.zl as ZL 
from dj_yg yg
left join dj_tsgl tsgl on tsgl.slbh=yg.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on h.lsztybm=z.tstybm
left join dj_qlrgl qlrgl on qlrgl.slbh=yg.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where yg.slbh = '{0}' and qlrgl.qlrlx='权利人'
group by h.bdcdyh,h.qllx,h.qlxz,yg.bdczmh,yg.zsxlh,yg.slbh,yg.fzrq,yg.fzjg,yg.djlx,yg.djyy,yg.sqrq,h.fttdmj,h.tdzzrq,h.dytdmj,yg.sqnr,yg.sqbz,yg.gyfs,qlrgl.qlrlx,
z.xmmc,z.zh,h.tdsyqr,h.sjc,z.zcs,h.fwxz,h.jzmj,h.ftjzmj,z.jgrq,z.zts,h.ghyt,h.qdjg,yg.fj,yg.dbr,z.fwjg,yg.djrq,h.tdsyqr,tsgl.bdclx,h.zl";
            sql = string.Format(sql, slbh,dah);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_FWYBDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        internal static void DeleteWFM_ATC(WFM_ATTACHLST wa)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from WFM_ATTACHLST where CID='{0}'";
                    sql = string.Format(sql, wa.CID);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static void Delete_Log(FC_DA_TAG tag)
        {
            string sql = "delete from FC_DA_TAG where slbh='{0}'";
            sql = string.Format(sql, tag.SLBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }
        }

        internal static List<ARCH_BDCDYDJ> GetARCH_BDCDYDJ(string slbh,string dah)
        {
            string sql = @"select '{1}' as DAH,h.bdcdyh as BDCDYH,dy.bdczmh as BDCZSH,dy.zsxlh as BDCZSXLH,'' as DABH,dy.slbh as SLBH,qlr.qlrmc as QLRMC,qlr.zjhm as ZJHM,dy.fzrq as FZRQ,
dy.fzjg as FZJG,dy.djlx as DJLX,dy.djyy as DJYY,qlr.qlrmc as SQR,dy.sqrq as SQRQ,dy.sqdjnr as SQNR,'' as SQBZ,'' as ZXSLBH,'' as ZJZL,qlrgl.gyfs as GYQK,qlrgl.qlrlx as QLRLX,
h.tdzzrq as ZZRQ,dy.fj as FJ,dy.dbr as DBR,dy.djrq as DJSJ,h.tdqsrq as QSRQ,dy.dyqx as SYQX,tsgl.bdclx as BDCLX,dy.xgzh as YBDCZSH,'' as YWRMC,
dy.dymj as DYMJ,dy.dyfs as DYFS,dy.dysw as DYSW,dy.qlqssj as QLQSSJ,dy.qljssj as QLJSSJ,dy.zjjzwzl as ZJJZWZL,dy.zjjzwdyfw as ZJJZWDYFW,dy.bdbzzqse as BDBZZQSE,
dy.dbfw as DBFW,dy.zgzqse as ZGZQSE,h.qllx as QLLX,h.qlxz as QLXZ,dy.zgzqqdss as ZGZQQDSS,dy.qt as ZMQT,'' as TDDYMJ
 from dj_dy dy
left
 join dj_tsgl tsgl on tsgl.slbh = dy.slbh
left
 join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left
 join fc_z_qsdc z on h.lsztybm = z.tstybm
left
 join dj_qlrgl qlrgl on qlrgl.slbh = dy.slbh
left
 join dj_qlr qlr on qlr.qlrid = qlrgl.qlrid
where dy.slbh = '{0}' and qlrgl.qlrlx = '抵押权人'";
            sql = string.Format(sql, slbh,dah);
            lock (lockKey)
            {
                //dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    DbHelper dbHelper = new DbHelper();
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return ModelHelper<ARCH_BDCDYDJ>.FillModel(dt);
                    }
                    return null;
                }
                catch (Exception ex) { throw ex; }
            }
        }

        private static void InsertGLDAXX(ARCH_GLDAXX gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_GLDAXX>(gd, "ARCH_GLDAXX", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_GLDAXX>(gd, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
                
            }
        }

        private static void ARCH_BDCZXDJ(ARCH_BDCZXDJ zx)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<ARCH_BDCZXDJ>(zx, "ARCH_BDCZXDJ", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<ARCH_BDCZXDJ>(zx, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, param);
               
            }
        }




        //private static DbParameter[] GetParams(ARCH_GLDAXX gb)
        //{
        //    return dbHelper.GetParamArray<ARCH_GLDAXX>(gb, MyDBType.Oracle);
        //}


        internal static DataTable GetCID(PageParams pageParams)
        {
            string sql = string.Concat(new string[]
            {"select lst.* from wfm_attachlst lst ",
"left join WFM_ACTIVITY b on b.pid=lst.pnode ",
"left join wfm_actinst c on c.mdlid=b.aid ",
"where c.prjid='",pageParams.PrjId,"' and c.stepname='受理'"
        });
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt= dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    dbHelper.Conn.Close();
                    return dt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        internal static string GetQLRXB(string qLRID)
        {
            string sql = "select xb from dj_qlr where qlrid='{0}' ";
            sql = string.Format(sql, qLRID);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                   
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
            
        }

        public static List<Models.BDCModel.DJ_QLR> GetQLR(string p)
        {
            //string sql = "select qlr.* from dj_qlr qlr left join dj_qlrgl gl on qlr.qlrid=gl.qlrid where gl.slbh like '{0}%'";

            string sql = @"select qlr.*,qlrgl.qlrlx,qlrgl.SXH glsxh from dj_qlr qlr  
left join dj_qlrgl qlrgl on qlrgl.qlrid = qlr.qlrid
where qlrgl.slbh = '{0}' and qlrgl.qlrlx not like '%第三方%'";

            //string sql = "select * from dj_qlrgl where slbh='{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.DJ_QLR>.FillModel(dt);
            }
        }

        internal static DJ_QLRGL GetQlrType(string qlrid, string slbh)
        {
            string sql = "select * from dj_qlrgl gl where gl.qlrid='{0}' and gl.slbh='{1}'";
            sql = string.Format(sql, qlrid, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.DJ_QLRGL>.FillModel(dt.Rows[0]);
                return null;
            }
        }

        internal static bool IsExist(ARCH_GLDAXX gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "select count(1) from ARCH_GLDAXX where DAH='{0}'";
                    sql = string.Format(sql, gd.DAH);
                    object o=dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    if(null!=o)
                    {
                        int count = Convert.ToInt32(o);
                        if(count>0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception ex) { throw ex; }
                return false;
            }
        }

        internal static List<DJ_QLR> GetYWR(string busiNO)
        {
            throw new NotImplementedException();
        }

        internal static void Delete_GD(ARCH_GLDAXX gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from ARCH_GLDAXX where dah='{0}'";
                    sql = string.Format(sql, gd.DAH);
                    
                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

               
            }
        }

        internal static void Delete_ZX(ARCH_BDCZXDJ gd)
        {
            
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from ARCH_BDCZXDJ where SLBH='{0}'";
                    sql = string.Format(sql, gd.SLBH);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

               
            
        }

        internal static void Delete_CF(ARCH_BDCCFDJ gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from ARCH_BDCCFDJ where dah='{0}'";
                    sql = string.Format(sql, gd.DAH);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static string GetFC_ZL(string tstybm)
        {
            //string sql = "select zl from dj_sjd where slbh='{0}'";
            string sql = @"select distinct sjd.zl from dj_sjd sjd
left join dj_tsgl gl on gl.slbh=sjd.slbh
left join fc_h_qsdc h on h.tstybm=gl.tstybm
where h.tstybm='{0}' and sjd.djdl in ('200','100','700')";
            sql = string.Format(sql, tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static FC_Z_QSDC GetFC_Z_QSDC(FC_H_QSDC fc_h)
        {
            string sql = @"select z.* from FC_H_QSDC t
left join fc_z_qsdc z on t.lsztybm = z.tstybm
where t.tstybm = '{0}'";
            sql = string.Format(sql, fc_h.TSTYBM);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=dt && dt.Rows.Count>0)
                {
                     return ModelHelper<Models.BDCModel.FC_Z_QSDC>.FillModel(dt.Rows[0]);
                }
                return null;
            }
        }

        internal static string GetOLDCGHouseID(string tSTYBM)
        {
            string sql = "select oldcghouseid from fc_h_qsdc where tstybm = '{0}'";
            sql = string.Format(sql, tSTYBM);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static void Delete_DY(ARCH_BDCDYDJ gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

                
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from ARCH_BDCDYDJ where dah='{0}'";
                    sql = string.Format(sql, gd.DAH);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

               
            }
        }

        internal static void Delete_YB(ARCH_FWYBDJ gd)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);

               
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "delete from ARCH_FWYBDJ where dah='{0}'";
                    sql = string.Format(sql, gd.DAH);

                    dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static List<Models.BDCModel.DJ_TSGL> GetTSGL(string slbh)
        {
            string sql = "select * from dj_tsgl where slbh like '{0}%' and bdclx<>'宗地'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.DJ_TSGL>.FillModel(dt);
            }
        }

       

        internal static List<DJ_XGDJGL> GetDJ_XGDJGL(string slbh)
        {
            //string sql = "select bdczh as 证号 from dj_djb where slbh='{0}' union select bdczmh as 证号 from dj_yg where slbh='{0}' union select bdczmh as 证号 from dj_dy where slbh='{0}' union select bdczmh as 证号 from dj_yy where slbh='{0}'";
            string sql = "select * from dj_xgdjgl where zslbh like  '{0}%' and xgzlx<>'土地证'";

            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.DJ_XGDJGL>.FillModel(dt);
                return null;
            }
        }

        internal static List<Models.BDCModel.FC_H_QSDC> GetFC_H_QSDC(string slbh)
        {
            //string sql =@ " select h.* from fc_h_qsdc h left join dj_tsgl t on t.tstybm=h.tstybm where t.slbh LIKE  '{0}%' and t.bdclx<>'宗地'";

            string sql = @"select distinct h.tstybm,h.BSM,h.zdtybm,h.zh,h.hh,h.bdcdyh,h.lsztybm,h.lsfwbh,nvl(fwxg.Qllx,h.qllx) as qllx,nvl(FWXG.QLXZ,h.qlxz) as qlxz,nvl(h.ghyt,fwxg.ghyt) as ghyt,nvl(sjd.zl,h.zl) as zl,
h.sjc,h.myc,h.dyh,h.fjh,h.ljzh,nvl(fwxg.qdjg,h.qdjg) as qdjg,nvl(fwxg.qdfs,h.qdfs) as qdfs,h.shbw,h.ycjzmj,h.yctnjzmj,h.ycdxbfjzmj,h.ycqtjzmj,h.ycftxs,
nvl(fwxg.jzmj,nvl(h.jzmj,h.ycjzmj)) as jzmj,nvl(fwxg.tnjzmj,nvl(h.tnjzmj,h.yctnjzmj)) as tnjzmj,nvl(fwxg.ftjzmj,nvl(h.ftjzmj,h.ycftjzmj)) as ftjzmj,nvl(fwxg.dxbfjzmj,nvl(h.dxbfjzmj,h.ycdxbfjzmj)) as dxbfjzmj,nvl(fwxg.qtjzmj,nvl(h.qtjzmj,h.ycqtjzmj)) as qtjzmj ,
h.ftxs,nvl(tdxg.zzrq,h.tdzzrq) as tdsyqr,nvl(tdxg.tdyt,h.tdyt) as tdyt,nvl(tdxg.tdsyqr,h.tdsyqr) as tdsyqr,nvl(tdxg.gytdmj,h.gytdmj),nvl(tdxg.fttdmj,h.fttdmj) as fttdmj,
nvl(tdxg.dytdmj,h.dytdmj) as dytdmj,h.fwlx,h.fwxz,h.sjcs,h.tcjs,h.cg,h.zt,h.oldbdcdyh,h.dymc,h.fjzb,h.zbtop,h.zbleft,h.zbright,h.zbbottom,h.hlx,h.csbsm,
h.xwbsm,h.yhbh,h.sharestate,h.bgjzxx,h.bgtsxx,h.lifecycle,h.yhid,h.transnum,h.fjbm,h.lacertnumfromfc,h.comesfromcg,h.gzwlx,h.bdcdyh_old,h.mark,h.hscid,
h.hycid,h.oracle_factmapping_id,h.oracle_houseinfo_id,h.h_hocode,h.oracle_wb_houseid,h.yghyt,h.oldcghouseid,h.shuoming,h.qxdm,h.yoldfczl
from fc_h_qsdc  h 
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
LEFT JOIN dj_sjd sjd on sjd.slbh=tsgl.slbh
left join ql_fwxg fwxg on fwxg.slbh=tsgl.slbh
left join ql_tdxg tdxg on tdxg.slbh=tsgl.slbh
where tsgl.slbh like '{0}%' and tsgl.bdclx<>'宗地' and sjd.djxl not like '%查封%' and tsgl.djzl <>'抵押注销'";


            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                return ModelHelper<Models.BDCModel.FC_H_QSDC>.FillModel(dt);
            }
        }

        internal static string GetGHYTMC(string ytbm)
        {
            string sql = "select ytmc from DIC_FWYTLX t where DICCODE=(SELECT DICCODE  FROM DIC_MAIN WHERE  DICNAME='房屋用途类型' ) and ytbm='{0}'";
            sql = string.Format(sql, ytbm);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);

                object o= dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if(null!=o)
                {
                    return o.ToString();
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        internal static string GetGHYTBM(string ytmc)
        {
            string sql = "select ytbm from DIC_FWYTLX t where DICCODE=(SELECT DICCODE  FROM DIC_MAIN WHERE  DICNAME='房屋用途类型' ) and ytmc='{0}'";
            sql = string.Format(sql, ytmc);
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);

                object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        internal static bool IsFW(string slbh)
        {
            bool flag = false;
            string sql = "select bdclx from dj_tsgl where slbh like '{0}%'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=dt && dt.Rows.Count>0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if(row[0].Equals("房屋"))
                        {
                            flag = true;
                        }
                    }
                }
                return flag;
            }

        }

        public static List<Models.BDCModel.WFM_ATTACHLST> GetWFM_ATTACHLST_SelectInfo(string slbh)
        {
            string sql = "SELECT * FROM WFM_ATTACHLST A WHERE A.PTYPE = '流程实例' AND A.PNODE = '{0}' AND A.PID IS NOT NULL AND A.CKIND = '文件夹' and revstate=1 and CNAME NOT LIKE '%人脸识别附件%'  ORDER BY A.FOLDERNUM, A.CSORT";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    dbHelper.Conn.Close();
                    return ModelHelper<Models.BDCModel.WFM_ATTACHLST>.FillModel(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        public static List<Models.BDCModel.WFM_ATTACHLST> GetWFM_ATTACHLST_AllInfo(string slbh)
        {
            string sql = "SELECT * FROM WFM_ATTACHLST A WHERE A.PTYPE = '流程实例' AND A.PNODE = '{0}' AND A.PID IS NOT NULL AND A.CKIND = '文件夹' and CNAME NOT LIKE '%人脸识别附件%'  ORDER BY A.FOLDERNUM, A.CSORT";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    dbHelper.Conn.Close();
                    return ModelHelper<Models.BDCModel.WFM_ATTACHLST>.FillModel(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }



        internal static string GetFZRQ(string slbh)
        {
            string sql = @"select fzrq
  from dj_djb
 where slbh = '{0}'
union
select fzrq
  from dj_yg
 where slbh = '{0}'
union
select fzrq
  from dj_dy
 where slbh = '{0}'
union
select fzrq
  from dj_yy
 where slbh = '{0}'
";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static List<Models.BDCModel.WFM_ATTACHLST> GetWFM_ATTACHLST(string slbh)
        {
            string sql = "SELECT A.CNAME, (SELECT COUNT(CID) FROM WFM_ATTACHLST WHERE CKIND = '文件' AND PID = A.CID) AS SL FROM WFM_ATTACHLST A WHERE A.PTYPE = '流程实例' AND A.PNODE = '{0}' AND A.PID IS NOT NULL AND A.CKIND = '文件夹' AND A.REVSTATE=1  ORDER BY A.FOLDERNUM, A.CSORT";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    dbHelper.Conn.Close();
                    return ModelHelper<Models.BDCModel.WFM_ATTACHLST>.FillModel(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal static string GetZSXLH(string slbh)
        {
            string sql = "select zsxlh  from dj_djb where slbh='{0}' union select zsxlh from dj_yg where slbh='{0}' union select zsxlh from dj_dy where slbh='{0}' union select zsxlh from dj_yy where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
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
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
            }

        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        internal static string GetDJLX(string sLBH)
        {
            string sql = @"
select nvl(djlx,djyy) as djlx from dj_djb where slbh like '{0}'
union select nvl(djlx,djyy) as djlx from dj_yg where slbh = '{0}'
union select nvl(djlx,djyy) as djlx from dj_dy where slbh = '{0}'
union select nvl(djlx,djyy) as djlx from dj_xgdjzx where slbh='{0}'";
            sql = string.Format(sql, sLBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static string GetBDCZH(string slbh)
        {
            string sql = @"select bdczh as 证号
  from dj_djb
 where slbh = '{0}'
union
select bdczmh as 证号
  from dj_yg
 where slbh = '{0}'
union
select bdczmh as 证号
  from dj_dy
 where slbh = '{0}'
union
select bdczmh as 证号
  from dj_yy
 where slbh = '{0}'
";


            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=o)
                {
                    return o.ToString();
                }
                return "";
            }
            
        }

        internal static DataTable GetCanInsertSLBH(string sql)
        {
            //string sql = "select slbh from dj_tsgl where djzl in ('预告','权属','抵押') and bdclx not like '%宗地%' and slbh not like '%FC%' order by slbh";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }

        }

        internal static string GetSlbhByBDCDYH(string bdcdyh, string slbh)
        {
            
            string sql = "select slbh from dj_tsgl where tstybm = '{0}' and slbh like '%{1}%' and bdclx<>'宗地'";
            sql = string.Format(sql, bdcdyh, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static string GetSlbhByBDCDYH(string bdcdyh,string slbh,string type)
        {
            string djzl = "";
            switch(type.ToLower())
            {
                case "c":
                    djzl = "'权属','预告'";
                    break;
                case "d":
                    djzl = "'抵押'";
                    break;
            }
            string sql = "select slbh from dj_tsgl where tstybm = '{0}' and slbh like '%{1}%' and djzl in ({2})  and bdclx<>'宗地'";
            sql = string.Format(sql, bdcdyh,slbh,djzl);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static DataTable GetXMSLBH(string prjId)
        {

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = "select prjid from WFM_PROCINST t where xmbh=(select xmbh from WFM_PROCINST where prjid = '{0}')  ";
                    sql = string.Format(sql, prjId);

                    DataTable dt= dbHelper.ExecuteTable(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                    return dt;

                }
                catch (Exception ex) { throw ex; }
                finally { dbHelper.Conn.Close();  }
            }
        }

        internal static string GetXGZH(string sLBH)
        {
            throw new NotImplementedException();
        }

        public static void Insert_HONEYPOT(HONEYPOT hy)
        {
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    string sql = dbHelper.CreateInsertStr<HONEYPOT>(hy, "HONEYPOT", MyDBType.Oracle);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<HONEYPOT>(hy, MyDBType.Oracle);

                    dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
                }
                catch (Exception ex) { throw ex; }
            }
        }

        public static void Insert_FC_DA_LOG(FC_DA_LOG hy)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                dbHelper.SetProvider(MyDBType.Oracle);
                string sql = dbHelper.CreateInsertStr<FC_DA_LOG>(hy, "FC_DA_LOG", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<FC_DA_LOG>(hy, MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
            }
            catch
            {

            }
        }
    }
}