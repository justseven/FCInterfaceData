namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class VolEleArcDtl
    {
        [Key]
        public Guid VolEleArcDtl_id { get; set; }

        public Guid? VolEleArc_ID { get; set; }

        [StringLength(500)]
        public string imgName { get; set; }

        
        public byte[] img { get; set; }

        [StringLength(50)]
        public string ScanDate { get; set; }

        public int? PageNo { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}
