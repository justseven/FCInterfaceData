using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DBCForFCWebService.Model.Tax
{
    public class HoInfoResponse
    {
        public string Msg { get; set; }
        public DataSet DataSource { get; set; }
        public string ToXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }
    }
}