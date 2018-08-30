using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.JGCXModel
{
    public class QSXX
    {
        private string djlx;
        public string DJLX { get { return string.IsNullOrEmpty(djlx) ? "/" : djlx; } set { djlx = value; } }
        private string dah;
        public string DAH { get { return string.IsNullOrEmpty(dah) ? "/" : dah; } set { dah = value; } }

        private string bdczh;
        public string BDCZH { get { return string.IsNullOrEmpty(bdczh) ? "/" : bdczh; } set { bdczh = value; } }
        public DateTime? DJRQ { get; set; }
        public string DJRQStr { get { return null == DJRQ ? "/":Convert.ToDateTime(DJRQ).ToShortDateString() ; } }

        private string qszt;
        public string QSZT { get { return string.IsNullOrEmpty(qszt) ? "/" : qszt; } set { qszt = value; } }
        private string syqr;
        public string SYQR { get { return string.IsNullOrEmpty(syqr) ? "/" : syqr; } set { syqr = value; } }

        private string zjhm;
        public string ZJHM { get { return string.IsNullOrEmpty(zjhm) ? "/" : zjhm; } set { zjhm = value; } }
        private string zjlx;
        public string ZJLX { get { return string.IsNullOrEmpty(zjlx) ? "/" : zjlx; } set { zjlx = value; } }
        private string bdcdyh;
        public string BDCDYH { get { return string.IsNullOrEmpty(bdcdyh) ? "/" : bdcdyh; } set { bdcdyh = value; } }

        private string gyqk;
        public string GYQK { get { return string.IsNullOrEmpty(gyqk) ? "/" : gyqk; } set { gyqk = value; } }
        public decimal TDSYMJ { get; set; }
        public string TDSYMJStr { get { return TDSYMJ > 0 ? TDSYMJ.ToString() : "/"; } }

        private string tdsyqr;
        public string TDSYQR { get { return string.IsNullOrEmpty(tdsyqr) ? "/" : tdsyqr; } set { tdsyqr = value; } }
        private string tdqllx;
        public string TDQLLX { get { return string.IsNullOrEmpty(tdqllx) ? "/" : tdqllx; } set { tdqllx = value; } }
        private string tdyt;
        public string TDYT { get { return string.IsNullOrEmpty(tdyt) ? "/" : tdyt; } set { tdyt = value; } }
        private string tdqlxz;
        public string TDQLXZ { get { return string.IsNullOrEmpty(tdqlxz) ? "/" : tdqlxz; } set { tdqlxz = value; } }
        private string tdsyqx;
        public string TDSYQX { get { return string.IsNullOrEmpty(tdsyqx) ? "/" : tdsyqx; } set { tdsyqx = value; } }
        private string fwyt;
        public string FWYT { get { return string.IsNullOrEmpty(fwyt) ? "/" : fwyt; } set { fwyt = value; } }

        private string fwxz;
        public string FWXZ { get { return string.IsNullOrEmpty(fwxz) ? "/" : fwxz; } set { fwxz = value; } }
        private string fwjg;
        public string FWJG { get { return string.IsNullOrEmpty(fwjg) ? "/" : fwjg; } set { fwjg = value; } }

        public string SZC { get; set; }
        public Nullable<decimal> ZCS { get; set; }
        public decimal JZMJ { get; set; }
        public string JZMJStr { get { return JZMJ > 0 ? JZMJ.ToString() : "/"; } }

        public decimal TNMJ { get; set; }
        public string TNMJStr { get { return TNMJ > 0 ? TNMJ.ToString() : "/"; } }
        public decimal FTJZMJ { get; set; }
        public string FTJZMJStr { get { return FTJZMJ > 0 ? FTJZMJ.ToString() : "/"; } }
        public DateTime? JGRQ { get; set; }
        public string JGRQStr { get { return null != JGRQ ? Convert.ToDateTime(JGRQ).ToShortDateString() : "/"; } }

        private string zl;
        public string ZL { get { return string.IsNullOrEmpty(zl) ? "/" : zl; } set { zl = value; } }
    }
}