namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class VolEleArc
    {
        [Key]
        public Guid EleArcVol_ID { get; set; }

        public Guid? ArchiveId { get; set; }

        [StringLength(20)]
        public string EleArcName { get; set; }

        public int? PageNumber { get; set; }

        public int? Ordinal { get; set; }

        [StringLength(1)]
        public string flag { get; set; }

        public int? thread { get; set; }

        [StringLength(1)]
        public string IsShow { get; set; }

        //public int? IsOld { get; set; }
    }
}
