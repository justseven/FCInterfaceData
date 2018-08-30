 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// Bdc2Fc 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Bdc2Fc_CLF : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
          [WebMethod]
        public DataSet FC_CLF_ZTXX(string syqr, string qzbh)
        {
            DataSet ds = new DataSet();
            FC_CLF_Data data = new FC_CLF_Data();
            DataTable dt = data.GetCLF_ZTXX(syqr, qzbh);
            ds.Tables.Add(dt);
            return ds;
        }

        [WebMethod]
        public DataSet FC_CLF_YZXX(string ywzh)
        {
            DataSet ds = new DataSet();
            FC_CLF_Data data = new FC_CLF_Data();
            DataTable dt = data.GetCLF_ZTXX(ywzh);
            ds.Tables.Add(dt); 
            DataTable dtFZXX = data.GetCLF_FZXX(ywzh);
            ds.Tables.Add(dtFZXX);
            DataTable dtDYXX = data.GetCLF_DYXX(ywzh);
            ds.Tables.Add(dtDYXX);
            DataTable dtCFXX = data.GetCLF_CFXX(ywzh);
            ds.Tables.Add(dtCFXX);  
            return ds;
        }
        /// <summary>
        /// 发证信息
        /// </summary>
        /// <param name="ywzh"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet FC_CLF_FZXX(string ywzh)
        {
            DataSet ds = new DataSet();
            FC_CLF_Data data = new FC_CLF_Data();
            DataTable dt = data.GetCLF_LZXX_QZBH(ywzh);
            ds.Tables.Add(dt);
            if (dt != null && dt.Rows.Count > 0 )
            {
                DataTable dtFZXX_SFFZ = data.GetCLF_LZXX_SFFZ(ywzh);
                ds.Tables.Add(dtFZXX_SFFZ);
                DataTable dtFZXX_QTXX = data.GetCLF_LZXX_QTXX(ywzh);
                ds.Tables.Add(dtFZXX_QTXX);
            }
            return ds;
        }
    }
}
