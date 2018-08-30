using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.TAXInterface
{
    public class SQRInfo
    {
        /// <summary>
        /// 李强
        /// </summary>
        public string XM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZJH { get; set; }
    }

    public class SQRPoInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string XM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZJH { get; set; }
    }

    public class SQRZnInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string XM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZJH { get; set; }
    }

    public class ZMInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string SQBH { get; set; }

        public string HTBAH { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SQRInfo SQRInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SQRPoInfo SQRPoInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SQRZnInfo SQRZnInfo { get; set; }

        public string BZ { get; set; }
    }
}