namespace Web4BDC.Models.FCSFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

   
    public partial class DJ_SFD_FB
    {
        [Key]
        [StringLength(36)]
        public string CWSFDXBBH { get; set; }

        [StringLength(36)]
        public string SLBH { get; set; }

        public decimal? QDXH { get; set; }

        [StringLength(128)]
        public string SFXM { get; set; }

        [StringLength(128)]
        public string JLDW { get; set; }

        public decimal? SL { get; set; }

        public decimal? SFBZ { get; set; }

        public decimal? HSJE { get; set; }

        [StringLength(256)]
        public string BZ { get; set; }

        public decimal? JMJE { get; set; }

        [StringLength(512)]
        public string JMYY { get; set; }
    }
}
