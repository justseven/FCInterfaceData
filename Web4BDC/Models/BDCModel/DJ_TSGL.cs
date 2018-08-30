namespace Web4BDC.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
   

    
    public partial class DJ_TSGL
    {
        [Key]
        [StringLength(36)]
        public string GLBM { get; set; }

        [StringLength(36)]
        public string SLBH { get; set; }

        [StringLength(36)]
        public string BDCLX { get; set; }

        [StringLength(36)]
        public string TSTYBM { get; set; }

        [StringLength(36)]
        public string BDCDYH { get; set; }

        [StringLength(36)]
        public string DJZL { get; set; }

        [StringLength(32)]
        public string GLMS { get; set; }

        public DateTime? CSSJ { get; set; }

        public decimal? LIFECYCLE { get; set; }

        public decimal? TRANSNUM { get; set; }

      
    }
}
