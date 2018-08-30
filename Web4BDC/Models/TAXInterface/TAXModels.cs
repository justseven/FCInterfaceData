using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.TAXInterface
{
    public class zfxxindex
    {
        public string UID { get; set; }
        public string 申请编号 { get; set; }
        public string 合同备案号 { get; set; }
        public string 买卖方标志 { get; set; }

        public string 包含的区域 { get; set; }
        public string 已处理的区域 { get; set; }
        public string 房屋套数 { get; set; }
        public string 权证号 { get; set; }
    }

    public class proofinfo
    {
        public string 申请编号 { get; set; }
        public string 利用单位 { get; set; }
        public DateTime? 申请时间 { get; set; }
        public DateTime? 经办日期 { get; set; }
        public string 经办人 { get; set; }
        public string 申请人 { get; set; }
        public string 申请人证件类型 { get; set; }
        public string 申请人证件号码 { get; set; }
        public string 业务发起 { get; set; }
        public string 备注 { get; set; }
        public string 流程 { get; set; }
    }

    public class proofperson
    {
        public string 申请编号 { get; set; }
        public string 姓名 { get; set; }
        public string 证件类型 { get; set; }
        public string 证件号码 { get; set; }
        public string 房屋座落 { get; set; }
        public string 户籍所在地 { get; set; }
        public string 数据来源 { get; set; }
    }

    
    public class TAXModels
    {
        public proofinfo proofInfo { get; set; }
        public List<proofperson> personList { get; set; }
        public zfxxindex zfxxList { get; set; }
    }
}