using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XZFCDA.Models
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
    }
}