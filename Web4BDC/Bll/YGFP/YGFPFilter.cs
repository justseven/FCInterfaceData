using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Models;

namespace Web4BDC.Bll.YGFP
{
    public class YGFPFilter
    {
        public BDCFilterResult Filter(PageParams param)
        {
            return YGFPBLL.PushData(param);
        }
    }
}