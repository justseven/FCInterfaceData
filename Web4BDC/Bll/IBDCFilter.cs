using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Models;

namespace Web4BDC.Bll
{
    public interface IBDCFilter
    {
        BDCFilterResult Filter(PageParams param);
    }
}