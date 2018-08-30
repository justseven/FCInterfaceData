using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.FCDAModel
{
    public class BDCDALog
    {
        public string ID { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public string ip { get; set; }
        public DateTime RequestTime { get; set; }
    }
}