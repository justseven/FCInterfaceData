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
using XZFCDA.Models.BDCModel;
using XZFCDA.Dal;
using XZFCDA.Models;
using XZFCDA.Tools;
using XZFCDA.FC.Models;

namespace XZFCDA.Bll
{
    public class FCDA_BLL
    {
        /// <summary>
        /// 插入产权处档案库
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>1:插入成功；0：宗地业务；异常信息：失败</returns>
        public static BDCFilterResult Insert_FCDA(PageParams pageParams)
        {
            BDCFilterResult res = new BDCFilterResult();
           
            try
            {
                if (IsFW(pageParams.PrjId) )
                {
                    if (CanPush(pageParams.PrjId))
                    {
                        ArchiveIndex archiveIndex = GetArchiveIndex(pageParams);

                        HouseInfo houseInfo = GetHouseInfo(pageParams);

                        Certificate certificate = GetCertificate(archiveIndex, houseInfo, pageParams);

                        List<Person> person_list = GetPerson(archiveIndex, pageParams);

                        List<VolEleArc> volEleArc = GetVolEleArc(archiveIndex, pageParams);
                        //List<VolEleArcDtl> volEleArcDtl_list = GetVolEleArcDtl(volEleArc, pageParams);

                        PropArchiveRelation propArchiveRelation = GetPropArchiveRelation(archiveIndex, certificate);
                        HouseArchiveRelation houseArchiveRelation = GetHouseArchiveRelation(archiveIndex, houseInfo);

                        FCDA_DAL.Insert(archiveIndex, volEleArc, houseInfo, person_list, certificate, houseArchiveRelation, propArchiveRelation, null);
                        res.ConfirmType = 0;
                        res.IsSuccess = true;
                        res.Message = "成功！业务已经成功推送！";


                    }
                    else
                    {
                        res.ConfirmType = 0;
                        res.IsSuccess = false;
                        res.Message = "失败！该业务已经推送！";
                    }
                }
                else
                {
                    res.ConfirmType = 0;
                    res.IsSuccess = false;
                    res.Message = "拒绝！未找到业务宗号或非房产业务，暂不推送";
                }
            }
            catch(Exception ex)
            {
                res.ConfirmType = 0;
                res.IsSuccess = false;
                res.Message = "失败！"+ex.Message;
            }
            FC_DA_TAG tag = new FC_DA_TAG();
            tag.ID = Guid.NewGuid().ToString();
            tag.ISSUCCESS = res.IsSuccess?"1":"0";
            tag.MESSAGE = res.Message;
            tag.PUSHDATE = DateTime.Now;
            tag.PUSHUSER = pageParams.UserName;
            tag.SLBH = pageParams.PrjId;

            InsertLog(tag);
            return res;

        }


       

        public static void InsertPIC(string p)
        {
            DataTable dt = FCDA_DAL.GetVolEleArcDtlByVolEleArcID(p);
            int i=1;
            string dir = "10000-450000/110000/100001/";
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FCFtpIP"],
                username = ConfigurationManager.AppSettings["FCFtpUser"],
                password = ConfigurationManager.AppSettings["FCFtpPWD"]
            };
            List<string> list = FTPHelper.ListDirectory(dir, souFTp, "");

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
            BDCDA_DAL.InsertLog(tag);
        }

        private static bool CanPush(string slbh)
        {
            return FCDA_DAL.CanPust(slbh);
        }

        

        private static bool IsFW(string slbh)
        {
            return BDCDA_DAL.IsFW(slbh);
        }

        private static HouseArchiveRelation GetHouseArchiveRelation(ArchiveIndex archiveIndex, HouseInfo houseInfo)
        {
            HouseArchiveRelation houseArchiveRelation = new HouseArchiveRelation();
            houseArchiveRelation.RelationID = CreateGuid(32);
            houseArchiveRelation.ArchiveId = archiveIndex.ArchiveId;
            houseArchiveRelation.HouseInfo_ID = houseInfo.HouseInfo_ID;

            return houseArchiveRelation;
        }

