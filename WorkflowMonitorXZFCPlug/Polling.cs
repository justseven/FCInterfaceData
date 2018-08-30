
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowMonitorXZFCPlug.Dal;

namespace WorkflowMonitorXZFCPlug
{

    public class Polling
    {
        /// <summary>
        /// 获取需要推送的数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetPushDataInfos(string wrkId, string pids)
        {
            string sql = string.Format(@"select wfm_actinst.prjid as slbh,wfm_process.pid as pid,wfm_actinst.stepname as stepName from wfm_actinst
left join wfm_activity on wfm_activity.aid=wfm_actinst.mdlid
left join wfm_process on wfm_process.pid=wfm_activity.pid
left join wfm_model on wfm_model.mid=wfm_process.mid
where 1=1 and wfm_process.pid in({1}) and WRKID='{0}'", wrkId, pids);

            return DBHelper.GetDataTable(sql, ConnectType.GGK);
        }
        private DataTable GetPushDataInfos4SLBH(string slbh, string pids)
        {
            string sql = string.Format(@"select wfm_actinst.prjid as slbh,wfm_process.pid as pid,wfm_actinst.stepname as stepName from wfm_actinst
left join wfm_activity on wfm_activity.aid=wfm_actinst.mdlid
left join wfm_process on wfm_process.pid=wfm_activity.pid
left join wfm_model on wfm_model.mid=wfm_process.mid
where 1=1 and wfm_process.pid in({1}) and wfm_actinst.prjid = '{0}'", slbh, pids);

            return DBHelper.GetDataTable(sql, ConnectType.GGK);
        }
        private SlbhAndPid DispatchData(DataTable dt, WriteBackWfm Wfm)
        {
            if (dt.Rows.Count > 0)
            {
                PID PID = Wfm.PIDS.FirstOrDefault(p => p.PId == dt.Rows[0]["pid"].ToString() && p.stepname == dt.Rows[0]["stepName"].ToString());
                if (PID != null)
                {
                    string slbh = dt.Rows[0]["slbh"].ToString();//受理号
                    string pid = dt.Rows[0]["pid"].ToString();
                    string stepName = dt.Rows[0]["stepName"].ToString();
                    return new SlbhAndPid
                    {
                        Slbh = slbh,
                        Pid = pid,
                        StepName = stepName
                    };
                }
            }
            else
                return null;
            return null;
        }

        private SlbhAndPid DispatchData2(DataTable dt, WriteBackWfm Wfm)
        {
            //string[] bzs = null;
            //if (!string.IsNullOrEmpty(LXBZ)) {
            //    bzs = LXBZ.Split('|');
            //}
            SlbhAndPid slbhandpid = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PID PID = Wfm.PIDS.FirstOrDefault(p => p.PId == dt.Rows[i]["pid"].ToString() && (p.stepname == dt.Rows[i]["stepName"].ToString()));//|| (bzs!=null&&bzs.Contains(p.stepname))));
                    //ILog log = new ErrorLog(GetType());
                    // log.WriteLog(new Exception("DT.PzzID：" + dt.Rows[i]["pid"].ToString() + "DT.PzzID：" + dt.Rows[i]["stepName"].ToString()), LogType.Information);
                    if (PID != null)
                    {
                        string slbh = dt.Rows[i]["slbh"].ToString();//受理号
                        string pid = dt.Rows[i]["pid"].ToString();
                        string stepName = dt.Rows[i]["stepName"].ToString();
                        slbhandpid = new SlbhAndPid
                        {
                            Slbh = slbh,
                            Pid = pid,
                            StepName = stepName
                        };
                        break;
                    }
                }
            }
            else
                return null;
            return slbhandpid;
        }

