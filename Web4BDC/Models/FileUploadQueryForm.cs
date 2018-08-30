using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class FileUploadQueryForm
    {
        [Display(Name = "受理编号")]
        public string Slbh { get; set; }

        [Display(Name = "文件名")]
        public string FileName { get; set; }

        public string GetWhere()
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(this.Slbh))
                where += string.Format(" and PNODE = '{0}'", Slbh);
            if (!string.IsNullOrEmpty(this.FileName))
                where += string.Format(" and CNAME = '{0}'", FileName);
            return where;
        }
    }
}