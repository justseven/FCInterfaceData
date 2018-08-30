namespace XZFCDA.FC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class HouseInfo
    {
        [Key]
        public Guid HouseInfo_ID { get; set; }

        [StringLength(200)]
        public string I_ItSite { get; set; }

        [StringLength(50)]
        public string I_ItName { get; set; }

        [StringLength(50)]
        public string BuNum { get; set; }

        [StringLength(100)]
        public string BuName { get; set; }

        [StringLength(2)]
        public string H_CeCode { get; set; }

        [StringLength(50)]
        public string H_RoNum { get; set; }

        [StringLength(50)]
        public string H_CurLay { get; set; }

        [StringLength(50)]
        public string H_HoStru { get; set; }

        [StringLength(50)]
        public string H_HoUse { get; set; }

        public decimal? H_ConAcre { get; set; }

        [StringLength(20)]
        public string BuFinishYear { get; set; }

        [StringLength(38)]
        public string CHID { get; set; }

        [StringLength(20)]
        public string BusiNo { get; set; }

        [StringLength(500)]
        public string HoSite { get; set; }
        [StringLength(500)]
        public string BDCDYH { get; set; }
    }
}
