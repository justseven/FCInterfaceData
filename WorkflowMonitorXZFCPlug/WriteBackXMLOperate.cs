using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WorkflowMonitorXZFCPlug
{
    /// <summary>
    /// xml操作类
    /// </summary>
    public class WriteBackXMLOperate
    {
        private XmlDocument ReadXml2Doc(string path) {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            return xml;
        }

        public WriteBackWfm Xml2Model(string path)
        {
            XmlDocument doc = ReadXml2Doc(path);
            WriteBackWfm wfm = new WriteBackWfm();
            XmlNode PidsNode = doc.DocumentElement.SelectSingleNode("PIDS");
            XmlNode ViewNamesNode = doc.DocumentElement.SelectSingleNode("ViewNames");
            XmlNode WebserviceAddsNode = doc.DocumentElement.SelectSingleNode("WebserviceAdds");
            if (PidsNode != null)
            {
                wfm.PIDS = new List<PID>();
                XmlNodeList pidnodes = PidsNode.SelectNodes("PID");
                foreach (XmlNode xn in pidnodes) { 
                    PID p=new PID();
                    p.PId=xn.InnerText;
                    p.name=xn.Attributes["name"].Value;
                    p.viewId=xn.Attributes["viewId"].Value;
                    p.WSAID=xn.Attributes["WSAID"].Value;
                    p.stepname = xn.Attributes["StepName"].Value;
                    wfm.PIDS.Add(p);
                }
            }
            if (ViewNamesNode != null)
            {
                wfm.ViewNames = new List<ViewName>();
                XmlNodeList viewNamenodes = ViewNamesNode.SelectNodes("ViewName");
                foreach (XmlNode xn in viewNamenodes)
                {
                    ViewName v = new ViewName();
                    v.name = xn.InnerText;
                    v.id = xn.Attributes["id"].Value;
                    v.sql = xn.Attributes["sql"].Value;
                    wfm.ViewNames.Add(v);
                }
            }
            if (WebserviceAddsNode != null)
            {
                wfm.WebserviceAdds = new List<WebserviceAdd>();
                XmlNodeList WebserviceAddnodes = WebserviceAddsNode.SelectNodes("WebserviceAdd");
                foreach (XmlNode xn in WebserviceAddnodes)
                {
                    WebserviceAdd w = new WebserviceAdd();
                    w.name = xn.InnerText;
                    w.id = xn.Attributes["id"].Value;
                    wfm.WebserviceAdds.Add(w);
                }
            }
            return wfm;
        }

        
    }
}
