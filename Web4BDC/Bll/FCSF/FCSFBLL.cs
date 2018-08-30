
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using Web4BDC.Dal.FCSF;
using Web4BDC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.FCSFModel;
using Web4BDC.Models.ZZJFModel;

namespace Web4BDC.Bll.FCSF
{
    public class FCSFBLL
    {
        public static List<FpmxList_tyfp> GetFpmxList_tyfp(string slbh)
        {
            List<FpmxList_tyfp> list = new List<FpmxList_tyfp>();
            //DJ_SFD sfd = FCSFDAL.GetSFD(param.PrjId);

            List<DJ_SFD_FB> sfd_fb = FCSFDAL.GetSFDFB(slbh);

            if (null != sfd_fb && sfd_fb.Count > 0)
            {
                foreach (var sfd in sfd_fb)
                {
                    FpmxList_tyfp fpmx = new FpmxList_tyfp();
                    
                    fpmx.IsNew = "1";
                    fpmx.Ywzh = sfd.SLBH;
                    fpmx.实收费用 = sfd.HSJE;
                    fpmx.类型 = sfd.SFXM;
                    list.Add(fpmx);
                }
               
            }
            return list;
        }

        public static BDCFilterResult PushSF(PageParams param)
        {
            BDCFilterResult res = new BDCFilterResult();
            FC_SF_TAG tag = new FC_SF_TAG();

            try
            {
                List<DJ_SFD> sfd_list = FCSFDAL.GetSFD(param.PrjId);
                if (null != sfd_list)
                {
                    foreach (DJ_SFD sfd in sfd_list)
                    {
                        FpList_tyfp fpList = CreateFpList_tyfp(sfd.SLBH);
                        List<FpmxList_tyfp> fpmx = GetFpmxList_tyfp(sfd.SLBH);
                        FCSFDAL.PushSF(fpList, fpmx);
                    }
                    res.IsSuccess = true;
                    res.Message = "成功";
                }
                else
                {
                    res.IsSuccess = true;
                    res.Message = "表单验证失败，获取收费单数据";
                }
                
            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;

               
            }
            try
            {

                tag.ID = Guid.NewGuid().ToString();
                tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                tag.MESSAGE = res.Message;
                tag.PUSHDATE = DateTime.Now;
                tag.PUSHUSER = param.UserName;
                tag.SLBH = param.PrjId;
                InsertLog(tag);
                
            }
            catch(Exception ex)
            {
                res.Message = ex.Message;
            }
            return res;
        }


