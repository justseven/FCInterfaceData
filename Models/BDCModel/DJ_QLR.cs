namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
   

    
    public partial class DJ_QLR
    {
        [Key]
        [StringLength(36)]
        public string QLRID { get; set; }

        [StringLength(256)]
        public string QLRMC { get; set; }

        public decimal? SXH { get; set; }

        [StringLength(2)]
        public string ZJLB { get; set; }

        [StringLength(256)]
        public string ZJHM { get; set; }

        [StringLength(128)]
        public string FZJG { get; set; }

        [StringLength(2)]
        public string SSHY { get; set; }

        [StringLength(4)]
        public string GJ { get; set; }

        [StringLength(8)]
        public string HJSZSS { get; set; }

        [StringLength(2)]
        public string XB { get; set; }

        public byte[] ZP { get; set; }

        [StringLength(32)]
        public string DH { get; set; }

        [StringLength(256)]
        public string DZ { get; set; }

        [StringLength(16)]
        public string YB { get; set; }

        [StringLength(128)]
        public string GZDW { get; set; }

        [StringLength(64)]
        public string DZYJ { get; set; }

        [StringLength(4)]
        public string QLRXZ { get; set; }

        [StringLength(16)]
        public string QLRZT { get; set; }

        [StringLength(16)]
        public string XYZT { get; set; }

        [StringLength(36)]
        public string FQLR { get; set; }

        [StringLength(16)]
        public string GX { get; set; }

        [StringLength(16)]
        public string JTRK { get; set; }

        [StringLength(36)]
        public string NSRSBH { get; set; }

        [StringLength(64)]
        public string FRDBXM { get; set; }

        [StringLength(2)]
        public string FRDBZJLX { get; set; }

        [StringLength(128)]
        public string FRDBZJH { get; set; }

        [StringLength(36)]
        public string FRDBDHHM { get; set; }

        [StringLength(128)]
        public string SJZGBM { get; set; }

        [StringLength(256)]
        public string TXDZ { get; set; }

        public byte[] ZW1 { get; set; }

        public byte[] ZWTZM1 { get; set; }

        public byte[] ZW2 { get; set; }

        public byte[] ZWTZM2 { get; set; }

        public byte[] ZW3 { get; set; }

        public byte[] ZWTZM3 { get; set; }

        public decimal? TRANSNUM { get; set; }

        public decimal? XYDJ { get; set; }
    }
}
