using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class BDCFilterResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        /// <summary>
        /// 提示方法，0：alert，1：Confirm，-1:不提示
        /// </summary>
        public int ConfirmType { get; set; }

        public static BDCFilterResult MergeResult(BDCFilterResult r1, BDCFilterResult r2)
        {
            BDCFilterResult retR = new BDCFilterResult { IsSuccess = true };

            //if(r1.IsSuccess && r2.IsSuccess)
            //{
            //    retR.Message = r1.Message + "|" + r2.Message;
            //}

            if (!r1.IsSuccess && r2.IsSuccess)
            {
                retR = r1;

            }
            else
                retR = r2;


            if (!r1.IsSuccess && !r2.IsSuccess)
            {
                retR.Message = r1.Message + "|" + r2.Message;
                retR.ConfirmType = r1.ConfirmType > r2.ConfirmType ? r1.ConfirmType : r2.ConfirmType;
            }
            //if (!r1.IsSuccess || !r2.IsSuccess) {
            //    retR.IsSuccess = false; 
            //    if ((!r1.IsSuccess&& r1.ConfirmType == 0) || (!r2.IsSuccess && r2.ConfirmType == 0))
            //    {
            //        retR.ConfirmType = 0;
            //    }
            //    else
            //    {
            //        retR.ConfirmType = 1;
            //    }
            //}
            //else {
            //    return retR;
            //} 
            //retR.Message = r1.Message + "\n" + r2.Message;
            return retR;
        }
    }
}