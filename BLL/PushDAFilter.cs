using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XZFCDA.Bll;




namespace Web4BDC.Bll
{
    public class PushDAFilter : IBDCFilter
    {
        XZFCDA.Models.BDCFilterResult IBDCFilter.Filter(XZFCDA.Models.PageParams param)
        {
            return XZFCDA.Bll.FCDA_BLL.Insert_FCDA(param);
        }

      

       
    }


}