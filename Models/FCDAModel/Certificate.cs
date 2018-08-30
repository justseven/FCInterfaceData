namespace XZFCDA.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Certificate
    {
        public Guid CertificateID { get; set; }

        public Guid? HouseInfo_ID { get; set; }

        [StringLength(20)]
        public string Prop { get; set; }

        [StringLength(20)]
        public string PrintNO { get; set; }

        [StringLength(2)]
        public string CertificateType { get; set; }

        [StringLength(20)]
        public string GrantDate { get; set; }

        public Guid? PersonID { get; set; }

        public Guid? ArchiveId { get; set; }
    }
}
