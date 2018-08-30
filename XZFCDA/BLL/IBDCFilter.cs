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
using XZFCDA.Models;

namespace XZFCDA.Bll
{
    public interface IBDCFilter
    {
        BDCFilterResult Filter(PageParams param);
    }
}