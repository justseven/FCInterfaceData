using DBCForFCWebService.Dal;
using DBCForFCWebService.Model.TongShan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace DBCForFCWebService
{
    /// <summary>
    /// TSZZCH 的摘要说明
    /// </summary>
    public class TSZZCH : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }





        public void GetBDCInfo(HttpContext context)
        {
            context.Request.ContentEncoding= Encoding.GetEncoding("utf-8");
            string queryJson=context.Request.Form["PostData"].ToString();
            QueryRoot queryModel=Newtonsoft.Json.JsonConvert.DeserializeObject<QueryRoot>(queryJson);
            
            if(null!=queryModel.owners && queryModel.owners.Count>0)
            {
                ReturnRoot root = GetBDCInfoByQLR(queryModel.owners);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(root);
                context.Response.ContentEncoding= Encoding.GetEncoding("utf-8");
                context.Response.ContentType= "text/plain";
                context.Response.Write(json);
                context.Response.End();
            }
        }

        private ReturnRoot GetBDCInfoByQLR(List<OwnersItem> owners)
        {
            ReturnRoot root = new ReturnRoot();
            TSZZCX_DAL dal = new TSZZCX_DAL();
            DataTable dt = dal.GetBDCInfo(owners);
            foreach (DataRow row in dt.Rows)
            {
                ListItem item = new ListItem();
                item.zl = row["zl"].ToString();
                item.qlr = row["qlrmc"].ToString();
                item.sfz = row["zjhm"].ToString();
                item.id = "";
                item.hj = "";
                root.list.Add(item);
            }
            return root;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class QLR
    {
        public string qlrmc { get; set; }
        public string zjhm { get; set; }
    }
}