using DBCForFCWebService.Dal;
using DBCForFCWebService.Model.Tax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// TaxWSBiz 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class TaxWSBiz : System.Web.Services.WebService
    {

       
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string GetHouseInfo(string contrRecNum)
        {
            HoInfoResponse info = new HoInfoResponse();
            try
            {
                if (CheckNum(contrRecNum))
                {
                    DataSet ds = new DataSet();
                    ds.DataSetName = "getHoInfoResponse";
                    DataTable dt=new TaxDal().GetHouseInfoByHTBH(contrRecNum);
                    if (null == dt || dt.Rows.Count == 0)
                    {
                        dt = new DataTable();
                        info.Msg = "失败！未查询到数据";

                    }
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        info.Msg = "查询成功";
                    }
                    ds.Tables.Add(dt.Copy());
                    info.DataSource = ds;



                }
                else
                {
                    info.Msg = "传入参数错误！";
                }
                
            }
            catch(Exception ex)
            {
                info.Msg = ex.Message;
            }
            return info.ToXml();
        }

        private bool CheckNum(string contrRecNum)
        {
            return !IsHasSQLInject(contrRecNum);
        }

        private bool IsHasSQLInject(string str)
        {
            bool isHasSQLInject = false;

            //字符串中的关键字更具需要添加
            //string inj_str = "'|and|exec|union|create|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|xp_|or|--|+";
            string inj_str = "'exec|create|insert|delete|update|count|chr|mid|master|truncate|declare|xp_|--|+";
            str = str.ToLower().Trim();
            string[] inj_str_array = inj_str.Split('|');
            foreach (string sql in inj_str_array)
            {
                if (str.IndexOf(sql) > -1)
                {
                    isHasSQLInject = true;
                    break;
                }
            }
            return isHasSQLInject;
        }
    }
}