        public List<dynamic> GetPushList(SlbhAndPid patchdata, WriteBackWfm Wfm, string area)
        {
            if (patchdata == null)
            {
                return null;
            }
            IList<PID> pidList = Wfm.PIDS.Where(p => p.PId == patchdata.Pid).ToList();
            if (!pidList.Any())
            {
                return null;
            }
            var modelList = new List<dynamic>();
            foreach (PID pid in pidList)
            {
                ViewName viewName = Wfm.ViewNames.Single(v => v.id == pid.viewId);
                DataSet retdata = GetPushData(patchdata, viewName, pid);
                if (retdata.Tables[0] == null || retdata.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                WebserviceAdd add = Wfm.WebserviceAdds.Single(w => w.id == pid.WSAID);
                for (int i = 0; i < retdata.Tables[0].Rows.Count; i++)
                {
                    dynamic model = new ExpandoObject();
                    var dict = (IDictionary<string, object>)model;
                    foreach (DataColumn column in retdata.Tables[0].Columns)
                    {
                        dict[column.ColumnName] = retdata.Tables[0].Rows[i][column];
                    }
                    model.WebServiceAdd = add.name;
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        /// <summary>
        /// 一次轮询执行的操作
        /// </summary>
        public bool GetOnce(string wrkId, WriteBackWfm Wfm, string area)
        {
            try
            {
                string pids = Wfm.GetPIDSString();
                DataTable dt = GetPushDataInfos(wrkId, pids);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return false;
                }
                SlbhAndPid patchdata = DispatchData(dt, Wfm);
                if (patchdata == null)
                {
                    return false;
                }
                //解析如果不存在，则抛异常 所以用single
                IList<PID> pidList = Wfm.PIDS.Where(p => p.PId == patchdata.Pid).ToList();
                if (!pidList.Any())
                {
                    return false;
                }
                foreach (PID pid in pidList)
                {
                    bool flag=PushDataTo(patchdata, Wfm, pid, area, "WB");
                    if(!flag)
                    {
                        break;
                    }
                    flag= PushDataTo(patchdata, Wfm, pid, area, "CG");
                    if(!flag)
                    {
                        break;
                    }

                }
                return true;
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 
                // return false;
            }
        }


        private bool PushDataTo(SlbhAndPid patchdata, WriteBackWfm Wfm, PID pid, string area,string addressType)
        {
            ViewName viewName = Wfm.ViewNames.Single(v => v.id == pid.viewId);
            DataSet retdata = GetPushData(patchdata, viewName, pid);
            if (retdata.Tables[0] == null || retdata.Tables[0].Rows.Count == 0)
            {
                //errInfo.WBerrInfo = "没有取到推送数据,View[" + viewName.name + "]";
                //errInfo.CGerrInfo = "没有取到推送数据,View[" + viewName.name + "]";
                ResetPushResult(patchdata.Slbh, pid.name, "-1", "", pid.PId, "没有取到推送数据,View[" + viewName.name + "]", patchdata.StepName, addressType);
                return false;
            }
            WebserviceAdd add = Wfm.WebserviceAdds.Single(w => w.id == pid.WSAID);
            if (addressType.Equals("WB"))
            {
                string pushRet = PushDataWB(retdata, add.name, area);
                //string pushRet = PushData(retdata, add.name, area);
                //if(!IsError(errInfo))
                if (string.IsNullOrEmpty(pushRet))
                {//推送成功
                    ResetPushResult(patchdata.Slbh, pid.name, "1", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, pushRet, patchdata.StepName, addressType);
                }
                else
                {
                    ResetPushResult(patchdata.Slbh, pid.name, "0", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, pushRet, patchdata.StepName, addressType);
                }
                return true;
            }
            if (addressType.Equals("CG"))
            {
                string pushRet = PushDataCG(retdata, add.name, area);
                //string pushRet = PushData(retdata, add.name, area);
                //if(!IsError(errInfo))
                if (string.IsNullOrEmpty(pushRet) || pushRet.Contains("推送成功"))
                {//推送成功
                    ResetPushResult(patchdata.Slbh, pid.name, "1", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, pushRet, patchdata.StepName, addressType);
                }
                else
                {
                    ResetPushResult(patchdata.Slbh, pid.name, "0", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, pushRet, patchdata.StepName, addressType);
                }
                return true;
            }
            return false;
        }

       

       /* public bool PushAHouse(string SLBH, WriteBackWfm Wfm, string area, string HouseID)
        {
            try
            {
                string pids = Wfm.GetPIDSString();
                DataTable dt = GetPushDataInfos4SLBH(SLBH, pids);
                if (dt == null || dt.Rows.Count == 0)
                {
                    //ILog log1 = new ErrorLog(typeof(Polling));
                    //log1.WriteLog(new Exception("1111111111111"));
                    return false;
                }
                //ILog log7= new ErrorLog(typeof(Polling));
                //log7.WriteLog(new Exception(dt.Rows.Count.ToString()));
                SlbhAndPid patchdata = DispatchData2(dt, Wfm);
                if (patchdata == null)
                {
                    //ILog log2 = new ErrorLog(typeof(Polling));
                    // log2.WriteLog(new Exception("222222222222222"));
                    return false;
                }
                //解析如果不存在，则抛异常 所以用single
                IList<PID> pidList = Wfm.PIDS.Where(p => p.PId == patchdata.Pid).ToList();
                if (!pidList.Any())
                {
                    //ILog log3 = new ErrorLog(typeof(Polling));
                    //log3.WriteLog(new Exception("3333333333333"));
                    return false;
                }
                foreach (PID pid in pidList)
                {
                    ViewName viewName = Wfm.ViewNames.Single(v => v.id == pid.viewId);
                    DataSet retdata = GetPushData4House(patchdata, viewName, pid, HouseID);
                    InterfaceErrInfo errInfo = new InterfaceErrInfo();
                    if (retdata.Tables[0] == null || retdata.Tables[0].Rows.Count == 0)
                    {
                        errInfo.WBerrInfo = "没有取到推送数据,View[" + viewName.name + "]";
                        errInfo.CGerrInfo = "没有取到推送数据,View[" + viewName.name + "]";
                        ResetPushResult(patchdata.Slbh, pid.name, "-1", "", pid.PId,errInfo, patchdata.StepName);
                        break;
                    }
                    WebserviceAdd add = Wfm.WebserviceAdds.Single(w => w.id == pid.WSAID);
                    errInfo = PushData(retdata, add.name, area,errInfo);
                    if (!IsError(errInfo))
                    //if (string.IsNullOrEmpty(pushRet))
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "1", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, errInfo, patchdata.StepName);
                    }
                    else
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "0", SerialisePushData(retdata.Tables[0], viewName.name, add.name), pid.PId, errInfo, patchdata.StepName);
                    }

                }
                return true;
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 
                // return false;
            }
        }*/


        public BusinessResult PushAStep(string Slbh, WriteBackWfm Wfm, string area, string WrkId)
        {
            try
            {
                string pids = Wfm.GetPIDSString();
                DataTable dt = GetPushDataInfos(WrkId, pids);

                if (dt == null || dt.Rows.Count == 0)
                {
                    //ILog log1 = new ErrorLog(typeof(Polling));
                    //log1.WriteLog(new Exception("1111111111111"));
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                //ILog log7= new ErrorLog(typeof(Polling));
                //log7.WriteLog(new Exception(dt.Rows.Count.ToString()));
                SlbhAndPid patchdata = DispatchData2(dt, Wfm);
                if (IfPushedSuccess(Slbh, patchdata.StepName))
                {
                    return new BusinessResult { IsSuccess = true, Message = "以前已经推送成功" };
                }
                if (patchdata == null)
                {
                    //ILog log2 = new ErrorLog(typeof(Polling));
                    // log2.WriteLog(new Exception("222222222222222"));
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                //解析如果不存在，则抛异常 所以用single
                IList<PID> pidList = Wfm.PIDS.Where(p => p.PId == patchdata.Pid).ToList();
                if (!pidList.Any())
                {
                    //ILog log3 = new ErrorLog(typeof(Polling));
                    //log3.WriteLog(new Exception("3333333333333")); 
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                string pushRet = string.Empty;
                string message = string.Empty;
                foreach (PID pid in pidList)
                {
                    string wbMessage = "";
                    string cgMessage = "";
                    ViewName viewName = Wfm.ViewNames.Single(v => v.id == pid.viewId);

                    DataSet retdata = GetPushData(patchdata, viewName, pid);

                    InterfaceAddress add = GetRoute(Slbh,area);
                    if (add.WB)
                    {
                        wbMessage = PushDataRouter(retdata, patchdata, pid, Wfm,  viewName, "WB", area);
                    }
                    if (add.CG)
                    {
                        cgMessage = PushDataRouter(retdata, patchdata, pid, Wfm,  viewName, "CG", area);
                    }
                    if (wbMessage.Contains("成功") && cgMessage.Contains("成功"))
                    {
                        message = "";
                    }
                    else
                    {
                        if (!wbMessage.Contains("成功"))
                        {
                            message += wbMessage;
                        }
                        if(!cgMessage.Contains("成功"))
                        {
                            message += cgMessage;
                        }
                    }

                }
                message = message.TrimEnd('\n');
                if (string.IsNullOrEmpty(message))
                    return new BusinessResult { IsSuccess = true, Message = pushRet };
                else
                {
                    return new BusinessResult { IsSuccess = false, Message = message };
                }
            }
            catch (Exception ex)
            {
                //return new BusinessResult { IsSuccess = false, Message = ex.Message };
                throw new Exception(ex.Message);

                // return false;
            }
        }

        private string PushDataRouter(DataSet retdata, SlbhAndPid patchdata, PID pid, WriteBackWfm Wfm, ViewName viewName,string addressType,string area)
        {
            string pushRet = "-1";
            string message = "";
            if (retdata.Tables[0] == null || retdata.Tables[0].Rows.Count == 0)//表示没有取到推送数据
            {
                
                
                ResetPushResult(patchdata.Slbh, pid.name, pushRet, "","",pid.PId,patchdata.StepName,addressType);
                message="没有数据";
            }
            else
            {
                WebserviceAdd add = Wfm.WebserviceAdds.Single(w => w.id == pid.WSAID);
                if (addressType.Equals("WB"))
                {
                    message += pushRet = PushDataWB(retdata, add.name, area) + "\n";
                    //errInfo = PushData(retdata, add.name, area, errInfo);
                    //if (!IsError(errInfo))
                    if (pushRet.Trim().Equals("推送成功"))
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "1", SerialisePushData(retdata.Tables[0], viewName.name, add.name), GetInLength(pushRet), pid.PId, patchdata.StepName,addressType);
                    }
                    else
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "0", SerialisePushData(retdata.Tables[0], viewName.name, add.name), GetInLength(pushRet), pid.PId, patchdata.StepName,addressType);
                    }
                }
                if (addressType.Equals("CG"))
                {
                    message += pushRet = PushDataCG(retdata, add.name, area) + "\n";
                    //errInfo = PushData(retdata, add.name, area, errInfo);
                    //if (!IsError(errInfo))
                    if (pushRet.Trim().Equals("推送成功"))
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "1", SerialisePushData(retdata.Tables[0], viewName.name, add.name), GetInLength(pushRet), pid.PId, patchdata.StepName, addressType);
                    }
                    else
                    {
                        ResetPushResult(patchdata.Slbh, pid.name, "0", SerialisePushData(retdata.Tables[0], viewName.name, add.name), GetInLength(pushRet), pid.PId, patchdata.StepName, addressType);
                    }
                }
            }
            return message;
        }

