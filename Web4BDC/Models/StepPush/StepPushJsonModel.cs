using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.StepPush
{
    public class StepPushJsonModel
    {
        public string ID { get; set; }
        public string XZQDM { get; set; }
        public string WDBS { get; set; }
        public string YWBH { get; set; }
        public string JDMC { get; set; }
        public string BLRY { get; set; }

        public string SQRXM { get; set; }
        public string SQRLXFS { get; set; }

        public int IsSuccess { get; set; }
        public string ErrorMsg { get; set; }

        public string SendStr { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}