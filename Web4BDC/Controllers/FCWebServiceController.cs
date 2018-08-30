using FCWebServices.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Bll;
using Web4BDC.Dal;
using Web4BDC.Easyui;
using Web4BDC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.TAXInterface;
using Web4BDC.Models.ZZJFModel;
using Web4BDC.Tools;
using WorkflowMonitorXZFCPlug;
using Zen.Barcode;

namespace Web4BDC.Controllers
{
    /// <summary>
    /// 读取webservice 
    /// </summary>
    public class FCWebServiceController : Controller
    {
        //
        // GET: /FCWebService/

        public ActionResult Index()
        {
            return View();
        }
        #region
        /// <summary>
        /// 获取查封的权利人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCFMan()
        {
            return View();
        }

        public ActionResult GetHouseInfoByAddress(string Address)
        {
            if (string.IsNullOrEmpty(Address))
            {
                return Content("[]", "application/json");
            }
            else
            {
                return null;

            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetXZFCPlugWebService()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult GetXZFCPlugWebService(FCWebServiceQueryForm query)
        {
            FCWebserviceBll bll = new FCWebserviceBll();
            IDictionary<string, string> dicParam = Class2Map.ToMap2(FCWebServiceQuery.Map2This(query));
            try
            {
                string pch = bll.GetDataFromFCWebservice(dicParam);
                string ret = bll.GetDataFromBDCByPCH(pch);
                return Content(ret, "application/json");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult GetXZFCPlugWebServiceGetH(string ZID)
        {
            FCWebserviceBll bll = new FCWebserviceBll();
            IDictionary<string, string> dicParam = new Dictionary<string, string>();
            dicParam.Add("ExecuteCode", "0000");
            dicParam.Add("ZID", ZID);
            try
            {
                string pch = bll.GetDataFromFCWebservice(dicParam);
                string ret = bll.GetDataFromBDCByPCH(pch);
                return Content(ret, "application/json");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult WriteBackResult()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WriteBackResult(WriteBackResultQueryForm form, EasyUIGridSetting gridSetting)
        {
            FCWebserviceBll bll = new FCWebserviceBll();
            EasyUIGridModel result = bll.GetWriteBackResult(form, gridSetting);
            return Content(result.ToJson(), "application/json");
        }
        [HttpPost]
        public ActionResult RePush(string slbh)
        {
            try
            { 
                WriteBackWfm wfm= WriteBackWfm.GetInstance(); 
                Polling p = new Polling();
                string area = ConfigurationManager.AppSettings["Area"].ToString();　
                var rt = p.PushASLBH(slbh, wfm, area);
                object ro = new { PushRet = rt.IsSuccess, SLBH = slbh,Message=rt.Message };
                return Json(ro);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                 
                throw new Exception(ex.Message);
#endif

            }
        }


        public ActionResult RePushFail_WB()
        {
            object ro = null;
            //DataTable dt = GetAllFailWB();
            DataTable dt = GetAllSLBHByProName("抵押注销");
            foreach (DataRow row in dt.Rows)
            {
                string slbh = row["SLBH"].ToString();
                try
                {
                    WriteBackWfm wfm = WriteBackWfm.GetInstance();
                    Polling p = new Polling();
                    string area = ConfigurationManager.AppSettings["Area"].ToString();
                    var rt = p.PushASLBH(slbh, wfm, area);
                    ro = new { PushRet = rt.IsSuccess, SLBH = slbh, Message = rt.Message };
                }
                catch(Exception ex)
                {
                    string str = ex.Message;
                    continue;
                }
            }
            return Json(ro);
        }

        private DataTable GetAllFailWB()
        {
            return FCDA_DAL.GetAllFailWB();
        }

        private DataTable GetAllSLBHByProName(string ProName)
        {
            return BDCDA_DAL.GetSLBHByProName(ProName);
        }

        public ActionResult WBInfoShow(string SLBH)
        {
            FCWebserviceBll bll = new FCWebserviceBll();
            WBQueryForm wf = bll.GetWBQueryFormBySlbh(SLBH);
            return View(wf);
        }

       

        public ActionResult GetWBInfo(WBQueryForm form)
        {
            FCWebserviceBll bll = new FCWebserviceBll();
            return Json(bll.GetWBInfoResult(form));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HouseIds"></param>
        /// <returns></returns>
        public ActionResult GetWBInfoByIds(string HouseIds) {
            if (string.IsNullOrEmpty(HouseIds)) {
                return new EmptyResult();
            }
            else
            {
                IList<string> HouseIdList = HouseIds.Split(',').ToList();
                FCWebserviceBll bll = new FCWebserviceBll();
                return Json(bll.GetWBInfoByHouseIds(HouseIdList));
            }
        }

        public ActionResult GetWBCLFInfoByIds(string slbh)
        {
            if (string.IsNullOrEmpty(slbh))
            {
                return new EmptyResult();
            }
            else
            {
                //IList<string> HouseIdList = HouseIds.Split(',').ToList();
                FCWebserviceBll bll = new FCWebserviceBll();
                return Json(bll.GetWBCLFInfoByHouseIds(slbh));
            }
        }


        public ActionResult WBInfoShowByHouseIDs(string SLBH)
        {
            if (!string.IsNullOrEmpty(SLBH))
            {
                FCWebserviceBll bll = new FCWebserviceBll();
                string ids = string.Join(",", bll.GetHouseIdsInCGBySLBH(SLBH));
                ViewData["Hids"] = ids;
                ViewData["SLBH"] = SLBH;
                return View( );
            }
            return View();
        }

        public ActionResult GetWBHouseInfo()
        {
            return View();
        }
        /// <summary>
        /// 从房产档案抽取附件
        /// </summary>
        /// <param name="SLBH"></param>
        /// <returns></returns>
        public ActionResult ImportFromFC(string SLBH) {

            if (!string.IsNullOrEmpty(SLBH))
            {
                BDCFilterResult res = Web4BDC.Bll.ImportBLL.ImportFromFC(SLBH, "Admin");
                if (res.IsSuccess)
                {
                    return Content("成功", "text/plain");
                }
                else
                {
                    return Content("失败!" + res.Message, "text/plain");
                }
            }
            return Content("失败!" + "受理编号为空！", "text/plain");



        }

        public ActionResult GetZLFromWB(string qlrmc,string zjhm)
        {
            string zl = string.Empty;
            BDCSrvSoap soap = new BDCSrvSoapClient();
            DataSet ds = soap.SPF_WFZMCX(qlrmc, zjhm, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        string lif = ds.Tables[0].Rows[i]["YGDJ"].ToString();
                        string zf = ds.Tables[0].Rows[i]["SaleHouseUse"].ToString();
                        if (lif.ToLower().Trim().Equals("0") && zf.ToLower().Trim().Equals("住宅"))
                        {
                            if (string.IsNullOrEmpty(zl))
                            {
                                zl = "\r\n"+ds.Tables[0].Rows[i]["ZL"].ToString();
                            }
                            else
                            {
                                zl+= "\r\n" + ds.Tables[0].Rows[i]["ZL"].ToString();
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return Content(zl, "text/plain");
        }


        public ActionResult ImportToTAX(string zmInfo)
        {

            ZMInfo resMode = null;
            try
            {
                byte[] data_byte = Encoding.UTF8.GetBytes(zmInfo);
                string date=Encoding.UTF8.GetString(data_byte, 0, data_byte.Length);
                if (!string.IsNullOrEmpty(date))
                {

                    resMode = Newtonsoft.Json.JsonConvert.DeserializeObject<ZMInfo>(date);
                    TAXModels model = CreateModel(resMode);
                    TAXDAL dal = new TAXDAL();
                    dal.Insert(model);
                    InsertLog(resMode, date, "成功", true);
                }
            }
            catch(Exception ex)
            {
                InsertLog(resMode, zmInfo, ex.Message, false);
                return Json(ex.Message);
            }
            
            return null;
        }

        private void InsertLog(ZMInfo resMode,string json,string msg,bool flag)
        {
            TaxLogModel tm = new TaxLogModel();
            tm.HTBAH = resMode.HTBAH;
            tm.SQBH = resMode.SQBH;
            tm.Json = json;
            tm.IsSuccess = flag.ToString();
            tm.Message = msg;
            tm.CreateTime = DateTime.Now;
            TAXDAL dal = new TAXDAL();
            dal.InserTaxLog(tm);
        }

        private TAXModels CreateModel(ZMInfo zmModel)
        {
            TAXModels model = new TAXModels();
            if(null== model.proofInfo)
            {
                model.proofInfo = new proofinfo();
            }
            model.proofInfo.经办日期 = DateTime.Now;
            model.proofInfo.利用单位 = "税务";
            model.proofInfo.申请编号 = zmModel.SQBH;
            model.proofInfo.申请人 = zmModel.SQRInfo.XM;
            model.proofInfo.申请人证件号码 = zmModel.SQRInfo.ZJH;
            model.proofInfo.申请人证件类型 = "身份证";
            model.proofInfo.申请时间 = DateTime.Now;
            model.proofInfo.备注 = zmModel.BZ;
            
            model.personList = CreatePerson(zmModel);
            model.zfxxList = CreateZFXX(zmModel,model.personList);
            return model;
        }

        private List<proofperson> CreatePerson(ZMInfo zmModel)
        {
            List<proofperson> list = new List<proofperson>();
            TAX_FX_DAL dal = new TAX_FX_DAL();
            //申请人
            CreateProofperson(zmModel.SQRInfo.XM,zmModel.SQRInfo.ZJH,zmModel.SQBH,zmModel.BZ, list, dal);

            CreateProofperson(zmModel.SQRPoInfo.XM, zmModel.SQRPoInfo.ZJH, zmModel.SQBH, zmModel.BZ,list, dal);

            CreateProofperson(zmModel.SQRZnInfo.XM, zmModel.SQRZnInfo.ZJH, zmModel.SQBH, zmModel.BZ, list, dal);

            return list;
        }

        private static void CreateProofperson(string qlrmc,string zjhm,string sqbh,string bz, List<proofperson> list, TAX_FX_DAL dal)
        {
            if (string.IsNullOrEmpty(qlrmc) || string.IsNullOrEmpty(zjhm))
            {
                return;
            }
            string[] qlrs = null;
            string[] zjhms = null;
            if (qlrmc.Contains(",") && zjhm.Contains(",")) 
            {
                qlrs = qlrmc.Split(',');
                zjhms = zjhm.Split(',');
            }
            if (qlrmc.Contains("，") && zjhm.Contains("，"))
            {
                qlrs = qlrmc.Split('，');
                zjhms = zjhm.Split('，');
            }

            if (null == qlrs || qlrs.Length < 1)
            {
                AddPerson(qlrmc, zjhm, sqbh, bz,list, dal);
            }
            else
            {
                for (int i = 0; i < qlrs.Length; i++)
                {
                    AddPerson(qlrs[i], zjhms[i], sqbh, bz,list, dal);
                }
            }
        }



        private static DataTable GetHouseInfoFrmWB(string qlrmc, string zjhm, DataTable dt)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            DataSet ds = soap.SPF_WFZMCX(qlrmc, zjhm, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        string lif = ds.Tables[0].Rows[i]["QZSTATE"].ToString();
                        string zf = ds.Tables[0].Rows[i]["USETYPE"].ToString();
                        if (lif.ToLower().Trim().Equals("0") && zf.ToLower().Trim().Equals("住宅"))
                        {
                            DataRow row = dt.NewRow();
                            row["房屋坐落"] = ds.Tables[0].Rows[i]["ZL"].ToString() + "  注：此信息来自网备";
                            dt.Rows.Add(row);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return dt;
        }


        private static void AddPerson(string qlrmc, string zjhm, string sqbh,string BZ, List<proofperson> list, TAX_FX_DAL dal)
        {
            DataTable dt = dal.GetHList(qlrmc, zjhm);
            string area = ConfigurationManager.AppSettings["Area"];
            if(area.Equals("睢宁"))
                dt = GetHouseInfoFrmWB(qlrmc, zjhm, dt);

            bool flag = false;

            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    proofperson p = new proofperson();
                    p.房屋座落 = row["房屋坐落"].ToString();
                    p.数据来源 = "不动产登记中心";
                    p.申请编号 = sqbh;
                    p.姓名 = qlrmc;
                    p.证件号码 = zjhm;
                    p.证件类型 = "身份证";
                    list.Add(p);
                    flag = true;
                }
            }

            if (!string.IsNullOrEmpty(BZ))
            {
                DataTable oldDT = dal.GetHListFrmOld(qlrmc, zjhm);
                if (null != oldDT && oldDT.Rows.Count > 0)
                {
                    foreach (DataRow row in oldDT.Rows)
                    {
                        proofperson p = new proofperson();
                        p.房屋座落 = row["房屋坐落"].ToString();
                        p.数据来源 = "不动产登记中心";
                        p.申请编号 = sqbh;
                        p.姓名 = qlrmc;
                        p.证件号码 = zjhm;
                        p.证件类型 = "身份证";
                        list.Add(p);
                        flag = true;
                    }
                }
                else
                {
                    string[] zls = BZ.Split('|');
                    foreach (string zl in zls)
                    {
                        proofperson p = new proofperson();
                        p.数据来源 = "不动产登记中心";
                        p.申请编号 = sqbh;
                        p.姓名 = qlrmc;
                        p.证件号码 = zjhm;
                        p.证件类型 = "身份证";
                        p.房屋座落 = zl.Trim();
                        list.Add(p);
                        flag = true;
                    }
                }

            }

            if (!flag)
            {
                proofperson p = new proofperson();
                p.数据来源 = "不动产登记中心";
                p.申请编号 = sqbh;
                p.姓名 = qlrmc;
                p.证件号码 = zjhm;
                p.证件类型 = "身份证";
                p.房屋座落 = "无";
                list.Add(p);
            }
            
        }

        private zfxxindex CreateZFXX(ZMInfo zmModel, List<proofperson> person)
        {
            
            zfxxindex h = new zfxxindex();
            h.包含的区域 = "01";
            h.已处理的区域 = "01";
            h.房屋套数 = null==person?"0":person.Count.ToString();
            h.合同备案号 = zmModel.HTBAH;
            h.申请编号 = zmModel.SQBH;
            h.UID = Guid.NewGuid().ToString();
            return h;
        }

        [HttpPost]
        public ActionResult ImportQSPic(string baseb4Str,string slbh)
        {
            BDCFilterResult res = null;

            if (!string.IsNullOrEmpty(baseb4Str))
            {
                res = Web4BDC.Bll.ImportBLL.ImportQSPic(baseb4Str, slbh, "网页导入");
            }
            else
            {
                res = new BDCFilterResult();
                res.IsSuccess = false;
                res.Message = "失败!" ;
            }
            return Json(res);
        }
        /// <summary>
        /// 寻找网备的房屋id
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FindHouseIdInWB(string slbh) {
            if (!string.IsNullOrEmpty(slbh))
            {
                FCWebserviceBll bll = new FCWebserviceBll();
                string ids = string.Join(",", bll.GetHouseIdsInCGBySLBH(slbh));
                ViewData["Hids"] = ids;
                ViewData["SLBH"] = slbh;
                return View();
            }
            return View();
        }

        public ActionResult GetHouseInfoByIds(string tstybms) {
            if (!string.IsNullOrEmpty(tstybms)) {
                FCWebserviceBll bll = new FCWebserviceBll(); 
                return Json( bll.GetHouseInfoByIds(tstybms));
            }
            return null;
        }
        [HttpGet]
        public ActionResult SearchWB()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchWB(string zl) {  
            if (!string.IsNullOrEmpty(zl))
            {
                FCWebserviceBll bll = new FCWebserviceBll();
                return Json(bll.GetWBHouseInfoByZL(zl));
            }
            else {
                return null;
            }
        }

        public ActionResult SetOracle_WB(string tstybm, string houseid) {
            FCWebserviceBll bll = new FCWebserviceBll();
            try
            {
                bll.SetOracle_WB(tstybm, houseid);
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex) {
                return Json(new { IsSuccess = false, Message = ex.Message });
            }
        }

        public ActionResult DeedTaxInfo(string SLBH) {
            ViewData["SLBH"] = SLBH;
            return View();
        }
        [HttpPost]
        public ActionResult GetDeedTaskInfo(string txm,string ry_id,string slbh) {
            TaxInterfaceBll bll = new TaxInterfaceBll();
            return Json(bll.GetTaxInterfaceModel(txm, ry_id,slbh));
        }

        public ActionResult GetDeedTaskBySLBH(string SLBH) {
            TaxInterfaceBll bll = new TaxInterfaceBll();
            string txm = string.Empty;
            TAXXML taxXml = bll.GetTaxInterfaceModelBySLBH(SLBH,out txm);
            return   Json(new { TaxXML=taxXml,TXM= txm });

        }

        /// <summary>
        /// 获取自助缴费信息
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        public ActionResult GetSFInfo(string slbh)
        {
            JFStateModel mod = new JFStateModel(slbh);
            return Json(mod);
        }

        public ActionResult GetBarCodeImg(string txm)
        {
            Code128BarcodeDraw qrcode = BarcodeDrawFactory.Code128WithChecksum;
            //Code128BarcodeDraw qrcode = BarcodeDrawFactory.Code128WithChecksum;
            Image barcode = qrcode.Draw(txm, qrcode.GetDefaultMetrics(40));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            barcode.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            Response.ContentType = "image/jpeg";
            Response.Clear();
            Response.BinaryWrite(ms.ToArray());

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult PushDA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PushDA(PageParams p)
        {

            try
            {
                if(p.PrjId.Length<12 || p.PrjId.Contains("-"))
                {
                    HONEYPOT hp = new HONEYPOT();
                    hp.ID = Guid.NewGuid().ToString();
                    hp.IP = IPNet.GetIP4Address();
                    hp.PUSHDATE = DateTime.Now;
                    hp.SLBH = p.PrjId;
                    hp.STEPNAME = "重推房产";

                    BDCDA_DAL.Insert_HONEYPOT(hp);
                    return Json(new BDCFilterResult
                    {
                        IsSuccess = false,
                        Message = "受理编号错误，请重新填写"
                    });
                }
                else
                {
                    p.UserId = FCDA_BLL.GetUserID(p.PrjId.Trim());//"guidangren";
                    BDCFilterResult res = FCDA_BLL.Insert_FCDA(p);
                    //return this.Content(message);
                    return Json(res);
                }
            }
            catch (Exception ex)
            {
                //return this.Content(ex.Message);
                return Json(new BDCFilterResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        private void MyDeleteRecode(string slbh)
        {
            DataTable dt = FCDA_DAL.GetDeleteInfo(slbh);
            if (null != dt && dt.Rows.Count > 0)
            {


                foreach (DataRow row in dt.Rows)
                {
                    string busino = row["busino"].ToString();

                    Guid ArchiveId = new Guid(row["ArchiveId"].ToString());

                    FCDA_DAL.deleteRecode(busino, ArchiveId);

                }


            }
        }




        [HttpPost]
        public ActionResult PushARCH(PageParams p)
        {


            //try
            //{
            //    HONEYPOT hp = new HONEYPOT();
            //    hp.ID = Guid.NewGuid().ToString();
            //    hp.IP = IPNet.GetIP4Address();
            //    hp.PUSHDATE = DateTime.Now;
            //    hp.SLBH = p.PrjId;
            //    hp.STEPNAME = "重推档案";

            //    BDCDA_DAL.Insert_HONEYPOT(hp);

            //    return Json(true);

            //}
            //catch (Exception ex)
            //{
            //    //return this.Content(ex.Message);
            //    return Json(new BDCFilterResult
            //    {
            //        IsSuccess = false,
            //        Message = ex.Message
            //    });
            //}
            try
            {
                if (p.PrjId.Length < 12)
                {
                    HONEYPOT hp = new HONEYPOT();
                    hp.ID = Guid.NewGuid().ToString();
                    hp.IP = IPNet.GetIP4Address();
                    hp.PUSHDATE = DateTime.Now;
                    hp.SLBH = p.PrjId;
                    hp.STEPNAME = "重推房产";

                    BDCDA_DAL.Insert_HONEYPOT(hp);
                    return Json(new BDCFilterResult
                    {
                        IsSuccess = false,
                        Message = "受理编号错误，请重新填写"
                    });
                }
                else
                {
                    PageParams param = new Web4BDC.Models.PageParams();
                    param.PrjId = p.PrjId.Trim();
                    //param.UserId = FCDA_BLL.GetUserID(p.PrjId.Trim());//"guidangren";
                    BDCFilterResult res = FCDA_BLL.Insert_ARCH(param);
                    //return this.Content(message);

                    return Json(res.IsSuccess);
                }

            }
            catch (Exception ex)
            {
                //return this.Content(ex.Message);
                return Json(new BDCFilterResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public ActionResult RePushFail()
        {
            try
            {
                HONEYPOT hp = new HONEYPOT();
                hp.ID = Guid.NewGuid().ToString();
                hp.IP= IPNet.GetIP4Address();
                hp.PUSHDATE = DateTime.Now;
                hp.SLBH = "重推错误";
                hp.STEPNAME = "重推错误";

                BDCDA_DAL.Insert_HONEYPOT(hp);

                return Json(true);

            }
            catch (Exception ex)
            {
                //return this.Content(ex.Message);
                return Json(new BDCFilterResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        private void ReRecode(string slbh)
        {

            PageParams pg = new PageParams();
            pg.PrjId = slbh;
            FCDA_BLL.Insert_FCDA(pg);



        }


    }
    public class IPNet
    {
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
    }
}
