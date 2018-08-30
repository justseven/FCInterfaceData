namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class DOC_FILE
    {
        [Key]
        [StringLength(32)]
        public string FILEID { get; set; }

        [Required]
        [StringLength(255)]
        public string FILENAME { get; set; }

        [StringLength(32)]
        public string DOCTYPE { get; set; }

        [StringLength(32)]
        public string FILECODE { get; set; }

        [StringLength(64)]
        public string SUBJECT { get; set; }

        [StringLength(128)]
        public string KEYWORDS { get; set; }

        [StringLength(512)]
        public string DIGIST { get; set; }

        [StringLength(16)]
        public string FILETYPE { get; set; }

        [StringLength(16)]
        public string FILESTATE { get; set; }

        [StringLength(32)]
        public string CREATOR { get; set; }

        public DateTime? CREATETIME { get; set; }

        [StringLength(512)]
        public string NOTE { get; set; }

        public decimal? SORTNUM { get; set; }
    }
}
