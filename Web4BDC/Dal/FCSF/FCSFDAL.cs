using Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web4BDC.Models.FCSFModel;
using Web4BDC.Tools;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.ZZJFModel;

namespace Web4BDC.Dal.FCSF
{
    public class FCSFDAL
    {

        private static readonly object lockKey = new object();
        internal static Models.FCSFModel.FpmxList_tyfp GetFpmxList_tyfp(string slbh)
        {
            throw new NotImplementedException();
        }


        internal static List<DJ_SFD> GetSFD(string slbh)
        {
            string sql = "select * from dj_sfd where slbh = '{0}'";
            sql = string.Format(sql,slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<DJ_SFD>.FillModel(dt);
                return null;
            }
        }

        internal static List<DJ_SFD_FB> GetSFDFB(string slbh)
        {
            string sql = "select * from dj_sfd_fb where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<DJ_SFD_FB>.FillModel(dt);
                return null;
            }
        }

        internal static void PushSF(FpList_tyfp fpList, List<FpmxList_tyfp> fpmx)
        {
            //using (var scope = new System.Transactions.TransactionScope())
            //{
            bool canUpdate = IsCanUpdate(fpList.Ywzh);
            if (ExistFpList(fpList.Ywzh))
            {
                if (canUpdate)
                    UpdateFpList(fpList);
            }
            else
            {
                InsertFpList(fpList);

            }

            if (null != fpmx && fpmx.Count > 0)
            {
                foreach (var fp in fpmx)
                {
                    if (ExistFpmx(fp.Ywzh,fp.类型))
                    {
                        if (canUpdate)
                        {
                            DeleteFpmx(fp);
                            InsertFpmx(fp);
                        }
                            
                    }
                    else
                    {
                        InsertFpmx(fp);

                    }
                }
            }

            //    scope.Complete();
            //}
        }

