namespace Web4BDC.Models.FCSFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class DJ_SFD
    {
        [Key]
        [StringLength(36)]
        public string SLBH { get; set; }

        [StringLength(36)]
        public string JFBH { get; set; }

        [StringLength(128)]
        public string XMMC { get; set; }

        [StringLength(128)]
        public string JFDW { get; set; }

        [StringLength(256)]
        public string TXDZ { get; set; }

        [StringLength(16)]
        public string DH { get; set; }

        [StringLength(32)]
        public string JFLX { get; set; }

        [StringLength(64)]
        public string JBR { get; set; }

        public DateTime? JBRQ { get; set; }

        [StringLength(1024)]
        public string JBYJ { get; set; }

        [StringLength(64)]
        public string SHR { get; set; }

        [StringLength(1024)]
        public string SHYJ { get; set; }

        public DateTime? SHRQ { get; set; }

        public decimal? YSJE { get; set; }

        public decimal? SSJE { get; set; }

        [StringLength(64)]
        public string SKR { get; set; }

        public DateTime? SKRQ { get; set; }

        [StringLength(1024)]
        public string SKYJ { get; set; }

        [StringLength(64)]
        public string SLR { get; set; }

        [StringLength(1024)]
        public string SFBZ { get; set; }

        [StringLength(16)]
        public string DYZT { get; set; }

        [StringLength(16)]
        public string SFZT { get; set; }

        [StringLength(36)]
        public string HBDH { get; set; }

        [StringLength(64)]
        public string DYR { get; set; }

        public DateTime? DYSJ { get; set; }

        [StringLength(64)]
        public string HBR { get; set; }

        public DateTime? HBSJ { get; set; }

        public decimal? YYS { get; set; }

        public decimal? GRSDS { get; set; }

        public decimal? QS { get; set; }

        public decimal? TDZZS { get; set; }

        public string ZZSFZT { get; set; }
        public string ZZSFZFFS { get; set; }
    }
}
