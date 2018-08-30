//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Script.Serialization;
//using Web4BDC.Bll;
//using Web4BDC.Models;
//using WorkflowMonitorXZFCPlug;

//namespace Web4BDC.Controllers
//{
//    public class FCInterfacesController : Controller
//    {
//        //
//        // GET: /FCInterfaces/

//        public ActionResult Index()
//        {
//            return View();
//        }

//        /// <summary>
//        /// 往网备中推送数据
//        /// </summary>
//        /// <param name="wrkId"></param>
//        /// <returns></returns>
        
//        //public ActionResult PushData2WB(string slbh,string wrkId) {
//        //    JavaScriptSerializer js = new JavaScriptSerializer();
//        //    try
//        //    {
//        //        //return Json(new { IsSucess = false, PushRet = "未找到配置文件，或配置不正确", SLBH = slbh }, JsonRequestBehavior.AllowGet);
//        //        WriteBackWfm wfm = WriteBackWfm.GetInstance(); 
//        //        if (wfm == null) {
//        //            object o=new { IsSucess = false, Message = "未找到配置文件，或配置不正确", SLBH = slbh }; 
//        //            return Content(js.Serialize(o), "text/html");
//        //        }
//        //        Polling p = new Polling();
//        //        string area = ConfigurationManager.AppSettings["Area"].ToString(); 
//        //        var rt = p.PushAStep(slbh, wfm, area,wrkId);
//        //        object ro = new { IsSuccess = rt.IsSuccess, Message = rt.Message, SLBH = slbh };
//        //        //return Json(ro, JsonRequestBehavior.AllowGet);
//        //        return Content(js.Serialize(ro), "text/html");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        object ro = new { IsSuccess = false, Message = ex.Message, SLBH = slbh } ;
//        //        return Content(js.Serialize(ro), "text/html");
//        //    }
//        //}

//        /// <summary>
//        /// 流程检查功能
//        /// 做出一系列检查
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult OutCheck(PageParams param) {
//            //param.IptUnEncryptStr
//            Dictionary<string, string> configs = GetConfigDic(param.IptUnEncryptStr);
//            BDCFilterResult filterResult = new BDCFilterResult { IsSuccess=true};
//            IBDCFilter filter;
//            if (configs.ContainsKey("PushDataFilter") && configs["PushDataFilter"].Equals("是")) {
//                filter = new PushDataFilter(configs);
//                filterResult = filter.Filter(param);
//            } 
//            if(filterResult.IsSuccess&& configs.ContainsKey("RegistCheckFilter") && configs["RegistCheckFilter"].Equals("是"))
//            {
//                filter = new RegistCheckFilter(configs);
//                filterResult = filter.Filter(param);
                
//            }
//            return Json(filterResult);
//        }
/**********************************************************************************************************/
/***********************推送档案***************************************************************************/
//        public ActionResult PushDA(PageParams param)
//        {
//            Dictionary<string, string> configs = GetConfigDic(param.IptUnEncryptStr);
//            BDCFilterResult filterResult = new BDCFilterResult { IsSuccess = true };
//            IBDCFilter filter;
//            if (configs.ContainsKey("PushDAFilter") && configs["PushDAFilter"].Equals("是"))
//            {
//                filter = new PushDAFilter();
//                filterResult = filter.Filter(param);
//            }
//            return Json(filterResult);
//        }
/***********************************************************************************************************/
//        private Dictionary<string, string> GetConfigDic(string str) {
//            Dictionary<string, string> dCos = new Dictionary<string, string>();
//            if (!string.IsNullOrEmpty(str)) {
//                string[] cos= str.Split(',');
//                if (cos.Length > 0) {
//                    foreach (string co in cos) {
//                        string[] sos = co.Split(';');
//                        if (sos.Length == 2) {
//                            if (dCos.ContainsKey(sos[0]))
//                            {
//                                dCos[sos[0]] = sos[1];
//                            }
//                            else
//                            {
//                                dCos.Add(sos[0], sos[1]);
//                            }
//                        }
                        
//                    }
//                }
//            }
//            return dCos;
//        }
//    }
//}
