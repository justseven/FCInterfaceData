using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Bll.FCSF;
using Web4BDC.Models;

namespace Web4BDC.Bll
{
    public class CheckStateFilter : IBDCFilter
    {
        public BDCFilterResult Filter(PageParams param)
        {
            return FCSFBLL.CheckSFState(param);
        }

      

       
    }
   
}