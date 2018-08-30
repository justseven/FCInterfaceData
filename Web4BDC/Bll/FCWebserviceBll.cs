using Geo.Plug.DataExchange.XZFCPlug;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Easyui;
using Web4BDC.Models;
using Web4BDC.Tools;
using System.Configuration;
using FCWebServices.ServiceReference1;

namespace Web4BDC.Bll
{

    /// <summary>
    /// 获取房产数据bll
    /// </summary>
    public class FCWebserviceBll
    {
        private const string CLMMHT_Webservice_Address = "CLF_FC_CLMMHT";
        private const string GFQLRXX_Webservice_Address = "FC_GFQLRXX";
        /// <summary>
        /// 从房产的webservice中获取数据到临时表中去
        /// </summary>
        /// <returns>批次号</returns>
        public string GetDataFromFCWebservice(IDictionary<string, string> dicParam) {
            try
            {
                DataExchange dataExchange = new DataExchange();
                return dataExchange.DataExtort(dicParam);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 从房产中根据PCH获取数据
        /// </summary>
        /// <returns></returns>
        public string GetDataFromBDCByPCH(string PCH) {
            if (string.IsNullOrEmpty(PCH)) { return "[]"; }
            IDictionary<string, string> sqls = new Dictionary<string, string>();
            sqls.Add("FC_Z_Table", string.Format("select * from FC_Z_TMP where pch='{0}'", PCH));
            sqls.Add("FC_H_Table", string.Format("select * from FC_H_TMP where pch='{0}'", PCH));
            sqls.Add("FC_SPFYGHT_Table", string.Format("select * from FC_SPFYGHT_TMP where pch='{0}'", PCH));
            sqls.Add("FC_CLMMHT_Table", string.Format("select * from FC_CLMMHT_TMP where pch='{0}'", PCH));
            sqls.Add("FC_GFQLRXX_Table", string.Format("select * from FC_GFQLRXX_TMP where pch='{0}'", PCH));
            DataSet ds = DBHelper.GetDataSet(sqls);
            return JsonConvert.SerializeObject(ds);
        }

        public EasyUIGridModel GetWriteBackResult(WriteBackResultQueryForm form, EasyUIGridSetting gridSetting)
        {
            
            string sql = string.Format(@"SELECT SLBH,DJLX,SFTS,PUSHDATA,PUSHDATE,PUSHRESULT,WBERRINFO,CGERRINFO,PID,ADDRESSTYPE 
 FROM (Select ROWNUM AS ROWNO, T.*
      from (select * from FC_SPFHX_TAG where {0}  order by pushdate desc) T 
      WHERE ROWNUM <= {1}) TABLE_ALIAS
WHERE TABLE_ALIAS.ROWNO >  {2} ", form.GetWhere(), gridSetting.PageIndex * gridSetting.PageSize, (gridSetting.PageIndex - 1) * gridSetting.PageSize, gridSetting.SortBy.PropertyName,gridSetting.SortBy.SortType);
            if(gridSetting.SortBy.PropertyName!="")
            {
                sql += " order by " + gridSetting.SortBy.PropertyName + " " + gridSetting.SortBy.SortType;
            }
            DataSet ds = DBHelper.GetDataSet(sql);
            TBToList<SPFHX_TAG> list = new TBToList<SPFHX_TAG>();
            IList<SPFHX_TAG> tags = list.ConvertToModel(ds.Tables[0]);
            string countSql = string.Format("Select count(1) from FC_SPFHX_TAG where {0}", form.GetWhere());
            int count = DBHelper.GetScalar(countSql);
            EasyUIGridModel ret = new EasyUIGridModel(gridSetting.PageIndex, count, tags);
            return ret;
        }

        public EasyUIGridModel GetWBInfoResult(WBQueryForm form)
        {
            //string cfwbService = ConfigurationManager.AppSettings["FWCFAddress"].ToString();
            BDCSrvSoap soap = new BDCSrvSoapClient();
            DataSet ds = soap.SPF_WFZMCX(form.QLRXM, form.ZJHM, form.Address);
            IList<WBInfo> wbinfos = new List<WBInfo>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WBInfo w = new WBInfo();
                    w.HouseID = ds.Tables[0].Rows[i]["HouseID"].ToString();
                    w.BuildingID = ds.Tables[0].Rows[i]["BuildingID"].ToString();
                    w.ZL = ds.Tables[0].Rows[i]["ZL"].ToString();
                    w.QLRMC = ds.Tables[0].Rows[i]["QLRMC"].ToString();
                    w.ZJHM = ds.Tables[0].Rows[i]["ZJHM"].ToString();
                    w.JZMJ = Convert.ToDouble(ds.Tables[0].Rows[i]["JZMJ"]);
                    w.ContractNO = ds.Tables[0].Rows[i]["ContractNO"].ToString();
                    w.SignDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["SignDate"]);
                    wbinfos.Add(w);
                }
            }
            EasyUIGridModel ret = new EasyUIGridModel(0, wbinfos.Count, wbinfos);
            return ret;
        }

