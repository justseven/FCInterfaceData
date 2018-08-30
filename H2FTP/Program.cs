using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace H2FTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Insert();
        }


        private static void InsertFCFHT(string fileName, string ftpAdd, string ftpUser, string ftpPassword, string ftpPort, byte[] buffer, string hid, string ZID)
        {
            string ftpHead;
            if (string.IsNullOrEmpty(ftpPort))
                ftpHead = "ftp://" + ftpAdd;
            else
            {
                ftpHead = "ftp://" + ftpAdd + ":" + ftpPort;
            }
            FtpCheckDirectoryExist(ftpHead, "FC_H/" + ZID + "/", ftpUser, ftpPassword);
            if (fileCheckExist(ftpHead + "/FC_H/" + ZID + "/", fileName, ftpUser, ftpPassword))
            {
                return;
            }
            string ftpAddree = ftpHead + "/FC_H/" + ZID + "/" + fileName;
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpAddree));
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = buffer.Length;
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPassword);

            //FtpWebResponse response = (FtpWebResponse)reqFTP.GetRequestStream();
            Stream ftpStream = reqFTP.GetRequestStream();
            ftpStream.Write(buffer, 0, buffer.Length);
            ftpStream.Close();
            //response.Close();
            string insertGGKSql = string.Format(@"Insert into pub_attachlst(attachid, fileid,parenttype,parentnode,attachname,attachtype,uploadby,uploadtime,sortnum)
                                                                             values(:attachid,:fileid,'户','{0}','分层分户图','jpg','权集',sysdate,0)", hid);

#if DEBUG
            string connectionstring = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
            DbConnection connection = new OleDbConnection(connectionstring);
#else 
                    XConnNode node = XConnNode.CreateNode();
                    string connectionstring = node.ConnString["公共数据"];
                    DbProviderFactory provider = DbProviderFactories.GetFactory(node.ProviderName["公共数据"]);
                    DbConnection connection = provider.CreateConnection();
                    connection.ConnectionString = connectionstring;
#endif
            DbCommand command = connection.CreateCommand();
            DbParameter attachidP = command.CreateParameter();
            attachidP.ParameterName = ":attachid";
            attachidP.Value = Guid.NewGuid().ToString("N");
            DbParameter fileidP = command.CreateParameter();
            fileidP.ParameterName = ":fileid";
            string fileid = Guid.NewGuid().ToString("N");
            fileidP.Value = fileid;
            command.Parameters.Add(attachidP);
            command.Parameters.Add(fileidP);
            command.Connection = connection;
            int ggint = DBHelper.ExecuteNonQuery(insertGGKSql, command);
            if (ggint > 0)
            {
                string insertWDKSql = string.Format(@"Insert into DOC_BINFILE(binid,fileid,filename,extname,filesize,isencrypted,iscompressed,ftpath)
                                                                        values(:binid,'{0}','分层分户图','jpg','{1}','否','否','{2}')", fileid, buffer.Length.ToString(), "/FC_H/" + ZID + "/" + fileName);

#if DEBUG
                string connectionwdstring = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ToString();
                DbConnection wdconnection = new OleDbConnection(connectionwdstring);
#else 
                    XConnNode node1 = XConnNode.CreateNode();
                    string connectionwdstring = node1.ConnString["文档数据"];
                    DbProviderFactory provider1 = DbProviderFactories.GetFactory(node.ProviderName["文档数据"]);
                    DbConnection wdconnection = provider1.CreateConnection();
                    wdconnection.ConnectionString = connectionwdstring;
#endif
                DbCommand wdcommand = wdconnection.CreateCommand();
                DbParameter binidp = wdcommand.CreateParameter();
                binidp.ParameterName = ":binid";
                binidp.Value = Guid.NewGuid().ToString("N");
                wdcommand.Parameters.Add(binidp);
                wdcommand.Connection = wdconnection;
                DBHelper.ExecuteNonQuery(insertWDKSql, wdcommand);
                string insertWDKSql2 = string.Format(@"Insert into DOC_FILE(fileid,filename,creator,createtime)
                                                                values('{0}','分层分户图','权集',sysdate)", fileid);
                DbCommand wdcommand2 = wdconnection.CreateCommand();
                wdcommand.Connection = wdconnection;
                DBHelper.ExecuteNonQuery(insertWDKSql2, wdcommand2);
            }
        }


        /// <summary>
        /// 文件存在检查
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public static bool fileCheckExist(string url, string ftpName, string ftpUser, string ftpPassword)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return success;
        }

        public static void FtpCheckDirectoryExist(string ftphead, string destFilePath, string ftpUserID, string ftpPassword)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空    
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        FtpMakeDir(ftphead, curDir, ftpUserID, ftpPassword);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public static string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        //创建目录  
        public static Boolean FtpMakeDir(string ftphead, string curDir, string ftpUserID, string ftpPassword)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftphead + curDir);
            req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }
        private static DataTable GetTable(int index)
        {
            string sql = string.Format("select a1.* from (select TSTYBM,LSZTYBM,FCFHT,rownum rn from FC_H_QSDC where rownum <={0} AND ZL LIKE '%青泉%') a1 where rn >={1}", index + 100, index);
            return DBHelper.GetDataTable(sql);
        }

        private static int GetCount()
        {
            string sql = string.Format("Select Count(1) from FC_H_QSDC  where ZL LIKE '%青泉%'");
            return Convert.ToInt32(DBHelper.GetScalar(sql));
        }

        private static void Insert()
        {
            string ftpAdd = ConfigurationManager.AppSettings["FtpAddr"].ToString();
            string ftpUser = ConfigurationManager.AppSettings["FtpUser"].ToString();
            string ftpPwd = ConfigurationManager.AppSettings["FtpPwd"].ToString();
            string ftpPort = ConfigurationManager.AppSettings["FtpPort"].ToString();
            int count = GetCount();
            if (count > 0)
            {
                int i = 0;
                while (i <= count)
                {
                    DataTable dt = GetTable(i);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Rows[j]["FCFHT"] != null && dt.Rows[j]["FCFHT"] != DBNull.Value)
                            {
                                Byte[] filebytes = (byte[])(dt.Rows[j]["FCFHT"]);
                                InsertFCFHT(dt.Rows[j]["TSTYBM"].ToString() + ".jpg", ftpAdd, ftpUser, ftpPwd, ftpPort, filebytes, dt.Rows[j]["TSTYBM"].ToString(), dt.Rows[j]["LSZTYBM"].ToString());

                            }
                        }
                    }
                    i = i + 100;
                }
            }
        }
    }
}
