using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Models;

namespace Web4BDC.Bll.CheckRegist
{
    public class CheckRegistFilter : IBDCFilter
    {
        public BDCFilterResult Filter(PageParams param)
        {
            return CheckRegistBLL.CheckState(param);
        }
    }
}