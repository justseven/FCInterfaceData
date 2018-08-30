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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Models.BDCModel;
using Web4BDC.Dal;
using Web4BDC.Models;
using Web4BDC.Tools;
using Web4BDC.FC.Models;
using System.IO;
using Web4BDC.Models.FCDAModel;

namespace Web4BDC.Bll
{
    public class FCDA_BLL
    {

        private static string errInfo = string.Empty;

        /// <summary>
        /// 插入产权处档案库
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>1:插入成功；0：宗地业务；异常信息：失败</returns>
        public static BDCFilterResult Insert_FCDA(PageParams pageParams)
        {
            try
            {
                BDCFilterResult res = null;
                DataTable dt = GetXMSLBH(pageParams.PrjId);
                if (null != dt && dt.Rows.Count > 0)
                {

                    string uid = GetUserIDInPL(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        string prjid = row["prjid"].ToString();
                        pageParams.PrjId = prjid;
                        pageParams.UserId = uid;
                        res = InsertFC(pageParams);
                        
                    }
                }
                else
                {

                    if (string.IsNullOrEmpty(pageParams.UserId))
                        pageParams.UserId = GetUserID(pageParams.PrjId);
                    res = InsertFC(pageParams);
                }
                return res;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        internal static DataTable GetRecSLBHZX()
        {
            return BDCDA_DAL.GetRecSLBHZX();
        }

        internal static DataTable GetRecSLBHs()
        {
            return  BDCDA_DAL.GetRecSLBHS();

        }

        public static List<BDCDAModel> GetBDCDA(PageParams pageParams)
        {
            try
            {
                List<BDCDAModel> res = null;
                DataTable dt = GetXMSLBH(pageParams.PrjId);
                if (null != dt && dt.Rows.Count > 0)
                {

                    string uid = GetUserIDInPL(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        string prjid = row["prjid"].ToString();
                        pageParams.PrjId = prjid;
                        pageParams.UserId = uid;
                        res = GetFCDA(pageParams);

                    }
                }
                else
                {

                    if (string.IsNullOrEmpty(pageParams.UserId))
                        pageParams.UserId = GetUserID(pageParams.PrjId);
                    res = GetFCDA(pageParams);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        internal static void UpdateBDCDAState(string slbh, int v)
        {
            BDCDA_DAL.UpdateBDCDAState(slbh, v);
        }

        internal static bool CheckProcState(string slbh)
        {
            return BDCDA_DAL.CheckProcState(slbh);
        }

        internal static bool CheckProcInDA(string slbh)
        {
            return BDCDA_DAL.CheckProcInDA(slbh);
        }

        internal static void InsertBDCLog(BDCDALog log)
        {
            BDCDA_DAL.InsertBDCLog(log);
        }

        public static List<BDCDAModel> GetFCDA(PageParams pageParams)
        {
            List<BDCDAModel> list = new List<BDCDAModel>();
            BDCFilterResult res = new BDCFilterResult();

            string slbh = pageParams.PrjId;
            string errInfo = "";

            try
            {
                if (IsFW(pageParams.PrjId))
                {

                    //获取业务信息
                    List<ArchiveIndex> archiveIndex_list = GetArchiveIndex(pageParams.PrjId);
                    //取不带-的受理编号
                    if (archiveIndex_list.Count == 1 && archiveIndex_list[0].BusiNO.Contains("-"))
                    {
                        archiveIndex_list[0].BusiNO = archiveIndex_list[0].BusiNO.Substring(0, archiveIndex_list[0].BusiNO.IndexOf('-'));
                    }

                    List<HouseInfo> houses = null;

                    if (null != archiveIndex_list && archiveIndex_list.Count > 0)
                    {
                        //遍历业务表
                        foreach (ArchiveIndex archiveIndex in archiveIndex_list)
                        {
                            BDCDAModel model = new BDCDAModel();
                            model.archiveIndex = archiveIndex;
                            //获取业务相关房屋信息
                            model.houses = GetHouseInfo(archiveIndex.SLBH, houses);
                            houses = model.houses;
                            //获取房屋对应的权证信息
                            model.certificates = GetCertificate(archiveIndex, model.houses, pageParams.PrjId);
                            //获取权利人相关信息
                            model.persons = GetPerson(archiveIndex, archiveIndex.BusiNO);
                            //获取业务收件目录
                            model.volEleArcs = GetVolEleArc(archiveIndex, pageParams.PrjId);
                            //List<VolEleArcDtl> volEleArcDtl_list = GetVolEleArcDtl(volEleArc, pageParams);
                            //创建权证关联表信息
                            model.propArchiveRelations = GetPropArchiveRelation(archiveIndex, model.certificates);
                            //创建房屋关联表信息
                            model.houseArchiveRelations = GetHouseArchiveRelation(archiveIndex, model.houses);

                            model.FSLBH_List = GetBDCXGDJGL(archiveIndex.BusiNO, archiveIndex.ArchiveType);//GetXGDJGL(archiveIndex.BusiNO);
                            list.Add(model);
                           
                        }
                        res.ConfirmType = 1;
                        res.IsSuccess = true;
                        res.Message = "成功！业务已经成功推送！";
                    }
                    else
                    {
                        res.ConfirmType = 1;
                        res.IsSuccess = false;
                        res.Message = "拒绝！非推送业务，暂不推送";
                    }
                   //UpdateBDCDAState(slbh, 1);

                }
                else
                {
                    res.ConfirmType = 1;
                    res.IsSuccess = false;
                    res.Message = "拒绝！未找到业务宗号或非房产业务，暂不推送";
                    
                }
                //创建日志信息
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = pageParams.UserId;
                tag.SLBH = pageParams.PrjId;

                //InsertLog(tag);
            }
            catch (Exception ex)
            {
                res.ConfirmType = 1;
                res.IsSuccess = false;
                res.Message = "失败！" + ex.Message + errInfo;
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = pageParams.UserId;
                tag.SLBH = pageParams.PrjId;
                //InsertLog(tag);
                string log = string.Format("InsertFC异常对象:{0},异常方法：{1},SLBH={2},错误信息：{3}", ex.Source, ex.TargetSite, pageParams.PrjId, ex.Message);
                //WriteLog("C:\\INETFACELOG.TXT", log);
            }
            if(res.IsSuccess)
                return list;
            throw new Exception(res.Message);
        }

        private static List<DJ_XGDJGL> GetBDCXGDJGL(string busiNO,string ywlx)
        {
            return BDCDA_DAL.GetBDC_XGDJGL(busiNO,ywlx);
        }

        private static void MyDeleteRecode(string slbh)
        {
            DataTable dt = GetDeleteInfo(slbh);
            if (null != dt && dt.Rows.Count > 0)
            {


                foreach (DataRow row in dt.Rows)
                {
                    string busino = row["busino"].ToString();

                    Guid ArchiveId = new Guid(row["ArchiveId"].ToString());



                    deleteRecode(busino, ArchiveId);

                }


            }
        }

        private static DataTable GetDeleteInfo(string slbh)
        {
            return FCDA_DAL.GetDeleteInfo(slbh);

        }


        private static void deleteRecode(string busino, Guid archiveId)
        {
             FCDA_DAL.deleteRecode(busino, archiveId);

        }


        private static BDCFilterResult InsertFC(PageParams pageParams)
        {
            BDCFilterResult res = new BDCFilterResult();
            string slbh = pageParams.PrjId;
            errInfo = string.Empty;

            try
            {
                if(IsFW(pageParams.PrjId))
                {
                    BDCDA_DAL.InsertFC_DA_SLBH(slbh, pageParams.PrjName);
                    //获取业务信息
                    List<ArchiveIndex> archiveIndex_list = GetArchiveIndex(pageParams.PrjId);
                    //取不带-的受理编号
                    if(archiveIndex_list.Count==1 && archiveIndex_list[0].BusiNO.Contains("-"))
                    {
                        archiveIndex_list[0].BusiNO = archiveIndex_list[0].BusiNO.Substring(0,archiveIndex_list[0].BusiNO.IndexOf('-'));
                    }

                    List<HouseInfo> houses = null;

                    if (null != archiveIndex_list && archiveIndex_list.Count > 0)
                    {
                        //遍历业务表
                        foreach (ArchiveIndex archiveIndex in archiveIndex_list)
                        {
                            //获取业务相关房屋信息
                            List<HouseInfo> houseInfo_list = GetHouseInfo(archiveIndex.SLBH, houses);
                            houses = houseInfo_list;
                            //获取房屋对应的权证信息
                            List<Certificate> certificate_list = GetCertificate(archiveIndex, houseInfo_list, pageParams.PrjId);
                            //获取权利人相关信息
                            List<Person> person_list = GetPerson(archiveIndex, archiveIndex.BusiNO);
                            //获取业务收件目录
                            List<VolEleArc> volEleArc = GetVolEleArc(archiveIndex, pageParams.PrjId);
                            //List<VolEleArcDtl> volEleArcDtl_list = GetVolEleArcDtl(volEleArc, pageParams);
                            //创建权证关联表信息
                            List<PropArchiveRelation> propArchiveRelation = GetPropArchiveRelation(archiveIndex, certificate_list);
                            //创建房屋关联表信息
                            List<HouseArchiveRelation> houseArchiveRelation = GetHouseArchiveRelation(archiveIndex, houseInfo_list);


                            //Insert(archiveIndex, volEleArc, houseInfo_list, person_list, certificate_list, houseArchiveRelation, propArchiveRelation, null);

                            //插入房产档案系统
                            //判断业务宗号是否已存在档案库，如不存在，则插入
                            //if(InFCDA(archiveIndex.BusiNO))
                            //{
                                //插入房产档案库
                                //errInfo = FCDA_DAL.Insert(archiveIndex, volEleArc, houseInfo_list, person_list, certificate_list, houseArchiveRelation, propArchiveRelation, null,false);

                                //创建不动产归档信息
                                ARCH_GLDAXX gd = CreateGLDAXX(pageParams, archiveIndex, houseInfo_list, person_list);
                                List<ARCH_BDCDYDJ> dy = CreateDYDJ(archiveIndex.BusiNO, gd.DAH);
                                List<ARCH_FWYBDJ> yb = CreateYBDJ(archiveIndex, gd.DAH);
                                List<ARCH_BDCCFDJ> cf = CreateCFDJ(archiveIndex.BusiNO, gd.DAH);
                                List<ARCH_BDCZXDJ> zx = CreateZXDJ(archiveIndex.BusiNO, gd.DAH);
                                List<WFM_ATTACHLST> wfm_ATTACHLST_list = GetWFM_ATTACHLST_list(archiveIndex.BusiNO);
                                //插入不动产归档库
                                Insert_Into_GD(gd, yb, dy, cf, zx, wfm_ATTACHLST_list);

                            //}
                            //else
                            //{
                            //    res.ConfirmType = 0;
                            //    res.IsSuccess = true;
                            //    res.Message = "该业务已经推送！";
                            //}

                        }
                        if (string.IsNullOrEmpty(errInfo))
                        {
                            res.ConfirmType = 1;
                            res.IsSuccess = true;
                            res.Message = "成功！业务已经成功推送！";
                        }
                        else
                        {
                            res.ConfirmType = 1;
                            res.IsSuccess = false;
                            res.Message ="失败！"+ errInfo;
                        }
                    }
                    else
                    {
                        res.ConfirmType = 1;
                        res.IsSuccess = false;
                        res.Message = "拒绝！非推送业务，暂不推送";
                    }

                }
                else
                {
                    res.ConfirmType = 1;
                    res.IsSuccess = false;
                    res.Message = "拒绝！未找到业务宗号或非房产业务，暂不推送";
                }
                //创建日志信息
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = pageParams.UserId;
                tag.SLBH = pageParams.PrjId;

                InsertLog(tag);
            }
            catch (Exception ex)
            {
                res.ConfirmType = 1;
                res.IsSuccess = false;
                res.Message = "失败！" + ex.Message ;
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = pageParams.UserId;
                tag.SLBH = pageParams.PrjId;
                InsertLog(tag);
                //string log = string.Format("InsertFC异常对象:{0},异常方法：{1},SLBH={2},错误信息：{3}", ex.Source, ex.TargetSite, pageParams.PrjId, ex.Message);
                LogHelper.WriteLog(pageParams.PrjId+"异常", ex);
            }

            return res;
        }


        private static void WriteLog(string path, string logText)
        {

            //string catalogName = itemCatalog.FullName;
            FileStream flagStream = new FileStream(path, FileMode.Append, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(flagStream);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(logText);
            sw.Flush();
            sw.Close();
        }
        private static DataTable GetXMSLBH(string prjId)
        {
            return BDCDA_DAL.GetXMSLBH(prjId);
        }

        //2017053010098

        //private static void Insert(ArchiveIndex archiveIndex, List<VolEleArc> volEleArc, List<HouseInfo> houseInfo_list, List<Person> person_list, List<Certificate> certificate_list, List<HouseArchiveRelation> houseArchiveRelation, List<PropArchiveRelation> propArchiveRelation, object p)
        //{
        //    ArchiveIndex fc_index = GetArchiveIndexFromFC(archiveIndex.BusiNO);
        //    if(null!=index)
        //    {
        //        FCDA_DAL.UpDateArchiveIndex(fc_index);
        //        List<VolEleArc> fc_VolEleArc = GetVolEleArcFromFC(fc_index);
        //        List<HouseInfo> fc_HouseInfo = GetHouseInfoFromFC(fc_index);
        //        List<Person> fc_Person = GetPersonFrom(fc_index);
        //        List<Certificate> fc_certificate = GetCertificateFromFC(fc_index);
        //        List<HouseArchiveRelation> fc_houseArchiveRelation = GetHouseArchiveRelationFromFC(fc_index);
        //        List<PropArchiveRelation> fc_propArchiveRelation = GetPropArchiveRelationFromFC(fc_index);
        //    }
        //}

        private static List<ARCH_BDCZXDJ> CreateZXDJ(string busiNO, string dAH)
        {
            List<ARCH_BDCZXDJ> list = BDCDA_DAL.GetARCH_BDCZXDJ(busiNO);
            List<ARCH_BDCZXDJ> res = new List<ARCH_BDCZXDJ>();
            if (null!=list && list.Count>0)
            {
                list[0].ZL += "等" + list.Count + "户";
                res.Add(list[0]);
                return res;
            }
            return list;
        }

        private static List<ARCH_BDCCFDJ> CreateCFDJ(string slbh,string dah)
        {
            List<ARCH_BDCCFDJ> list= BDCDA_DAL.GetARCH_BDCCFDJ(slbh,dah);
            List<ARCH_BDCCFDJ> RES = new List<ARCH_BDCCFDJ>();
            if (null != list && list.Count > 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        if (list[i].BDCDYH == list[j].BDCDYH)
                        {
                            if (!list[i].QLRMC.Contains(list[j].QLRMC))
                            {
                                list[i].QLRMC += " " + list[j].QLRMC;

                                list[i].ZJHM += " " + list[j].ZJHM;
                            }


                            RES.Add(list[i]);
                        }
                    }
                }

                if (null != RES && RES.Count > 0)
                    return RES;
            }
            return list;
        }

        private static List<ARCH_FWYBDJ> CreateYBDJ(ArchiveIndex arch, string dah)
        {
            List<ARCH_FWYBDJ> list = BDCDA_DAL.GetARCH_FWYBDJ(arch, dah); 
          
            List<ARCH_FWYBDJ> RES = new List<ARCH_FWYBDJ>();
            if (null != list && list.Count > 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i+1; j < list.Count; j++)
                    {
                        if (list[i].BDCDYH == list[j].BDCDYH)
                        {
                            if (!list[i].QLRMC.Contains(list[j].QLRMC))
                            {
                                list[i].QLRMC += " " + list[j].QLRMC;

                                list[i].ZJHM += " " + list[j].ZJHM;
                            }
                            list[i].SQR = list[i].QLRMC;
                           
                            list[i].BDCZSH = list[i].BDCZSH + "等" + list.Count + "个";
                            RES.Add(list[i]);
                        }
                    }
                }

                if (null != RES && RES.Count > 0)
                    return RES;
            }
            return list;
        }

        private static List<ARCH_BDCDYDJ> CreateDYDJ(string slbh,string dah)
        {
            List<ARCH_BDCDYDJ> list= BDCDA_DAL.GetARCH_BDCDYDJ(slbh,dah);
            List<ARCH_BDCDYDJ> RES = new List<ARCH_BDCDYDJ>();
            if (null!= list && list.Count > 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        if (list[i].BDCDYH == list[j].BDCDYH)
                        {
                            if (!list[i].QLRMC.Contains(list[j].QLRMC))
                            {
                                list[i].QLRMC += " " + list[j].QLRMC;

                                list[i].ZJHM += " " + list[j].ZJHM;
                            }
                            list[i].SQR = list[i].QLRMC;
                            RES.Add(list[i]);
                        }
                    }
                }

                if (null != RES && RES.Count > 0)
                    return RES;
            }
            return list;
        }

