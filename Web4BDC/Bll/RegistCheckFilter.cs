using FCWebServices.ServiceReference1;
using Geo.Plug.DataExchange.XZFCPlug;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Models; 

namespace Web4BDC.Bll
{
    /// <summary>
    /// 登记时网备检查
    /// </summary>
    public class RegistCheckFilter : IBDCFilter
    {
        private Dictionary<string, string> configFromBDC = new Dictionary<string, string>();
        public RegistCheckFilter(Dictionary<string, string> configs) {
            configFromBDC = configs;
        }
        public BDCFilterResult Filter(PageParams param)
        {
            try
            {
                if (configFromBDC.ContainsKey("RegistCheckFilter") && configFromBDC["RegistCheckFilter"].Equals("是"))
                {

                    string djlx = getDJLXBySLBH(param.PrjId);
                    IList<BDCFilterResult> retS = new List<BDCFilterResult>();
                    IList<string> hs = GetDJHouses(param.PrjId);
                    if (hs == null || hs.Count == 0)
                    {
                        return new BDCFilterResult { IsSuccess = true };
                    }
                    string tzr = GetTXRXM(param.PrjId, djlx);
                    if (hs.Count > 0)
                    {
                        foreach (string h in hs)
                        {//在不动产系统中安装
                            BDCFilterResult ret = new BDCFilterResult();
                            ret.IsSuccess = true;
                            if (!configFromBDC.ContainsKey("RegistCheckFrom"))
                            {
                                int how2Check = How2Check(h, param.PrjId);
                                if (how2Check == 1 && djlx != "预告")
                                {
                                    ret = CheckInBDC(h, djlx, param.PrjId, tzr);
                                }
                                else if (how2Check == 2 || djlx == "预告")
                                {
                                    WBInfoForAHouse aH = GetInfoInWB(h);
                                    if (aH != null)
                                    {
                                        ret = CheckInWB(aH, djlx, param.PrjId, tzr);
                                    }
                                    else
                                    {
                                        FCWebServiceDal dal = new FCWebServiceDal();
                                        if (!string.IsNullOrEmpty(dal.GetHYCId(h)))
                                        {
                                            ret.IsSuccess = false;
                                            ret.Message = "户ID为[" + h + "]存在预测信息,但在网备中未找到任何信息。\n";
                                            ret.ConfirmType = 1;
                                        }
                                    }
                                }
                                retS.Add(ret);
                            }
                            else
                            {
                                if (configFromBDC["RegistCheckFrom"].Contains("不动产"))
                                {
                                    ret = CheckInBDC(h, djlx, param.PrjId, tzr);
                                    retS.Add(ret);
                                }
                                if (configFromBDC["RegistCheckFrom"].Contains("网备"))
                                {
                                    WBInfoForAHouse aH = GetInfoInWB(h);
                                    if (aH != null)
                                    {
                                        ret = CheckInWB(aH, djlx, param.PrjId, tzr);
                                    }
                                    else
                                    {
                                        FCWebServiceDal dal = new FCWebServiceDal();
                                        if (!string.IsNullOrEmpty(dal.GetHYCId(h)))
                                        {
                                            ret.IsSuccess = false;
                                            ret.Message = "户ID为[" + h + "]存在预测信息,但在网备中未找到任何信息。\n";
                                            ret.ConfirmType = 1;
                                        }
                                    }
                                    retS.Add(ret);
                                } 
                            }

                            
                        }
                    }
                    if (configFromBDC.ContainsKey("RegistCheckFrom")&&configFromBDC["RegistCheckFrom"].Equals("存量房"))
                    {
                        FCWebServiceDal dal = new FCWebServiceDal();
                        IList<XGZHAndQLR> xgzhAndQlrs = dal.GetFDJBInfo(param.PrjId);
                        BDCFilterResult ret = CheckInCLF(xgzhAndQlrs, djlx, param.PrjId, tzr);
                        retS.Add(ret);
                    }
                    BDCFilterResult retn = new BDCFilterResult();
                    bool isSuccess = true;
                    string message = "";
                    int confirmType = -1;
                    foreach (BDCFilterResult r in retS)
                    {
                        if (!r.IsSuccess)
                        {
                            isSuccess = false;
                            message += r.Message + "\n";
                            confirmType = 1;
                        }
                        if (r.ConfirmType == 0)
                        {
                            confirmType = 0;
                        }
                    }
                    retn.ConfirmType = confirmType;
                    retn.Message = message;
                    retn.IsSuccess = isSuccess;
                    return retn;
                }
                else
                {
                    return new BDCFilterResult { IsSuccess = true };
                }
            }
            catch (Exception ex) {
                return new BDCFilterResult { IsSuccess = false, ConfirmType = 0,Message=ex.Message };
            }
        }

