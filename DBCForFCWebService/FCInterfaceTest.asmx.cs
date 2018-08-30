using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// FCInterfaceTest 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FCInterfaceTest : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string UpdateSealStateForSPF(DataSet dsHouses, string Area)
        {
            return Serialize(dsHouses.Tables[0]);
        }

        [WebMethod]
        public string UpdateMortgageStateForSPF(DataSet dsHouses, string Area)
        {
            return Serialize(dsHouses.Tables[0]);
        }

        [WebMethod]
        public string UpdateYGDJStateForSPF(DataSet dsHouses, string Area)
        {
            return Serialize(dsHouses.Tables[0]); 
        }

        [WebMethod]
        public string UpdateCSDJStateForSPF(DataSet dsHouses, string Area)
        { 
            return Serialize(dsHouses.Tables[0]);
        } 
        /// <summary>
        /// 不需要分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="flag">false</param>
        /// <returns></returns>
        public static string Serialize(DataTable dt)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return serializer.Serialize(list); ;
        }
    }
}
