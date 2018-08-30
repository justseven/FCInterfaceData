using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// Verification4CG 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Verification4CG : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 是否做过登记
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool IsRegistedBuild(string buildingId)
        {
            if (string.IsNullOrEmpty(buildingId))
            {
                return false;
            }
            FC_CG_Data cgData = new FC_CG_Data();
            return cgData.GetIsRegistedBuilding(buildingId);
        }

        [WebMethod]
        public bool IsRegistedHouse(string HouseId)
        {
            if (string.IsNullOrEmpty(HouseId))
            {
                return false;
            }
            FC_CG_Data cgData = new FC_CG_Data();
            return cgData.GetIsRegistedHouse(HouseId);
        }
        [WebMethod]
        public DataSet GetFirstRegistedInfoByHouseId(string HouseId) {
            Guid Gh; string sh;
            if (!Guid.TryParse(HouseId, out Gh))
            {
                sh = HouseId;
            }
            else {
                sh = Gh.ToString();
            }
            FC_CG_Data cgData = new FC_CG_Data();
            return cgData.GetFirstRegistedInfo(sh);
        }
    }
}
