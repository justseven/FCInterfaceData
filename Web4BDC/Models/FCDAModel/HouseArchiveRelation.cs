namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class HouseArchiveRelation
    {
        [Key]
        public Guid RelationID { get; set; }

        public Guid? ArchiveId { get; set; }

        public Guid? HouseInfo_ID { get; set; }

        public int? OrderNO { get; set; }

        [StringLength(20)]
        public string BusiNO { get; set; }
    }
}
