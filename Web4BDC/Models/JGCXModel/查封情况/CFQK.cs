using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class CFQK
    {
        public CFQK()
        {
            CFXXList = new List<CFXX>();
        }
        public string BDCZH { get; set; }
        public string BDCDYH { get; set; }
        public string ZL { get; set; }
        public List<CFXX> CFXXList { get; set; }
    }
}