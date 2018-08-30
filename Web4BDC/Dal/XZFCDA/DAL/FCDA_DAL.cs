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
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using Web4BDC.FC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.FCDAModel;

namespace Web4BDC.Dal
{
    public class FCDA_DAL
    {

        private static readonly object lockKey = new object();

        public static DataTable GetDeleteInfo(string slbh)
        {
            string sql = @"SELECT [ArchiveId],busino
  FROM [dbo].[ArchiveIndex] WITH (updlock) 
  where busino like '{0}%' and isOld = 2 and ([SerialNo] is null or [SerialNo]='')";
            sql = string.Format(sql, slbh);

            //            string sql = @"select * from archiveindex where archiveid in (
            //select archiveid from person where persontype=1 group by archiveid having(count(*)>1)
            //) and busino like '201710%'
            //order by busino";
            //sql = string.Format(sql, slbh);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Sql);
            return dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }

        public static void deleteRecode(string busino, Guid archiveId)
        {
            string[] arr = new string[] { string.Format(@"DELETE FROM [dbo].ArchiveIndex where archiveid='{0}'",archiveId),
            string.Format(@"DELETE FROM [dbo].HouseInfo WHERE BusiNo = '{0}'",busino),
             string.Format(@"DELETE FROM [dbo].HouseArchiveRelation WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].[Certificate] WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].PropArchiveRelation WHERE archiveid='{0}'", archiveId),
            string.Format(@"DELETE FROM [dbo].Person WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].VolEleArc WHERE archiveid='{0}'", archiveId) };

            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Sql);

            for (int i = 0; i < arr.Length; i++)
            {
                string sql = arr[i];
                
                dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            }


        }

        private static string GetNumber(string str)
        {
            //string tmpStr = string.Empty;
            if(str.Contains("号"))
            {
                str = str.Substring(0, str.IndexOf("号"));
            }
            Regex r = new Regex("\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(str);
            MatchCollection mc = r.Matches(str);

            string result = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                result += mc[i];//匹配结果是完整的数字，此处可以不做拼接的
            }
            return result;
        }

        internal static void InsertBDCLog(BDCDALog log)
        {
            throw new NotImplementedException();
        }

        public static string Insert(ArchiveIndex archiveIndex, List<VolEleArc> volEleArc, List<HouseInfo> houseInfo_list, List<Person> persons, List<Certificate> certificate_list, List<HouseArchiveRelation> houseArchiveRelation_list, List<PropArchiveRelation> propArchiveRelation_list, List<VolEleArcDtl> volEleArcDtl)
        {

            string errinfo = "";
            string hid = string.Empty;
            string tstybm = string.Empty;
            string zl = string.Empty;
            string qlrmc = string.Empty;
            string prop = string.Empty;

            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Sql);
            dbHelper.CreateConn();
            if (dbHelper.Conn.State != System.Data.ConnectionState.Open)
            {
                dbHelper.Conn.Open();
            }
            using (DbTransaction trans = dbHelper.Conn.BeginTransaction(System.Data.IsolationLevel.Snapshot))
            {
                try
                {
                    //如果是注销业务
                    if (archiveIndex.ArchiveType.ToUpper().Equals("Z"))
                    {
                        if (!archiveIndex.DJZL.Equals("抵押注销"))
                        {
                            //判断相关登记关联表记录个数
                            if (null != archiveIndex.FSLBH_List && archiveIndex.FSLBH_List.Count > 0)
                                UpdateHistory(archiveIndex, persons, certificate_list, dbHelper, trans);
                        }
                    }
                    else
                    {
                        //权属、抵押业务
                        //判断是否需要更新上一手的历史状态
                        if (IsNeedUpdate(archiveIndex))
                        {
                            UpdateHistory(archiveIndex, persons, certificate_list, dbHelper, trans);
                        }


                        // 插入房产档案信息表
                        Insert_ArchiveIndex(archiveIndex, dbHelper, trans);

                        //插入条目表
                        foreach (VolEleArc item in volEleArc)
                        {

                            Insert_VolEleArc(item, dbHelper, trans);

                        }

                        //插入房屋表


                        foreach (HouseInfo houseInfo in houseInfo_list)
                        {
                            hid += " " + houseInfo.HouseInfo_ID;
                            tstybm += " " + houseInfo.TSTYBM;
                            zl += " " + houseInfo.HoSite;
                            Insert_HouseInfo(houseInfo, dbHelper, trans);

                        }


                        //插入权利人表
                        foreach (Person person in persons)
                        {
                            qlrmc += " " + person.Name;
                            Insert_Person(person, dbHelper, trans);

                        }
                        //插入权证表

                        foreach (Certificate certificate in certificate_list)
                        {
                            prop += " " + certificate.Prop;
                            Insert_Certificate(certificate, dbHelper, trans);

                        }


                        //插入房屋关联表
                        foreach (var houseArchiveRelation in houseArchiveRelation_list)
                        {

                            Insert_HouseArchiveRelation(houseArchiveRelation, dbHelper, trans);

                        }


                        //插入权证关联表
                        foreach (var propArchiveRelation in propArchiveRelation_list)
                        {

                            Insert_PropArchiveRelation(propArchiveRelation, dbHelper, trans);

                        }
                        //插入附件表
                        if (null != volEleArcDtl)
                        {
                            foreach (VolEleArcDtl v in volEleArcDtl)
                            {

                                Insert_VolEleArcDtl(v, dbHelper, trans);

                            }
                        }


                    }
                    InsertNewLog(archiveIndex, hid, tstybm, zl, prop, "成功", "");

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    errinfo = ex.Message;
                    InsertNewLog(archiveIndex, hid, tstybm, zl, prop, "失败", errinfo);
                    trans.Rollback();
                }
                finally
                {
                    dbHelper.CloseConn();
                }


                //ts.Complete();

            }

