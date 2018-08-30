namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
   

    public partial class DJ_DY
    {
        [Key]
        [StringLength(36)]
        public string SLBH { get; set; }

        [StringLength(128)]
        public string DJLX { get; set; }

        [StringLength(256)]
        public string DJYY { get; set; }

        [StringLength(256)]
        public string XGZH { get; set; }

        public DateTime? SQRQ { get; set; }

        [StringLength(512)]
        public string SQDJYJ { get; set; }

        [StringLength(512)]
        public string SQDJNR { get; set; }

        [StringLength(128)]
        public string DLJGMC { get; set; }

        [StringLength(64)]
        public string DLRXM { get; set; }

        [StringLength(2)]
        public string DLRZJLX { get; set; }

        [StringLength(36)]
        public string DLRZJH { get; set; }

        [StringLength(36)]
        public string DLRZGZH { get; set; }

        [StringLength(36)]
        public string DLRDH { get; set; }

        [StringLength(128)]
        public string DLJGMC2 { get; set; }

        [StringLength(64)]
        public string DLRXM2 { get; set; }

        [StringLength(2)]
        public string DLRZJLX2 { get; set; }

        [StringLength(36)]
        public string DLRZJH2 { get; set; }

        [StringLength(36)]
        public string DLRZGZH2 { get; set; }

        [StringLength(36)]
        public string DLRDH2 { get; set; }

        [StringLength(64)]
        public string DYLX { get; set; }

        public decimal? DYSW { get; set; }

        [StringLength(64)]
        public string ZWR { get; set; }

        [StringLength(2)]
        public string ZWRZJLX { get; set; }

        [StringLength(36)]
        public string ZWRZJH { get; set; }

        [StringLength(2)]
        public string DYFS { get; set; }

        public decimal? DYMJ { get; set; }

        public decimal? DYMJ2 { get; set; }

        [StringLength(256)]
        public string ZJJZWZL { get; set; }

        [StringLength(1024)]
        public string ZJJZWDYFW { get; set; }

        public decimal? BDBZZQSE { get; set; }

        public decimal? PGJE { get; set; }

        [StringLength(256)]
        public string DBFW { get; set; }

        [StringLength(512)]
        public string ZGZQQDSS { get; set; }

        public decimal? ZGZQSE { get; set; }

        [StringLength(64)]
        public string DYQX { get; set; }

        public DateTime? QLQSSJ { get; set; }

        public DateTime? QLJSSJ { get; set; }

        [StringLength(64)]
        public string SPDW { get; set; }

        [StringLength(512)]
        public string SPBZ { get; set; }

        [StringLength(64)]
        public string BDCZMH { get; set; }

        [StringLength(36)]
        public string SSJC { get; set; }

        [StringLength(36)]
        public string JGJC { get; set; }

        [StringLength(20)]
        public string FZND { get; set; }

        [StringLength(8)]
        public string ZSH { get; set; }

        [StringLength(64)]
        public string SZR { get; set; }

        public DateTime? DJRQ { get; set; }

        [StringLength(64)]
        public string DBR { get; set; }

        [StringLength(64)]
        public string ZSR { get; set; }

        [StringLength(128)]
        public string FZJG { get; set; }

        public DateTime? FZRQ { get; set; }

        [StringLength(36)]
        public string ZSXLH { get; set; }

        public decimal? DYCS { get; set; }

        [StringLength(50)]
        public string GDH { get; set; }

        [StringLength(16)]
        public string DAMJ { get; set; }

        [StringLength(1024)]
        public string QT { get; set; }

        [StringLength(1024)]
        public string FJ { get; set; }

        [StringLength(36)]
        public string BDCDYH { get; set; }

        [StringLength(16)]
        public string HDPCH { get; set; }

        [StringLength(16)]
        public string SZPCH { get; set; }

        public decimal? DYBDCDY { get; set; }

        public decimal? LIFECYCLE { get; set; }

        public decimal? TRANSNUM { get; set; }

        [StringLength(132)]
        public string SLBH2 { get; set; }

        [StringLength(132)]
        public string DJLX2 { get; set; }

        [StringLength(132)]
        public string BDCZMH_BAK { get; set; }

        [StringLength(32)]
        public string MARK { get; set; }

        [StringLength(128)]
        public string WZ { get; set; }

        [StringLength(36)]
        public string YGDJZL { get; set; }
    }
}
