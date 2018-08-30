using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.YGFP
{
    public class YGFPModel
    {
        /// <summary>
        /// SaveImmovableProperty
        /// </summary>
        public string Function { get { return "SaveImmovableProperty"; } }
        /// <summary>
        /// SU045979
        /// </summary>
        public string UserID { get { return "SU045979"; }  }
        /// <summary>
        /// 不动产单元号
        /// </summary>
        public string ANum { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 房屋面积
        /// </summary>
        public decimal? HouseArea { get; set; }
        /// <summary>
        /// 房屋坐落
        /// </summary>
        public string HousePosition { get; set; }
        /// <summary>
        /// 登簿时间
        /// </summary>
        public DateTime BuyDate { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string HouseMoney { get; set; }
        /// <summary>
        /// 数字签名
        /// </summary>
        public string MD5 { get; set; }
    }
}