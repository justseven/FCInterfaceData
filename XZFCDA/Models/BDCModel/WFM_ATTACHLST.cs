namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
   

  
    public partial class WFM_ATTACHLST
    {
        [Key]
        [StringLength(36)]
        public string CID { get; set; }

        [StringLength(36)]
        public string PID { get; set; }

        [StringLength(128)]
        public string CNAME { get; set; }

        [StringLength(36)]
        public string PNODE { get; set; }

        [StringLength(16)]
        public string PTYPE { get; set; }

        [StringLength(16)]
        public string CTYPE { get; set; }

        [StringLength(16)]
        public string CKIND { get; set; }

        public decimal? CSORT { get; set; }

        [StringLength(16)]
        public string CISEMPTY { get; set; }

        public DateTime? CREATEDATE { get; set; }

        [StringLength(64)]
        public string CREATEBY { get; set; }

        public decimal? FOLDERNUM { get; set; }

        public decimal? ISUPLOAD { get; set; }

        [StringLength(1024)]
        public string UNACTIDS { get; set; }

        public decimal? REVSTATE { get; set; }

        [StringLength(16)]
        public string FILETYPE { get; set; }

        public decimal? FILENUM { get; set; }

        public decimal? ISINIT { get; set; }

        [StringLength(1024)]
        public string ISNUHIDE { get; set; }

        public decimal? CSHARE { get; set; }
    }
}
