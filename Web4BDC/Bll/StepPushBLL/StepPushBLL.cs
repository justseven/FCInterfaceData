using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Web4BDC.Dal.StepPushDAL;
using Web4BDC.Models.StepPush;

namespace Web4BDC.Bll.StepPushBLL
{
    
    public class StepPushBLL
    {
        /// <summary>
        /// 多个步骤以|分割
        /// </summary>
        private static string stepNames = ConfigurationManager.AppSettings["PushStepName"].ToString();
        private static string PushStephUrl= ConfigurationManager.AppSettings["PushStephUrl"].ToString();
        private string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
        private string pushStr = "xzqdm={0}&wdbs={1}&ywbh={2}&jdmc={3}&blry={4}&sqrxm={5}&sqrlxfs={6}";
        internal bool CanPush(string v)
        {
            WriteLog("stepNames=" + stepNames);
            WriteLog("v=" + v);
            bool flag = stepNames.Contains(v);
            WriteLog("flag=" + flag);
            return flag;
                 
        }

        internal string GetWDBS(string prjID)
        {
            StepPushDAL dal = new StepPushDAL();
            return dal.GetWDBS(prjID);
        }

        internal string GetXZQDM(string prjID)
        {
            //StepPushDAL dal = new StepPushDAL();
            //string qxdm = dal.GetXZQDM(prjID);
            //WriteLog("qxdm=" + qxdm);
            //return qxdm;
            return "320301";
        }

        internal string SendPostMessage(StepPushJsonModel spjm)
        {
            StepPushDAL dal = new StepPushDAL();
            
            try
            {

                pushStr = string.Format(pushStr, spjm.XZQDM, spjm.WDBS, spjm.YWBH, spjm.JDMC, spjm.BLRY,spjm.SQRXM,spjm.SQRLXFS);
                //pushStr = System.Web.HttpUtility.UrlEncode(PushStephUrl + "?" + pushStr, System.Text.Encoding.UTF8);
                pushStr = PushStephUrl + "?" + System.Web.HttpUtility.UrlEncode(pushStr,System.Text.Encoding.UTF8);
                spjm.SendStr = pushStr;
                
            }
            catch (Exception ex)
            {
                
                spjm.IsSuccess = -1;
                spjm.ErrorMsg = "推送数据失败：" + ex.Message;
            }
            dal.InsertStepLog(spjm);
            return pushStr;
        }

        private void WriteLog(string str)
        {
            string path = "c:\\StepPushBll.log";
            StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            try
            {

                wr.WriteLine(str);
                wr.WriteLine("--------------------------------------------------------------------");
            }
            catch { }
            finally
            {
                wr.Close();
            }
            

        }
    }
}