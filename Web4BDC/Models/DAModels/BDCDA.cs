using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Models.BDCModel;

namespace Web4BDC.Models.DAModels
{
    public class BDCDA
    {
        public ARCH_GLDAXX GLDA { get; set; }
        public List<ARCH_BDCDYDJ> DYDJList { get; set; }
        public List<ARCH_FWYBDJ> YBDJList { get; set; }
        public List<ARCH_BDCCFDJ> CFList { get; set; }

        public List<ARCH_BDCZXDJ> ZXDJ { get; set; }

        public List<WFM_ATTACHLST> AttachList { get; set; }
    }
}