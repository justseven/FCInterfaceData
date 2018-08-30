using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model.ZZDZ
{
    public class ZZDYLOG
    {
        public string ID { get; set; }
        public string IntefaceName { get; set; }

        public string keyValues { get; set; }

        public string ReturnState { get; set; }
        public string RevurnVale { get; set; }
        public DateTime CallTime { get; set; }
    }
}