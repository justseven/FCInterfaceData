using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class WriteBackResultQueryForm
    {
        [Display(Name="受理编号")]
        public string Slbh { get; set; }

        [Display(Name="是否推送成功")]
        public string IsPushSuccess { get; set; }

        public string GetWhere()
        {
            string where = " 1=1 ";
            if(!string.IsNullOrEmpty(this.Slbh))
             where += string.Format(" and slbh like '{0}%'",Slbh);
            if(!string.IsNullOrEmpty(this.IsPushSuccess))
                where += string.Format(" and SFTS like {0}", IsPushSuccess);
            return where;
        }
    }
}