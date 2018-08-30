using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Dal.MKevaluate;

namespace Web4BDC.Bll.MKevaluate
{
    public class MKevaluateBLL
    {
        public string GetSQRLXFS(string yWBH)
        {
            MKevaluateDAL dal = new MKevaluateDAL();
            return dal.GetSQRLXFS(yWBH);
        }

        public string GetSQRXM(string yWBH)
        {
            MKevaluateDAL dal = new MKevaluateDAL();
            return dal.GetSQRXM(yWBH);
        }
    }
}