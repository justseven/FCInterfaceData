using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.BDCModel
{
    public class HONEYPOT
    {
        public string ID { get; set; }
        public string SLBH { get; set; }
        public DateTime PUSHDATE { get; set; }
        public string IP { get; set; }
        public string STEPNAME { get; set; }
    }
}