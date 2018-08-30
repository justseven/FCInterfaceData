using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Model.TongShan
{
    public class OwnersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string ownername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ownercode { get; set; }
        /// <summary>
        /// 市区
        /// </summary>
        public string owneraddress { get; set; }
    }

    public class QueryRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string selarea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientusername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientusercid { get; set; }
        /// <summary>
        /// 徐州市地方税务局及银行
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qzh { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string computerid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string computermac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string computername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OwnersItem> owners { get; set; }
    }

    public class ListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 海边有一套房
        /// </summary>
        public string zl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qlr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sfz { get; set; }
        /// <summary>
        /// 市区
        /// </summary>
        public string hj { get; set; }
    }

    public class ReturnRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> list { get; set; }
    }
}