        private static bool IsCanUpdate(string ywzh)
        {
            string sql = "select count(1) from FpList_tyfp where ywzh='{0}' and SFState=0";
            sql = string.Format(sql, ywzh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                object o = dbHelper.ExecuteScalar(MyDBType.Other, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static List<Models.BDCModel.DJ_QLRGL> GetDyqr(string p)
        {
            string sql = "select gl.* from dj_qlrgl gl left join dj_dy dy on dy.slbh=gl.slbh where gl.slbh= '{0}'";
            
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.DJ_QLRGL>.FillModel(dt);
                return null;
            }
        }

        internal static void InsertFpmx(FpmxList_tyfp fpmx)
        {
            string sql = "insert into FpmxList_tyfp (Ywzh,类型,实收费用,IsNew) " +
               "values(@Ywzh,@lx,@ssfy,@IsNew)";
            List<DbParameter> list = new List<DbParameter>();
            Sql_ListAdd(list, "@Ywzh", fpmx.Ywzh);
            Sql_ListAdd(list, "@lx", fpmx.类型);
            
            Sql_ListAdd(list, "@ssfy", fpmx.实收费用);
            Sql_ListAdd(list, "@IsNew", fpmx.IsNew);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                dbHelper.ExecuteNonQuery(MyDBType.Other, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        internal static int CheckIsXCJF(string prjId)
        {
            string sql = "select count(1) from dj_sfd where slbh='{0}' and ZZSFZT is null";
            sql = string.Format(sql, prjId);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return Convert.ToInt32(o);
                return 0;
            }
        }

        internal static DataTable GetSFSLBH(string prjId)
        {
            string sql = "select slbh from dj_sfd where slbh like '{0}%' and SFZT is null";
            sql = string.Format(sql, prjId);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }

        }

        internal static void UpdateSFD_SFZT(DJ_SFD sfd)
        {
            DbHelper dbHelper = new DbHelper();
            string keyStr = "SLBH='" + sfd.SLBH + "'";
            string sql = dbHelper.CreateUpdateStr<DJ_SFD>(sfd, "DJ_SFD", keyStr, MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<DJ_SFD>(sfd, MyDBType.Oracle);
            dbHelper.SetProvider(MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
        }

        internal static void UpdateSFD_SFZT(JFStateModel model)
        {
            string sql = "update dj_sfd set ZZSFZT='1',ZZSFZFFS='{1}',dyr='自助缴费',dysj='{2}',SSJE={3},SKRQ='{4}' where slbh='{0}'";

            sql = string.Format(sql,model.SLBH,model.ZFFS,model.ZFSJ,model.ZFJE,model.ZFSJ);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static void UpdateFpmx(FpmxList_tyfp fpmx)
        {
            string sql = "update FpmxList_tyfp set 实收费用=@ssfy,IsNew=@IsNew where Ywzh=@Ywzh and 类型=@lx";
            List<DbParameter> list = new List<DbParameter>();
            
            
            Sql_ListAdd(list, "@ssfy", fpmx.实收费用);
            
            Sql_ListAdd(list, "@IsNew", fpmx.IsNew);
            Sql_ListAdd(list, "@Ywzh", fpmx.Ywzh);
            Sql_ListAdd(list, "@lx", fpmx.类型);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                dbHelper.ExecuteNonQuery(MyDBType.Other, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        internal static List<DJ_SFD> GetXCSFState(string prjId)
        {
            string sql = "select * from dj_sfd where slbh like '{0}'%";
            sql = string.Format(sql, prjId);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
            return ModelHelper<DJ_SFD>.FillModel(dt);

        }

        internal static void DeleteFpmx(FpmxList_tyfp fpmx)
        {
            string sql = "delete from FpmxList_tyfp where Ywzh=@Ywzh ";
            List<DbParameter> list = new List<DbParameter>();
            Sql_ListAdd(list, "@Ywzh", fpmx.Ywzh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                dbHelper.ExecuteNonQuery(MyDBType.Other, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        internal static void UpdateTag(FC_SF_TAG tag)
        {
            //string sql = "update FC_SF_TAG set ID=@ID,PUSHUSER=@PUSHUSER,PUSHDATE=@PUSHDATE,ISSUCCESS=@ISSUCCESS,MESSAGE=@MESSAGE where SLBH=@SLBH";
            
            //List<DbParameter> list = new List<DbParameter>();
            //ListAdd(list, "@ID", tag.ID);
            //ListAdd(list, "@PUSHUSER", tag.PUSHUSER);
            //ListAdd(list, "@PUSHDATE", tag.PUSHDATE);
            //ListAdd(list, "@ISSUCCESS", tag.ISSUCCESS);
            //ListAdd(list, "@MESSAGE", tag.MESSAGE);
            //ListAdd(list, "@SLBH", tag.SLBH);

            string sql = "update FC_SF_TAG set ID='{0}',PUSHUSER='{1}',PUSHDATE=to_date('{2}','yyyy-mm-dd hh24:mi:ss'),ISSUCCESS={3},MESSAGE='{4}' where SLBH='{5}'";
            sql = string.Format(sql, tag.ID, tag.PUSHUSER, tag.PUSHDATE, tag.ISSUCCESS, tag.MESSAGE, tag.SLBH);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                //dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static bool ExistTag(FC_SF_TAG tag)
        {
            string sql = "select count(1) from FC_SF_Tag where slbh='{0}'";
            sql = string.Format(sql, tag.SLBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static List<FC_SF_TAG> GetFailTag()
        {
            string sql = "select ID,SLBH,PUSHUSER,PUSHDATE,ISSUCCESS,MESSAGE from FC_SF_TAG where ISSUCCESS<>'1'";
            
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.FC_SF_TAG>.FillModel(dt);
                return null;
            }
        }
        internal static string GetDJLXBySlbh(string slbh)
        {
            string sql = "select DJDL from dj_sjd where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null !=o)
                    return o.ToString();
                return null;
            }
        }


        internal static bool ExistFpmx(string ywzh,string lx)
        {
            string sql = "select count(1) from FpmxList_tyfp where ywzh='{0}' and 类型='{1}'";
            sql = string.Format(sql, ywzh,lx);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                object o = dbHelper.ExecuteScalar(MyDBType.Other, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static decimal? GetFJZMJ(string p)
        {
            throw new NotImplementedException();
        }

        internal static decimal GetPgjz(string p)
        {
            string sql = "select BDBZZQSE from dj_dy where slbh like '%{0}%'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return Convert.ToDecimal(o);
                return 0;
            }
        }

        internal static string GetQzbh(string p)
        {
            string sql = "select bdczh as 证号 from dj_djb where slbh='{0}' union select bdczmh as 证号 from dj_yg where slbh='{0}' union select bdczmh as 证号 from dj_dy where slbh='{0}' union select bdczmh as 证号 from dj_yy where slbh='{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return o.ToString();
                return "";
            }
        }

        internal static List<Models.BDCModel.FC_H_QSDC> GetFC_H_QSDC_List(string p)
        {
            string sql = "select qsdc.* from dj_tsgl tsgl left join fc_h_qsdc  qsdc on tsgl.tstybm=qsdc.tstybm where tsgl.slbh = '{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.FC_H_QSDC>.FillModel(dt);
                return null;
            }
        }

        

        internal static string GetYsyqr(string p)
        {
            string sql = "select qlrgl.qlrmc from dj_xgdjgl djgl left join dj_qlrgl qlrgl on qlrgl.slbh=djgl.fslbh where djgl.zslbh='{0}' and qlrgl.qlrlx='权利人'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt)
                {
                    if (dt.Rows.Count > 1)
                    {
                        string name = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            if (name.Equals(""))
                            {
                                name += row[0].ToString();
                            }
                            else
                            {
                                name += "," + row[0];
                            }
                        }
                        return name;
                    }
                    if(dt.Rows.Count==1)
                    {
                        return dt.Rows[0][0].ToString();
                    }

                }
                return "";
            }
        }


        internal static bool ExistFpList(string p)
        {
            string sql = "select count(1) from FpList_tyfp where ywzh='{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                object o = dbHelper.ExecuteScalar(MyDBType.Other, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static void InsertFpList(FpList_tyfp fpList)
        {
            string sql = "insert into FpList_tyfp (Ywzh,syqr,dyqr,qzbh,djlx,zl,zlxz,FJZMJ,TS,Pgjz,SFState,IsNew,ysyqr) " +
                "values(@Ywzh,@syqr,@dyqr,@qzbh,@djlx,@zl,@zlxz,@FJZMJ,@TS,@Pgjz,@SFState,@IsNew,@ysyqr)";
            List<DbParameter> list = new List<DbParameter>();
            Sql_ListAdd(list, "@Ywzh", fpList.Ywzh);
            Sql_ListAdd(list, "@syqr", fpList.syqr);
            Sql_ListAdd(list, "@dyqr", fpList.dyqr);
            Sql_ListAdd(list, "@qzbh", fpList.qzbh);
            Sql_ListAdd(list, "@djlx", fpList.djlx);
            Sql_ListAdd(list, "@zl", fpList.zl);
            Sql_ListAdd(list, "@zlxz", fpList.zlxz);
            Sql_ListAdd(list, "@FJZMJ", fpList.FJZMJ);
            Sql_ListAdd(list, "@TS", fpList.TS);
            Sql_ListAdd(list, "@Pgjz", fpList.Pgjz);
            Sql_ListAdd(list, "@SFState", fpList.SFState);
            Sql_ListAdd(list, "@IsNew", fpList.IsNew);
            Sql_ListAdd(list, "@ysyqr", fpList.ysyqr);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                dbHelper.ExecuteNonQuery(MyDBType.Other, System.Data.CommandType.Text, sql, list.ToArray());
            }

        }

        internal static void UpdateFpList(FpList_tyfp fpList)
        {
            string sql = "update FpList_tyfp set syqr=@syqr,dyqr=@dyqr,qzbh=@qzbh,djlx=@djlx,zl=@zl,zlxz=@zlxz,FJZMJ=@FJZMJ,TS=@TS,Pgjz=@Pgjz,SFState=@SFState,IsNew=@IsNew,ysyqr=@ysyqr where Ywzh=@Ywzh";
            List<DbParameter> list = new List<DbParameter>();

            Sql_ListAdd(list, "@syqr", fpList.syqr);
            Sql_ListAdd(list, "@dyqr", fpList.dyqr);
            Sql_ListAdd(list, "@qzbh", fpList.qzbh);
            Sql_ListAdd(list, "@djlx", fpList.djlx);
            Sql_ListAdd(list, "@zl", fpList.zl);
            Sql_ListAdd(list, "@zlxz", fpList.zlxz);
            Sql_ListAdd(list, "@FJZMJ", fpList.FJZMJ);
            Sql_ListAdd(list, "@TS", fpList.TS);
            Sql_ListAdd(list, "@Pgjz", fpList.Pgjz);
            Sql_ListAdd(list, "@SFState", fpList.SFState);
            Sql_ListAdd(list, "@IsNew", fpList.IsNew);
            Sql_ListAdd(list, "@ysyqr", fpList.ysyqr);

            Sql_ListAdd(list, "@Ywzh", fpList.Ywzh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                dbHelper.ExecuteNonQuery(MyDBType.Other, System.Data.CommandType.Text, sql, list.ToArray());
            }

        }

        internal static void InsertLog(Models.BDCModel.FC_SF_TAG tag)
        {
           
            string sql = "insert into FC_SF_TAG (ID,SLBH,PUSHUSER,PUSHDATE,ISSUCCESS,MESSAGE) values(:ID,:SLBH,:PUSHUSER,:PUSHDATE,:ISSUCCESS,:MESSAGE)";
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

        private static void Sql_ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new SqlParameter(paraName, DBNull.Value));
            else
                list.Add(new SqlParameter(paraName, value));
        }

        internal static bool CheckSFState(string p)
        {
            string sql = "select count(1) from FpList_tyfp where Ywzh='{0}' and SFState='1'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Other);
                object o = dbHelper.ExecuteScalar(MyDBType.Other, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }
    }
}