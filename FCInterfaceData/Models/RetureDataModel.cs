using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FCInterfaceData.Models
{
    [Serializable]
    public class BDC2
    {
        public BDC2() { }
        protected BDC2(SerializationInfo info, StreamingContext context)
        { 
            //未实现
        }
        public Head head { get; set; }
        public MyDataTable data { get; set; }

        public MyFiles attach { get; set; }
    }
    [Serializable]
    public class Head
    {
        public int flag { get; set; }
        public string msg { get; set; }
    }
    /// <summary>
    /// 用于自定义datatable序列化
    /// </summary>
    [Serializable]
    public class MyDataTable : IXmlSerializable
    { 
        public DataTable dt { get; set; }
        public MyDataTable() { }
        public XmlSchema GetSchema()
        {
            return null ;
        }

        public void ReadXml(XmlReader reader)
        { 
            
        }

        public void WriteXml(XmlWriter writer)
        {
            if (dt != null && dt.Rows.Count > 0)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    writer.WriteStartElement("row");
                    writer.WriteAttributeString("id", (i + 1).ToString());
                    foreach (DataColumn clo in dt.Columns)
                    { 
                        writer.WriteStartElement(clo.ColumnName.ToUpper());
                        if (clo.DataType == typeof(System.Byte[])&&dt.Rows[i][clo] != DBNull.Value) { 
                            byte[] v=(byte[])dt.Rows[i][clo];
                            writer.WriteBinHex(v, 0, v.Length);
                        }
                        else
                        writer.WriteString(dt.Rows[i][clo] == DBNull.Value ? " " : dt.Rows[i][clo].ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
        }
    }


    [Serializable]
    public class MyFiles : IXmlSerializable
    {
        public IList<FileStream> Files { get; set; }

        public MyFiles() { }
        public XmlSchema GetSchema()
        {
            return null ;
        }

        public void ReadXml(XmlReader reader)
        { 
            
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Files != null && Files.Count>0)
            { 
            for (int i = 0; i < Files.Count; i++)
            {
                writer.WriteStartElement("file");
                writer.WriteAttributeString("id", (i+1).ToString()); 
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