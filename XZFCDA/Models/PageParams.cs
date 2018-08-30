using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XZFCDA.Models
{
    public class PageParams
    {
        public string WriId { get; set; }//步骤名称

        public string PrjName { get; set; }//流程名称

        public string PrjLimitTime { get; set; }//流程限制时间

        public string UserId { get; set; }//用户ID

        public string UserName { get; set; }//用户名

        public string FtpAddr { get; set; }//FTP地址
        public string FtpPwd { get; set; }//FTP密码

        public string FtpPort { get; set; }//FTP端口

        public string FtpPassive { get; set; }//FTP验证类型

        public string FtpSaveMod { get; set; }//FTP保存模式

        public string PrjId { get; set; }//受理编号

        public string IptUnEncryptStr { get; set; }//解密字符串
        
    }
}