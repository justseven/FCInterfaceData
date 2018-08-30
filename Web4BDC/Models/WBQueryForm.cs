using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class WBQueryForm
    {
        [Display(Name = "房屋地址")]
        public string Address { get; set; }
        [Display(Name="名字")]
        public string QLRXM   { get; set; }
        [Display(Name = "身份证")]
        public string ZJHM { get; set; }
    }

    public class WBFWQueryForm
    {
        [Display(Name = "项目")]
        public string XMMC { get; set; }


    }
}