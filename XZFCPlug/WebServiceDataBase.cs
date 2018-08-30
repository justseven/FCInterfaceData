//#if !DEBUG
//using Geo.Core;
//#endif
using Geo.Plug.DataExchange.XZFCPlug.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;  

namespace Geo.Plug.DataExchange.XZFCPlug
{

    public abstract class WebServiceDataBase : IGetDataBase
    {
        private string _WebServiceAddress;
        private string _TabelName;//要插入临时表的表名
        protected string _excuteCode;
        public WebServiceDataBase(string add,string table,string excuteCode) {
            _WebServiceAddress = add;
            _TabelName = table;
            _excuteCode = excuteCode;
        } 
        /// <summary>
        /// 获取webService的数据
        /// </summary>
        /// <returns></returns>
        public virtual string GetWebServiceData(IDictionary<string, string> ps, IList<string> paramNeeded)
        {
            string WebServiceAdd = DataExchange.Webservice_Address;
            string sParams = ParamPrepare(ps, paramNeeded);
            Uri uri = new Uri(WebServiceAdd);
            WebRequest webreq = WebRequest.Create(uri);
            //string postData = "sParams="+HttpUtility.HtmlEncode(sParams);
            byte[] bs = Encoding.UTF8.GetBytes(sParams);  
            webreq.ContentType = "text/xml; charset=utf-8";
            webreq.Headers.Add("SOAPAction", "/" + _WebServiceAddress);
            webreq.ContentLength = bs.Length;
            webreq.Timeout = webreq.Timeout*10;
            webreq.Method = "POST";
            using (Stream reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            Stream s = webreq.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.UTF8);
            string all = sr.ReadToEnd();
            return all; 
        }
        /// <summary>
        /// 请求webservice的地址准备,用于Post
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="paramNeeded"></param>
        /// <returns></returns>
        //protected virtual string ParamPrepare(IDictionary<string, string> ps, IList<string> paramNeeded) {
        //    if (paramNeeded != null && paramNeeded.Count > 0) {
        //        if (paramNeeded.Any(p => !ps.ContainsKey(p)||string.IsNullOrWhiteSpace(ps[p]))) {
        //            throw new ArgumentNullException();//抛出参数异常
        //        }
        //    }
        //    string raw= string.Join(",", ps.Select(kv => kv.Key + "=" + kv.Value));
        //    return System.Web.HttpUtility.UrlEncode(raw);
        //}

        protected virtual string ParamPrepare(IDictionary<string, string> ps, IList<string> paramNeeded) 
        { 
             try{ 
                if (paramNeeded != null && paramNeeded.Count > 0) {
                    if (paramNeeded.Any(p => !ps.ContainsKey(p)||string.IsNullOrWhiteSpace(ps[p]))) {
                        throw new ArgumentNullException();//抛出参数异常
                    }
                }
                string raw= string.Join(",", ps.Select(kv => kv.Key + "=" + kv.Value));
                string param = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                          <soap:Body>
                                            <{0}>
                                              <sParams>{1}</sParams> 
                                            </{0}>
                                          </soap:Body>
                                        </soap:Envelope>", _WebServiceAddress, raw);
                return param;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 解析返回来的XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public virtual BDC XMLParsing(string xml)
        {
            //xml = File.ReadAllText(@"C:\Users\Administrator\Desktop\1.xml");
            xml= xml.Replace("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>", "");
            xml = xml.Replace("</soap:Body></soap:Envelope>", "");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlElement root = xmlDoc.DocumentElement;
            root.Attributes.RemoveAll(); 
            XmlNode bodyNode = xmlDoc.ChildNodes[1].FirstChild;
            xml = string.Format("<BDC>{0}</BDC>",bodyNode.InnerXml);
            using(StringReader sr=new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BDC));
                BDC bdc = (BDC)serializer.Deserialize(sr);
                return bdc;
            }
        }

