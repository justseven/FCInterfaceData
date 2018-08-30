using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model.ZZSF
{
    public class SF_Submit_Request
    {
        public string UUID { get; set; }
        public string RequestContent { set; get; }
        public DateTime RequestTime { get; set; }
        public string Result { get; set; }
        public int ResFlag { get; set; }

    }
}