using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model.ZZSF
{

    public class MessageBody
    {
        public int Count { get; set; }
        public List<JFInfoModel> JFInfoList { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrMsg { get; set; }
        public string Token { get; set; }

        public string UserID { get; set; }
    }

    public class SFDModel
    {
        public string ID { get; set; }
        public string Uuid { get; set; }
        public string Slbh { get; set; }
        public decimal? Money { get; set; }

        public string State { get; set; }
        public string CreateTime { get; set; }

        public string MachineID { get; set; }
    }
    public class JFInfoModel
    {
        public string SLBH { get; set; }
        public string JFBH { get; set; }
        public string JFDW { get; set; }
        public string DH { get; set; }
        public string JFLX { get; set; }
        public decimal YSJE { get; set; }
        public decimal SSJE { get; set; }
        public DateTime SKRQ { get; set; }

    }


}