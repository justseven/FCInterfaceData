namespace Web4BDC.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class DOC_BINFILE
    {
        [Key]
        [StringLength(32)]
        public string BINID { get; set; }

        [StringLength(32)]
        public string FILEID { get; set; }

        [StringLength(255)]
        public string FILENAME { get; set; }

        [StringLength(16)]
        public string EXTNAME { get; set; }

        public decimal? FILESIZE { get; set; }

        [StringLength(2)]
        public string ISENCRYPTED { get; set; }

        [StringLength(2)]
        public string ISCOMPRESSED { get; set; }

        [StringLength(128)]
        public string OPENBY { get; set; }

        [StringLength(32)]
        public string OPENPWD { get; set; }

        [StringLength(32)]
        public string COMEFROM { get; set; }

        public byte[] FILECONTENT { get; set; }

        public decimal? SORTNUM { get; set; }

        public decimal? PAGECOUNT { get; set; }

        [StringLength(256)]
        public string FTPATH { get; set; }
    }
}