        internal WBQueryForm GetWBQueryFormBySlbh(string sLBH)
        {
            string sql = "select q";
            return new WBQueryForm();
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="HouseIds"></param>
        /// <returns></returns>
        public EasyUIGridModel GetWBInfoByHouseIds(IList<string> HouseIds) {
            if (HouseIds.Count > 0)
            {
                BDCSrvSoap soap = new BDCSrvSoapClient();
                IList<WBInfoForAHouse> wbinfos = new List<WBInfoForAHouse>();
                foreach (string hid in HouseIds) {
                    string paramstring = "HouseID=" + hid;
                    DataSet ds = soap.SPF_FC_FWQK(paramstring);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        WBInfoForAHouse w = new WBInfoForAHouse(); 
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
                        wbinfos.Add(w);
                    }                                                                                                                                        
                    else {
                        wbinfos.Add(new WBInfoForAHouse {
                            XMMC = "网备中无此房屋"
                        });
                    }
                }
                EasyUIGridModel ret = new EasyUIGridModel(0, wbinfos.Count, wbinfos);
                return ret;
            }
            else {
                return null;
            }
        }
        public EasyUIGridModel GetWBCLFInfoByHouseIds(string slbh)
        {
            IList<WBCLFInfo> wbinfos = new List<WBCLFInfo>();

            FCWebServiceDal dal = new FCWebServiceDal();
            IList<XGZHAndQLR> xgzhAndQlrs = dal.GetFDJBInfo(slbh);
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
                        FC_CLMMHT_WSData ht_Data = new FC_CLMMHT_WSData(CLMMHT_Webservice_Address, "2000");
                       
                        string backDatas = ht_Data.GetWebServiceData(paramss, null);
                       
                        var bdc = ht_Data.XMLParsing(backDatas);
                        
                        if (bdc.data != null && bdc.data.dt != null && bdc.data.dt.Rows.Count > 0)
                        {
                            string htid = bdc.data.dt.Rows[0]["HTID"].ToString().Trim();
                            string htbah = bdc.data.dt.Rows[0]["CLHTBAH"].ToString();

                            var qlrBdc = GetQLR(htbah);
                            string yrwmc = "";
                            string ywrno = "";
                            if (qlrBdc.data != null && qlrBdc.data.dt != null && qlrBdc.data.dt.Rows.Count > 0)
                            {
                                
                               
                                    foreach (DataRow row in qlrBdc.data.dt.Rows)
                                    {
                                        if (row["XGRLX"].ToString() == "1")
                                        {
                                            if (yrwmc == "")
                                            {
                                                yrwmc = row["XGRMC"].ToString();
                                                ywrno = row["ZJHM"].ToString();
                                            }
                                            else
                                            {
                                                yrwmc += "," + row["XGRMC"].ToString();
                                                ywrno += "," + row["ZJHM"].ToString();
                                            }
                                        }
                                    }
                            }
                            WBCLFInfo wci = (from wi in wbinfos where wi.HTID == htid select wi).FirstOrDefault();
                            if (null != wci)
                            {
                                wci.SYQR += "," + bdc.data.dt.Rows[0]["SYQR"].ToString();
                                wci.BARPASSNO+=","+ bdc.data.dt.Rows[0]["BARPASSNO"].ToString();
                            }
                            else
                            {
                                wci = new WBCLFInfo();
                                wci.BARPASSNO = bdc.data.dt.Rows[0]["BARPASSNO"].ToString();
                                wci.SYQR = bdc.data.dt.Rows[0]["SYQR"].ToString();
                                wci.HTID = htid.Trim();
                                wci.CQZH = bdc.data.dt.Rows[0]["CQZH"].ToString();


                                wci.HID = bdc.data.dt.Rows[0]["HID"].ToString();
                                wci.ZID = bdc.data.dt.Rows[0]["ZID"].ToString();
                                wci.CLHTBAH = bdc.data.dt.Rows[0]["CLHTBAH"].ToString();
                                wci.HTBASJ = bdc.data.dt.Rows[0]["HTBASJ"] == null ? null : Convert.ToDateTime(bdc.data.dt.Rows[0]["HTBASJ"]).ToString();
                                wci.FWZL = bdc.data.dt.Rows[0]["FWZL"].ToString();
                                wci.FWLX = bdc.data.dt.Rows[0]["FWLX"].ToString();
                                wci.FWJG = bdc.data.dt.Rows[0]["FWJG"].ToString();
                                wci.HX = bdc.data.dt.Rows[0]["HX"].ToString();
                                wci.JZMJ = bdc.data.dt.Rows[0]["JZMJ"].ToString();
                                wci.TNJZMJ = bdc.data.dt.Rows[0]["TNJZMJ"].ToString();

                                wci.FTJZMJ = bdc.data.dt.Rows[0]["FTJZMJ"].ToString();
                                wci.JYJG = bdc.data.dt.Rows[0]["JYJG"].ToString();
                                wci.FKLX = bdc.data.dt.Rows[0]["FKLX"].ToString();
                                wci.DKFS = bdc.data.dt.Rows[0]["DKFS"].ToString();
                                wci.FKSJ = bdc.data.dt.Rows[0]["FKSJ"].ToString();

                                wci.QDSJ = bdc.data.dt.Rows[0]["QDSJ"].ToString();
                                wci.QRSJ = bdc.data.dt.Rows[0]["QRSJ"].ToString();
                                wci.QRQZSJ = bdc.data.dt.Rows[0]["QRQZSJ"].ToString();
                                wci.ZHBGTGSJ = bdc.data.dt.Rows[0]["ZHBGTGSJ"].ToString();
                                wci.JSSJ = bdc.data.dt.Rows[0]["JSSJ"].ToString();
                                wci.SFYX = bdc.data.dt.Rows[0]["SFYX"].ToString();

                                wci.YWR = yrwmc;
                                wci.YWRNO = ywrno;

                                wbinfos.Add(wci);
                            }
                        }
                        
                    }
                }
            }
            if(wbinfos.Count==0)
            {
                wbinfos.Add(new WBCLFInfo { HTID = "网备中无此房屋合同信息" });
            }
                EasyUIGridModel ret = new EasyUIGridModel(0, wbinfos.Count, wbinfos);
                return ret;
         
        }

        private Geo.Plug.DataExchange.XZFCPlug.BDC GetQLR(string htbah)
        {
            IDictionary<string, string> paramss = new Dictionary<string, string>();
            paramss["HTBAH"] = htbah;
            FC_GFQLRXX_WSData ht_Data = new FC_GFQLRXX_WSData("FC_GFQLRXX", "2000");
            string backDatas = ht_Data.GetWebServiceData(paramss, null);
            var qlrBdc = ht_Data.XMLParsing(backDatas);
            return qlrBdc;
        }

        //private string GetQlr(string hid)
        //{
        //    FCWebServiceDal dal = new FCWebServiceDal();
        //    DataTable dt = dal.GetQLRMCByHouseId(hid);
        //    if(null!=dt && dt.Rows.Count>0)
        //    {
        //        return 
        //    }
        //}

        private string GetCQZH(string hid)
        {
            FCWebServiceDal dal = new FCWebServiceDal();
            return dal.GetCQZHByHouseId(hid);
        }

        public IList<string> GetHouseIdsInCGBySLBH(string slbh) {
            FCWebServiceDal dal = new FCWebServiceDal();
            DataTable dt = dal.GetHouseIdsBySLBH(slbh);
                IList<string> ids = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids.Add(dt.Rows[i]["TSTYBM"].ToString());
                }
            }
            return ids; 
        }

        public IList<HouseInfoBase> GetHouseInfoByIds(string ids) {
            string[] idArray = ids.Split(',');
            string where = string.Join(",", idArray.Select(t => "'" + t + "'"));
            FCWebServiceDal dal = new FCWebServiceDal();
            IList<HouseInfoBase> houseinfos= dal.GetHouseInfoByIds(where);
            return houseinfos;
        }

        public IList<HouseInfoBase> GetWBHouseInfoByZL(string zl) {
            FCWebServiceDal dal = new FCWebServiceDal();
            IList<HouseInfoBase> houseinfos = dal.GetWBHouseInfoByZL(zl);
            return houseinfos;
        }

        public int SetOracle_WB(string tstybm, string houseid) {
            FCWebServiceDal dal = new FCWebServiceDal();
            return dal.SetOracle_WB(tstybm, houseid);
        }
    }

}