using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models
{
    public class WBInfo
    {
        public string HouseID             {get;set;}
        public string BuildingID          {get;set;}
        public string ZL                  {get;set;}
        public string QLRMC               {get;set;}
        public string ZJHM                {get;set;}
        public double JZMJ                {get;set;}
        public string ContractNO { get; set; }
        public DateTime SignDate { get; set;  }

    }

    public class WBCLFInfo
    {
        public string HTID { get; set; }
        public string SYQR { get; set; }
        public string BARPASSNO { get; set; }
        public string YWR { get; set; }
        public string YWRNO { get; set; }
        public string CQZH { get; set; }
        public string HID { get; set; }
        public string ZID { get; set; }
        public string CLHTBAH { get; set; }
        public string HTBASJ { get; set; }
        public string FWZL { get; set; }
        public string FWLX { get; set; }
        public string FWJG { get; set; }
        public string HX { get; set; }
        public string JZMJ { get; set; }
        public string TNJZMJ { get; set; }
        public string FTJZMJ { get; set; }
        public string PGJG { get; set; }
        public string JYJG { get; set; }
        public string FKLX { get; set; }
        public string DKFS { get; set; }
        public string FKSJ { get; set; }
        public string QDSJ { get; set; }
        public string QRSJ { get; set; }
        public string QRQZSJ { get; set; }
        public string ZHBGTGSJ { get; set; }
        public string CXSJ { get; set; }
        public string JSSJ { get; set; }
        public string SFYX { get; set; }
    }
    public class WBInfoForAHouse {
        public string HID { get; set; }

        public string XMMC { get; set; }

        public string LPMC { get; set; }

        public string DYH { get; set; }

        public string FJH { get; set; }

        public string JZMJ { get; set; }

        public string FWZT { get; set; }

        public string CFZT { get; set; }

        public string DYZT { get; set; }

        public string ZXZT { get; set; }

        public string MCString { get {
                if (MCList != null && MCList.Count > 0)
                {
                    return string.Join(",", MCList);
                }
                else {
                    return "";
                }
            } }

        public IList<string> MCList { get; set; }

        public string ZJLXString { get {
                if (ZJLXList != null && ZJLXList.Count > 0)
                {
                    return string.Join(",", ZJLXList);
                }
                else
                {
                    return "";
                }
            } }

        public IList<string> ZJLXList { get; set; }
        public string ZJHMString
        {
            get
            {
                if (ZJHMList != null && ZJHMList.Count > 0)
                {
                    return string.Join(",", ZJHMList);
                }
                else
                {
                    return "";
                }
            }
        }

        public IList<string> ZJHMList { get; set; }

        public string LXDHString
        {
            get
            {
                if (LXDHList != null && LXDHList.Count > 0)
                {
                    return string.Join(",", LXDHList);
                }
                else
                {
                    return "";
                }
            }
        }

        public IList<string> LXDHList { get; set; }

        public string LXDZString
        {
            get
            {
                if (LXDZList != null && LXDZList.Count > 0)
                {
                    return string.Join(",", LXDZList);
                }
                else
                {
                    return "";
                }
            }
        }

        public IList<string> LXDZList { get; set; }

        public string ZL { get {
                return this.XMMC + " " + this.LPMC + "#" + this.DYH + "-" + this.FJH;
            } }
    }
}