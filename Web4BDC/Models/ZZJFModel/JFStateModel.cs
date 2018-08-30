using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Models.ZZJFModel
{
    public class JFStateModel
    {


        public JFStateModel() { }

        public JFStateModel(string ywzh)
        {
            ZZSF_WS.publicpayServiceClient ws = new ZZSF_WS.publicpayServiceClient();
            XMLHelper.XmlHelper xml = new XMLHelper.XmlHelper(ws.GetPayInfo(ywzh));
            IS_SUCCESS = xml.GetXMLNodeText("ROOT/IS_SUCCESS");
            CODE = xml.GetXMLNodeText("ROOT/IS_SUCCESS", "CODE");
            ERRMSG= xml.GetXMLNodeText("ROOT/ERRMSG");
            
            SLBH= xml.GetXMLNodeText("ROOT/RESPONSE/RESULT/DETAIL/SLBH");
            ZFSJ = xml.GetXMLNodeText("ROOT/RESPONSE/RESULT/DETAIL/ZFSJ");
            ZFFS=xml.GetXMLNodeText("ROOT/RESPONSE/RESULT/DETAIL/ZFFS");
            string je = xml.GetXMLNodeText("ROOT/RESPONSE/RESULT/DETAIL/ZFJE");
            if(string.IsNullOrEmpty(je))
            {
                ZFJE = -1;
            }
            else
            {
                ZFJE = Convert.ToDecimal(je);
            }
        }
        public string IS_SUCCESS { get; set; }
        public string CODE { get; set; }
         public string ERRMSG { get; set; }
        public string SLBH { get; set; }
        public string ZFSJ { get; set; }
        public string ZFFS { get; set; }
        public decimal? ZFJE { get; set; }

    }
}