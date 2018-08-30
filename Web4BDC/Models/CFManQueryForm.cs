using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    /// <summary>
    /// 查询查封权利人Form
    /// </summary>
    public class CFManQueryForm
    {

        [Display(Name="房屋地址")]
        public string Address { get; set; }
    }
}