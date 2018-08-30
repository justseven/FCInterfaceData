using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class PageParams
    {
        public string WriId { get; set; }

        public string PrjName { get; set; }

        public string PrjLimitTime { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string FtpAddr { get; set; }
        public string FtpPwd { get; set; }

        public string FtpPort { get; set; }

        public string FtpPassive { get; set; }

        public string FtpSaveMod { get; set; }

        public string PrjId { get; set; }

        public string IptUnEncryptStr { get; set; }
        public string StepName { get; set; }

        public Dictionary<string, string> Configs { internal get; set; }


    }
}