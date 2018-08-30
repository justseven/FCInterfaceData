using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    [Serializable]
    public class BDC //: IXmlSerializable
    {
        public BDC() { }
        public XmlSchema GetSchema()
        {
            return null;
        }
        [XmlElement("head")]
        public Head head { get; set; }
        public MyDataTable data { get; set; }

        public MyFiles attach { get; set; }
        public void ReadXml(XmlReader reader)
        { }
        public void WriteXml(XmlWriter writer) {
            //writer.WriteStartDocument();
            //writer.WriteEndDocument();
            writer.WriteStartElement("bdc");
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(Head),null,null,new XmlRootAttribute("head"),"");
            serializer.Serialize(writer, head,null);
            serializer = new XmlSerializer(typeof(MyDataTable));
            serializer.Serialize(writer, data);
            writer.WriteEndElement();
        }
    }
}
