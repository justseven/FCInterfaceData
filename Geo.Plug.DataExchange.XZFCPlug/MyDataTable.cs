using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    [Serializable]
    public class MyDataTable : IXmlSerializable
    {
        public DataTable dt { get; set; }
        public MyDataTable() { }
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string tempColumns = string.Empty;
            try {
                DataTable dt=new DataTable();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "row") {
                            DataRow r = dt.NewRow();
                            while (reader.Read()) {
                                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "row")
                                {
                                    break;
                                }
                                if (reader.NodeType == XmlNodeType.Element) {
                                    if (!dt.Columns.Contains(reader.Name)) {
                                        dt.Columns.Add(reader.Name);
                                    }
                                    tempColumns = reader.Name;
                                    if(reader.Read())
                                        r[tempColumns] = reader.Value;
                                }
                            }
                            dt.Rows.Add(r);
                        }
                    }
                }
                this.dt = dt;
            }
            catch (Exception ex){
                throw new Exception(ex.Message);
            }
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
                        if (clo.DataType == typeof(System.Byte[]) && dt.Rows[i][clo] != DBNull.Value)
                        {
                            byte[] v = (byte[])dt.Rows[i][clo];
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
}