        private static PropArchiveRelation GetPropArchiveRelation(ArchiveIndex archiveIndex, Certificate certificate)
        {
            PropArchiveRelation propArchiveRelation = new PropArchiveRelation();
            propArchiveRelation.RelationID = CreateGuid(32);
            propArchiveRelation.ArchiveId = archiveIndex.ArchiveId;
            propArchiveRelation.CertificateID = certificate.CertificateID;

            return propArchiveRelation;
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

        private static List<VolEleArc> GetVolEleArc(ArchiveIndex archiveIndex, PageParams pageParams)
        {
            List<VolEleArc> list = new List<VolEleArc>();

            List<WFM_ATTACHLST> wfm_ATTACHLST_list = GetWFM_ATTACHLST_list(pageParams);

            foreach (WFM_ATTACHLST doc in wfm_ATTACHLST_list)
            {
                if (doc.CNAME == "流程附件")
                {
                    continue;
                }
                VolEleArc volEleArc = new VolEleArc();
                volEleArc.ArchiveId = archiveIndex.ArchiveId;
                volEleArc.EleArcVol_ID = CreateGuid(32);
                volEleArc.EleArcName = doc.CNAME;
                list.Add(volEleArc);
            }
            
            
            return list;
        }

        private static List<WFM_ATTACHLST> GetWFM_ATTACHLST_list(PageParams pageParams)
        {
            return BDCDA_DAL.GetWFM_ATTACHLST(pageParams.PrjId);
        }

        private static List<Person> GetPerson(ArchiveIndex archiveIndex, PageParams pageParams)
        {
            List<Person> list = new List<Person>();

            List<DJ_QLR> qlr_list = GetQlr(pageParams.PrjId);
            if (null != qlr_list && qlr_list.Count > 0)
            {
                foreach (DJ_QLR qlr in qlr_list)
                {
                    Person person = new Person();
                    person.ArchiveId = archiveIndex.ArchiveId;
                    person.PersonID = CreateGuid(32);
                    person.PersonType = GetPersonType(qlr.QLRID,pageParams.PrjId);
                    person.Name = qlr.QLRMC;
                    person.CardNO = qlr.ZJHM;
                    person.Sex = qlr.XB;
                    list.Add(person);
                }
            }

            return list;
            
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

        private static HouseInfo GetHouseInfo(PageParams pageParams)
        {
            FC_H_QSDC fc_h = GetFC_H(pageParams.PrjId);
            HouseInfo houseInfo = new HouseInfo();
            houseInfo.HouseInfo_ID = CreateGuid(32);
            houseInfo.HoSite = fc_h.ZL;
            houseInfo.H_HoUse = fc_h.GHYT;
            houseInfo.H_ConAcre = fc_h.JZMJ;
            houseInfo.BDCDYH = fc_h.BDCDYH;

            return houseInfo;
        }

        private static FC_H_QSDC GetFC_H(string slbh)
        {
            return BDCDA_DAL.GetFC_H_QSDC(slbh);
        }

        private static Certificate GetCertificate(ArchiveIndex archiveIndex,HouseInfo houseInfo ,PageParams pageParams)
        {
            //GetDJB(pageParams.PrjId);
            
            

            Certificate cer = new Certificate();
            cer.ArchiveId = archiveIndex.ArchiveId;
            cer.CertificateID = CreateGuid(32);
            cer.HouseInfo_ID = houseInfo.HouseInfo_ID;
            cer.Prop = GetBDCZH(pageParams.PrjId);
            cer.CertificateType = GetCertificateType(cer.Prop);
            
            return cer;
        }

        private static string GetBDCZH(string slbh)
        {
            return BDCDA_DAL.GetBDCZH(slbh);
        }

        private static string GetCertificateType(string p)
        {
            if (p.Contains("证明"))
                return "03";
            if (p.Contains("权证"))
                return "01";
            return "01";
        }

        private static DJ_DJB GetDJB(string p)
        {
            throw new NotImplementedException();
        }

        private static ArchiveIndex GetArchiveIndex(PageParams pageParams)
        {
            DJ_XGDJGL xgdjgl=GetXGDJGL(pageParams.PrjId);
            DJ_TSGL tsgl = GetTSGL(pageParams.PrjId);

            ArchiveIndex archiveIndex = new ArchiveIndex();

            archiveIndex.ArchiveId = CreateGuid(32);
            //archiveIndex.DaCode = archiveIndex.ArchiveId.ToString();
            archiveIndex.ArchiveType = GetArchiveType(tsgl.DJZL);
            archiveIndex.BusiNO = pageParams.PrjId;
            archiveIndex.IsHistoray = GetHistry(tsgl.LIFECYCLE);
            archiveIndex.ArchiveDate = DateTime.Now.ToString("yyyy-MM-dd").Replace("-","");
            archiveIndex.ReqType = tsgl.DJZL;
            if (null != xgdjgl)
                archiveIndex.FmBusiNo = xgdjgl.FSLBH;
            return archiveIndex;
        }

        private static string GetHistry(decimal? nullable)
        {
            return (nullable != 1 ? 0 : nullable).ToString();
        }

        private static DJ_TSGL GetTSGL(string slbh)
        {
            return BDCDA_DAL.GetTSGL(slbh);
        }

     
        private static DJ_XGDJGL GetXGDJGL(string slbh)
        {
            return BDCDA_DAL.GetDJ_XGDJGL(slbh);
        }

        private static string GetArchiveType(string djzl)
        {
            switch(djzl)
            {
                case "权属":
                    return "C";
                default:
                    return "D";
            }
        }

        private static Guid CreateGuid(int length)
        {
            string str = Guid.NewGuid().ToString().Substring(3);
            return new Guid("BDC" + str);
        }

       
    }
}