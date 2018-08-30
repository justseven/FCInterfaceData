namespace Web4BDC.Models.BDCModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;




    public partial class FC_H_QSDC
    {
        /// <summary>
        /// 户ID
        /// </summary>
        [Key]
        [StringLength(36)]
        public string TSTYBM { get; set; }

        public decimal? BSM { get; set; }
        /// <summary>
        /// 宗地代码
        /// </summary>
        [StringLength(19)]
        public string ZDTYBM { get; set; }
        /// <summary>
        /// 幢号
        /// </summary>
        [StringLength(4)]
        public string ZH { get; set; }
        /// <summary>
        /// 户号
        /// </summary>
        [StringLength(4)]
        public string HH { get; set; }
        /// <summary>
        /// 不动产单元号
        /// </summary>
        [StringLength(29)]
        public string BDCDYH { get; set; }
        /// <summary>
        /// 幢ID
        /// </summary>
        [StringLength(38)]
        public string LSZTYBM { get; set; }
        /// <summary>
        /// 房屋编码
        /// </summary>
        [StringLength(24)]
        public string LSFWBH { get; set; }
        /// <summary>
        /// 权利类型
        /// </summary>
        [StringLength(32)]
        public string QLLX { get; set; }
        /// <summary>
        /// 权利性质
        /// </summary>
        [StringLength(32)]
        public string QLXZ { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [StringLength(3)]
        public string HX { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        [StringLength(3)]
        public string HXJG { get; set; }
        /// <summary>
        /// 建成时装修程度
        /// </summary>
        [StringLength(3)]
        public string ZXCD { get; set; }
        /// <summary>
        /// 规划用途
        /// </summary>
        [StringLength(30)]
        public string GHYT { get; set; }
        /// <summary>
        /// 坐落
        /// </summary>
        [StringLength(100)]
        public string ZL { get; set; }
        /// <summary>
        /// 实际层
        /// </summary>
        public decimal? SJC { get; set; }
        /// <summary>
        /// 名义层
        /// </summary>
        [StringLength(32)]
        public string MYC { get; set; }
        /// <summary>
        /// 单元号
        /// </summary>
        [StringLength(3)]
        public string DYH { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        [StringLength(48)]
        public string FJH { get; set; }
        /// <summary>
        /// 逻辑幢号
        /// </summary>
        [StringLength(20)]
        public string LJZH { get; set; }
        /// <summary>
        /// 取得价格
        /// </summary>
        public decimal? QDJG { get; set; }
        /// <summary>
        /// 取得方式
        /// </summary>
        [StringLength(32)]
        public string QDFS { get; set; }
        /// <summary>
        /// 室号部位
        /// </summary>
        [StringLength(20)]
        public string SHBW { get; set; }
        /// <summary>
        /// 预测建筑面积
        /// </summary>
        public decimal? YCJZMJ { get; set; }
        /// <summary>
        /// 预测套内建筑面积
        /// </summary>
        public decimal? YCTNJZMJ { get; set; }
        /// <summary>
        /// 预测地下部分建筑面积
        /// </summary>
        public decimal? YCDXBFJZMJ { get; set; }
        /// <summary>
        /// 预测分摊建筑面积
        /// </summary>
        public decimal? YCFTJZMJ { get; set; }
        /// <summary>
        /// 预测其它建筑面积
        /// </summary>
        public decimal? YCQTJZMJ { get; set; }
        /// <summary>
        /// 预测分摊系数
        /// </summary>
        public decimal? YCFTXS { get; set; }
        /// <summary>
        /// 实测建筑面积
        /// </summary>
        public decimal? JZMJ { get; set; }
        /// <summary>
        /// 实测套内建筑面积
        /// </summary>
        public decimal? TNJZMJ { get; set; }
        /// <summary>
        /// 实测分摊建筑面积
        /// </summary>
        public decimal? FTJZMJ { get; set; }
        /// <summary>
        /// 实测地下部分建筑面积
        /// </summary>
        public decimal? DXBFJZMJ { get; set; }
        /// <summary>
        /// 实测其他建筑面积
        /// </summary>
        public decimal? QTJZMJ { get; set; }
        /// <summary>
        /// 实测分摊系数
        /// </summary>
        public decimal? FTXS { get; set; }
        /// <summary>
        /// 土地终止日期
        /// </summary>
        public DateTime? TDZZRQ { get; set; }
        /// <summary>
        /// 土地用途
        /// </summary>
        [StringLength(32)]
        public string TDYT { get; set; }
        /// <summary>
        /// 土地使用权人
        /// </summary>
        [StringLength(64)]
        public string TDSYQR { get; set; }
        /// <summary>
        /// 共有土地面积
        /// </summary>
        public decimal? GYTDMJ { get; set; }
        /// <summary>
        /// 分摊土地面积
        /// </summary>
        public decimal? FTTDMJ { get; set; }
        /// <summary>
        /// 独用土地面积
        /// </summary>
        public decimal? DYTDMJ { get; set; }
        /// <summary>
        /// 房屋类型
        /// </summary>
        [StringLength(4)]
        public string FWLX { get; set; }
        /// <summary>
        /// 房屋性质
        /// </summary>
        [StringLength(6)]
        public string FWXZ { get; set; }
        /// <summary>
        /// 实际层数
        /// </summary>
        public decimal? SJCS { get; set; }
        /// <summary>
        /// 同层房间数量
        /// </summary>
        public decimal? TCJS { get; set; }
        /// <summary>
        /// 层高
        /// </summary>
        public decimal? CG { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public decimal? ZT { get; set; }
        /// <summary>
        /// 调查者
        /// </summary>
        [StringLength(50)]
        public string DCZ { get; set; }
        /// <summary>
        /// 调查日期
        /// </summary>
        public DateTime? DCSJ { get; set; }
        /// <summary>
        /// 调查意见
        /// </summary>
        [StringLength(1200)]
        public string DCYJ { get; set; }
        /// <summary>
        /// 附加说明
        /// </summary>
        [StringLength(1200)]
        public string FJSM { get; set; }
        /// <summary>
        /// 实际层列号
        /// </summary>
        public decimal? SJCLH { get; set; }
        /// <summary>
        /// 变更前不动产单元号
        /// </summary>
        [StringLength(28)]
        public string OLDBDCDYH { get; set; }
        /// <summary>
        /// 单元名称
        /// </summary>
        [StringLength(64)]
        public string DYMC { get; set; }
        /// <summary>
        /// 土地使用期限
        /// </summary>
        [StringLength(50)]
        public string TDSYQX { get; set; }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public DateTime? TDQSRQ { get; set; }
        /// <summary>
        /// 土地权利性质
        /// </summary>
        [StringLength(32)]
        public string TDQLXZ { get; set; }
        /// <summary>
        /// 小于2.2米储藏间面积
        /// </summary>
        public decimal? XCCJMJ { get; set; }
        /// <summary>
        /// 高于2.2米储藏间面积
        /// </summary>
        public decimal? GCCJMJ { get; set; }
       
       /// <summary>
       /// 原户编号
       /// </summary>
        [StringLength(36)]
        public string YHBH { get; set; }

       /// <summary>
       ///  房间编码
       /// </summary>

        [StringLength(100)]
        public string FJBM { get; set; }

        [StringLength(100)]
        public string LACERTNUMFROMFC { get; set; }

        /// <summary>
        /// 户实测ID
        /// </summary>
        [StringLength(36)]
        public string HSCID { get; set; }
        /// <summary>
        ///  户预测ID
        /// </summary>
        [StringLength(36)]
        public string HYCID { get; set; }
        
        [StringLength(36)]
        public string ORACLE_HOUSEINFO_ID { get; set; }
        /// <summary>
        /// 网备户ID
        /// </summary>
        [StringLength(36)]
        public string ORACLE_WB_HOUSEID { get; set; }
        /// <summary>
        /// 原测管户ID
        /// </summary>
        public string OLDCGHOUSEID { get; set; }
        /// <summary>
        /// 区名
        /// </summary>
        public string QM { get; set; }
        /// <summary>
        /// 区县代码
        /// </summary>
        public string QXDM { get; set; }

    }
}
