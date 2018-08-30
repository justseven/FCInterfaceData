using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.Models.BDCModel;

namespace Web4BDC.Models.JGCXModel
{
    public class DJDY
    {
        private string bdczmh;
        public string BDCZMH
        {
            get
            {
                return string.IsNullOrEmpty(bdczmh) ? "/" : bdczmh;
            }
            set { bdczmh = value; }
        }

        private string bdcdyh;


        public string BDCDYH
        {
            get
            {
                return string.IsNullOrEmpty(bdcdyh) ? "/" : bdcdyh;
            }
            set { bdcdyh = value; }
        }
        private string bdclx;
        public string BDCLX
        {
            get
            {
                return string.IsNullOrEmpty(bdclx) ? "/" : bdclx;
            }
            set { bdclx = value; }
        }

        private string slbh;
        public string SLBH
        {
            get
            {
                return string.IsNullOrEmpty(slbh) ? "/" : slbh;
            }
            set { slbh = value; }
        }

        private string djlx;
        public string DJLX
        {
            get
            {
                return string.IsNullOrEmpty(djlx) ? "/" : djlx;
            }
            set { djlx = value; }
        }
        private string dyfs;
        public string DYFS {
            get
            {
                return string.IsNullOrEmpty(dyfs) ? "/" : dyfs;
            }
            set { dyfs = value; }
        }
        public string DYFWStr { get { return DYFW > 0 ? DYFW.ToString() : "/"; } }
        public decimal DYFW { get; set; }
        public string DYJEStr { get { return DYJE > 0 ? DYJE.ToString() : "/"; } }
        public decimal DYJE { get; set; }

        public DateTime? DYQX { get; set; }
        public string DYQXStr { get { return null!= DYQX ? Convert.ToDateTime(DYQX).ToShortDateString() : "/"; } }

        public DateTime? DYRQ { get; set; }
        public string DYRQStr { get { return null != DYRQ ? Convert.ToDateTime(DYRQ).ToShortDateString() : "/"; } }
        

        private string dyqr;
        public string DYQR {
            get
            {
                return string.IsNullOrEmpty(dyqr) ? "/" : dyqr;
            }
            set { dyqr = value; }
        }
        private string dyr;
        public string DYR
        {
            get
            {
                return string.IsNullOrEmpty(dyr) ? "/" : dyr;
            }
            set { dyr = value; }
        }
    }
}