        public static string GetUserID(string slbh)
        {
            try
            {
                //if(slbh.Contains("-"))
                //{
                //    slbh = slbh.Substring(0, slbh.IndexOf("-"));
                //}
                //string userName = FCDA_DAL.GetUserNameFromTag(slbh);
                //if (string.IsNullOrEmpty(userName) || userName.Equals("guidangren"))
                //{
                string userName = BDCDA_DAL.GetUserNameBySlbh(slbh);
                //}

                if(string.IsNullOrEmpty(userName) || userName.Equals("guidangren"))
                {
                    return "";
                }
                return BDCDA_DAL.GetUserID(userName);
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                return "guidangren";
            }
        }

        public static string GetUserIDInPL(DataTable dt)
        {
            try
            {
                string UID = "";
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row[0].ToString();
                    string userName = BDCDA_DAL.GetUserNameInPL(slbh);
                    if (string.IsNullOrEmpty(userName) || userName.Equals("guidangren"))
                    {
                        continue;
                    }
                    else
                    {
                        UID = BDCDA_DAL.GetUserID(userName);
                        break;
                    }
                }
                return UID;

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return "guidangren";
            }
        }

        public static string GetUserID(DataTable dt)
        {
            try
            {
                string UID = "";
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row[0].ToString();
                    string userName = BDCDA_DAL.GetUserNameBySlbh(slbh);
                    if (string.IsNullOrEmpty(userName) || userName.Equals("guidangren"))
                    {
                        continue;
                    }
                    else
                    {
                       UID= BDCDA_DAL.GetUserID(userName);
                        break;
                    }
                }
                return UID;
                
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return "guidangren";
            }
        }


        private static ARCH_GLDAXX CreateGLDAXX(PageParams pageParams, ArchiveIndex archiveIndex, List<HouseInfo> houseInfo_list, List<Person> person_list)
        {
            ARCH_GLDAXX gd = new ARCH_GLDAXX();
            gd.DAH = archiveIndex.SLBH;
            gd.BDCLX = "房屋";
            gd.DJLX = archiveIndex.ReqType;
            gd.DJRQ = DateTime.ParseExact(archiveIndex.ArchiveDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            gd.GDR = pageParams.UserId;
            gd.GDRQ = DateTime.Now;
            gd.QLRMC = GetQLRMC(archiveIndex.BusiNO);
            gd.SLBH = archiveIndex.BusiNO;
            gd.DABH = archiveIndex.BusiNO;
            gd.ZT = "1";
            gd.FLH = GetFLH(archiveIndex.ReqType);//"C41";
            gd.ZL = houseInfo_list.Count > 1 ? houseInfo_list[0].I_ItSite + "等" + houseInfo_list.Count + "户" : houseInfo_list[0].I_ItSite;
            gd.BDCDYH = houseInfo_list.Count > 1 ? houseInfo_list[0].CHID + "等" + houseInfo_list.Count + "户" : houseInfo_list[0].CHID;
            return gd;
        }

        public static BDCFilterResult Insert_ARCH(PageParams param)
        {
            BDCFilterResult res = null;
            DataTable dt = GetXMSLBH(param.PrjId);
            if (null != dt && dt.Rows.Count > 0)
            {
                string uid = GetUserIDInPL(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string prjid = row["prjid"].ToString();
                    param.PrjId = prjid;
                    param.UserId = uid;
                    res = InsertARCH(param);
                }
            }
            else
            {
                //if (string.IsNullOrEmpty(param.UserId))
                param.UserId = GetUserID(param.PrjId);
                res = InsertARCH(param);
            }
            return res;
            
        }

        private static BDCFilterResult InsertARCH(PageParams param)
        {
            BDCFilterResult res = new BDCFilterResult();
            string slbh = param.PrjId;
            string errInfo = "";

            try
            {
                if (IsFW(param.PrjId))
                {
                    if (true)
                    {
                        List<ArchiveIndex> archiveIndex_list = GetArchiveIndex(param.PrjId);
                        if (archiveIndex_list.Count == 1 && archiveIndex_list[0].BusiNO.Contains("-"))
                        {
                            archiveIndex_list[0].BusiNO = archiveIndex_list[0].BusiNO.Substring(0, archiveIndex_list[0].BusiNO.IndexOf('-'));
                        }
                        List<HouseInfo> houses = null;
                        if (null != archiveIndex_list && archiveIndex_list.Count > 0)
                        {

                            foreach (ArchiveIndex archiveIndex in archiveIndex_list)
                            {
                                List<HouseInfo> houseInfo_list = GetHouseInfo(archiveIndex.SLBH, houses);
                                houses = houseInfo_list;

                                List<Certificate> certificate_list = GetCertificate(archiveIndex, houseInfo_list, param.PrjId);

                                List<Person> person_list = GetPerson(archiveIndex, archiveIndex.BusiNO);

                                List<VolEleArc> volEleArc = GetVolEleArc(archiveIndex, archiveIndex.BusiNO);
                                //List<VolEleArcDtl> volEleArcDtl_list = GetVolEleArcDtl(volEleArc, pageParams);

                                List<PropArchiveRelation> propArchiveRelation = GetPropArchiveRelation(archiveIndex, certificate_list);
                                List<HouseArchiveRelation> houseArchiveRelation = GetHouseArchiveRelation(archiveIndex, houseInfo_list);

                                //Insert(archiveIndex, volEleArc, houseInfo_list, person_list, certificate_list, houseArchiveRelation, propArchiveRelation, null);

                                //errInfo = FCDA_DAL.Insert(archiveIndex, volEleArc, houseInfo_list, person_list, certificate_list, houseArchiveRelation, propArchiveRelation, null);

                                ARCH_GLDAXX gd = CreateGLDAXX(param, archiveIndex, houseInfo_list, person_list);
                                List<ARCH_BDCDYDJ> dy = CreateDYDJ(archiveIndex.BusiNO, gd.DAH);
                                List<ARCH_FWYBDJ> yb = CreateYBDJ(archiveIndex, gd.DAH);
                                List<ARCH_BDCCFDJ> cf = CreateCFDJ(archiveIndex.BusiNO, gd.DAH);
                                List<ARCH_BDCZXDJ> zx = CreateZXDJ(archiveIndex.BusiNO, gd.DAH);
                                List<WFM_ATTACHLST> wfm_ATTACHLST_list = GetWFM_ATTACHLST_list(param.PrjId);
                                Insert_Into_GD(gd, yb, dy, cf, zx, wfm_ATTACHLST_list);

                            }


                            res.ConfirmType = 1;
                            res.IsSuccess = true;
                            res.Message = "成功！业务已经成功推送！";


                        }
                        else
                        {
                            res.ConfirmType = 0;
                            res.IsSuccess = true;
                            res.Message = "该业务已经推送！";
                        }
                    }
                    else
                    {
                        res.ConfirmType = 0;
                        res.IsSuccess = false;
                        res.Message = "拒绝！非推送业务，暂不推送！";
                    }
                }
                else
                {
                    res.ConfirmType = 1;
                    res.IsSuccess = false;
                    res.Message = "拒绝！未找到业务宗号或非房产业务，暂不推送";
                }
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = param.UserId;
                tag.SLBH = param.PrjId;

                InsertLog(tag);
            }
            catch (Exception ex)
            {
                res.ConfirmType = -1;
                res.IsSuccess = false;
                res.Message = "失败！" + ex.Message + errInfo;
                FC_DA_TAG tag = new FC_DA_TAG();
                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message + errInfo;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = param.UserId;
                tag.SLBH = param.PrjId;
                InsertLog(tag);
            }

            return res;
        }

        private static string GetFLH(string archiveType)
        {
            if(archiveType.Contains("注销"))
            {
                return "C47";
            }
            if(archiveType.Contains("抵押"))
            {
                return "C42";
            }
            if(archiveType.Contains("预告"))
            {
                return "C45";
            }
            if(archiveType.Contains("查封"))
            {
                return "C43";
            }
            if(archiveType.Contains("异议"))
            {
                return "C44";   
            }
            return "C41";
        }

        private static void Insert_Into_GD(ARCH_GLDAXX gd, List<ARCH_FWYBDJ> ybList, List<ARCH_BDCDYDJ> dyList, List<ARCH_BDCCFDJ> cfList, List<ARCH_BDCZXDJ> zxList, List<WFM_ATTACHLST> wfm_ATTACHLST_list)
        {
            if(null!=gd)//(IsExist(gd))
            {
                string zdph = BDCDA_DAL.GetZDPH(gd);
                if(!string.IsNullOrEmpty(zdph))
                {
                    gd.ZDPH = zdph;
                }
                BDCDA_DAL.Delete_GD(gd);
            }
            if(null!=ybList)
            {
                foreach (ARCH_FWYBDJ yb in ybList)
                {
                    BDCDA_DAL.Delete_YB(yb);
                }
            }
            if (null != dyList)
            {
                foreach (ARCH_BDCDYDJ dy in dyList)
                {
                    BDCDA_DAL.Delete_DY(dy);
                }
            }
            if (null != cfList)
            {
                foreach (ARCH_BDCCFDJ cf in cfList)
                {
                    BDCDA_DAL.Delete_CF(cf);
                }
            }
            if(null!=zxList)
            {
                foreach (ARCH_BDCZXDJ zx in zxList)
                {
                    BDCDA_DAL.Delete_ZX(zx);
                }
            }
            if(null!=wfm_ATTACHLST_list)
            {
                foreach (WFM_ATTACHLST wa in wfm_ATTACHLST_list)
                {
                    BDCDA_DAL.DeleteWFM_ATC(wa);
                }
            }

            BDCDA_DAL.Insert_Into_GD(gd,ybList,dyList,cfList, zxList, wfm_ATTACHLST_list);
        }

        private static bool IsExist(ARCH_GLDAXX gd)
        {
            return BDCDA_DAL.IsExist(gd);
        }

        private static string GetQLRMC(string slbh)
        {
            string qlrmc = "";

            List<DJ_QLRGL> qlrgl_list = BDCDA_DAL.GetQLRGL(slbh);
            if (qlrgl_list.Count == 1)
            {
                qlrmc = qlrgl_list[0].QLRMC;
            }
            else
            {
                foreach (DJ_QLRGL qlrgl in qlrgl_list)
                {
                    if (qlrgl.QLRLX.Equals("权利人") || qlrgl.QLRLX.Equals("抵押权人"))
                    {
                        if (string.IsNullOrEmpty(qlrmc))
                        {
                            qlrmc = qlrgl.QLRMC;
                        }
                        else
                        {
                            qlrmc += " " + qlrgl.QLRMC;
                        }
                    }
                }
            }

            //if (null != person_list && person_list.Count > 0)
            //{
            //    foreach (Person person in person_list)
            //    {
            //        if (person.PersonType.Equals("1") || person.PersonType.Equals("3"))
            //        {
            //            if (string.IsNullOrEmpty(qlrmc))
            //            {
            //                qlrmc = person.Name;
            //            }
            //            else
            //            {
            //                qlrmc += " " + person.Name;
            //            }
            //        }
                    
            //    }
            //}
            return qlrmc;
        }
        
        public static DataTable GetCanInsertSlbh(string sql)
        {
            return BDCDA_DAL.GetCanInsertSLBH(sql);
        }


       

        public static void InsertPIC(string p)
        {
            DataTable dt = FCDA_DAL.GetVolEleArcDtlByVolEleArcID(p);
            int i=1;
            string dir = "10000-450000/110000/100001/";
            FTPHelper ftpHelper = new FTPHelper();
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FCFtpIP"],
                username = ConfigurationManager.AppSettings["FCFtpUser"],
                password = ConfigurationManager.AppSettings["FCFtpPWD"]
            };
            List<string> list = ftpHelper.ListDirectory(dir, souFTp, "");

            foreach (DataRow row in dt.Rows)
            {
                string s = row[0].ToString();
                VolEleArcDtl v = new VolEleArcDtl();
                v.VolEleArcDtl_id = Guid.NewGuid();
                v.VolEleArc_ID = new Guid(s);
               
                v.imgName = dir+list[i-1];
                FCDA_DAL.Insert_VolEleArcDtl(v);
                i++;
            }

        }

        private static void InsertLog(FC_DA_TAG tag)
        {
            
                BDCDA_DAL.Delete_Log(tag);
                BDCDA_DAL.InsertLog(tag);
            
        }

        private static bool CanPush(string slbh)
        {
            try
            {
                bool flag = false;
                if (slbh.Contains("-"))
                {
                    slbh = slbh.Substring(0, slbh.IndexOf("-"));
                }
                flag = FCDA_DAL.CanPust(GetRealSLBH(slbh));
                if (!flag)
                    return flag;
                return FCDA_DAL.CanPust(slbh);
            }
            catch
            {
                return true;
            }
            //return true;
        }

        private static bool InFCDA(string slbh)
        {
            try
            {
                bool flag = false;
                flag = FCDA_DAL.CanPust(GetRealSLBH(slbh));
                if (!flag)
                    return flag;
                return FCDA_DAL.CanPust(slbh);
            }
            catch
            {
                return true;
            }
            //return true;
        }
        private static bool IsFW(string slbh)
        {
            return BDCDA_DAL.IsFW(slbh);
        }

        private static List<HouseArchiveRelation> GetHouseArchiveRelation(ArchiveIndex archiveIndex, List<HouseInfo> houseInfo_list)
        {
            List<HouseArchiveRelation> list = new List<HouseArchiveRelation>();
            foreach (HouseInfo houseInfo in houseInfo_list)
            {
                HouseArchiveRelation houseArchiveRelation = new HouseArchiveRelation();
                houseArchiveRelation.RelationID = CreateGuid(32);
                houseArchiveRelation.ArchiveId = archiveIndex.ArchiveId;
                houseArchiveRelation.HouseInfo_ID = houseInfo.HouseInfo_ID;
                list.Add(houseArchiveRelation);
            }
            

            return list;
        }

        private static List<PropArchiveRelation> GetPropArchiveRelation(ArchiveIndex archiveIndex, List<Certificate> certificate_list)
        {
            List<PropArchiveRelation> list = new List<PropArchiveRelation>();

            foreach (Certificate certificate in certificate_list)
            {
                PropArchiveRelation propArchiveRelation = new PropArchiveRelation();
                propArchiveRelation.RelationID = CreateGuid(32);
                propArchiveRelation.ArchiveId = archiveIndex.ArchiveId;
                propArchiveRelation.CertificateID = certificate.CertificateID;
                list.Add(propArchiveRelation);
            }
            

            return list;
        }

        private static List<VolEleArcDtl> GetVolEleArcDtl(List<VolEleArc> volEleArc, PageParams pageParams)
        {
            
            List<VolEleArcDtl> list = new List<VolEleArcDtl>();
            List<DOC_BINFILE> doc_bin_list = GetDoc_binfile_list(pageParams);

            for (int i = 0; i < doc_bin_list.Count; i++)
            {
                DOC_BINFILE doc = doc_bin_list[i];
            
                VolEleArcDtl volEleArcDtl = new VolEleArcDtl();
                volEleArcDtl.VolEleArc_ID = volEleArc[i].EleArcVol_ID;
                volEleArcDtl.VolEleArcDtl_id = CreateGuid(32);
                volEleArcDtl.imgName = doc.FTPATH;
                try
                {
                    volEleArcDtl.PageNo = null == doc.PAGECOUNT ? 0 : Convert.ToInt32(doc.PAGECOUNT);
                }
                catch { }
                volEleArcDtl.ScanDate = DateTime.Now.ToString("YYYYMMdd");
                list.Add(volEleArcDtl);
            }
            
            return list;
        }

        private static List<DOC_BINFILE> GetDoc_binfile_list(PageParams pageParams)
        {
            DataTable cid_dt = BDCDA_DAL.GetCID(pageParams);
            List<DOC_BINFILE> list=new List<DOC_BINFILE>();
            if (null != cid_dt && cid_dt.Rows.Count > 0)
            {
                foreach (DataRow row in cid_dt.Rows)
                {
                    DOC_BINFILE doc= BDCDA_DAL.GetDoc_binfile(row[0].ToString());
                    if(null!=doc)
                    {
                        list.Add(doc);
                    }
                }
            }
            
            
            return list;
        }

        private static List<VolEleArc> GetVolEleArc(ArchiveIndex archiveIndex, string slbh)
        {
            List<VolEleArc> list = new List<VolEleArc>();

            List<WFM_ATTACHLST> wfm_ATTACHLST_list = GetWFM_ATTACHLST_list(slbh);

            if (null != wfm_ATTACHLST_list && wfm_ATTACHLST_list.Count > 0)
            {
                foreach (WFM_ATTACHLST doc in wfm_ATTACHLST_list)
                {
                    if (doc.CNAME == "流程附件")
                    {
                        continue;
                    }
                    VolEleArc volEleArc = new VolEleArc();
                    volEleArc.ArchiveId = archiveIndex.ArchiveId;
                    volEleArc.EleArcVol_ID = CreateGuid(32);
                    volEleArc.EleArcName = doc.CNAME.Trim();
                    volEleArc.PageNumber = Convert.ToInt32(doc.FILENUM);
                    volEleArc.IsShow = "1";
                    volEleArc.Ordinal = GetOrdinal(doc.CNAME.Trim());//Convert.ToInt32(doc.CSORT);
                    list.Add(volEleArc);
                }
            }
            
            return list.Where((x, i) => list.FindIndex(z => z.EleArcName == x.EleArcName) == i).ToList<VolEleArc>();
        }

        private static int? GetOrdinal(string cNAME)
        {
            return FCDA_DAL.GetOrdinal(cNAME.Trim());
        }

        private static List<WFM_ATTACHLST> GetWFM_ATTACHLST_list(string slbh)
        {
            List<WFM_ATTACHLST> list= BDCDA_DAL.GetWFM_ATTACHLST_SelectInfo(slbh);
            if (null != list && list.Count > 0)
                return list;
            return BDCDA_DAL.GetWFM_ATTACHLST_AllInfo(slbh);
        }

        public static List<Person> GetPerson(ArchiveIndex archiveIndex, string slbh)
        {
            List<Person> list = new List<Person>();

            List<DJ_QLR> qlr_list = GetQlr(archiveIndex.BusiNO);

            List<DJ_QLR> qlrList = InitQLR(qlr_list);

            //List<DJ_QLR> ywr_list = GetYWR(archiveIndex.BusiNO);

            if (null != qlr_list && qlrList.Count > 0)
            {
                foreach (DJ_QLR qlr in qlrList)
                {
                    Person person = new Person();
                    person.ArchiveId = archiveIndex.ArchiveId;
                    person.PersonID = CreateGuid(32);
                    person.PersonType = GetPersonType_New(qlr);//GetPersonType(qlr.QLRID, archiveIndex.BusiNO);
                    person.Name = qlr.QLRMC;
                    person.CardNO = string.IsNullOrEmpty(qlr.ZJHM)?"无":qlr.ZJHM;
                    person.IDCardType = GetIDTYPE(qlr.ZJLB);
                    person.Sex = qlr.XB;
                    list.Add(person);
                }
            }

            return list;
            
        }

        private static List<DJ_QLR> InitQLR(List<DJ_QLR> qlr_list)
        {
            List<DJ_QLR> list = new List<DJ_QLR>();

            var qlr_res = qlr_list.Where(u => u.QLRLX == "权利人" || u.QLRLX=="抵押权人");
            if (null != qlr_res && qlr_res.Count<DJ_QLR>() > 0)
            {
               
                list.AddRange(qlr_res);
            }


            var ywr_res= qlr_list.Where(u => u.QLRLX == "义务人" || u.QLRLX=="抵押人");
            if (null != ywr_res && ywr_res.Count<DJ_QLR>() > 0)
            {
                foreach (DJ_QLR item in ywr_res)
                {
                    var qy_res = list.Where(u => u.QLRID == item.QLRID);
                    if (null == qy_res || qy_res.Count<DJ_QLR>() == 0)
                    {
                        list.Add(item);
                    }
                }
            }

            //    foreach (DJ_QLR qlr in qlr_list)
            //{
            //    bool flag = false;
            //    if (list.Count == 0)
            //    {
            //        list.Add(qlr);
            //    }
            //    else
            //    {
            //        foreach (DJ_QLR new_qlr in list)
            //        {
            //            if(qlr.QLRID==new_qlr.QLRID)
            //            {
            //                if(new_qlr.QLRLX=="义务人" && qlr.QLRLX=="权利人")
            //                {
            //                    new_qlr.QLRLX = "权利人";
            //                }
            //            }
            //            else
            //            {
            //                var res = list.Where(u => u.QLRID == qlr.QLRID && u.QLRLX==qlr.QLRLX);
            //                if (null == res || res.Count<DJ_QLR>() == 0)
            //                {
            //                    flag = true;
            //                    break;
            //                }
                            
            //            }
            //        }
            //    }
            //    if(flag)
            //    {
            //        list.Add(qlr);
            //    }
                
            //}
            return list;
        }

        private static string GetPersonType_New(DJ_QLR qlr)
        {
            switch (qlr.QLRLX)
            {
                case "义务人":
                case "抵押人":
                    if (qlr.SXH > 1)
                        return "4";
                    return "2";
                case "权利人代理人":
                    return "5";
                default:
                    if ((qlr.QLRLX == "权利人" || qlr.QLRLX == "抵押权人") && qlr.SXH > 1)
                        return "3";

                    return "1";
            }
        }

        private static List<DJ_QLR> GetYWR(string busiNO)
        {
            List<DJ_QLR> list = BDCDA_DAL.GetYWR(busiNO);
            return list;
        }

        private static string GetIDTYPE(string zJLB)
        {
            switch(zJLB)
            {
                case "1":
                    return "01";
                case "2":
                    return "17";
                case "3":
                    return "06";
                case "4":
                    return "07";
                case "5":
                    return "02";
                case "6":
                    return "04";
                case "7":
                    return "03";
                case "8":
                    return "99";
                default:
                    return "99";
            }
        }

        private static List<DJ_QLR> GetQlr(string p)
        {
            List<DJ_QLR> list = BDCDA_DAL.GetQLR(p);
            return list;
        }

        private static string GetPersonType(string qlrid,string slbh)
        {
            DJ_QLRGL qlrgl= BDCDA_DAL.GetQlrType(qlrid,slbh);
            switch (qlrgl.QLRLX)
            {
                case "义务人":
                case "抵押人":
                    if (qlrgl.QLRLX == "义务人" && qlrgl.SXH > 1)
                        return "4";
                    return "2";
                case "权利人代理人":
                    return "5";
                default:
                    if (qlrgl.QLRLX == "权利人" && qlrgl.SXH > 1)
                        return "3";
                    return "1";
            }
        }
       
        private static List<HouseInfo> GetHouseInfo(string slbh, List<HouseInfo> h_list)
        {
            
            List<HouseInfo> list = new List<HouseInfo>();
            string queryslbh = "";
            if (slbh.Contains("-"))
            {
                queryslbh = slbh.Substring(0, slbh.IndexOf("-"));
            }
            else
            {
                queryslbh = slbh;
            }
            //根据受理编号获取对应楼盘表
            List<FC_H_QSDC> fc_h_list = GetFC_H(queryslbh);

            if (null != fc_h_list && fc_h_list.Count > 0)
            {
                foreach (FC_H_QSDC fc_h in fc_h_list)
                {
                  
                    HouseInfo houseInfo = new HouseInfo();
                    FC_Z_QSDC fc_z = GetFC_Z_QSDC(fc_h);
                    houseInfo.HouseInfo_ID = CreateGuid(32);//GetCGHouseID(fc_h.TSTYBM);// new Guid(fc_h.TSTYBM);//CreateGuid(32);
                    houseInfo.HoSite = fc_h.ZL;//GetFCZL(slbh,fc_h);
                    houseInfo.I_ItSite = houseInfo.HoSite;
                    houseInfo.H_HoUse = GetYTMC(fc_h.GHYT).Replace("成套住宅","住宅");
                    houseInfo.H_CeCode = fc_h.DYH;
                    houseInfo.H_ConAcre = fc_h.JZMJ==0?fc_h.YCJZMJ:fc_h.JZMJ;
                    houseInfo.TSTYBM = fc_h.TSTYBM;
                    houseInfo.BusiNo = slbh;//GetRealSLBH(queryslbh);
                    houseInfo.SLBH = slbh;
                    houseInfo.CHID = fc_h.BDCDYH;
                    houseInfo.I_ItName = fc_z.XMMC;
                    houseInfo.H_RoNum = fc_h.FJH;
                    houseInfo.H_CurLay = fc_h.SJC.ToString();
                    houseInfo.H_HoStru = ConvertFWJG(fc_z.FWJG);
                    houseInfo.BuName = GetBuName(fc_z);
                    houseInfo.BuNum = fc_z.LPBH;
                    houseInfo.BuFinishYear = null==fc_z.JGRQ?"":Convert.ToDateTime(fc_z.JGRQ).ToShortDateString();
                    if (string.IsNullOrEmpty(houseInfo.H_HoUse))
                    {
                        errInfo += queryslbh + "|" + "tstybm=" + fc_h.TSTYBM + "|" + "规划用途为空"+"\r\n";
                        Exception ex = new Exception(errInfo);
                        LogHelper.WriteLog("异常", ex);
                        //throw ex;
                    }

                    list.Add(houseInfo);
                }
               
            }
            return list;
        }

        

        private static string GetBuName(FC_Z_QSDC fc_z)
        {
            if (!string.IsNullOrEmpty(fc_z.JZWMC))
                return fc_z.JZWMC;
            if (!string.IsNullOrEmpty(fc_z.ZMC))
                return fc_z.ZMC;
            if (!string.IsNullOrEmpty(fc_z.LJZH))
                return fc_z.LJZH;
            if (!string.IsNullOrEmpty(fc_z.ZH))
                return fc_z.ZH;
            
            return "";
        }

        private static string ConvertFWJG(string fWJG)
        {
            switch(fWJG)
            {
                case "1":
                    return "钢";
                case "2":
                    return "砼";
                case "3":
                    return "钢混";
                case "4":
                    return "混";
                case "5":
                    return "砖";
                case "6":
                    return "其他";
                case "7":
                    return "木";
                case "8":
                    return "石";
                case "9":
                    return "竹";
                default:
                    return "";
            }
        }

        private static FC_Z_QSDC GetFC_Z_QSDC(FC_H_QSDC fc_h)
        {
            return  BDCDA_DAL.GetFC_Z_QSDC(fc_h);
            
        }

        public static string GetFCZL(string slbh, FC_H_QSDC fc_h)
        {


            //if(string.IsNullOrEmpty(fc_h.ZL))
            //{
            //    string zl = BDCDA_DAL.GetFC_ZL(fc_h.TSTYBM);
            //    return zl;
            //}
            string zl = BDCDA_DAL.GetFC_ZL(fc_h.TSTYBM);
            if (!string.IsNullOrEmpty(zl))
                return zl;
            return fc_h.ZL;
        }

        

        private static Guid GetCGHouseID(string tSTYBM)
        {
            try
            {
                Guid hid = new Guid(tSTYBM);
                return hid;
            }
            catch
            {
                try
                {
                    string str = BDCDA_DAL.GetOLDCGHouseID(tSTYBM);
                    if (string.IsNullOrEmpty(str))
                    {
                        //throw new Exception("获取房屋测管ID失败,请关联测管数据。");
                        return Guid.NewGuid();
                    }
                    return new Guid(str);
                }
                catch(Exception ex2)
                {
                    throw ex2;
                }
                
            }
        }

        private static string GetYTMC(string input)
        {
            if (!string.IsNullOrEmpty(input) && !(input.Contains(",") || input.Contains("，")))
            {
                string ghyt = string.Empty;
                string[] yts = input.Split(',');
                for (int i = 0; i < yts.Length; i++)
                {
                    string tmp = BDCDA_DAL.GetGHYTMC(yts[i]);
                    if (string.IsNullOrEmpty(ghyt))
                    {
                        ghyt = tmp;
                    }
                    else
                    {
                        ghyt += ',' + tmp;
                    }
                }
                return ghyt;
            }
            
            return "";

        }

        private static string ChangeYT(string input)
        {
            if (input == "10")
                return "住宅";
            else if (input == "11")
                return "成套住宅";
            else if (input == "111")
                return "别墅";
            else if (input == "112")
                return "高档公寓";
            else if (input == "12")
                return "非成套住宅、阁楼";
            else if (input == "13")
                return "集体宿舍";
            else if (input == "15")
                return "地下室";
            else if (input == "20")
                return "工业、交通、仓储";
            else if (input == "21")
                return "工业";
            else if (input == "23")
                return "铁路";
            else if (input == "24")
                return "民航";
            else if (input == "25")
                return "航运";
            else if (input == "26")
                return "公共运输";
            else if (input == "27" )
                return "仓储、车库位、储藏、车库";
            else if (input == "30")
                return "商业、金融、信息";
            else if (input == "31")
                return "商业服务";
            else if (input == "32")
                return "经营";
            else if (input == "33")
                return "旅游";
            else if (input == "34")
                return "金融保险";
            else if (input == "35")
                return "电讯信息";
            else if (input == "40")
                return "教育、医疗、卫生、科研";
            else if (input == "41")
                return "教育";
            else if (input == "42")
                return "医疗卫生";
            else if (input == "43")
                return "科研";
            else if (input == "50")
                return "文化、娱乐、体育";
            else if (input == "51")
                return "文化";
            else if (input == "52")
                return "新闻";
            else if (input == "53")
                return "娱乐";
            else if (input == "54")
                return "园林绿化";
            else if (input == "55")
                return "体育";
            else if (input == "60")
                return "办公";
            else if (input == "70")
                return "军事";
            else if (input == "80")
                return "其他";
            else if (input == "81")
                return "涉外";
            else if (input == "82")
                return "宗教";
            else if (input == "83")
                return "监狱";
            else if (input == "84")
                return "物管用房";
            else if (input == "88")
                return "公共设施、人防";
            else
                return "其他";
        }
        private static List<FC_H_QSDC> GetFC_H(string slbh)
        {
            //string prjID = GetRealSLBH(slbh);
            
            List<FC_H_QSDC> tmplist= BDCDA_DAL.GetFC_H_QSDC(slbh).Distinct<FC_H_QSDC>().ToList<FC_H_QSDC>();


            List<FC_H_QSDC> list = tmplist.Where(p => p.JZMJ > 0).ToList<FC_H_QSDC>();
            if (null!=list && list.Count>0)
                return list.Where((x, i) => list.FindIndex(z => z.TSTYBM == x.TSTYBM) == i).ToList<FC_H_QSDC>();
           else
                return tmplist.Where((x, i) => tmplist.FindIndex(z => z.TSTYBM == x.TSTYBM) == i).ToList<FC_H_QSDC>();
        }

        private static List<Certificate> GetCertificate(ArchiveIndex archiveIndex,List<HouseInfo> houseInfo_List ,string slbh)
        {
            //GetDJB(pageParams.PrjId);

            List<Certificate> list = new List<Certificate>();

            foreach (HouseInfo houseInfo in houseInfo_List)
            {
                Certificate cer = new Certificate();
                cer.ArchiveId = archiveIndex.ArchiveId;
                cer.CertificateID = CreateGuid(32);
                cer.HouseInfo_ID = houseInfo.HouseInfo_ID;
                cer.Prop = GetBDCZH_By_BDCDYH(houseInfo.TSTYBM, archiveIndex);
                cer.CertificateType = GetCertificateType(cer.Prop);
                cer.GrantDate = GetFZRQ(archiveIndex.BusiNO);
                cer.PrintNO = GetZJXLH(archiveIndex.BusiNO);
                list.Add(cer);
            }
            
            
            return list;
        }

        private static string GetZJXLH(string prop)
        {
            return BDCDA_DAL.GetZSXLH(prop);
        }

        private static string GetFZRQ(string slbh)
        {
            return BDCDA_DAL.GetFZRQ(slbh);
        }

        private static string GetBDCZH_By_BDCDYH(string tytybm,ArchiveIndex archiveIndex)
        {
            string realslbh = "";
            //if (!archiveIndex.ReqType.Equals("首次登记"))
            //{
            //    realslbh = archiveIndex.BusiNO;
            //}
            //else
            //{

            if (archiveIndex.ArchiveType.ToUpper().Equals("Z"))
            {
                realslbh = BDCDA_DAL.GetSlbhByBDCDYH(tytybm, archiveIndex.FmBusiNo);
            }
            else
            {
                realslbh = BDCDA_DAL.GetSlbhByBDCDYH(tytybm, archiveIndex.SLBH,archiveIndex.ArchiveType);
            }
           
            //}
            return BDCDA_DAL.GetBDCZH(realslbh);
        }

        private static string GetBDCZH(string slbh)
        {
            //string slbh = BDCDA_DAL.GetSlbhByBDCDYH(bdcdyh);
            return BDCDA_DAL.GetBDCZH(slbh);
        }

        private static string GetCertificateType(string p)
        {
            if (p.Contains("证明") || p.Contains("YG"))
                return "03";
            if (p.Contains("权证"))
                return "01";
            return "01";
        }

        private static DJ_DJB GetDJB(string p)
        {
            throw new NotImplementedException();
        }

        private static string GetRealSLBH(string slbh)
        {
            slbh = slbh.Replace("FC", "").Replace("O", "").Replace("OF", "").Replace("M","").Replace("OCS","").Replace("F","").Replace("C","").Replace("S","");
            if (slbh.Contains("_"))
                slbh = slbh.Substring(0, slbh.IndexOf('_'));
            //if(slbh.Contains("-"))
            //{
            //    slbh = slbh.Substring(0, slbh.IndexOf('-'));
            //}
            return slbh;
        }

        public static List<ArchiveIndex> GetArchiveIndex(string slbh)
        {
            List<ArchiveIndex> archiveIndex_list = new List<ArchiveIndex>();

            List<DJ_XGDJGL> xgdjgl_List=GetXGDJGL(slbh);

            //if (null != xgdjgl_List && xgdjgl_List.Count > 0)
            //{
                string querySLBH = slbh;
                if (slbh.Contains("-"))
                {
                    querySLBH = querySLBH.Substring(0, querySLBH.IndexOf("-"));
                }


                List<DJ_TSGL> tsgl_list = GetTSGL(querySLBH);
            //根据图属关联登记种类，判定业务流程个数
                if (null != tsgl_list && tsgl_list.Count > 1)
                {
                    tsgl_list = InitTSGL_List(tsgl_list);
                }

                foreach (DJ_TSGL tsgl in tsgl_list)
                {
                    //填充档案业务表数据
                    if (tsgl.SLBH.Contains("ZX") || tsgl.SLBH.Contains("JF"))
                    {
                        continue;
                    }
                    ArchiveIndex archiveIndex = new ArchiveIndex();

                    archiveIndex.ArchiveId = CreateGuid(32);
                    //archiveIndex.DaCode = archiveIndex.ArchiveId.ToString();
                    archiveIndex.ArchiveType = GetArchiveType(tsgl.DJZL);

                    archiveIndex.IsHistoray = GetHistry(tsgl.LIFECYCLE);
                    archiveIndex.ArchiveDate = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "");
                    archiveIndex.ReqType = GetDJLX(tsgl.SLBH);// tsgl.DJZL;
                    if (archiveIndex.ReqType.Equals("首次登记"))
                    {
                        archiveIndex.BusiNO = slbh;
                    }
                    else
                    {
                        archiveIndex.BusiNO = tsgl.SLBH;//GetRealSLBH(tsgl.SLBH);
                    }
                    if (null != xgdjgl_List && xgdjgl_List.Count > 0)
                        archiveIndex.FmBusiNo = GetRealSLBH(GetFmBusino(xgdjgl_List, tsgl.SLBH));//GetRealSLBH(xgdjgl_List.FSLBH);
                    archiveIndex.SLBH = tsgl.SLBH;
                    archiveIndex.DJZL = tsgl.DJZL;
                    archiveIndex.IsOld = "2";
                    //archiveIndex.XGZH = GetXGZH(tsgl.SLBH);
                    //archiveIndex.FSLBH_List = xgdjgl_List;
                    archiveIndex_list.Add(archiveIndex);
                }

            return archiveIndex_list;
        }

        private static string GetXGZH(string sLBH)
        {
            return BDCDA_DAL.GetXGZH(sLBH);
        }

        private static string GetFmBusino(List<DJ_XGDJGL> xgdjgl_List, string sLBH)
        {
            Predicate<DJ_XGDJGL> findValue = delegate (DJ_XGDJGL p)
            {
                return p.ZSLBH.Equals(sLBH);
            };
            DJ_XGDJGL res = xgdjgl_List.Find(findValue);
            if (null == res)
                return "";
            return res.FSLBH;
        }

        private static string GetDJLX(string sLBH)
        {
            return BDCDA_DAL.GetDJLX(sLBH);
        }

        private static List<DJ_TSGL> InitTSGL_List(List<DJ_TSGL> tsgl_list)
        {
            List<DJ_TSGL> list = new List<DJ_TSGL>();
            foreach (DJ_TSGL tsgl in tsgl_list)
            {
                var res = list.Where(u => u.DJZL == tsgl.DJZL);
                if(null==res || res.Count<DJ_TSGL>()==0)
                {
                    list.Add(tsgl);
                }
            }
            return list;
        }

        private static string GetHistry(decimal? nullable)
        {
            return (nullable != 1 ? 0 : nullable).ToString();
        }

        private static List<DJ_TSGL> GetTSGL(string slbh)
        {
            return BDCDA_DAL.GetTSGL(slbh);
        }

     
        private static List<DJ_XGDJGL> GetXGDJGL(string slbh)
        {
            return BDCDA_DAL.GetDJ_XGDJGL(slbh);
        }

        private static string GetArchiveType(string djzl)
        {
            switch(djzl)
            {
                case "权属":
                case "预告":
                    return "C";
                
                case "抵押":
                    return "D";
                case "权属注销":
                case "预告注销":
                case "抵押注销":
                    return "Z";
                default:
                    return "N";
            }
        }

        private static Guid CreateGuid(int length)
        {
            //string str = Guid.NewGuid().ToString().Substring(3);
            //return new Guid("BDC" + str);
            return Guid.NewGuid();
        }

       
    }
}