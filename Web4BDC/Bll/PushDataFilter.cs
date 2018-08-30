using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Models; 

namespace Web4BDC.Bll
{
    public class PushDataFilter : IBDCFilter
    {
        private Dictionary<string, string> ConfigsFromBDC;

        public PushDataFilter(Dictionary<string, string> configsFromBDC) {
            this.ConfigsFromBDC = configsFromBDC;
        }

        public BDCFilterResult Filter(PageParams param)
        {
            //throw new NotImplementedException();
            try
            {
                //return Json(new { IsSucess = false, PushRet = "未找到配置文件，或配置不正确", SLBH = slbh }, JsonRequestBehavior.AllowGet);
                //获取xml文件配置
                WorkflowMonitorXZFCPlug.WriteBackWfm wfm = WorkflowMonitorXZFCPlug.WriteBackWfm.GetInstance();
                if (wfm == null)
                {
                    BDCFilterResult o = new BDCFilterResult { IsSuccess = false, Message = "未找到配置文件，或配置不正确", ConfirmType = 0 };
                    return o;
                }
                else {
                    if (ConfigsFromBDC != null)
                        MeageConfig(wfm, ConfigsFromBDC, param.PrjId);
                }
                WorkflowMonitorXZFCPlug.Polling p = new WorkflowMonitorXZFCPlug.Polling();
                string area = ConfigurationManager.AppSettings["Area"].ToString();
               
                var rt = p.PushAStep(param.PrjId, wfm, area, param.WriId);
                if (rt.IsSuccess)
                    return new BDCFilterResult { IsSuccess = true };
                else {
                    return new BDCFilterResult { IsSuccess = false, Message = rt.Message, ConfirmType = 1 };
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //object ro = new { IsSuccess = false, Message = ex.Message, SLBH = slbh };
                //return Content(js.Serialize(ro), "text/html");
               //return new BDCFilterResult { IsSuccess = false, Message = ex.Message, ConfirmType = 1 };
            }
        }
        /// <summary>
        /// 将配置合并
        /// </summary>
        /// <param name="wfm"></param>
        /// <param name="DicConfig"></param>
        private void MeageConfig(WorkflowMonitorXZFCPlug.WriteBackWfm wfm, Dictionary<string, string> DicConfig,string slbh) {
            if (DicConfig != null && DicConfig.Count > 0) {
                if (DicConfig.ContainsKey("PushDataFilter") && DicConfig["PushDataFilter"].Equals("是") ) {
                    BDCInfo4WWWDal dal = new BDCInfo4WWWDal();
                    string pid = dal.GetPIDBySLBH(slbh);
                    if (!string.IsNullOrEmpty(pid) && !wfm.PIDS.Any(p=>p.PId==pid&&p.stepname.Equals(DicConfig["StepName"]))) {
                        string viewId = DicConfig.ContainsKey("PushDataFilterViewId") ? DicConfig["PushDataFilterViewId"] : "";
                        string wsaid = DicConfig.ContainsKey("PushDataFilterWASID") ? DicConfig["PushDataFilterWASID"] : "";
                        string stepName = DicConfig.ContainsKey("StepName") ? DicConfig["StepName"] : "";
                        if (!string.IsNullOrEmpty(stepName) && !string.IsNullOrEmpty(viewId) && !string.IsNullOrEmpty(wsaid))
                        {
                            wfm.PIDS.Add(new WorkflowMonitorXZFCPlug.PID
                            {
                                name = DicConfig.ContainsKey("FlowName") ? DicConfig["FlowName"] : "",
                                viewId = viewId,
                                WSAID = wsaid,
                                PId = pid,
                                stepname = stepName
                            });
                        }
                        else {
                            string viewString= DicConfig.ContainsKey("PushDataFilterViewString") ? DicConfig["PushDataFilterViewString"] : "";
                            string wsaString = DicConfig.ContainsKey("WSAString") ? DicConfig["PushDataFilterWASString"] : "";
                            if (!string.IsNullOrEmpty(viewString) && !string.IsNullOrEmpty(wsaString)) {
                                if (wfm.ViewNames.Any(a => a.sql.Equals(viewString)))
                                {
                                    viewId = wfm.ViewNames.First(a => a.sql.Equals(viewString)).id;
                                }
                                else {
                                    int intViewId = wfm.ViewNames.FirstOrDefault()==null? 0:int.Parse(wfm.ViewNames.FirstOrDefault().id);
                                    bool existsView = true;
                                    while (existsView) {
                                        if (!wfm.ViewNames.Any(a => a.id.Equals(intViewId.ToString())))
                                        {
                                            existsView = false ;
                                        }
                                        else {
                                            intViewId++;
                                            existsView = true;
                                        }
                                    }
                                    viewId = intViewId.ToString();
                                    wfm.ViewNames.Add(new WorkflowMonitorXZFCPlug.ViewName {
                                        id= viewId,
                                        name= viewString,
                                        sql= viewString
                                    });
                                    int intWSAId = wfm.WebserviceAdds.FirstOrDefault() == null ? 0 : int.Parse(wfm.WebserviceAdds.FirstOrDefault().id);
                                    bool existsWsa = true;
                                    while (existsWsa) {
                                        if (!wfm.WebserviceAdds.Any(a => a.id.Equals(intWSAId.ToString())))
                                        {
                                            existsWsa = false;
                                        }
                                        else {
                                            intWSAId++;
                                            existsWsa = true;
                                        }
                                    }
                                    wsaid = intWSAId.ToString();
                                    wfm.WebserviceAdds.Add(new WorkflowMonitorXZFCPlug.WebserviceAdd {
                                        id=wsaid,
                                        name= wsaString
                                    });
                                    wfm.PIDS.Add(new WorkflowMonitorXZFCPlug.PID {
                                        name = DicConfig.ContainsKey("FlowName") ? DicConfig["FlowName"] : "",
                                        viewId = viewId,
                                        WSAID = wsaid,
                                        PId = pid,
                                        stepname = stepName
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}