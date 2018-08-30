using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{

    public class FCWebServiceQuery
    {
        /// <summary>
        /// 执行码
        /// </summary>
        public string ExecuteCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string XMMC { get; set; }
        /// <summary>
        /// 幢号
        /// </summary>
        public string JZWMC { get; set; }
        /// <summary>
        /// 合同号
        /// </summary>
        public string HTBH { get; set; }

        public static FCWebServiceQuery Map2This(FCWebServiceQueryForm form)
        {
            return new FCWebServiceQuery{
                ExecuteCode = form.ExcuteCode,
                XMMC = form.ItemName,
                JZWMC = form.BuildingNo,
                HTBH = form.ContractNo
            };
        }
    }
}