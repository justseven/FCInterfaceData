using DBCForFCWebService.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// ProcinstWS 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ProcinstWS : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetProStateResponse(string ywzh)
        {
            ReturnResponse(ywzh);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonModel GetProStateModel(string ywzh)
        {
            return GetStateModel(ywzh);
        }

        private void ReturnResponse(string ywzh)
        {
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(GetState(ywzh));
            Context.Response.End();
        }

        private string GetState(string ywzh)
        {
            GetPrjStateDAL dal = new GetPrjStateDAL();
            JsonModel jm = dal.GetPrjState(ywzh);
            return Newtonsoft.Json.JsonConvert.SerializeObject(jm, new Newtonsoft.Json.JsonSerializerSettings() { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii });
        }

        private JsonModel GetStateModel(string ywzh)
        {
            GetPrjStateDAL dal = new GetPrjStateDAL();
            JsonModel model= dal.GetPrjState(ywzh);
            model.prjState = dal.GetStepName(model.ywzh);
            if(string.IsNullOrEmpty(model.zl))
            {
                model.zl = dal.GetZLBySlbh(ywzh);
            }
            return model;
        }
    }

   
}
