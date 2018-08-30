using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkflowMonitorXZFCPlug
{
    public class WriteBackWfm
    {
       public  IList<PID> PIDS{get;set;}
       public IList<ViewName> ViewNames{get;set;}

       public IList<WebserviceAdd> WebserviceAdds{get;set;}

       public string GetPIDSString()
       {
           if (this.PIDS != null && this.PIDS.Count > 0)
           {
               IEnumerable<string> ps = this.PIDS.Select(pid => "'" + pid.PId + "'");
               return string.Join(",", ps);
           }
           else
               return string.Empty;
       }

        public static WriteBackWfm GetInstance() {
            WriteBackWfm wfm;
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
            return wfm;
        }

        /***************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        public static WriteBackWfm GetInstanceFromDB(string slbh)
        {
            WriteBackWfm wfm = new WriteBackWfm();
            wfm.PIDS = GetPIDS(slbh);
            wfm.ViewNames = GetViewName(slbh);
            wfm.WebserviceAdds = GetWebserviceAdds();
            return wfm;
        }
        *********************************************************************/
    }
    public class PID
    {
        public string name { get; set; }

        public string viewId { get; set; }

        public string WSAID { get; set; }

        public string PId { get; set; }

        public string stepname { get; set; } 
    }
    public class ViewName
    {
        public string id { get; set; }
        public string name { get; set; }

        public string sql { get;set;}
    }

    public class WebserviceAdd
    {
        public string id { get; set; }

        public string name { get; set; }
    }
}