        public static void RePushSF()
        {
            List<FC_SF_TAG> list = FCSFDAL.GetFailTag();
            if(null!=list && list.Count>0)
            {
                foreach (FC_SF_TAG tag in list)
                {
                    try
                    {
                        PageParams param = new PageParams();

                        param.PrjId = tag.SLBH;
                        param.UserName = tag.PUSHUSER;
                        PushSF(param);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }


        

        private static bool CheckIsXCJF(string prjId)
        {
            int count=FCSFDAL.CheckIsXCJF(prjId);
            if(count>0)
            {
                return true;
            }
            return false;
        }

        private static void UpdateSFD_SFZT(DJ_SFD sfd)
        {
            FCSFDAL.UpdateSFD_SFZT(sfd);
        }

        private static void UpdateSFD_SFZT(JFStateModel model)
        {
            FCSFDAL.UpdateSFD_SFZT(model);
        }

        

        public static BDCFilterResult CheckSFState(PageParams param)
        {
            BDCFilterResult res = new BDCFilterResult();
            List<DJ_SFD> list = FCSFDAL.GetXCSFState(param.PrjId);
            if(null!=list && list.Count>0)
            {
                foreach (DJ_SFD sfd in list)
                {
                    FC_SF_TAG tag = new FC_SF_TAG();
                    tag.SLBH = sfd.SLBH;
                    tag.ID = Guid.NewGuid().ToString();
                    tag.PUSHDATE = DateTime.Now;
                    tag.PUSHUSER = param.UserName;

                    if(sfd.SFZT.Contains("已缴费") || string.IsNullOrEmpty(sfd.ZZSFZT))
                    {
                        tag.ISSUCCESS = true.ToString();
                        tag.MESSAGE = "缴费成功";
                    }
                    else
                    {
                        JFStateModel model = new JFStateModel(sfd.SLBH);
                        if (model.CODE.Equals("00"))
                        {
                            tag.ISSUCCESS = true.ToString();
                            tag.MESSAGE = "缴费成功";
                            
                            if (sfd.YSJE != model.ZFJE)
                            {
                                tag.ISSUCCESS = false.ToString();
                                tag.MESSAGE = "应缴金额与实际缴费金额不符";
                            }
                            if (!model.SLBH.Equals(sfd.SLBH))
                            {
                                tag.ISSUCCESS = false.ToString();
                                tag.MESSAGE = "业务宗号与缴费业务宗号不符";
                            }
                            if (tag.ISSUCCESS.Equals(true.ToString()))
                            {
                                sfd.ZZSFZT = "1";
                                sfd.ZZSFZFFS = model.ZFFS;
                                sfd.DYSJ = TryParse(model.ZFSJ);
                                sfd.SSJE = model.ZFJE;
                                sfd.DYR = "自助缴费";
                                sfd.SKRQ= TryParse(model.ZFSJ);
                                sfd.SFZT = "已缴费";
                                UpdateSFD_SFZT(sfd);
                            }
                        }
                        else
                        {
                            tag.ISSUCCESS = false.ToString();
                            tag.MESSAGE = model.ERRMSG;//"缴费失败!";
                        }
                    }
                    if(tag.ISSUCCESS.Equals(false.ToString()))
                    {
                        res.IsSuccess = false;
                        res.Message = tag.MESSAGE;
                        InsertLog(tag);
                        break;
                    }
                    InsertLog(tag);
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Message = "未查询到相关流程";
            }
            return res;
        }


        private static DateTime TryParse(string timeStr)
        {
            DateTime dt;


            IFormatProvider ifp = new CultureInfo("zh-CN", true);


            bool flag = DateTime.TryParseExact(timeStr, "yyyyMMddHHmmss", ifp, DateTimeStyles.None, out dt);
            return dt;
           
        }

        private static void InsertLog(FC_SF_TAG tag)
        {
            if (FCSFDAL.ExistTag(tag))
            {
                FCSFDAL.UpdateTag(tag);
            }
            else
            {
                FCSFDAL.InsertLog(tag);
            }
        }

        private static FpList_tyfp CreateFpList_tyfp(string slbh)
        {
            FpList_tyfp fpList = new FpList_tyfp();

            List<FC_H_QSDC> h_list=FCSFDAL.GetFC_H_QSDC_List(slbh);
            fpList.addTime = DateTime.Now;
            fpList.djlx = GetDJLX(slbh);
            List<DJ_QLRGL> qlr_list=FCSFDAL.GetDyqr(slbh);

            fpList.dyqr = GetDyqr(qlr_list);
            fpList.FJZMJ = GetFJZMJ(h_list);
            fpList.IsNew = "1";
            fpList.Pgjz = FCSFDAL.GetPgjz(slbh);
            fpList.qzbh = FCSFDAL.GetQzbh(slbh);
            //fpList.SFDate=
            //fpList.SFName=
            fpList.SFState = "0";
            fpList.syqr = GetSyqr(qlr_list);
            fpList.TS = h_list.Count.ToString();
            fpList.ysyqr = FCSFDAL.GetYsyqr(slbh);
            fpList.Ywzh = slbh;
            fpList.zl =GetZL(h_list);
            fpList.zlxz = GetZL(h_list);

            return fpList;

        }

        private static string GetDJLX(string slbh)
        {
            string djCode = FCSFDAL.GetDJLXBySlbh(slbh);
            if(!string.IsNullOrEmpty(djCode))
            {
                switch(djCode)
                {
                    case "910":
                        return "抵押登记";
                    case "400":
                        return "注销登记";
                    case "300":
                        return "变更登记";
                    case "600":
                        return "异议登记";
                    case "700":
                        return "预告登记";
                    case "800":
                        return "查封登记";
                    case "500":
                        return "更正登记";
                    case "200":
                        return "转移登记";
                    case "100":
                        return "首次登记";
                    case "900":
                        return "其它登记";
                    default:
                        return "";
                }
            }
            return "";
        }

        private static string GetSyqr(List<DJ_QLRGL> qlr_list)
        {
            List<DJ_QLRGL> list = qlr_list.Where(x => x.QLRLX == "权利人" || x.QLRLX=="抵押人").ToList<DJ_QLRGL>();
            return ReturnNameByList(list);
        }

        private static string GetDyqr(List<DJ_QLRGL> qlr_list)
        {
            List<DJ_QLRGL> list=qlr_list.Where(x=>x.QLRLX=="抵押权人").ToList<DJ_QLRGL>();
            return ReturnNameByList(list);
        }

        private static string ReturnNameByList(List<DJ_QLRGL> list)
        {
            if (null != list)
            {
                if (list.Count > 1)
                {
                    string name = "";
                    foreach (var item in list)
                    {
                        if (name.Equals(""))
                            name += item.QLRMC;
                        else
                            name += "," + item.QLRMC;
                    }
                    return name;
                }
                if(list.Count==1)
                return list[0].QLRMC;
            }
            return "";
        }

        private static string GetZL(List<FC_H_QSDC> h_list)
        {
            if (h_list.Count > 1)
                return h_list[0].ZL+"(共计"+h_list.Count+"套)";
            return h_list[0].ZL;
        }

       

        private static decimal? GetFJZMJ(List<FC_H_QSDC> h_list)
        {
            return h_list.Sum<FC_H_QSDC>(x => x.JZMJ);
        }
    }
}