            return errinfo;
        }


        public static string Insert(ArchiveIndex archiveIndex, List<VolEleArc> volEleArc, List<HouseInfo> houseInfo_list, List<Person> persons, List<Certificate> certificate_list, List<HouseArchiveRelation> houseArchiveRelation_list, List<PropArchiveRelation> propArchiveRelation_list, List<VolEleArcDtl> volEleArcDtl, bool isflag)
        {
            string errinfo = "";
            string hid = string.Empty;
            string tstybm = string.Empty;
            string zl = string.Empty;
            string qlrmc = string.Empty;
            string prop = string.Empty;


            try
            {
                //如果是注销业务
                if (archiveIndex.ArchiveType.ToUpper().Equals("Z"))
                {
                    if (!archiveIndex.DJZL.Equals("抵押注销"))
                    {
                        //判断相关登记关联表记录个数
                        if (null != archiveIndex.FSLBH_List && archiveIndex.FSLBH_List.Count > 0)
                            UpdateHistory(archiveIndex, persons, certificate_list);
                    }

                }
                else
                {
                    //权属、抵押业务
                    //判断是否需要更新上一手的历史状态
                    if (IsNeedUpdate(archiveIndex))
                    {
                        UpdateHistory(archiveIndex, persons, certificate_list);
                    }


                    // 插入房产档案信息表
                    Insert_ArchiveIndex(archiveIndex);

                    //插入条目表
                    foreach (VolEleArc item in volEleArc)
                    {
                        Insert_VolEleArc(item);
                    }

                    //插入房屋表


                    foreach (HouseInfo houseInfo in houseInfo_list)
                    {
                        hid += " " + houseInfo.HouseInfo_ID;
                        tstybm += " " + houseInfo.TSTYBM;
                        zl += " " + houseInfo.HoSite;
                        Insert_HouseInfo(houseInfo);

                    }


                    //插入权利人表
                    foreach (Person person in persons)
                    {
                        qlrmc += " " + person.Name;
                        Insert_Person(person);

                    }
                    //插入权证表

                    foreach (Certificate certificate in certificate_list)
                    {
                        prop += " " + certificate.Prop;
                        Insert_Certificate(certificate);

                    }

                    //插入房屋关联表
                    foreach (var houseArchiveRelation in houseArchiveRelation_list)
                    {

                        Insert_HouseArchiveRelation(houseArchiveRelation);

                    }


                    //插入权证关联表
                    foreach (var propArchiveRelation in propArchiveRelation_list)
                    {

                        Insert_PropArchiveRelation(propArchiveRelation);

                    }
                    //插入附件表
                    if (null != volEleArcDtl)
                    {
                        foreach (VolEleArcDtl v in volEleArcDtl)
                        {

                            Insert_VolEleArcDtl(v);

                        }
                    }


                }
                InsertNewLog(archiveIndex, hid, tstybm, zl, prop, "成功", "");


            }
            catch (Exception ex)
            {
                errinfo = ex.Message;
                InsertNewLog(archiveIndex, hid, tstybm, zl, prop, "失败", errinfo);
                throw ex;

            }

            return errinfo;
        }

        private static void UpdateHistory(ArchiveIndex archiveIndex, List<Person> persons, List<Certificate> certificate_list, DbHelper dbHelper, DbTransaction trans)
        {

            //查找抵押注销关联信息
            //DJ_XGDJGL res = archiveIndex.FSLBH_List.Find(u => u.XGZLX.Contains("抵押证明"));
            //if (null != res)
            //{
            //    //是抵押注销
            //    string fmbusino = res.FSLBH;
            //    archiveIndex.FmBusiNo = GetRealSLBH(fmbusino);
            //}
            //else
            //{
                //通过权证号、权利人姓名、权利人证件号码等查询
            List<string> oldYwzh = GetOldYwzh(archiveIndex.BusiNO, persons);
            //权属注销
            if (null!=oldYwzh && oldYwzh.Count>0)
            {
                foreach (string fmbusino in oldYwzh)
                {
                    if (!string.IsNullOrEmpty(fmbusino))
                    {
                        archiveIndex.FmBusiNo = fmbusino;
                        Update_ArchiveIndex(archiveIndex, dbHelper, trans);
                    }
                }
            }
               
                //判断上一手业务宗号是否在房产档案库
                //if (CanPust(archiveIndex.FmBusiNo))
                //{
                //    //档案库不存在上一手的业务宗号
                //    archiveIndex.FmBusiNo = oldYwzh;
                //}
                //else
                //{
                //    if (!archiveIndex.FmBusiNo.Trim().Equals(oldYwzh))
                //    {
                //        //业务宗号不一致
                //        archiveIndex.FmBusiNo = string.Empty;
                //    }
                //}
            //}
            //更新上一手业务的历史状态为1
            //if (!string.IsNullOrEmpty(archiveIndex.FmBusiNo))
            //    Update_ArchiveIndex(archiveIndex, dbHelper, trans);
        }

        private static void UpdateHistory(ArchiveIndex archiveIndex, List<Person> persons, List<Certificate> certificate_list)
        {


            //通过权证号、权利人姓名、权利人证件号码等查询
            List<string> oldYwzh = GetOldYwzh(archiveIndex.BusiNO, persons);
            //权属注销
            if (null != oldYwzh && oldYwzh.Count > 0)
            {
                foreach (string fmbusino in oldYwzh)
                {
                    if (!string.IsNullOrEmpty(fmbusino))
                    {
                        archiveIndex.FmBusiNo = fmbusino;
                        Update_ArchiveIndex(archiveIndex);
                    }
                }
            }
            else
            {
                InsertNewLog(archiveIndex, "", "", "", "", "失败", "档案库中未找到对应的证号和权利人！");
                //throw new Exception("档案库中未找到对应的证号和权利人！");
            }

        }
        private static List<string> GetOldYwzh(string busiNO,  List<Person> persons)
        {
            List<string> list = new List<string>();
            //bool isFind = false;
            //string ywzh=string.Empty;
            DataTable dt = GetFmbusino(busiNO);
            foreach (DataRow cer in dt.Rows)
            {
                foreach (Person per in persons)
                {
                    string ywzh = GetHistroyYWZH(busiNO, cer["XGZH"].ToString(), per.Name, per.CardNO, true);
                    if(!string.IsNullOrEmpty(ywzh))
                    {
                        if(!list.Contains(ywzh))
                        list.Add(ywzh);
                    }
                }
               
                
            }
            return list;
          
        }

        private static void InsertNewLog(ArchiveIndex archiveIndex, string hid, string tstybm, string zl, string prop,string state,string errMes)
        {
            DbHelper BDCHelper = new DbHelper();
            BDCHelper.SetProvider(MyDBType.Oracle);
            BDCHelper.CreateConn();
            if (BDCHelper.Conn.State != System.Data.ConnectionState.Open)
            {
                BDCHelper.Conn.Open();
            }
            using (DbTransaction bdctrans = BDCHelper.Conn.BeginTransaction())
            {

                try
                {
                    FC_DA_NewLog newLog = new FC_DA_NewLog();
                    newLog.UUID = Guid.NewGuid().ToString();
                    newLog.Busino = archiveIndex.BusiNO;
                    newLog.HID = hid.Trim().Length>1024?hid.Trim().Substring(0,1023):hid.Trim();
                    newLog.Prop = prop.Trim().Length > 1024 ? prop.Trim().Substring(0, 1023) : prop.Trim();
                    newLog.ZL = zl.Trim().Length > 1024 ? zl.Trim().Substring(0, 1023) : zl.Trim();
                    newLog.Tstybm = tstybm.Trim().Length > 1024 ? tstybm.Trim().Substring(0, 1023) : tstybm.Trim();
                    newLog.State = state;
                    newLog.ErrMessage = errMes;
                    BDCDA_DAL.InsertPushDALog(newLog, BDCHelper, bdctrans);
                    bdctrans.Commit();
                }
                catch (Exception ex)
                {
                    bdctrans.Rollback();
                    throw ex;
                }
                finally
                {
                    BDCHelper.CloseConn();
                }
            }
        }

        internal static DataTable GetAllFailWB()
        {
            string sql = "select slbh from FC_SPFHX_TAG where SFTS<>1";

            //string sql= "select prjid slbh from WFM_PROCINST t where t.procname like ' % 抵押注销 % ' and prjstate='已完成'";
            try
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex) { throw ex; }
        }

        private static DataTable GetFmbusino(string busiNO)
        {
            string sql = @"select djgl.fslbh,djgl.xgzh,qlr.qlrmc,qlr.zjhm from dj_qlrgl qlrgl
left join dj_xgdjgl djgl on djgl.fslbh=qlrgl.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where djgl.zslbh='{0}' and djgl.xgzlx<>'土地证' and djgl.xgzlx not like '%抵押%' and qlrgl.qlrlx='权利人'";
            sql = string.Format(sql, busiNO);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                 return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                
            }
        }


        private static string GetHistroyYWZH(string slbh, string prop, string name, string cardNO, bool isZX)
        {
            if (string.IsNullOrEmpty(prop) && isZX)
            {
                prop = GetXGZH(slbh);
            }
            else
            {
                if (!prop.Contains("不动产"))
                {
                    string propTmp = ReplaceProp(prop);
                    prop = GetNumber(propTmp);
                }
            }
            DataTable dt = GetCountFromFC(prop, name);
           if(null!=dt && dt.Rows.Count==1)
            {
                return dt.Rows[0]["业务宗号"].ToString();
            }
           else
            {
                return GetZXFmbusino(slbh, prop, name, cardNO, isZX);
            }
        }


       private static DataTable GetCountFromFC(string prop,string qlrmc)
        {
           string sql = "select distinct 业务宗号 from [dbo].[vw_档案信息查询] WITH (updlock)  where 1=1  ";
            if (prop.Contains("不动产"))
            {
                sql += " and 不动产证号 = '" + prop + "' ";
            }
            else
            {
                string propTmp = ReplaceProp(prop);
                sql += " and 权证号 like '%" + GetNumber(propTmp) + "%'";
            }
            sql += " and 姓名='" + qlrmc + "' ";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return dt;
            }

        }




        private static string GetZXFmbusino(string slbh,string prop, string name, string cardNO,bool isZX)
        {
//            string sql = @"SELECT a.busino
//  FROM [dbo].[ArchiveIndex] as a
//  left join[dbo].[HouseArchiveRelation] as hr on hr.[ArchiveId]=a.[ArchiveId]
//left join[dbo].[HouseInfo] as h on h.[HouseInfo_ID]=hr.[HouseInfo_ID]
//left join[dbo].[PropArchiveRelation] as pr on pr.[ArchiveId]=a.[ArchiveId]
//left join[dbo].[Certificate] as c on c.[CertificateID]=pr.[CertificateID]
//left join[dbo].[Person] as p on p.[ArchiveId]=a.[ArchiveId]
//where c.prop like  '%{0}%' and p.Name like '%{1}%' ;

            string sql = "select distinct 业务宗号 from [dbo].[vw_档案信息查询] WITH (updlock) where 1=1  ";

            sql += GetSelectWhere(slbh,prop, name, cardNO,isZX);

            //sql = string.Format(sql, prop);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object ywzh= dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if(null!=ywzh)
                {
                    return ywzh.ToString();
                }
                else
                    return "";
            }
        }

        private static string GetSelectWhere(string slbh,string prop,string name,string zjhm,bool isZX)
        {
            string str = "";
            
            if(prop.Contains("不动产权"))
            {
                str += " and 不动产证号 like '%" + prop + "%' ";
            }
            else
            {
                string propTmp = ReplaceProp(prop);
                str += " and 权证号 like '%" + GetNumber(prop) + "%'";
            }
            //if(!string.IsNullOrEmpty(name))
            //{
                str += " and 姓名='" + name + "' ";
            //}
            if (!string.IsNullOrEmpty(zjhm) && zjhm!="无")
            {
                string oldCardID = Get15CardNo(zjhm);
                str += " and (证件号码='" + zjhm + "' or 证件号码='" + oldCardID + "' )";
            }
            return str;
        }

        private static string ReplaceProp(string prop)
        {
            string qz = "B{0}S";
            string tx = "B{0}T";
            for (int i = 1; i < 6; i++)
            {
                

                string tmp1 = string.Format(qz, i);
                string tmp2 = string.Format(tx, i);
                prop = prop.Replace(tmp1, "").Replace(tmp2,"");
            }
            if (prop.Contains("-"))
                prop = prop.Substring(0, prop.IndexOf("-"));
            return prop;
        }

        private static string GetXGZH(string slbh)
        {
            string sql = @"select gl.xgzh from dj_xgdjzx zx 
left join dj_xgdjgl gl on gl.zslbh = zx.slbh
where zx.slbh = '{0}' and gl.xgzlx <> '土地证'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object count = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if(null!=count)
                {
                    return count.ToString();
                }
                return "";
            }
        }

        private static string Get15CardNo(string cardID)
        {
            if(cardID.Length==18)
            {
                cardID = cardID.Substring(0, 6) + cardID.Substring(8, 9);
            }
            return cardID;
        }

        private static bool IsNeedUpdate(ArchiveIndex archiveIndex)
        {
            if (archiveIndex.DJZL.Contains("注销") && null != archiveIndex.FmBusiNo && !archiveIndex.FmBusiNo.Equals("")&& !archiveIndex.DJZL.Equals("抵押注销"))
                return true;
            if (null != archiveIndex.FmBusiNo && !archiveIndex.FmBusiNo.Equals("") && archiveIndex.ArchiveType == "C")
                return true;
            return false;
        }



        private static int Insert_VolEleArcDtl(VolEleArcDtl volEleArcDtl,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into VolEleArcDtl(VolEleArcDtl_id,VolEleArc_ID,imgName,PageNo,ScanDate) " +
                "values(@VolEleArcDtl_id,@VolEleArc_ID,@imgName,@PageNo,@ScanDate)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@VolEleArcDtl_id", volEleArcDtl.VolEleArcDtl_id);
            ListAdd(list, "@VolEleArc_ID", volEleArcDtl.VolEleArc_ID);
            ListAdd(list, "@imgName", volEleArcDtl.imgName);
            ListAdd(list, "@PageNo", volEleArcDtl.PageNo);
            ListAdd(list, "@ScanDate", volEleArcDtl.ScanDate);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                return dbHelper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static int Insert_PropArchiveRelation(PropArchiveRelation propArchiveRelation,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into PropArchiveRelation(RelationID,ArchiveId,CertificateID) "+
                "values(@RelationID,@ArchiveId,@CertificateID)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", propArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", propArchiveRelation.ArchiveId);
            ListAdd(list,"@CertificateID", propArchiveRelation.CertificateID);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                return dbHelper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        
        }

        

        internal static bool CheckCertificate(string bdczh)
        {
            string sql = "select count(1) from Certificate WITH (updlock) where Prop='{0}'";
            sql = string.Format(sql, bdczh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object count = dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (Convert.ToInt32(count) > 0)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        private static int Insert_HouseArchiveRelation(HouseArchiveRelation houseArchiveRelation,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into HouseArchiveRelation(RelationID,ArchiveId,HouseInfo_ID,BusiNO) " +
                "values(@RelationID,@ArchiveId,@HouseInfo_ID,@BusiNO)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@RelationID", houseArchiveRelation.RelationID);
            ListAdd(list, "@ArchiveId", houseArchiveRelation.ArchiveId);
            ListAdd(list, "@HouseInfo_ID", houseArchiveRelation.HouseInfo_ID);
            ListAdd(list, "@BusiNO", houseArchiveRelation.BusiNO);

            lock (lockKey)
            {
               
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static int Insert_Certificate(Certificate certificate,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into Certificate(CertificateID,HouseInfo_ID,Prop,PrintNO,CertificateType,GrantDate,PersonID,ArchiveId) " +
                 "values(@CertificateID,@HouseInfo_ID,@Prop,@PrintNO,@CertificateType,@GrantDate,@PersonID,@ArchiveId)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@CertificateID", certificate.CertificateID);
            ListAdd(list, "@HouseInfo_ID", certificate.HouseInfo_ID);
            ListAdd(list, "@Prop", certificate.Prop);
            ListAdd(list, "@PrintNO", certificate.PrintNO);
            ListAdd(list, "@CertificateType", certificate.CertificateType);
            ListAdd(list, "@GrantDate", certificate.GrantDate);
            ListAdd(list, "@PersonID", certificate.PersonID);
            ListAdd(list, "@ArchiveId", certificate.ArchiveId);

            lock (lockKey)
            {
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        

        internal static string GetUserNameFromTag(string slbh)
        {
            string sql = "select PushUser from fc_da_tag where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object count = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null!=count)
                {
                    return count.ToString();
                }
                else
                    return "guidangren";
            }
        }

        private static int Insert_Person(Person person,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into Person(PersonID,ArchiveId,Name,CardNO,PersonType,IDCardType,RightMan_ID,Sex) " +
               "values(@PersonID,@ArchiveId,@Name,@CardNO,@PersonType,@IDCardType,@RightMan_ID,@Sex)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@PersonID", person.PersonID);
            ListAdd(list, "@ArchiveId", person.ArchiveId);
            ListAdd(list, "@Name", person.Name);
            ListAdd(list, "@CardNO", person.CardNO);
            ListAdd(list, "@PersonType", person.PersonType);
            ListAdd(list, "@IDCardType", person.IDCardType);
            ListAdd(list, "@RightMan_ID", person.RightMan_ID);
            ListAdd(list, "@Sex", person.Sex);

            lock (lockKey)
            {
                
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static int Insert_HouseInfo(HouseInfo houseInfo,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into HouseInfo(HouseInfo_ID,H_HoUse,HoSite,H_ConAcre,I_ItSite,I_ItName,BuNum,BuName,H_CeCode,H_RoNum,H_CurLay,H_HoStru,CHID,BusiNo) " +
                "values(@HouseInfo_ID,@H_HoUse,@HoSite,@H_ConAcre,@I_ItSite,@I_ItName,@BuNum,@BuName,@H_CeCode,@H_RoNum,@H_CurLay,@H_HoStru,@CHID,@BusiNo)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@HouseInfo_ID", houseInfo.HouseInfo_ID);

            ListAdd(list, "@HoSite", houseInfo.HoSite);
            ListAdd(list, "@H_HoUse", houseInfo.H_HoUse);
            ListAdd(list, "@H_CeCode", null);
            ListAdd(list, "@H_ConAcre", houseInfo.H_ConAcre);
            ListAdd(list, "@I_ItSite", houseInfo.I_ItSite);
            ListAdd(list, "@I_ItName", houseInfo.I_ItName);
            ListAdd(list, "@BuNum", houseInfo.BuNum);
            ListAdd(list, "@BuName", houseInfo.BuName);
            ListAdd(list, "@H_RoNum", houseInfo.H_RoNum);
            ListAdd(list, "@H_CurLay", houseInfo.H_CurLay);
            ListAdd(list, "@H_HoStru", houseInfo.H_HoStru);
            ListAdd(list, "@CHID", houseInfo.CHID);
            ListAdd(list, "@BusiNo", houseInfo.BusiNo);



            lock (lockKey)
            {
               
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static int Insert_VolEleArc(VolEleArc volEleArc,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into VolEleArc(EleArcVol_ID,ArchiveId,Ordinal,EleArcName,IsShow) values(@EleArcVol_ID,@ArchiveId,@Ordinal,@EleArcName,@IsShow)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@EleArcVol_ID", volEleArc.EleArcVol_ID);
            ListAdd(list, "@ArchiveId", volEleArc.ArchiveId);
            ListAdd(list, "@Ordinal", volEleArc.Ordinal);
            ListAdd(list, "@EleArcName", volEleArc.EleArcName);
            ListAdd(list, "@IsShow", volEleArc.IsShow);

            lock (lockKey)
            {
                
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static int Insert_ArchiveIndex(ArchiveIndex archiveIndex,DbHelper helper, DbTransaction trans)
        {
            string sql = "insert into ArchiveIndex(ArchiveId,ArchiveType,BusiNO,IsHistoray,ArchiveDate,Source,ReqType,HousePropertyType,FmBusiNo,IsOld) values(@ArchiveId,@ArchiveType,@BusiNO,@IsHistoray,@ArchiveDate,@Source,@ReqType,@HousePropertyType,@FmBusiNo,@IsOld)";
            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list, "@ArchiveId", archiveIndex.ArchiveId);
            //list.Add(new SqlParameter);
            //ListAdd(list,"@DaCode", archiveIndex.DaCode);
            ListAdd(list, "@ArchiveType", archiveIndex.ArchiveType);
            ListAdd(list, "@BusiNO", archiveIndex.BusiNO);
            ListAdd(list, "@IsHistoray", archiveIndex.IsHistoray);
            ListAdd(list, "@ArchiveDate", archiveIndex.ArchiveDate);

            ListAdd(list, "@Source", archiveIndex.Source);
            ListAdd(list, "@ReqType", archiveIndex.ReqType);
            ListAdd(list, "@HousePropertyType", archiveIndex.HousePropertyType);
            ListAdd(list, "@FmBusiNo", archiveIndex.FmBusiNo);
            ListAdd(list, "@IsOld", archiveIndex.IsOld);

            lock (lockKey)
            {
               
                return helper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }
        public static int Insert_ArchiveIndex(ArchiveIndex archiveIndex)
        {
            string sql = "insert into ArchiveIndex(ArchiveId,ArchiveType,BusiNO,IsHistoray,ArchiveDate,Source,ReqType,HousePropertyType,FmBusiNo,IsOld) values(@ArchiveId,@ArchiveType,@BusiNO,@IsHistoray,@ArchiveDate,@Source,@ReqType,@HousePropertyType,@FmBusiNo,@IsOld)";
            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list,"@ArchiveId", archiveIndex.ArchiveId);
            //list.Add(new SqlParameter);
            //ListAdd(list,"@DaCode", archiveIndex.DaCode);
            ListAdd(list,"@ArchiveType", archiveIndex.ArchiveType);
            ListAdd(list,"@BusiNO", archiveIndex.BusiNO);
            ListAdd(list,"@IsHistoray", archiveIndex.IsHistoray);
            ListAdd(list,"@ArchiveDate", archiveIndex.ArchiveDate);
            
            ListAdd(list,"@Source", archiveIndex.Source);
            ListAdd(list,"@ReqType", archiveIndex.ReqType);
            ListAdd(list,"@HousePropertyType", archiveIndex.HousePropertyType);
            ListAdd(list, "@FmBusiNo", archiveIndex.FmBusiNo);
            ListAdd(list, "@IsOld", archiveIndex.IsOld);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.EXCE_SQL = sql;
                log.SLBH = archiveIndex.BusiNO;
                log.FMSLBH = archiveIndex.FmBusiNo;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入ArchiveIndex,影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }



        public static int Update_ArchiveIndex(ArchiveIndex arch,DbHelper helper,DbTransaction trans)
        {
            int i = 0;
            string sql1 = "Update [dbo].[ArchiveIndex] set IsHistoray='1' where BusiNO='{0}'";
            sql1 = string.Format(sql1, arch.FmBusiNo);

            i = helper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql1, null);
            FC_DA_LOG log = new FC_DA_LOG();
            log.EXCE_DATE = DateTime.Now;
            log.EXCE_SQL = sql1;
            log.SLBH = arch.BusiNO;
            log.FMSLBH = arch.FmBusiNo;
            log.ID = Guid.NewGuid().ToString();
            log.RETURNINFO = string.Format("更新archiveIndex，影响{0}行", i);
            BDCDA_DAL.Insert_FC_DA_LOG(log);
            return i;
            
        }

        public static int Update_ArchiveIndex(ArchiveIndex arch)
        {
            int i = 0;
            string sql1 = "Update [dbo].[ArchiveIndex] set IsHistoray='1' where BusiNO='{0}' and ArchiveType='C'";
            sql1 = string.Format(sql1, arch.FmBusiNo);

            DbHelper dbhelper = new DbHelper();
            dbhelper.SetProvider(MyDBType.Sql);
            i = dbhelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql1, null);
            FC_DA_LOG log = new FC_DA_LOG();
            log.EXCE_DATE = DateTime.Now;
            log.EXCE_SQL = sql1;
            log.SLBH = arch.BusiNO;
            log.FMSLBH = arch.FmBusiNo;
            log.ID = Guid.NewGuid().ToString();
            log.RETURNINFO = string.Format("更新archiveIndex，影响{0}行", i);
            BDCDA_DAL.Insert_FC_DA_LOG(log);
            return i;

        }

        private static string GetRealSLBH(string slbh)
        {
            slbh = slbh.Replace("FC", "").Replace("O", "").Replace("OF", "").Replace("M", "").Replace("OCS", "").Replace("F", "").Replace("C", "").Replace("S", "");
            if (slbh.Contains("_"))
                slbh = slbh.Substring(0, slbh.IndexOf('_'));
            //if(slbh.Contains("-"))
            //{
            //    slbh = slbh.Substring(0, slbh.IndexOf('-'));
            //}
            return GetNumber(slbh);



        }

        private static int Update_ArchiveIndex(ArchiveIndex archiveIndex, DbTransaction trans)
        {
            string sql = "Update ArchiveIndex set IsHistoray='1' where BusiNO={0}";
            sql = string.Format(sql, archiveIndex.FmBusiNo);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                return dbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, null);
            }
        }

        private static void ListAdd(List<DbParameter> list,string paraName,object value)
        {
            if (null == value)
                list.Add(new SqlParameter(paraName, DBNull.Value));
            else
                list.Add(new SqlParameter(paraName, value));
        }


        public static int Insert_VolEleArc(VolEleArc volEleArc)
        {
            string sql = "insert into VolEleArc(EleArcVol_ID,ArchiveId,Ordinal,EleArcName,IsShow) values(@EleArcVol_ID,@ArchiveId,@Ordinal,@EleArcName,@IsShow)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@EleArcVol_ID", volEleArc.EleArcVol_ID);
            ListAdd(list,"@ArchiveId", volEleArc.ArchiveId);
            ListAdd(list,"@Ordinal", volEleArc.Ordinal);
            ListAdd(list,"@EleArcName", volEleArc.EleArcName);
            ListAdd(list,"@IsShow", volEleArc.IsShow);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = volEleArc.ArchiveId.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入volEleArc，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static int Insert_HouseInfo(HouseInfo houseInfo)
        {
            string sql = "insert into HouseInfo(HouseInfo_ID,H_HoUse,HoSite,H_ConAcre,I_ItSite,I_ItName,BuNum,BuName,H_CeCode,H_RoNum,H_CurLay,H_HoStru,CHID,BusiNo) " +
                "values(@HouseInfo_ID,@H_HoUse,@HoSite,@H_ConAcre,@I_ItSite,@I_ItName,@BuNum,@BuName,@H_CeCode,@H_RoNum,@H_CurLay,@H_HoStru,@CHID,@BusiNo)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@HouseInfo_ID", houseInfo.HouseInfo_ID);

            ListAdd(list, "@HoSite", houseInfo.HoSite);
            ListAdd(list, "@H_HoUse", houseInfo.H_HoUse);
            ListAdd(list, "@H_CeCode", null);
            ListAdd(list, "@H_ConAcre", houseInfo.H_ConAcre);
            ListAdd(list, "@I_ItSite", houseInfo.I_ItSite);
            ListAdd(list, "@I_ItName", houseInfo.I_ItName);
            ListAdd(list, "@BuNum", houseInfo.BuNum);
            ListAdd(list, "@BuName", houseInfo.BuName);
            ListAdd(list, "@H_RoNum", houseInfo.H_RoNum);
            ListAdd(list, "@H_CurLay", houseInfo.H_CurLay);
            ListAdd(list, "@H_HoStru", houseInfo.H_HoStru);
            ListAdd(list, "@CHID", houseInfo.CHID);
            ListAdd(list,"@BusiNo", houseInfo.BusiNo);



            //ListAdd(list,"@HoSite", houseInfo.HoSite);
            //ListAdd(list,"@H_HoUse", houseInfo.H_HoUse);
            //ListAdd(list, "@H_CeCode", houseInfo.H_CeCode);
            //ListAdd(list,"@H_ConAcre", houseInfo.H_ConAcre);
            //ListAdd(list,"@I_ItSite", houseInfo.I_ItSite);
            //ListAdd(list,"@I_ItName", houseInfo.I_ItName);
            //ListAdd(list,"@BuNum", houseInfo.BuNum);
            //ListAdd(list,"@BuName", houseInfo.BuName);
            //ListAdd(list,"@H_RoNum", houseInfo.H_RoNum);
            //ListAdd(list,"@H_CurLay", houseInfo.H_CurLay);
            //ListAdd(list,"@H_HoStru", houseInfo.H_HoStru);
            //ListAdd(list,"@CHID", houseInfo.CHID);
            //ListAdd(list,"@BusiNo", houseInfo.BusiNo);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = houseInfo.HouseInfo_ID.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入houseInfo，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static void UpdateGXJD(ArchiveIndex index, string needUpdateID)
        {
            string[] arr = new string[] { string.Format(@"update [dbo].ArchiveIndex set archiveid='{1}',Dacode='{2}',ArcSite='{3}',process='{4}'  where archiveid='{0}'",needUpdateID,index.ArchiveId,index.DaCode,index.ArcSite,index.process),
            
             string.Format(@"update [dbo].HouseArchiveRelation set archiveid='{1}' WHERE archiveid='{0}'", needUpdateID,index.ArchiveId),
             string.Format(@"update [dbo].[Certificate] set  archiveid='{1}' WHERE archiveid='{0}'", needUpdateID,index.ArchiveId),
             string.Format(@"update [dbo].PropArchiveRelation set archiveid='{1}' WHERE archiveid='{0}'", needUpdateID,index.ArchiveId),
            string.Format(@"update [dbo].Person set archiveid='{1}' WHERE archiveid='{0}'", needUpdateID,index.ArchiveId),
             string.Format(@"update [dbo].VolEleArc set archiveid='{1}' WHERE archiveid='{0}'", needUpdateID,index.ArchiveId) };

            for (int i = 0; i < arr.Length; i++)
            {
                string sql = arr[i];
                DbHelper db = new DbHelper();
                db.SetProvider(MyDBType.Sql);
                db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            }

        }

        public static string GetArchive(string busiNO)
        {
            string sql = "select ArchiveId from ArchiveIndex WITH (updlock) where busino='{0}' and isOld='2'";
            sql = string.Format(sql, busiNO);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object o = dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (o != null)
                {
                    return o.ToString();
                }
                return "";
            }
        }

        internal static int GetOrdinal(string cNAME)
        {
            string sql = @"select t2.ArcVolTypeNO+t1.ReceiveVolNO from dbo.ReceiveVol t1 WITH (updlock) 
inner join dbo.ArcVolType t2 WITH (updlock) on t1.ArcVolTypeID = t2.ArcVolTypeID
where t1.ReceiveVolName like '{0}%'";
            sql = string.Format(sql, cNAME);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object o= dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if(o!=null)
                {
                    return Convert.ToInt32(o);
                }
                return 2001;
            }
        }

        public static int Insert_Person(Person person)
        {
            string sql = "insert into Person(PersonID,ArchiveId,Name,CardNO,PersonType,IDCardType,RightMan_ID,Sex) " +
                "values(@PersonID,@ArchiveId,@Name,@CardNO,@PersonType,@IDCardType,@RightMan_ID,@Sex)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@PersonID", person.PersonID);
            ListAdd(list,"@ArchiveId", person.ArchiveId);
            ListAdd(list,"@Name", person.Name);
            ListAdd(list,"@CardNO", person.CardNO);
            ListAdd(list,"@PersonType", person.PersonType);
            ListAdd(list,"@IDCardType", person.IDCardType);
            ListAdd(list,"@RightMan_ID", person.RightMan_ID);
            ListAdd(list,"@Sex", person.Sex);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = person.ArchiveId.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入Person，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static int Insert_Certificate(Certificate certificate)
        {
            string sql = "insert into Certificate(CertificateID,HouseInfo_ID,Prop,PrintNO,CertificateType,GrantDate,PersonID,ArchiveId) "+
                "values(@CertificateID,@HouseInfo_ID,@Prop,@PrintNO,@CertificateType,@GrantDate,@PersonID,@ArchiveId)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@CertificateID", certificate.CertificateID);
            ListAdd(list,"@HouseInfo_ID", certificate.HouseInfo_ID);
            ListAdd(list,"@Prop", certificate.Prop);
            ListAdd(list,"@PrintNO", certificate.PrintNO);
            ListAdd(list,"@CertificateType", certificate.CertificateType);
            ListAdd(list,"@GrantDate", certificate.GrantDate);
            ListAdd(list,"@PersonID", certificate.PersonID);
            ListAdd(list,"@ArchiveId", certificate.ArchiveId);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = certificate.ArchiveId.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入Certificate，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static int Insert_HouseArchiveRelation(HouseArchiveRelation houseArchiveRelation)
        {
            string sql = "insert into HouseArchiveRelation(RelationID,ArchiveId,HouseInfo_ID,BusiNO) " +
                "values(@RelationID,@ArchiveId,@HouseInfo_ID,@BusiNO)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", houseArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", houseArchiveRelation.ArchiveId);
            ListAdd(list,"@HouseInfo_ID", houseArchiveRelation.HouseInfo_ID);
            ListAdd(list,"@BusiNO", houseArchiveRelation.BusiNO);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = houseArchiveRelation.ArchiveId.ToString();
                log.FMSLBH = houseArchiveRelation.HouseInfo_ID.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入houseArchiveRelation，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static int Insert_PropArchiveRelation(PropArchiveRelation propArchiveRelation)
        {
            string sql = "insert into PropArchiveRelation(RelationID,ArchiveId,CertificateID) "+
                "values(@RelationID,@ArchiveId,@CertificateID)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", propArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", propArchiveRelation.ArchiveId);
            ListAdd(list,"@CertificateID", propArchiveRelation.CertificateID);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = propArchiveRelation.ArchiveId.ToString();
                log.FMSLBH = propArchiveRelation.CertificateID.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入propArchiveRelation，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

        public static int Insert_VolEleArcDtl(VolEleArcDtl volEleArcDtl)
        {
            string sql = "insert into VolEleArcDtl(VolEleArcDtl_id,VolEleArc_ID,imgName,PageNo,ScanDate) "+
                "values(@VolEleArcDtl_id,@VolEleArc_ID,@imgName,@PageNo,@ScanDate)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@VolEleArcDtl_id", volEleArcDtl.VolEleArcDtl_id);
            ListAdd(list,"@VolEleArc_ID", volEleArcDtl.VolEleArc_ID);
            ListAdd(list,"@imgName", volEleArcDtl.imgName);
            ListAdd(list,"@PageNo", volEleArcDtl.PageNo);
            ListAdd(list,"@ScanDate", volEleArcDtl.ScanDate);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i= dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
                FC_DA_LOG log = new FC_DA_LOG();
                log.EXCE_DATE = DateTime.Now;
                log.SLBH = volEleArcDtl.VolEleArc_ID.ToString();
                log.FMSLBH = volEleArcDtl.VolEleArc_ID.ToString();
                log.EXCE_SQL = sql;
                log.ID = Guid.NewGuid().ToString();
                log.RETURNINFO = string.Format("插入volEleArcDtl，影响{0}行", i);
                BDCDA_DAL.Insert_FC_DA_LOG(log);
                return i;
            }
        }

       

        private String ArchiveId(int length)
        {
            return Guid.NewGuid().ToString().Substring(0, length);
        }


        internal static bool IsExistZH(string zh)
        {
            string sql = "select count(1) from Certificate WITH (updlock) where prop='{0}'";
            sql = string.Format(sql, zh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object count = dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (Convert.ToInt32(count) > 0)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        public static bool CanPust(string slbh)
        {
            string sql = "select count(1) from ArchiveIndex WITH (updlock) where BusiNO='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                object count = dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (Convert.ToInt32(count) > 0)
                {
                    return false;
                }
                else
                    return true;
            }
        }




        internal static DataTable GetVolEleArcDtlByVolEleArcID(string p)
        {
            string sql = "select [EleArcVol_ID]  from [VolEleArc] WITH (updlock) where [ArchiveId]='{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                return dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            }
        }
    }
}