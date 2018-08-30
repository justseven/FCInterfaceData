namespace Web4BDC.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Web4BDC.Models.BDCModel;

    public partial class ArchiveIndex
    {
        [Key]
        public Guid ArchiveId { get; set; }

        [StringLength(12)]
        public string SerialNo { get; set; }

        [StringLength(36)]
        public string Operation_ID { get; set; }

        [StringLength(20)]
        public string BusiNO { get; set; }

        [StringLength(10)]
        public string DaCode { get; set; }

        [StringLength(14)]
        public string ArcSite { get; set; }

        [StringLength(36)]
        public string ArchiveType { get; set; }

        [StringLength(36)]
        public string SecrityLevel { get; set; }

        [StringLength(20)]
        public string FilingTime { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(1)]
        public string IsOld { get; set; }

        [StringLength(1)]
        public string IsHistoray { get; set; }

        [StringLength(36)]
        public string process { get; set; }

        public Guid? Prior { get; set; }

        [StringLength(36)]
        public string MainArc { get; set; }

        [StringLength(20)]
        public string ListNumber { get; set; }

        [StringLength(20)]
        public string Source { get; set; }

        [StringLength(20)]
        public string ReqType { get; set; }

        [StringLength(20)]
        public string HousePropertyType { get; set; }

        [StringLength(20)]
        public string ArchiveNo { get; set; }

        [StringLength(20)]
        public string ArchiveDate { get; set; }

        [StringLength(20)]
        public string FmBusiNo { get; set; }

        [StringLength(20)]
        public string PreDaCode { get; set; }

        [StringLength(20)]
        public string DaOrder { get; set; }

        [StringLength(20)]
        public string Region { get; set; }

        [StringLength(20)]
        public string NewRegion { get; set; }

        public string SLBH { get; set; }

        public string DJZL { get; set; }

        public string XGZH { get; set; }

        public List<DJ_XGDJGL> FSLBH_List{ get;set; }
    }
}