        protected virtual string PushData2DB(BDC bdc, DbConnection conn, string pch)
        {
            if (bdc.head.flag == 0)
            { //取数据失败
                //if (conn.State == ConnectionState.Open) conn.Close();
                return string.Empty;
            }
            else
            {
                if (bdc.data != null && bdc.data.dt != null && bdc.data.dt.Rows.Count > 0)
                {
                    //string insertSql = InsertSql(pch, bdc.data.dt, _TabelName);
                    //IList<DbCommand> commands = new List<DbCommand>();
                    //if (conn == null || conn.State == ConnectionState.Closed)
                    //{
                    //    conn = DBHelper.Connection;
                    //}
                    DbTransaction Tran = conn.BeginTransaction();
                    DbCommand command = conn.CreateCommand();
                    command.Transaction = Tran;
                    try
                    {
                        
                        for (int i = 0; i < bdc.data.dt.Rows.Count; i++)
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.Clear();
                            CreateParameters(command, bdc.data.dt, i);
                            if (string.IsNullOrEmpty(command.CommandText))
                                command.CommandText = InsertSql(pch, command.Parameters, _TabelName);
                            command.ExecuteNonQuery();
                        }
                        Tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        Tran.Rollback();
                        throw new Exception(ex.Message + "在从webservice中取出数据再插入临时表时出错。pch：" + pch);
#if DEBUG 
#else
                        ILog log = new ErrorLog(typeof(WebServiceDataBase));
                log.WriteLog(ex);
                throw new Exception(ex.Message+"在从webservice中取出数据再插入临时表时出错。pch："+pch);
#endif
                    }
                    finally
                    {
                        //conn.Close();
                    }
                    return pch;
                }
                else
                {
                    //if (conn.State == ConnectionState.Open) conn.Close();
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        protected abstract void CreateParameters(DbCommand command, DataTable data,int index);
        
        public virtual string Data2DB(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            string webServiceData = GetWebServiceData(ps, paramNeeded);//获取webservice数据
            BDC bdc = XMLParsing(webServiceData);
            return PushData2DB(bdc, connection, pch);
        }
        /// <summary>
        /// 插入数据库，并返回id
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="paramNeeded"></param>
        /// <param name="connection"></param>
        /// <param name="pch"></param>
        /// <param name="idIdentified"></param>
        /// <returns></returns>
        public virtual IDictionary<string, string> Data2DBAndReturnId(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            string webServiceData = GetWebServiceData(ps, paramNeeded);//获取webservice数据
            BDC bdc = XMLParsing(webServiceData);
            PushData2DB(bdc, connection, pch);
            return GetReturnId(bdc);
        }
        /// <summary>
        /// 返回需要继续查询的ID，变态
        /// </summary>
        /// <param name="bdc"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetReturnId(BDC bdc){
            return null;
        }

        /// <summary>
        /// 插入的sql
        /// </summary>
        /// <param name="pch"></param>
        /// <returns></returns>
        protected virtual string InsertSql(string pch, DataTable data, string tableName)
        {
            StringBuilder sbCmdText = new StringBuilder();
            if (data.Columns.Count > 0)
            {
                IList<string> dbColumns = new List<string>();
                foreach (DataColumn dc in data.Columns)
                {
                    dbColumns.Add(dc.ColumnName);
                }
                sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
                sbCmdText.Append(string.Join(",", dbColumns));
                sbCmdText.Append(",PCH) VALUES (");
                sbCmdText.Append(":" + string.Join(",:", dbColumns));
                sbCmdText.AppendFormat(",'{0}')", pch);
            }
            return sbCmdText.ToString();
        }

        protected virtual string InsertSql(string pch, DbParameterCollection parameters, string tableName)
        {
            StringBuilder sbCmdText = new StringBuilder();
            if (parameters.Count > 0)
            {
                IList<string> dbColumns = new List<string>();
                foreach (DbParameter dc in parameters)
                {
                    dbColumns.Add(dc.ParameterName);
                }
                sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
                sbCmdText.Append(string.Join(",", dbColumns));
                sbCmdText.Append(",PCH) VALUES (");
                sbCmdText.Append(":" + string.Join(",:", dbColumns));
                sbCmdText.AppendFormat(",'{0}')", pch);
            }
            return sbCmdText.ToString();
        }
        
    }
}
 