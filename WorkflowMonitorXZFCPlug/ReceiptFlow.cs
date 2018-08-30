 
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace WorkflowMonitorXZFCPlug
{
    public class ReceiptFlow 
    { 
        private WriteBackWfm wfm;
        public ReceiptFlow()
        {
            try
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;
                object o = cache["FCWriteBackWfm"];
                if (o == null)
                { 
                    string path = ConfigurationManager.AppSettings["FCWriteBackWfm_Address"].ToString();  
                    WriteBackXMLOperate op = new WriteBackXMLOperate();  
                    wfm = op.Xml2Model(path);
                    cache.Insert("FCWriteBackWfm", wfm);
                }
                else
                {
                    wfm = (WriteBackWfm)o;
                }
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 

            }
        }
        #region IWorkFlowRouter 成员

        public void ActivitiChangeState(string wrkId, string rawState, string newState)
        {
            return;
        }

        public void ProcessChangeState(string prjId, string rawState, string newState)
        {
            return;
        }

        public void ProcessEnd(string prjId)
        {
            return;
        }

        public void ProcessFallBack(string wrkId, string previewWrkId)
        {
            return;
        }

        public void ProcessStart(string prjId)
        {
            return;
        }

        public void ProcessSubmit(string wrkId, IList<string> nextActs)
        {  
            try
            {  
                FCWriteBackWhenSubmit(wrkId);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message); 
            }
        }

        #endregion

        /// <summary>
        /// 房产数据回写操作
        /// </summary>
        /// <param name="wrkId"></param>
        private void FCWriteBackWhenSubmit(string wrkId)
        {
            string area = ConfigurationManager.AppSettings["Area"].ToString();  
            Polling p = new Polling();
            p.GetOnce(wrkId, wfm, area); 
        }
    }
}
