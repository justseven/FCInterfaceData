using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class DJCF
    {
        private string slbh;
        public string SLBH
        {
            get
            {
                return string.IsNullOrEmpty(slbh) ? "/" : slbh;
            }
            set { slbh = value; }
        }

        private string cfjg;
        public string CFJG {
            get
            {
                return string.IsNullOrEmpty(cfjg) ? "/" : cfjg;
            }
            set { cfjg = value; }
        }
        private string cfwh;
        public string CFWH
        {
            get
            {
                return string.IsNullOrEmpty(cfwh) ? "/" : cfwh;
            }
            set { cfwh = value; }
        }

        private string cflx;
        public string CFLX
        {
            get
            {
                return string.IsNullOrEmpty(cflx) ? "/" : cflx;
            }
            set { cflx = value; }
        }

        private string cffw;
        public string CFFW {
            get
            {
                return string.IsNullOrEmpty(cffw) ? "/" : cffw;
            }
            set { cffw = value; }
        }

        private string cfwj;
        public string CFWJ {
            get
            {
                return string.IsNullOrEmpty(cfwj) ? "/" : cfwj;
            }
            set { cfwj = value; }
        }

        private string cfqx;
        public string CFQX
        {
            get
            {
                return string.IsNullOrEmpty(cfqx) ? "/" : cfqx;
            }
            set { cfqx = value; }
        }

        public decimal CFSX { get; set; } 
        public DateTime? CFRQ { get; set;  }
        public DateTime? CFQSSJ { get; set; }
        public DateTime? CFJSSJ { get; set; }

        public string CFRQStr { get { return CFRQ == null ? "/" : Convert.ToDateTime(CFRQ).ToShortDateString(); } }
        public string CFQSSJStr { get { return CFQSSJ == null ? "/" : Convert.ToDateTime(CFQSSJ).ToShortDateString(); } }
        public string CFJSSJStr { get { return CFJSSJ == null ? "/" : Convert.ToDateTime(CFJSSJ).ToShortDateString(); } }
        public string CFSXStr { get { return CFSX > 0 ? CFSX.ToString() : "/"; } }
    }
}