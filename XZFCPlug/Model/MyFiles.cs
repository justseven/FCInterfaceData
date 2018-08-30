using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    [Serializable]
    public class MyFiles : IXmlSerializable
    {
        public IList<FileStream> Files { get; set; }

        public MyFiles() { }
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {

        }

        public void WriteXml(XmlWriter writer)
        {
            if (Files != null && Files.Count > 0)
            {
                for (int i = 0; i < Files.Count; i++)
                {
                    writer.WriteStartElement("file");
                    writer.WriteAttributeString("id", (i + 1).ToString());
                    writer.WriteStartElement("name");
                    writer.WriteString(Files[i].Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("size");
                    writer.WriteString(Files[i].Length.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("ext");
                    writer.WriteString(GetExt(Files[i].Name));
                    writer.WriteEndElement();
                    writer.WriteStartElement("bin");
                    byte[] buffer = new byte[Files[i].Length];
                    Files[i].Read(buffer, 0, buffer.Length);
                    Files[i].Seek(0, SeekOrigin.Begin);
                    writer.WriteString(Encoding.UTF8.GetString(buffer));
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
        }

        private string GetExt(string name)
        {
            return name.Substring(name.LastIndexOf("."), name.Length - name.LastIndexOf("."));
        }
    }
}
