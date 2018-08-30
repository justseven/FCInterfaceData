using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DBCForFCWebService
{
    /// <summary>
    /// Bdc2TSDa1 的摘要说明
    /// </summary>
    public class Bdc2TSDa1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ret = GetNewData(context.Request["Name"], context.Request["CardNo"]);
            context.Response.Write(ret);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public string GetNewData(string Name, string CardNo)
        {
            FC_DA_Data data = new FC_DA_Data();
            NewDataSet set = data.GetNewDataSet(Name, CardNo);
            try
            {
                if (set != null)
                {
                    return SerializerNewDataSet(set);
                }
                else
                {
                    return "<?xml version=\"1.0\" encoding=\"GB2312\"?><NewDataSet><NOResult>无返回结果</NOResult></NewDataSet> ";
                }
            }
            catch (Exception ex)
            {
                return string.Format("<?xml version=\"1.0\" encoding=\"GB2312\"?>< NewDataSet ><NOResult>{0}</NOResult></NewDataSet> ", ex.Message);
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
                    for (int i = 0; i < set.Tables.Length; i++)
                    {
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
                else
                {
                    sb.AppendFormat("<NOResult>无返回结果</NOResult>");
                }
                sb.AppendFormat("</NewDataSet>");
                return sb.ToString();
            }
            else
            {
                return "<?xml version=\"1.0\" encoding=\"GB2312\"?>< NewDataSet ><NOResult>无返回结果</NOResult></NewDataSet> ";
            }
        }
    }
}