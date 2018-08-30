using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class CFXX
    {
        public CFXX()
        {
            CF = new DJCF();
            JF = new DJJF();
        }
        public DJCF CF { get; set; }
        public DJJF JF { get; set; }
    }
}