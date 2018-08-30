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
using Web4BDC.Models;




namespace Web4BDC.Bll
{
    public class PushDAFilter : IBDCFilter
    {
        public BDCFilterResult Filter(PageParams param)
        {
            return Web4BDC.Bll.FCDA_BLL.Insert_FCDA(param);
        }

      

       
    }


}