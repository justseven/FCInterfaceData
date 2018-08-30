namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
   
    public partial class PropArchiveRelation
    {
        [Key]
        public Guid RelationID { get; set; }

        public Guid ArchiveId { get; set; }

        public Guid CertificateID { get; set; }
    }
}
