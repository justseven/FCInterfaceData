using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class FCWebServiceQueryForm
    {
        /// <summary>
        /// 合同号
        /// </summary>
        [Display(Name="合同号")]
        public string ContractNo { get; set; }
        /// <summary>
        /// 执行码
        /// </summary>
        [Display(Name="执行码")]
        public string ExcuteCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "项目名称")]
        public string ItemName { get; set; }

        /// <summary>
        /// 幢号
        /// </summary>
        [Display(Name = "幢号")]
        public string BuildingNo { get; set; }


    }
}