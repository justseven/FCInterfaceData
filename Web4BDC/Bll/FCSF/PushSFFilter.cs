/****************************************************************************************
 *                              2017.7.17
 *                                 by seven
 * 
 * 
 * 
 * 
 * 
 * 
 * ***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Bll;
using Web4BDC.Bll.FCSF;
using Web4BDC.Models;




namespace Web4BDC.Bll
{
    public class PushSFFilter : IBDCFilter
    {
        public BDCFilterResult Filter(PageParams param)
        {
            return FCSFBLL.PushSF(param);
        }

      

       
    }


}