        private string GetInLength(string str)
        {
            if (str.Length > 2000)
                return str.Substring(0, 1999);
            return str;
        }

        public BusinessResult PushASLBH(string Slbh, WriteBackWfm Wfm, string area)
        {
            try
            {
                string pids = Wfm.GetPIDSString();
                DataTable dt = GetPushDataInfos4SLBH(Slbh, pids);
                if (dt == null || dt.Rows.Count == 0)
                {
                    //ILog log1 = new ErrorLog(typeof(Polling));
                    //log1.WriteLog(new Exception("1111111111111"));
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                //ILog log7= new ErrorLog(typeof(Polling));
                //log7.WriteLog(new Exception(dt.Rows.Count.ToString()));
                SlbhAndPid patchdata = DispatchData2(dt, Wfm);
                if (patchdata == null)
                {
                    //ILog log2 = new ErrorLog(typeof(Polling));
                    // log2.WriteLog(new Exception("222222222222222"));
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                //解析如果不存在，则抛异常 所以用single
                IList<PID> pidList = Wfm.PIDS.Where(p => p.PId == patchdata.Pid).ToList();
                if (!pidList.Any())
                {
                    //ILog log3 = new ErrorLog(typeof(Polling));
                    //log3.WriteLog(new Exception("3333333333333")); 
                    return new BusinessResult { IsSuccess = true, Message = "对应的流程不需要推送" };
                }
                string pushRet = string.Empty;
                string message = string.Empty;

                foreach (PID pid in pidList)
                {
                    string wbMessage = "";
                    string cgMessage = "";
                    ViewName viewName = Wfm.ViewNames.Single(v => v.id == pid.viewId);

                    DataSet retdata = GetPushData(patchdata, viewName, pid);

                    InterfaceAddress add = GetRoute(Slbh,area);
                    if (add.WB)
                    {
                        wbMessage = PushDataRouter(retdata, patchdata, pid, Wfm, viewName, "WB", area);
                    }
                    if (add.CG)
                    {
                        cgMessage = PushDataRouter(retdata, patchdata, pid, Wfm, viewName, "CG", area);
                    }
                    if (wbMessage.Contains("成功") && cgMessage.Contains("成功"))
                    {
                        message = "";
                    }
                    else
                    {
                        if (!wbMessage.Contains("成功"))
                        {
                            message += wbMessage;
                        }
                        if (!cgMessage.Contains("成功"))
                        {
                            message += cgMessage;
                        }
                    }

                }
                if (string.IsNullOrEmpty(message))
                    return new BusinessResult { IsSuccess = true, Message = pushRet };
                else
                {
                    return new BusinessResult { IsSuccess = false, Message = message };
                }
            }
            catch (Exception ex)
            { 
                return new BusinessResult { IsSuccess = false, Message = ex.Message };
 
                // return false;
            }
        }

        private InterfaceAddress GetRoute(string slbh,string arae)
        {
            InterfaceAddress errInfo = new InterfaceAddress();
            errInfo.CG = false;
            errInfo.WB = false;
            string sql = "select * from fc_spfhx_tag where slbh='{0}'";
            sql = string.Format(sql, slbh);
            DataTable dt = DBHelper.GetDataTable(sql, ConnectType.SXK);
            if(!arae.Equals("徐州"))
            {
                errInfo.WB = true;
                errInfo.CG = false;
                return errInfo;
            }
            if(null!=dt && dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if(!row["SFTS"].ToString().Equals("1"))
                    {
                        if(string.IsNullOrEmpty(row["ADDRESSTYPE"].ToString()))
                        {
                            if(!row["WBERRINFO"].ToString().Equals("推送成功"))
                            {
                                errInfo.WB = true;
                            }
                            if (!row["CGERRINFO"].ToString().Equals("推送成功"))
                            {
                                errInfo.CG = true;
                            }
                        }
                        else if(row["ADDRESSTYPE"].ToString().Equals("WB"))
                        {
                            errInfo.WB = true;
                        }
                        else
                        {
                            errInfo.CG = true;
                        }
                    }
                }
            }
            else
            {
                errInfo.CG = true;
                errInfo.WB = true;
            }
            return errInfo;
        }

        private DataSet GetPushData(SlbhAndPid patchdata, ViewName viewName, PID pid)
        {
            string sql = string.Empty;
            sql = viewName.sql + string.Format(" where slbh like '{0}%'", patchdata.Slbh);
 
            return DBHelper.GetDataSet(sql, ConnectType.SXK);
        }
        private DataSet GetPushData4House(SlbhAndPid patchdata, ViewName viewName, PID pid, string HouseId)
        {
            string sql = string.Empty;
            sql = viewName.sql + string.Format(" where slbh like '{0}%' and HouseId='{0}'", patchdata.Slbh, HouseId);
 
            return DBHelper.GetDataSet(sql, ConnectType.SXK);
        }
        public string PushDataWB(DataSet ds, string address, string area)
        {
            if (area.Equals("贾汪"))
            {
                area = "3";
            }
            else if (area.Equals("睢宁"))
            {
                area = "2";
            }
            else if (area.Equals("邳州"))
            {
                area = "4";
            }
            else if (area.Equals("沛县"))
            {
                area = "5";
            }
            else if (area.Equals("丰县"))
            {
                area = "6";
            }
            else if (area.Equals("铜山"))
            {
                area = "7";
            }
            else if (area.Equals("新沂"))
            {
                area = "8";
            }
            else
            {
                area = "1";
            }
            VisitWebService visit = new VisitWebService();
            DataSet rDs = null;
            DataSet cDs = null;
            if (address == "UpdateCSDJStateForSPF")
            {
                
                    rDs = visit.UpdateCSDJStateForSPF(ds, area);
                           
            }
            else if (address == "UpdateMortgageStateForSPF")
            {
                
                    rDs = visit.UpdateMortgageStateForSPF(ds, area);
         
                
            }
            else if (address == "UpdateSealStateForSPF")
            {
                
                    rDs = visit.UpdateSealStateForSPF(ds, area);
              
                
            }
            else if (address == "UpdateYGDJStateForSPF")
            {
               
                    rDs = visit.UpdateYGDJStateForSPF(ds, area);
                              
            }
            else
            {
                rDs = new DataSet();
            }
            

            return PushResult(rDs, "网备接口");
        }


        public string PushDataCG(DataSet ds, string address, string area)
        {
            if (area.Equals("贾汪"))
            {
                area = "3";
            }
            else if (area.Equals("睢宁"))
            {
                area = "2";
            }
            else if (area.Equals("邳州"))
            {
                area = "4";
            }
            else if (area.Equals("沛县"))
            {
                area = "5";
            }
            else if (area.Equals("丰县"))
            {
                area = "6";
            }
            else if (area.Equals("铜山"))
            {
                area = "7";
            }
            else if (area.Equals("新沂"))
            {
                area = "8";
            }
            else
            {
                area = "1";
            }
            VisitWebService visit = new VisitWebService();
            
            DataSet cDs = null;
            if (address == "UpdateCSDJStateForSPF")
            {
                
                
                    cDs = visit.UpdateCSDJStateForCG(ds, area);
                
            }
            else if (address == "UpdateMortgageStateForSPF")
            {
               
               
                    cDs = visit.UpdateMortgageStateForCG(ds, area);
                
            }
            else if (address == "UpdateSealStateForSPF")
            {
                
                
                    cDs = visit.UpdateSealStateForCG(ds, area);
                
            }
            else if (address == "UpdateYGDJStateForSPF")
            {
                
               
                    cDs = visit.UpdateYGDJStateForCG(ds, area);
                
            }
            else
            {
                
                cDs = new DataSet();
            }

            return PushResult(cDs, "测管接口");
        }

        private static string PushResult(DataSet rDs,string interfaceName)
        {
            if (rDs != null)
            {
                if (rDs.Tables.Count > 0 && rDs.Tables[0].Rows.Count > 0)
                {
                    string ret = "";
                    for (int i = 0; i < rDs.Tables[0].Rows.Count; i++)
                    {
                        if (!rDs.Tables[0].Rows[i]["result"].ToString().Equals("1"))
                        {
                            ret += interfaceName+"--"+rDs.Tables[0].Rows[i]["message"].ToString() + ",房屋ID为：[" + rDs.Tables[0].Rows[i]["HouseID"].ToString() + "]" + "\n";
                        }
                      
                    }
                    ret = ret.TrimEnd('\n');
                    if(string.IsNullOrEmpty(ret))
                    {
                        ret = "推送成功";
                    }
                    return ret;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return string.Format("{0}无数据返回", interfaceName);
            }
        }

        /// <summary>
        /// 在标记表中返回
        /// </summary>
        private void ResetPushResult(string slbhs, string pname, string sret, string pushData, string pushresult,string Pid, string PushStepName,string addressType)
        {
            string sql = string.Empty;
            string existsSql = string.Format("Select Count(1) from FC_SPFHX_TAG Where SLBH='{0}' and PushStepName='{1}' and ADDRESSTYPE='{2}'", slbhs, PushStepName,addressType);
            int exists = Convert.ToInt32(DBHelper.GetScalar(existsSql, ConnectType.SXK));
            int ret;
            if (!int.TryParse(sret,out ret)) 
            {
                ret = 0;
            }
            //PushResult = PushResult.Substring(0, PushResult.Length >= 2000 ? 1999 : PushResult.Length);
            if (exists > 0)
            {
                sql = string.Format("Update FC_SPFHX_TAG Set DJLX='{1}',SFTS='{2}',PushData='{3}',PId='{4}',PushResult='{5}',PushStepName='{6}' Where slbh='{0}' and ADDRESSTYPE='{7}'", slbhs, pname, ret, pushData, Pid, pushresult, PushStepName, addressType);
            }
            else
            {
                sql = string.Format("insert into FC_SPFHX_TAG(slbh,DJLX,SFTS,PushData,PId,PushResult,PushStepName,ADDRESSTYPE) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", slbhs, pname, ret, pushData, Pid, pushresult, PushStepName,addressType);
            }
            DBHelper.ExecuteCommand(sql, ConnectType.SXK);
        }

        private bool IfPushedSuccess(string slbh, string stepName)
        {
            string sql = string.Format("Select Count(1) from FC_SPFHX_TAG Where SLBH='{0}' and PushStepName='{1}' and SFTS='1'", slbh, stepName);
            object o = DBHelper.GetScalar(sql, ConnectType.SXK);
            return int.Parse(o.ToString()) > 0;
        }
        //private void ResetPushResult(string slbhs, string pname, string ret, string pushData)
        //{
        //    string sql = string.Empty;
        //    sql = string.Format("insert into FC_SPFHX_TAG(slbh,DJLX,SFTS,PushData) values ('{0}','{1}',{2},'{3}')", slbhs, pname, ret, pushData);
        //    DBHelper.SetConnectType(ConnectType.SXK);
        //    DBHelper.ExecuteCommand(sql);
        //}
        /// <summary>
        /// 序列化回写数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string SerialisePushData(DataTable dt, string viewName, string add)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ViewName:{0},Address:{1},Data:{2}", viewName, add, DataTableToJson(dt));
            string ret = sb.ToString();
            ret = ret.Substring(0, ret.Length >= 2000 ? 1999 : ret.Length);
            return ret;
        }


        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }
    }

    public class SlbhAndPid
    {
        public string Slbh { get; set; }

        public string Pid { get; set; }
        public string StepName { get; set; }
    }

    public class BusinessResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
