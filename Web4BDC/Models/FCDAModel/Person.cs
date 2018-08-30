namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Person
    {
        public Guid PersonID { get; set; }

        public Guid ArchiveId { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(50)]
        public string CardNO { get; set; }

        [StringLength(36)]
        public string IDCardType { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(1)]
        public string PersonType { get; set; }

        public Guid? RightMan_ID { get; set; }
    }
}