        //private IList<string> GetLastQlrMc(string houseid) {
        //    IList<string> ret = GetQlrMCInBdc(houseid);
        //    if (ret == null || ret.Count == 0)
        //    {
        //        return GetQlrMCInWB(houseid);
        //    }
        //    else {
        //        return ret;
        //    }
        //}

        /// <summary>
        /// 如果没有
        /// 怎样检查数据。1，在不动产中检查，2，在网备中检查
        /// </summary>
        /// <returns></returns>
        private int How2Check(string h,string slbh)
        {//是否做过登记，如果做个登记，就在不动产中检查，否则在网备中检查
            FCWebServiceDal dal = new FCWebServiceDal();
            return dal.HasQSOrYGRegister(h, slbh) > 0 ? 1 : 2;
        }
        private IList<string> GetQlrMCInBdc(string houseId) {
            FCWebServiceDal dal = new FCWebServiceDal();
            DataTable dt = dal.GetQLRMCByHouseId(houseId);
            IList<string> mc = new List<string>();
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    mc.Add(dt.Rows[i][0].ToString());
                }
            }
            return mc;
        }
        /// <summary>
        /// 获取网备中的权利人名称
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        private WBInfoForAHouse GetInfoInWB(string houseId)
        {
            WBInfoForAHouse mc = getQlrMCInWB(houseId);
            if (mc != null) {
                return mc;
            }
            else {
                BDCExtendDal dal = new BDCExtendDal();
                string ychouseid = dal.GetYCHouseIdBy(houseId);
                mc = getQlrMCInWB(houseId);
                if (mc != null)
                {
                    return mc;
                }
                else {
                    string schouseid = dal.GetSCHouseIdBy(houseId);
                    return getQlrMCInWB(houseId);
                }
            }
        }

        private WBInfoForAHouse getQlrMCInWB(string houseid)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            string paramstring = "HouseID=" + houseid;
            DataSet ds = soap.SPF_FC_FWQK(paramstring);
            WBInfoForAHouse w = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                w = new WBInfoForAHouse();
                w.HID = ds.Tables[0].Rows[0]["HID"].ToString();
                w.XMMC = ds.Tables[0].Rows[0]["XMMC"].ToString();
                w.LPMC = ds.Tables[0].Rows[0]["LPMC"].ToString();
                w.DYH = ds.Tables[0].Rows[0]["DYH"].ToString();
                w.FJH = ds.Tables[0].Rows[0]["FJH"].ToString();
                w.JZMJ = ds.Tables[0].Rows[0]["JZMJ"].ToString();
                w.FWZT = ds.Tables[0].Rows[0]["FWZT"].ToString();
                w.CFZT = ds.Tables[0].Rows[0]["CFZT"].ToString();
                w.DYZT = ds.Tables[0].Rows[0]["DYZT"].ToString();
                w.ZXZT = ds.Tables[0].Rows[0]["ZXZT"].ToString();
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    w.MCList = new List<string>();
                    w.ZJLXList = new List<string>();
                    w.ZJHMList = new List<string>();
                    w.LXDHList = new List<string>();
                    w.LXDZList = new List<string>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        w.MCList.Add(ds.Tables[1].Rows[i]["MC"].ToString());
                        w.ZJLXList.Add(ds.Tables[1].Rows[i]["ZJLX"].ToString());
                        w.ZJHMList.Add(ds.Tables[1].Rows[i]["ZJHM"].ToString());
                        w.LXDZList.Add(ds.Tables[1].Rows[i]["LXDZ"].ToString());
                    }
                }
            }
            return w;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns>0,权属，1，抵押，2，预告，3，查封</returns>
        private string getDJLXBySLBH(string slbh) {
            BDCExtendDal dal = new BDCExtendDal();
            return dal.GetDJLX(slbh);
        }

        private IList<string> GetDJHouses(string slbh) {
            BDCExtendDal dal = new BDCExtendDal();
            return dal.GetHouseIdsBySLBH(slbh);
        }

        private string GetTXRXM(string slbh,string djlx) {
            BDCExtendDal dal = new BDCExtendDal();
            DataTable dt = null;
            if (djlx.Equals("权属"))
            {//返回权利人
                dt = dal.GetQLRBySLBH(slbh);
            }
            else if (djlx.Equals("抵押"))//返回抵押人
            {
                dt = dal.GetDYRBySLBH(slbh);
            }
            else {
                  dt = dal.GetTZRXMBySLBH(slbh); 
            }
            string r = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!r.Contains(dt.Rows[i][0].ToString()))
                    {
                        r += " " + dt.Rows[i][0].ToString();
                    }
                }
            }
            return r.Trim();
        }

        private BDCFilterResult CheckInBDC(string h, string djlx, string slbh, string tzr) {
            if (djlx.Equals("查封") || djlx.Equals("抵押"))
            {
                IList<string> qs = GetQlrMCInBdc(h);
                if (qs != null && qs.Count > 0)
                {
                    bool hasQ = false;
                    foreach (string q in qs)
                    {
                        if (!string.IsNullOrEmpty(q)&&tzr.Contains(q))
                        {
                            hasQ = true;
                        }
                    }
                    if (!hasQ)
                    {
                        return new BDCFilterResult { IsSuccess = false, Message = "通知人姓名和当前登记权利人不符合", ConfirmType = 0 };
                    }
                }
                return new BDCFilterResult { IsSuccess = true };
            }
            return new BDCFilterResult { IsSuccess = true };
        }
        private BDCFilterResult CheckInWB(WBInfoForAHouse wbInfo,string djlx,string slbh, string tzr) {
            BDCFilterResult ret = new BDCFilterResult { IsSuccess = true };
            if (djlx.Equals("抵押"))
            {
                if (wbInfo.CFZT != "0")
                {//网备里面有查封，不能做权属登记
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有查封状态";
                    ret.ConfirmType = 0;
                    return ret;
                }
                if (wbInfo.DYZT != "0")
                {
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有抵押状态";
                    ret.ConfirmType = 1;
                    return ret;
                }
            }
            else if (djlx.Equals("权属"))
            {
                if (wbInfo.CFZT != "0")
                {//网备里面有查封，不能做权属登记
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有查封状态";
                    ret.ConfirmType = 0;
                    return ret;
                }
                if (wbInfo.DYZT != "0")
                {
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有抵押状态";
                    ret.ConfirmType = 0;
                    return ret;
                }
            }
            else if (djlx.Equals("预告"))
            {
                if (wbInfo.CFZT != "0")
                {//网备里面有查封，不能做权属登记
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有查封状态";
                    ret.ConfirmType = 0;
                    return ret;
                }
                if (wbInfo.DYZT != "0")
                {
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中有抵押状态";
                    ret.ConfirmType = 0;
                    return ret;
                }
                bool hasQ = false;
                foreach (string q in wbInfo.MCList)
                {
                    if (tzr.Contains(q))
                    {
                        hasQ = true;
                    }
                }
                if (!hasQ)
                {
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中的人名和通知人名不一致";
                    ret.ConfirmType = 0;
                    return ret;
                }
            }
            else if (djlx.Equals("查封")) {
                bool hasQ = false;
                foreach (string q in wbInfo.MCList)
                {
                    if (tzr.Contains(q))
                    {
                        hasQ = true;
                    }
                }
                if (!hasQ)
                {
                    ret.IsSuccess = false;
                    ret.Message = wbInfo.ZL + ",在网签系统中的人名和通知人名不一致";
                    ret.ConfirmType = 0;
                    return ret;
                }
            }
            return ret; 
        }

        private BDCFilterResult CheckInCLF(IList<XGZHAndQLR> xgzhAndQlrs, string djlx, string slbh, string tzr)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            bool find = false;
            string htbah = "";
            foreach (XGZHAndQLR xgzhAndQlr in xgzhAndQlrs)
            {
                IList<string> qlrs = xgzhAndQlr.QLRS;
                
                if (qlrs.Count > 0)
                {
                    foreach (string qlr in qlrs)
                    {
                        IDictionary<string, string> paramss = new Dictionary<string, string>();
                        paramss["CQZH"] = xgzhAndQlr.XGZH;
                        paramss["SYQR"] = qlr;
                        FC_CLMMHT_WSData ht_Data = new FC_CLMMHT_WSData("CLF_FC_CLMMHT", "2000");
                        string backDatas = ht_Data.GetWebServiceData(paramss, null);
                        var bdc = ht_Data.XMLParsing(backDatas);
                        if (bdc.data != null && bdc.data.dt != null && bdc.data.dt.Rows.Count > 0)
                        {
                            find = true;
                            htbah = bdc.data.dt.Rows[0]["CLHTBAH"].ToString();
                            break;
                        }
                    }
                }
            }
            BDCFilterResult ret = new BDCFilterResult { IsSuccess = true };
            if (djlx.Equals("权属"))//转移的通知人是不是和义务人一样
            {//
                if (find)
                { 
                    string message = "";
                    bool isSucess = IfTZRInCLR( htbah,tzr,ref message);
                    if (isSucess)
                    {
                        return new BDCFilterResult { IsSuccess = true };
                    }
                    else
                    {
                        return new BDCFilterResult { IsSuccess = false, Message = "存量房系统中购房者为：" + message, ConfirmType = 1 };
                    }
                }
                else {
                    return new BDCFilterResult { IsSuccess = false, Message = "存量房系统中未发现交易信息" };
                } 
            }
            else if (djlx.Equals("查封")|| djlx.Equals("抵押"))
            {
                if (find)
                { 
                    string message = "";
                    bool isSucess = IfTZRInCLR( htbah, tzr, ref message);
                    if (isSucess)
                    {
                        return new BDCFilterResult { IsSuccess = true };
                    }
                    else
                    {
                        return new BDCFilterResult { IsSuccess = false, Message = "存量房系统中存在交易信息购房者为：" + message, ConfirmType = 1 };
                    }
                }
                else {
                    return new BDCFilterResult { IsSuccess = true };
                }
            }
            return new BDCFilterResult { IsSuccess = true };
        }

        private bool IfTZRInCLR( string htbah,string tzr,ref string message)
        {
            //BDC qlrBdc = soap.FC_GFQLRXX("HTBAH=" + htbah);
            IDictionary<string, string> paramss = new Dictionary<string, string>();
            paramss["HTBAH"] = htbah;
            FC_GFQLRXX_WSData ht_Data = new FC_GFQLRXX_WSData("FC_GFQLRXX", "2000");
            string backDatas = ht_Data.GetWebServiceData(paramss, null);
            var qlrBdc = ht_Data.XMLParsing(backDatas);
            bool isSucess = false; 
            if (qlrBdc != null && qlrBdc.data != null && qlrBdc.data.dt.Rows.Count > 0)
            {
                for (int i = 0; i < qlrBdc.data.dt.Rows.Count; i++)
                {
                    if (qlrBdc.data.dt.Rows[i]["XGRLX"].ToString().Equals("1"))
                    {
                        message += qlrBdc.data.dt.Rows[i]["XGRMC"].ToString() + " ";
                        if (qlrBdc.data.dt.Rows[i]["XGRMC"].ToString().Contains(tzr)|| tzr.Contains(qlrBdc.data.dt.Rows[i]["XGRMC"].ToString()))
                        {
                            isSucess = true;
                            break;
                        }
                    }

                }
            }
            return isSucess;
        }
    }
}