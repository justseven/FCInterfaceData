using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class QSQK
    {
        public QSXX Qsxx{get;set;}

        public List<DJDY> DYXX { get; set; }
        public List<DJCF> CFXX { get; set; }

    }
}