 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// ZTJA_DJForSPF_Srv 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ZTJA_DJForSPF_Srv : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 获取楼盘下初始房屋的登记信息
        /// </summary>
        /// <param name="BuNo">32位(Oracle形式的GUID)</param>
        /// <param name="Area"></param>
        /// <returns></returns>
         [WebMethod]
        public DataSet GetBuildRightInfo(string BuildingID, string Area)
        {
            FC_SPF_Data data = new FC_SPF_Data();
            Guid GBuNo;
            if (Guid.TryParse(BuildingID, out GBuNo))
            {
                BuildingID = GBuNo.ToString("D").ToUpper();
            } 
            return data.GetBuildRightInfo(BuildingID, Area);
        }
        /// <summary>
        /// 获取指定房屋的各种状态
        /// </summary>
         /// <param name="HouseID">32位(Oracle形式的GUID)</param>
        /// <returns></returns>
         [WebMethod]
         public DataSet GetHouseState(string HouseID, string Area)
        {
            FC_SPF_Data data = new FC_SPF_Data();
            Guid GHouseID;
            if( Guid.TryParse(HouseID,out GHouseID)){
                HouseID = GHouseID.ToString("D").ToUpper();
            }
            return data.GetHouseState(HouseID);
        }
        /// <summary>
        /// 获取制定房屋是否转移登记
        /// </summary>
         /// <param name="HouseID">32位(Oracle形式的GUID)</param>
        /// <param name="Area"></param>
        /// <returns>0 否 1 是</returns>
         [WebMethod]
        public string GetPrprtcertInfo(string HouseID, string Area)
        {
            FC_SPF_Data data = new FC_SPF_Data();
            Guid GHouseID;
            if (Guid.TryParse(HouseID, out GHouseID))
            {
                HouseID = GHouseID.ToString("D").ToUpper();
            }
           DataSet ds=  data.GetPrprtcertInfo(HouseID, Area);
           if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               return "1";
           }
           else
               return "0";
        }
         [WebMethod]
         public DataSet GetRegistesByHouseId(string HouseId)
         {
             FC_SPF_Data data = new FC_SPF_Data();
             return data.GetRegistesByHouseId(HouseId);
         }
    }
}
