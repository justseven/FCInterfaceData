using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace DBCForFCWebService
{
    /// <summary>
    /// Bdc2Da 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Bdc2Da : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        //public string GetNewData(string Name,string CardNo) {
        //    FC_DA_Data data = new FC_DA_Data();
        //    NewDataSet set= data.GetNewDataSet(Name, CardNo);
        //    try
        //    {
        //        if (set != null)
        //        {
        //            return SerializerNewDataSet(set);
        //        }
        //        else
        //        {
        //            return "<?xml version=\"1.0\" encoding=\"GB2312\"?><NewDataSet><NOResult>无返回结果</NOResult></NewDataSet> ";
        //        } 
        //    }
        //    catch (Exception ex) {
        //        return string.Format("<?xml version=\"1.0\" encoding=\"GB2312\"?>< NewDataSet ><NOResult>{0}</NOResult></NewDataSet> ",ex.Message);
        //    }
        //}

        public NewDataSet GetNewData(string Name, string CardNo)
        {
            FC_DA_Data data = new FC_DA_Data();
            NewDataSet set = data.GetNewDataSet(Name, CardNo);
            try
            {
                if (set != null)
                {
                    return set;
                }
                else
                {
                    return new NewDataSet { NOResult= "无返回结果" };
                }
            }
            catch (Exception ex)
            {
                return new NewDataSet { NOResult = ex.Message };
            }
        }

        private string SerializerNewDataSet(NewDataSet set)
        {
            if (set != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<?xml version=\"1.0\" encoding=\"GB2312\"?><NewDataSet>");
                if (set.Tables.Length > 0)
                {
                    for (int i = 0; i < set.Tables.Length; i++) {
                        sb.AppendFormat("<Table>");
                        sb.AppendFormat("<ProveResultID>");
                        sb.AppendFormat(set.Tables[i].ProveResultID);
                        sb.AppendFormat("</ProveResultID>");
                        sb.AppendFormat("<BusiID>"); 
                        sb.AppendFormat(set.Tables[i].BusiID);
                        sb.AppendFormat("</BusiID>");
                        sb.AppendFormat("<CardNo>");
                        sb.AppendFormat(set.Tables[i].CardNo);
                        sb.AppendFormat("</CardNo>");
                        sb.AppendFormat("<HouseSite>");
                        sb.AppendFormat(set.Tables[i].HouseSite);
                        sb.AppendFormat("</HouseSite>");
                        sb.AppendFormat("<Source>");
                        sb.AppendFormat(set.Tables[i].Source);
                        sb.AppendFormat("</Source>");
                        sb.AppendFormat("<SourceDes>");
                        sb.AppendFormat(set.Tables[i].SourceDes);
                        sb.AppendFormat("</SourceDes>");
                        sb.AppendFormat("<Area>");
                        sb.AppendFormat(set.Tables[i].Area);
                        sb.AppendFormat("</Area>");
                        sb.AppendFormat("<RightNo>");
                        sb.AppendFormat(set.Tables[i].RightNo);
                        sb.AppendFormat("</RightNo>");
                        sb.AppendFormat("</Table>");
                    }
                }
                else {
                    sb.AppendFormat("<NOResult>无返回结果</NOResult>");
                }
                sb.AppendFormat("</NewDataSet>"); 
                return sb.ToString();
            }
            else {
                return "<?xml version=\"1.0\" encoding=\"GB2312\"?>< NewDataSet ><NOResult>无返回结果</NOResult></NewDataSet> ";
            }
        }
    }
}
