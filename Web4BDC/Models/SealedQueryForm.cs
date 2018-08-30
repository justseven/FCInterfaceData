using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    /// <summary>
    /// 查封查询类
    /// </summary>
    public class SealedQueryForm
    {
        [Display(Name = "房屋地址")]
        public string Address { get; set; }
        [Display(Name = "姓名")] 
        public string Name { get; set; }
        [Display(Name = "身份证号")] 
        public string IDNumber { get; set; }
    }
}