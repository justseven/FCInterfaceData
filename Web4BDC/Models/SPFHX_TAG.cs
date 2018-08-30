using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class SPFHX_TAG
    {
        public string SLBH { get; set; }
        public string DJLX { get; set; }
        public int SFTS { get; set; }
        public string PUSHDATA { get; set; }
        public DateTime PUSHDATE { get; set; }
        public string WBERRINFO { get; set; }
        public string CGERRINFO { get; set; }
        public string PUSHRESULT { get; set; }
        public string ADDRESSTYPE { get; set; }
    }
}