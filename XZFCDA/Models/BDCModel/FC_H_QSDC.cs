namespace XZFCDA.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;




    public partial class FC_H_QSDC
    {
        [Key]
        [StringLength(36)]
        public string TSTYBM { get; set; }

        public decimal? BSM { get; set; }

        [StringLength(19)]
        public string ZDTYBM { get; set; }

        [StringLength(4)]
        public string ZH { get; set; }

        [StringLength(4)]
        public string HH { get; set; }

        [StringLength(29)]
        public string BDCDYH { get; set; }

        [StringLength(38)]
        public string LSZTYBM { get; set; }

        [StringLength(24)]
        public string LSFWBH { get; set; }

        [StringLength(32)]
        public string QLLX { get; set; }

        [StringLength(32)]
        public string QLXZ { get; set; }

        [StringLength(3)]
        public string HX { get; set; }

        [StringLength(3)]
        public string HXJG { get; set; }

        [StringLength(3)]
        public string ZXCD { get; set; }

        [StringLength(30)]
        public string GHYT { get; set; }

        [StringLength(100)]
        public string ZL { get; set; }

        public decimal? SJC { get; set; }

        [StringLength(32)]
        public string MYC { get; set; }

        [StringLength(3)]
        public string DYH { get; set; }

        [StringLength(48)]
        public string FJH { get; set; }

        [StringLength(20)]
        public string LJZH { get; set; }

        public decimal? QDJG { get; set; }

        [StringLength(32)]
        public string QDFS { get; set; }

        [StringLength(20)]
        public string SHBW { get; set; }

        public decimal? YCJZMJ { get; set; }

        public decimal? YCTNJZMJ { get; set; }

        public decimal? YCDXBFJZMJ { get; set; }

        public decimal? YCFTJZMJ { get; set; }

        public decimal? YCQTJZMJ { get; set; }

        public decimal? YCFTXS { get; set; }

        public decimal? JZMJ { get; set; }

        public decimal? TNJZMJ { get; set; }

        public decimal? FTJZMJ { get; set; }

        public decimal? DXBFJZMJ { get; set; }

        public decimal? QTJZMJ { get; set; }

        public decimal? FTXS { get; set; }

        public DateTime? TDZZRQ { get; set; }

        [StringLength(32)]
        public string TDYT { get; set; }

        [StringLength(64)]
        public string TDSYQR { get; set; }

        public decimal? GYTDMJ { get; set; }

        public decimal? FTTDMJ { get; set; }

        public decimal? DYTDMJ { get; set; }

        [StringLength(4)]
        public string FWLX { get; set; }

        [StringLength(6)]
        public string FWXZ { get; set; }

        public decimal? SJCS { get; set; }

        public decimal? TCJS { get; set; }

        public decimal? CG { get; set; }

        public decimal? ZT { get; set; }

        [StringLength(50)]
        public string DCZ { get; set; }

        public DateTime? DCSJ { get; set; }

        [StringLength(1200)]
        public string DCYJ { get; set; }

        [StringLength(1200)]
        public string FJSM { get; set; }

        public decimal? SJCLH { get; set; }

        [StringLength(28)]
        public string OLDBDCDYH { get; set; }

        [StringLength(64)]
        public string DYMC { get; set; }

        [StringLength(50)]
        public string TDSYQX { get; set; }

        public DateTime? TDQSRQ { get; set; }

        [StringLength(32)]
        public string TDQLXZ { get; set; }

        public decimal? XCCJMJ { get; set; }

        public decimal? GCCJMJ { get; set; }

        [StringLength(64)]
        public string WZNF { get; set; }

        public decimal? WZJZMJ { get; set; }

        [StringLength(128)]
        public string FJZB { get; set; }

        public decimal? ZBTOP { get; set; }

        public decimal? ZBLEFT { get; set; }

        public decimal? ZBRIGHT { get; set; }

        public decimal? ZBBOTTOM { get; set; }

        [StringLength(32)]
        public string HLX { get; set; }

        [StringLength(2)]
        public string CHZT { get; set; }

        [StringLength(2000)]
        public string CSBSM { get; set; }

        [StringLength(2000)]
        public string XWBSM { get; set; }

        [StringLength(36)]
        public string YHBH { get; set; }

        [StringLength(2)]
        public string SHARESTATE { get; set; }

        [StringLength(128)]
        public string BGJZXX { get; set; }

        [StringLength(128)]
        public string BGTSXX { get; set; }

        public byte[] FCFHT { get; set; }

        public byte[] FCFHCT { get; set; }

        [StringLength(2)]
        public string ISRGHS { get; set; }

        [StringLength(50)]
        public string RGHSRY { get; set; }

        public DateTime? RGHSRQ { get; set; }

        [StringLength(1200)]
        public string CSYJ { get; set; }

        [StringLength(50)]
        public string CSR { get; set; }

        public DateTime? CSRQ { get; set; }

        [StringLength(1200)]
        public string FSYJ { get; set; }

        [StringLength(50)]
        public string FSR { get; set; }

        public DateTime? FSRQ { get; set; }

        [StringLength(1200)]
        public string ZSYJ { get; set; }

        [StringLength(50)]
        public string ZSR { get; set; }

        public DateTime? ZSRQ { get; set; }

        public decimal? LIFECYCLE { get; set; }

        [StringLength(36)]
        public string YHID { get; set; }

        [StringLength(512)]
        public string QTD { get; set; }

        [StringLength(512)]
        public string QTN { get; set; }

        [StringLength(512)]
        public string QTX { get; set; }

        [StringLength(512)]
        public string QTB { get; set; }

        public decimal? TRANSNUM { get; set; }

        [StringLength(100)]
        public string FJBM { get; set; }

        [StringLength(100)]
        public string LACERTNUMFROMFC { get; set; }

        public decimal? COMESFROMCG { get; set; }

        [StringLength(32)]
        public string GZWLX { get; set; }

        [StringLength(36)]
        public string IMPORTNAME { get; set; }

        [StringLength(33)]
        public string BDCDYH_OLD { get; set; }

        [StringLength(32)]
        public string MARK { get; set; }

        [StringLength(36)]
        public string HSCID { get; set; }

        [StringLength(36)]
        public string HYCID { get; set; }

        [StringLength(36)]
        public string ORACLE_FACTMAPPING_ID { get; set; }

        [StringLength(36)]
        public string ORACLE_HOUSEINFO_ID { get; set; }

        [StringLength(36)]
        public string H_HOCODE { get; set; }

        [StringLength(36)]
        public string ORACLE_WB_HOUSEID { get; set; }

        [StringLength(36)]
        public string YGHYT { get; set; }

       // public DateTime? SHARETIME { get; set; }

       // public decimal? HDJ { get; set; }

        //public virtual FC_Z_QSDC FC_Z_QSDC { get; set; }
    }
}
