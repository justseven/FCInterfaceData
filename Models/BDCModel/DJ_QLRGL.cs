namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class DJ_QLRGL
    {
        [Key]
        [StringLength(36)]
        public string GLBM { get; set; }

        [StringLength(36)]
        public string SLBH { get; set; }

        [StringLength(36)]
        public string YWBM { get; set; }

        [Required]
        [StringLength(36)]
        public string QLRID { get; set; }

        [StringLength(64)]
        public string GYFS { get; set; }

        [StringLength(36)]
        public string GYFE { get; set; }

        [StringLength(64)]
        public string CZFS { get; set; }

        [StringLength(16)]
        public string CZBH { get; set; }

        [StringLength(36)]
        public string ZSXLH { get; set; }

        public decimal? SXH { get; set; }

        [StringLength(32)]
        public string QLRLX { get; set; }

        public decimal? LIFECYCLE { get; set; }

        [StringLength(256)]
        public string QLRMC { get; set; }

        public decimal? TRANSNUM { get; set; }

        //[StringLength(132)]
        //public string SLBH2 { get; set; }

        [StringLength(16)]
        public string GX { get; set; }

        [StringLength(64)]
        public string BDCZH { get; set; }
    }
}
