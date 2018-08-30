using Geo.Plug.DataExchange.XZFCPlug;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Web4BDC.Bll;
using Web4BDC.Bll.CheckRegist;
using Web4BDC.Bll.MergeImage;
using Web4BDC.Bll.MKevaluate;
using Web4BDC.Bll.StepPushBLL;
using Web4BDC.Bll.YGFP;
using Web4BDC.Models;
using Web4BDC.Models.FCDAModel;
using Web4BDC.Models.StepPush;

namespace Web4BDC.Controllers
{
    public class FCInterfacesController : Controller
    {
        //
        // GET: /FCInterfaces/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult MergeImage(string slbh)
        {
            MergeImageBLL bll = new MergeImageBLL();
            string path = bll.MergeImage(slbh);
            return Content(path);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetRecSLBH()
        {
            DataTable slbhs = FCDA_BLL.GetRecSLBHs();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(slbhs));
        }

        public ActionResult GetRecSLBHZX()
        {
            DataTable slbhs = FCDA_BLL.GetRecSLBHZX();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(slbhs));
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetBDCDA(string slbh)
        {
            try
            {
                PageParams pg = new PageParams();
                if(string.IsNullOrEmpty(slbh))
                {
                    slbh = Request.Params[0].ToString();
                }
                if(string.IsNullOrEmpty(slbh))
                {
                    throw new Exception("错误！参数为空");
                }
                if(!CheckProcState(slbh))
                {
                    throw new Exception("流程尚未完成");
                }
                pg.PrjId = slbh;
                //pg.UserId = FCDA_BLL.GetUserID(pg.PrjId.Trim());
                List<BDCDAModel> list = FCDA_BLL.GetFCDA(pg);
                string str=Newtonsoft.Json.JsonConvert.SerializeObject(list);
                InsertGetBDCLog(slbh, str, GetClientIPv4Address());
                
                return Content(str);
            }
            catch (Exception ex)
            {
                InsertGetBDCLog(slbh, ex.Message, GetClientIPv4Address());
                return Content(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult UpdateBDCDAState(string slbh, int v)
        {
            try
            {
                FCDA_BLL.UpdateBDCDAState(slbh, v);
                return Content("Success");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private bool CheckProcState(string slbh)
        {
            return (FCDA_BLL.CheckProcState(slbh) || FCDA_BLL.CheckProcInDA(slbh));
        }

        private void InsertGetBDCLog(string slbh, string str,string ip)
        {
            BDCDALog log = new BDCDALog();
            log.ip = ip;
            log.ID = Guid.NewGuid().ToString("N");
            log.RequestContent = slbh;
            log.ResponseContent = str;
            log.RequestTime = DateTime.Now;
            FCDA_BLL.InsertBDCLog(log);
        }


        /// <summary>
        /// 获取访问客户端的IPV4地址
        /// </summary>
        /// <returns></returns>
        public  string GetClientIPv4Address()
        {
            string ipv4 = String.Empty;
            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
            //foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
        public  string GetClientIP()
        {
            if (null == HttpContext.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }



        /// <summary>
        /// 获取接口信息
        /// </summary>
        /// <returns></returns>
        ///ExecuteCode: 0000,ZH:,XMMC: 香榭,JZWMC:,FWZL:,ZID:
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult DataExchange(string Params)
        {
            
            try
            {
                Dictionary<string, string> dicParams = this.GetDataExchangeParams(Params);
                IDataExchange dataExchange = new DataExchange();
                string message = dataExchange.DataExtort(dicParams);
                //return this.Content(message);
                return Json(new BDCFilterResult
                {
                    IsSuccess = true,
                    Message = message
                });

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
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult MKevaluate(string IptUnEncryptStr)
        {
            try
            {
                if(string.IsNullOrEmpty(IptUnEncryptStr))
                {
                    return Content("The param is null");
                }
                
                string str = System.Web.HttpUtility.UrlDecode(IptUnEncryptStr, System.Text.Encoding.UTF8);
                
                Dictionary<string, string> configs = GetConfigDic(str);
                
                //2018.6.20 向摩柯推送流程信息
                //if (CanPush(configs["StepName"]))
                //{
                StepPushJsonModel spjm = new StepPushJsonModel();
                spjm.ID = Guid.NewGuid().ToString();
                spjm.BLRY = configs["UserName"];

                spjm.JDMC = configs["StepName"];  //configs["StepName"];

                spjm.YWBH = configs["PrjId"];

                spjm.WDBS = GetWDBS(configs["UserId"]);

                spjm.XZQDM = GetXZQDM(configs["PrjId"]);

                spjm.SQRXM = GetSQRXM(spjm.YWBH);
                spjm.SQRLXFS = GetSQRLXFS(spjm.YWBH);

                spjm.CreateTime = DateTime.Now;
                spjm.ErrorMsg = "";
                spjm.IsSuccess = 1;
                string spjmJson = Newtonsoft.Json.JsonConvert.SerializeObject(spjm);

                string url = SendPostMessage(spjm);
                
                return Content(url);
            }
            catch(Exception ex)
            {
                return Content("异常:"+ex.Message);
            }
                
            //}
        }

        private string GetSQRLXFS(string yWBH)
        {
            MKevaluateBLL bll = new MKevaluateBLL();
            return bll.GetSQRLXFS(yWBH);
        }

        private string GetSQRXM(string yWBH)
        {
            MKevaluateBLL bll = new MKevaluateBLL();
            return bll.GetSQRXM(yWBH);
        }

        private void WriteLog(string str)
        {
            string path = "d:\\web4bdc.log";
            StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            try
            {
                wr.WriteLine(str);
                wr.WriteLine("--------------------------------------------------------------------");
                wr.Close();
            }
            catch { }
            finally { wr.Close(); }



        }
        /// <summary>
        /// 流程检查功能
        /// 做出一系列检查
        /// </summary>
        /// <returns></returns>
        public ActionResult OutCheck(PageParams param)
        {
try
            {
            //WriteLog("进入程序");
            //param.IptUnEncryptStr
            //WriteLog("SLBH=" + param.PrjId + "\r\n" + "流程名称=" + param.PrjName + "\r\n" + "IptUnEncryptStr=" + param.IptUnEncryptStr);
            Dictionary<string, string> configs = GetConfigDic(param.IptUnEncryptStr);
            param.Configs = configs;
            BDCFilterResult filterResult = new BDCFilterResult { IsSuccess = true };
            IBDCFilter filter;
            


                
                //发证环节推送档案登记信息
               
                if (configs.ContainsKey("PushDAFilter") && configs["PushDAFilter"].Equals("是"))
                {

                    PushDAFilter pf = new PushDAFilter();
                    filterResult = pf.Filter(param);
                    if (configs.ContainsKey("PushDAFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["PushDAFilterConfirmType"], out confirmType))
                        {
                            filterResult.ConfirmType = confirmType;
                        }
                    }
                }



                
                BDCFilterResult filterResult2 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("RegistCheckFilter") && configs["RegistCheckFilter"].Equals("是"))
                {
                    filter = new RegistCheckFilter(configs);
                    filterResult2 = filter.Filter(param);
                    if (configs.ContainsKey("RegistCheckFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["RegistCheckFilterConfirmType"], out confirmType))
                        {
                            filterResult2.ConfirmType = confirmType;
                        }
                    }
                }
                filterResult2 = BDCFilterResult.MergeResult(filterResult2, filterResult);

                BDCFilterResult filterResult3 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("PushDataFilter") && configs["PushDataFilter"].Equals("是"))
                {
                    filter = new PushDataFilter(configs);
                    filterResult = filter.Filter(param);
                    if (configs.ContainsKey("PushDataFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["PushDataFilterConfirmType"], out confirmType))
                        {
                            filterResult.ConfirmType = confirmType;
                        }

                        if (configs.ContainsKey("PushDataFilterErrInfo"))
                        {
                            string errInfo = configs["PushDataFilterErrInfo"].ToString();
                            if (!string.IsNullOrEmpty(errInfo))
                                filterResult.Message = string.Format(errInfo, filterResult.Message);
                        }
                    }


                }
                filterResult3 = BDCFilterResult.MergeResult(filterResult3, filterResult2);
                //收费流程推送登记信息
                BDCFilterResult filterResult4 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("PushSFFilter") && configs["PushSFFilter"].Equals("是"))
                {
                    PushSFFilter sf = new PushSFFilter();
                    filterResult4 = sf.Filter(param);
                    if (configs.ContainsKey("PushSFFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["PushSFFilterConfirmType"], out confirmType))
                        {
                            filterResult4.ConfirmType = confirmType;
                        }
                    }
                }

                filterResult4 = BDCFilterResult.MergeResult(filterResult3, filterResult4);

                ///发证前收费验证
                BDCFilterResult filterResult5 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("CheckSFFilter") && configs["CheckSFFilter"].Equals("是"))
                {
                    CheckStateFilter csf = new CheckStateFilter();
                    filterResult5 = csf.Filter(param);
                    if (configs.ContainsKey("CheckSFFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["CheckSFFilterConfirmType"], out confirmType))
                        {
                            filterResult5.ConfirmType = confirmType;
                        }
                    }
                }
                filterResult5 = BDCFilterResult.MergeResult(filterResult4, filterResult5);


                ///阳光扶贫验证
                BDCFilterResult filterResult6 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("YGFPFilter") && configs["YGFPFilter"].Equals("是"))
                {
                    YGFPFilter csf = new YGFPFilter();
                    filterResult6 = csf.Filter(param);
                    if (configs.ContainsKey("YGFPFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["YGFPFilterConfirmType"], out confirmType))
                        {
                            filterResult6.ConfirmType = confirmType;
                        }
                    }
                }
                filterResult6 = BDCFilterResult.MergeResult(filterResult5, filterResult6);
                

                BDCFilterResult filterResult7 = new BDCFilterResult { IsSuccess = true };
                if (configs.ContainsKey("CheckRegistFilter") && configs["CheckRegistFilter"].Equals("是"))
                {
                    CheckRegistFilter csf = new CheckRegistFilter();
                    filterResult7 = csf.Filter(param);
                    if (configs.ContainsKey("CheckRegistFilterConfirmType"))
                    {
                        int confirmType = 0;
                        if (int.TryParse(configs["CheckRegistFilterConfirmType"], out confirmType))
                        {
                            filterResult7.ConfirmType = confirmType;
                        }
                    }
                }

                filterResult7 = BDCFilterResult.MergeResult(filterResult6, filterResult7);

                return Json(filterResult7);
            }
            catch(Exception ex)
            {
                string log = string.Format("OutCheck异常对象:{0},异常方法：{1},SLBH={2},错误信息：{3}",ex.Source,ex.TargetSite, param.PrjId, ex.Message);
                //WriteLog(log);
                return Json(new BDCFilterResult { IsSuccess = false,Message=ex.Message,ConfirmType=-1 });
            }
            
        }

        private string GetStepName(string iptUnEncryptStr)
        {
            throw new NotImplementedException();
        }

        private string SendPostMessage(StepPushJsonModel pushStr)
        {
            StepPushBLL bll = new StepPushBLL();
            return bll.SendPostMessage(pushStr);
        }

        private string GetXZQDM(string prjID)
        {
            StepPushBLL bll = new StepPushBLL();
            return bll.GetXZQDM(prjID);
        }

        private string GetWDBS(string prjID)
        {
            StepPushBLL bll = new StepPushBLL();
            return bll.GetWDBS(prjID);
        }
        private bool CanPush(string v)
        {
            StepPushBLL bll = new StepPushBLL();
            return bll.CanPush(v);
        }

      

        private Dictionary<string, string> GetConfigDic(string str) {
            Dictionary<string, string> dCos = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(str)) {
                string[] cos= str.Split(',');
                if (cos.Length > 0) {
                    foreach (string co in cos) {
                        string[] sos = co.Split(';');
                        if (sos.Length == 2) {
                            if (dCos.ContainsKey(sos[0]))
                            {
                                dCos[sos[0]] = sos[1];
                            }
                            else
                            {
                                dCos.Add(sos[0], sos[1]);
                            }
                        }
                        
                    }
                }
            }
            return dCos;
        }
        /// <summary>
        ///  ExecuteCode: 0000,ZH:,XMMC: 香榭,JZWMC:,FWZL:,ZID:
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        private Dictionary<string,string> GetDataExchangeParams(string sParams)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(sParams)) {
                string[] ssParams = sParams.Split(',');
                foreach (string sssParams in ssParams) {
                    if (!string.IsNullOrEmpty(sssParams)) {
                        string[] ssssParams = sssParams.Split(':');
                        if (ssssParams.Length > 1) {
                            if (!dic.ContainsKey(ssssParams[0]))
                            {
                                dic.Add(ssssParams[0], ssssParams[1]);
                            }
                            else {
                                dic[ssssParams[0]] = ssssParams[1];
                            }
                        }
                    }
                }
            }
            return dic;
        }
    }
}
