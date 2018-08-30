using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.BDCModel
{
    public class FC_DA_NewLog
    {
        public string UUID { get; set; }
        public string Busino { get; set; }
        public string HID { get ; set; }
        public string Tstybm { get; set; }
        public string Prop { get; set; }
        public string ZL { get; set; }

        public string State { get; set; }
        public string ErrMessage { get; set; }
    }
}