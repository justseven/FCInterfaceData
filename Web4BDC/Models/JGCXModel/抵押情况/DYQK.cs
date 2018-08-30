using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class DYQK
    {
        public string bdczh { get; set; }
        public string bdcdyh { get; set; }
        public string bdclx { get; set; }
        public string zl { get; set; }

        public List<DJDY> dyxx { get; set; }
    }
}