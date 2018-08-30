using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Models;

namespace Web4BDC.Bll.CheckRegist
{
    public class CheckRegistBLL
    {
        internal static BDCFilterResult CheckState(PageParams param)
        {
            BDCFilterResult res = new BDCFilterResult();
            BDCExtendDal dal = new BDCExtendDal();
            DataTable hs = dal.GetHousesBySLBH(param.PrjId);
            if (hs == null || hs.Rows.Count == 0)
            {
                return new BDCFilterResult { IsSuccess = true };
            }
            string resStr = string.Empty;
            if (hs.Rows.Count > 0)
            {
                foreach (DataRow h in hs.Rows)
                {
                    string tmp = How2Check(h["TSTYBM"].ToString(), param.PrjId);
                    if(!string.IsNullOrEmpty(tmp))
                    {
                        resStr += h["ZL"].ToString()+"已存在"+tmp + "业务";
                    }
                }
            }
            if(string.IsNullOrEmpty(resStr))
            {
                res.IsSuccess = true;
            }
            else
            {
                res.IsSuccess = false;
                res.Message = resStr;
                //res.ConfirmType = -1;
            }
            return res;
        }

        private static string How2Check(string h, string slbh)
        {//是否做过登记，如果做个登记，就在不动产中检查，否则在网备中检查
            FCWebServiceDal dal = new FCWebServiceDal();
            DataTable dt= dal.GetRegistState(h, slbh);
            string res = string.Empty;
            if(null!=dt &&dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (string.IsNullOrEmpty(res))
                    {
                        res = row[0].ToString();
                    }
                    else
                    {
                        res+="、"+ row[0].ToString();
                    }
                }
            }
            return res;
        }
    }
}