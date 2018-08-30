namespace Web4BDC.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;




    public partial class FC_H_QSDC
    {
        /// <summary>
        /// ��ID
        /// </summary>
        [Key]
        [StringLength(36)]
        public string TSTYBM { get; set; }

        public decimal? BSM { get; set; }
        /// <summary>
        /// �ڵش���
        /// </summary>
        [StringLength(19)]
        public string ZDTYBM { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(4)]
        public string ZH { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(4)]
        public string HH { get; set; }
        /// <summary>
        /// ��������Ԫ��
        /// </summary>
        [StringLength(29)]
        public string BDCDYH { get; set; }
        /// <summary>
        /// ��ID
        /// </summary>
        [StringLength(38)]
        public string LSZTYBM { get; set; }
        /// <summary>
        /// ���ݱ���
        /// </summary>
        [StringLength(24)]
        public string LSFWBH { get; set; }
        /// <summary>
        /// Ȩ������
        /// </summary>
        [StringLength(32)]
        public string QLLX { get; set; }
        /// <summary>
        /// Ȩ������
        /// </summary>
        [StringLength(32)]
        public string QLXZ { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(3)]
        public string HX { get; set; }
        /// <summary>
        /// ���ͽṹ
        /// </summary>
        [StringLength(3)]
        public string HXJG { get; set; }
        /// <summary>
        /// ����ʱװ�޳̶�
        /// </summary>
        [StringLength(3)]
        public string ZXCD { get; set; }
        /// <summary>
        /// �滮��;
        /// </summary>
        [StringLength(30)]
        public string GHYT { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(100)]
        public string ZL { get; set; }
        /// <summary>
        /// ʵ�ʲ�
        /// </summary>
        public decimal? SJC { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        [StringLength(32)]
        public string MYC { get; set; }
        /// <summary>
        /// ��Ԫ��
        /// </summary>
        [StringLength(3)]
        public string DYH { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        [StringLength(48)]
        public string FJH { get; set; }
        /// <summary>
        /// �߼�����
        /// </summary>
        [StringLength(20)]
        public string LJZH { get; set; }
        /// <summary>
        /// ȡ�ü۸�
        /// </summary>
        public decimal? QDJG { get; set; }
        /// <summary>
        /// ȡ�÷�ʽ
        /// </summary>
        [StringLength(32)]
        public string QDFS { get; set; }
        /// <summary>
        /// �ҺŲ�λ
        /// </summary>
        [StringLength(20)]
        public string SHBW { get; set; }
        /// <summary>
        /// Ԥ�⽨�����
        /// </summary>
        public decimal? YCJZMJ { get; set; }
        /// <summary>
        /// Ԥ�����ڽ������
        /// </summary>
        public decimal? YCTNJZMJ { get; set; }
        /// <summary>
        /// Ԥ����²��ֽ������
        /// </summary>
        public decimal? YCDXBFJZMJ { get; set; }
        /// <summary>
        /// Ԥ���̯�������
        /// </summary>
        public decimal? YCFTJZMJ { get; set; }
        /// <summary>
        /// Ԥ�������������
        /// </summary>
        public decimal? YCQTJZMJ { get; set; }
        /// <summary>
        /// Ԥ���̯ϵ��
        /// </summary>
        public decimal? YCFTXS { get; set; }
        /// <summary>
        /// ʵ�⽨�����
        /// </summary>
        public decimal? JZMJ { get; set; }
        /// <summary>
        /// ʵ�����ڽ������
        /// </summary>
        public decimal? TNJZMJ { get; set; }
        /// <summary>
        /// ʵ���̯�������
        /// </summary>
        public decimal? FTJZMJ { get; set; }
        /// <summary>
        /// ʵ����²��ֽ������
        /// </summary>
        public decimal? DXBFJZMJ { get; set; }
        /// <summary>
        /// ʵ�������������
        /// </summary>
        public decimal? QTJZMJ { get; set; }
        /// <summary>
        /// ʵ���̯ϵ��
        /// </summary>
        public decimal? FTXS { get; set; }
        /// <summary>
        /// ������ֹ����
        /// </summary>
        public DateTime? TDZZRQ { get; set; }
        /// <summary>
        /// ������;
        /// </summary>
        [StringLength(32)]
        public string TDYT { get; set; }
        /// <summary>
        /// ����ʹ��Ȩ��
        /// </summary>
        [StringLength(64)]
        public string TDSYQR { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        public decimal? GYTDMJ { get; set; }
        /// <summary>
        /// ��̯�������
        /// </summary>
        public decimal? FTTDMJ { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        public decimal? DYTDMJ { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(4)]
        public string FWLX { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(6)]
        public string FWXZ { get; set; }
        /// <summary>
        /// ʵ�ʲ���
        /// </summary>
        public decimal? SJCS { get; set; }
        /// <summary>
        /// ͬ�㷿������
        /// </summary>
        public decimal? TCJS { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        public decimal? CG { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        public decimal? ZT { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [StringLength(50)]
        public string DCZ { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? DCSJ { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [StringLength(1200)]
        public string DCYJ { get; set; }
        /// <summary>
        /// ����˵��
        /// </summary>
        [StringLength(1200)]
        public string FJSM { get; set; }
        /// <summary>
        /// ʵ�ʲ��к�
        /// </summary>
        public decimal? SJCLH { get; set; }
        /// <summary>
        /// ���ǰ��������Ԫ��
        /// </summary>
        [StringLength(28)]
        public string OLDBDCDYH { get; set; }
        /// <summary>
        /// ��Ԫ����
        /// </summary>
        [StringLength(64)]
        public string DYMC { get; set; }
        /// <summary>
        /// ����ʹ������
        /// </summary>
        [StringLength(50)]
        public string TDSYQX { get; set; }
        /// <summary>
        /// ������ʼ����
        /// </summary>
        public DateTime? TDQSRQ { get; set; }
        /// <summary>
        /// ����Ȩ������
        /// </summary>
        [StringLength(32)]
        public string TDQLXZ { get; set; }
        /// <summary>
        /// С��2.2�״��ؼ����
        /// </summary>
        public decimal? XCCJMJ { get; set; }
        /// <summary>
        /// ����2.2�״��ؼ����
        /// </summary>
        public decimal? GCCJMJ { get; set; }
       
       /// <summary>
       /// ԭ�����
       /// </summary>
        [StringLength(36)]
        public string YHBH { get; set; }

       /// <summary>
       ///  �������
       /// </summary>

        [StringLength(100)]
        public string FJBM { get; set; }

        [StringLength(100)]
        public string LACERTNUMFROMFC { get; set; }

        /// <summary>
        /// ��ʵ��ID
        /// </summary>
        [StringLength(36)]
        public string HSCID { get; set; }
        /// <summary>
        ///  ��Ԥ��ID
        /// </summary>
        [StringLength(36)]
        public string HYCID { get; set; }
        
        [StringLength(36)]
        public string ORACLE_HOUSEINFO_ID { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        [StringLength(36)]
        public string ORACLE_WB_HOUSEID { get; set; }
        /// <summary>
        /// ԭ��ܻ�ID
        /// </summary>
        public string OLDCGHOUSEID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string QM { get; set; }
        /// <summary>
        /// ���ش���
        /// </summary>
        public string QXDM { get; set; }

    }
}
