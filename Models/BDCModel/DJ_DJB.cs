namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
   

    
    public partial class DJ_DJB
    {
        [Key]
        [StringLength(36)]
        public string SLBH { get; set; }

        [StringLength(128)]
        public string DJLX { get; set; }

        [StringLength(256)]
        public string DJYY { get; set; }

        [StringLength(16)]
        public string SQZSBS { get; set; }

        [StringLength(16)]
        public string SQFBCZ { get; set; }

        public DateTime? SQRQ { get; set; }

        [StringLength(1024)]
        public string SQNR { get; set; }

        [StringLength(1024)]
        public string SQBZ { get; set; }

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
        public string SPDW { get; set; }

        public DateTime? SPRQ { get; set; }

        [StringLength(1024)]
        public string SPBZ { get; set; }

        [StringLength(1024)]
        public string BDCZH { get; set; }

        [StringLength(36)]
        public string SSJC { get; set; }

        [StringLength(36)]
        public string JGJC { get; set; }

        [StringLength(20)]
        public string FZND { get; set; }

        [StringLength(256)]
        public string ZSH { get; set; }

        [StringLength(64)]
        public string SZR { get; set; }

        [StringLength(64)]
        public string GYFS { get; set; }

        [StringLength(256)]
        public string GYFE { get; set; }

        public DateTime? DJRQ { get; set; }

        [StringLength(64)]
        public string DBR { get; set; }

        [StringLength(64)]
        public string ZSR { get; set; }

        [StringLength(128)]
        public string FZJG { get; set; }

        public DateTime? FZRQ { get; set; }

        [StringLength(32)]
        public string ZSLX { get; set; }

        [StringLength(256)]
        public string ZSXLH { get; set; }

        public decimal? DYCS { get; set; }

        [StringLength(36)]
        public string GDH { get; set; }

        [StringLength(16)]
        public string DAMJ { get; set; }

        [StringLength(1024)]
        public string QT { get; set; }

        [StringLength(1024)]
        public string FJ { get; set; }

        [StringLength(256)]
        public string XGZH { get; set; }

        [StringLength(64)]
        public string BDCDYH { get; set; }

        [StringLength(16)]
        public string HDPCH { get; set; }

        [StringLength(16)]
        public string SZPCH { get; set; }

        [StringLength(36)]
        public string ZXDJDYBH { get; set; }

        public decimal? LIFECYCLE { get; set; }

        public decimal? TRANSNUM { get; set; }

        [StringLength(132)]
        public string SLBH2 { get; set; }

        [StringLength(132)]
        public string DJLX2 { get; set; }

        [StringLength(132)]
        public string BDCZH_BAK { get; set; }

        [StringLength(32)]
        public string MARK { get; set; }

        [StringLength(256)]
        public string GLZH { get; set; }

        [StringLength(128)]
        public string WZ { get; set; }

        [StringLength(32)]
        public string OZSLX { get; set; }

        [StringLength(128)]
        public string TDFCZH { get; set; }
    }
}
