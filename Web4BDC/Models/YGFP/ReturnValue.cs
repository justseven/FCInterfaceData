using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.YGFP
{
    public class ReturnValue
    {
        public string result { get; set; }
        public string message { get; set; }

        public List<object> list { get; set; }

        public int totalcount { get; set; }
    }
}