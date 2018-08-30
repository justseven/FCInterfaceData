using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.BDCModel
{
    public class FC_DA_LOG
    {
        public string ID { get; set; }

        public string SLBH { get; set; }
        public string FMSLBH { get; set; }
        public DateTime EXCE_DATE { get; set; }
        public string EXCE_SQL { get; set; }
        public string RETURNINFO { get; set; }
    }
}