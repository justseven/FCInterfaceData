using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Web4BDC.Tools
{
    public class FTPHelper
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public  void UploadFile(FileInfo fileinfo, string targetDir, FTP ftpSrv,bool DeleteSource)
        {
            //1. check target
            string target;
            if (targetDir.Trim() == "")
            {
                return;
            }
            target = Guid.NewGuid().ToString();  //使用临时文件名


            string URI ="ftp://" + ftpSrv.hostname+":"+ftpSrv.port + "/" + targetDir + "/" + target;
            ///WebClient webcl = new WebClient();
            System.Net.FtpWebRequest ftp = GetRequest(URI, ftpSrv.username, ftpSrv.password);

            //设置FTP命令 设置所要执行的FTP命令，
            //ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;//假设此处为显示指定路径下的文件列表
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            //告诉ftp文件大小
            ftp.ContentLength = fileinfo.Length;
            //缓冲大小设置为2KB
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize - 1 + 1];
            int dataRead;

            //打开一个文件流 (System.IO.FileStream) 去读上传的文件
            using (FileStream fs = fileinfo.OpenRead())
            {
                try
                {
                    //把上传的文件写入流
                    using (Stream rs = ftp.GetRequestStream())
                    {
                        do
                        {
                            //每次读文件流的2KB
                            dataRead = fs.Read(content, 0, BufferSize);
                            rs.Write(content, 0, dataRead);
                        } while (!(dataRead < BufferSize));
                        rs.Close();
                    }

                }
                catch (Exception ex) { }
                finally
                {
                    fs.Close();
                }

            }

            ftp = null;
            //设置FTP命令
            ftp = GetRequest(URI, ftpSrv.username, ftpSrv.password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.Rename; //改名
            ftp.RenameTo = fileinfo.Name;
            try
            {
                ftp.GetResponse();
            }
            catch (Exception ex)
            {
                ftp = GetRequest(URI, ftpSrv.username, ftpSrv.password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile; //删除
                ftp.GetResponse();
                throw ex;
            }
            finally
            {
                //fileinfo.Delete();
            }

            // 可以记录一个日志  "上传" + fileinfo.FullName + "上传到" + "FTP://" + hostname + "/" + targetDir + "/" + fileinfo.Name + "成功." );
            ftp = null;
            if(DeleteSource)
            {
                fileinfo.Delete();
            }
            #region
            /*****
             *FtpWebResponse
             * ****/
            //FtpWebResponse ftpWebResponse = (FtpWebResponse)ftp.GetResponse();
            #endregion
        }

        /// <summary>
        /// ftp方式上传 
        /// </summary>
        public  string UploadFtp(string filePath, string filename,string FtpDir, FTP ftpSvr)
        {
            string path = "";
            
            if(filePath.Equals(""))
            {
                path= filename;
            }
            else
            {
                path=filePath + "\\" + filename;
            }

            FileInfo fileInf = new FileInfo(path);
            string fileName=FtpDir + "/" + fileInf.Name;
            string uri = "ftp://" + ftpSvr.hostname + "/" + fileName;
            FtpWebRequest reqFTP;
            // Create FtpWebRequest object from the Uri provided 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            try
            {
                // Provide the WebPermission Credintials 
                reqFTP.Credentials = new NetworkCredential(ftpSvr.username, ftpSvr.password);

                // By default KeepAlive is true, where the control connection is not closed 
                // after a command is executed. 
                reqFTP.KeepAlive = false;

                // Specify the command to be executed. 
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type. 
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file 
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
                //FileStream fs = fileInf.OpenRead(); 
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Stream to which the file to be upload is written 
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time 
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends 
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream 
                strm.Close();
                fs.Close();
                return fileName;
            }
            catch (Exception ex)
            {
                reqFTP.Abort();
                //  Logging.WriteError(ex.Message + ex.StackTrace);
                return "";
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localDir">下载至本地路径</param>
        /// <param name="FtpDir">ftp目标文件路径</param>
        /// <param name="FtpFile">从ftp要下载的文件名</param>
        /// <param name="hostname">ftp地址即IP</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public  string DownloadFile(string localDir, string FtpDir, string FtpFile, FTP ftpserver)
        {
            string URI = "";
            if (FtpDir.Equals(""))
            {
                URI = "FTP://" + ftpserver.hostname + "/" + FtpFile;
            }
            else
            {
                URI = "FTP://" + ftpserver.hostname + "/" + FtpDir + "/" + FtpFile;
            }
            string tmpname = Guid.NewGuid().ToString();
            string localfile = localDir + @"\" + tmpname;

            System.Net.FtpWebRequest ftp = GetRequest(URI, ftpserver.username, ftpserver.password);
            ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
            ftp.UseBinary = true;
            ftp.UsePassive = false;

            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    //loop to read & write to file
                    using (FileStream fs = new FileStream(localfile, FileMode.CreateNew))
                    {
                        try
                        {
                            byte[] buffer = new byte[2048];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        catch (Exception)
                        {
                            //catch error and delete file only partially downloaded
                            fs.Close();
                            //delete target file as it's incomplete
                            File.Delete(localfile);
                            throw;
                        }
                    }

                    responseStream.Close();
                }

                response.Close();
            }



            try
            {
                File.Delete(localDir + @"\" + FtpFile);
                File.Move(localfile, localDir + @"\" + FtpFile);


                ftp = null;
                ftp = GetRequest(URI, ftpserver.username, ftpserver.password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
                ftp.GetResponse();

            }
            catch (Exception ex)
            {
                File.Delete(localfile);
                throw ex;
            }

            // 记录日志 "从" + URI.ToString() + "下载到" + localDir + @"\" + FtpFile + "成功." );
            ftp = null;
            return localDir + @"\" + FtpFile;
        }

        /// <summary>
        /// ftp方式下载 
        /// </summary>
        public  string DownloadFtp(string filePath, string ftpdir,string fileName, FTP ftpSvr)
        {
            FtpWebRequest reqFTP;
            try
            {
                //filePath = < <The full path where the file is to be created.>>, 
                //fileName = < <Name of the file to be created(Need not be the name of the file on FTP server).>> 
               


                string uri = "";
                if(ftpdir.Equals(""))
                {
                    uri = "ftp://" + ftpSvr.hostname + ":" + ftpSvr.port + "/" + fileName;
                }
                else
                {
                    uri = "ftp://" + ftpSvr.hostname + ":" + ftpSvr.port + "/" + ftpdir + "/" + fileName;
                }
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpSvr.username, ftpSvr.password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(filePath, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return filePath ;
            }
            catch (Exception ex)
            {
                // Logging.WriteError(ex.Message + ex.StackTrace);
                // System.Windows.Forms.MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 搜索远程文件
        /// </summary>
        /// <param name="targetDir"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="SearchPattern"></param>
        /// <returns></returns>
        public  List<string> ListDirectory(string targetDir, FTP ftpSrv, string SearchPattern)
        {
            List<string> result = new List<string>();
            try
            {
                string URI ="ftp://" + ftpSrv.hostname+":"+ftpSrv.port + "/" + targetDir + "/" + SearchPattern;

                System.Net.FtpWebRequest ftp = GetRequest(URI, ftpSrv.username, ftpSrv.password);
                ftp.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
                ftp.UsePassive = true;
                ftp.UseBinary = true;


                string str = GetStringResponse(ftp);
                str = str.Replace("\r\n", "\r").TrimEnd('\r');
                str = str.Replace("\n", "\r");
                if (str != string.Empty)
                    result.AddRange(str.Split('\r'));

                return result;
            }
            catch { }
            return null;
        }

        private  string GetStringResponse(FtpWebRequest ftp)
        {
            //Get the result, streaming to a string
            string result = "";
            using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
            {
                long size = response.ContentLength;
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(datastream, System.Text.Encoding.Default))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    datastream.Close();
                }

                response.Close();
            }

            return result;
        }

        /// 在ftp服务器上创建目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public  void MakeDir(string dirName, FTP ftpSrv)
        {
            try
            {
                string uri ="ftp://" + ftpSrv.hostname+":"+ftpSrv.port + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, ftpSrv.username, ftpSrv.password);
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        /// <param name="ftpHostIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void delDir(string dirName, FTP ftpSrv)
        {
            try
            {
                string uri ="ftp://" + ftpSrv.hostname+":"+ftpSrv.port + "/" + dirName;
                System.Net.FtpWebRequest ftp = GetRequest(uri, ftpSrv.username, ftpSrv.password);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="currentFilename">当前目录名称</param>
        /// <param name="newFilename">重命名目录名称</param>
        /// <param name="ftpServerIP">ftp地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public void Rename(string currentFilename, string newFilename, FTP ftpSrv)
        {
            try
            {

                FileInfo fileInf = new FileInfo(currentFilename);
                string uri ="ftp://" + ftpSrv.hostname+":"+ftpSrv.port + ":" + ftpSrv.port + "/" + fileInf.Name;
                System.Net.FtpWebRequest ftp = GetRequest(uri, ftpSrv.username, ftpSrv.password);
                ftp.Method = WebRequestMethods.Ftp.Rename;

                ftp.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="URI"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public  bool DirectoryExist(string dirName, FTP ftpSrv)
        {
            //hwh 20170616 修复iis设置为framework4.0的时候，ftp目录不存在时返回还是true的问题。
            string URI = "ftp://" + ftpSrv.hostname+":"+ftpSrv.port + "/" + dirName;
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(URI));
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpSrv.username, ftpSrv.password);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            try
            {
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string line = reader.ReadLine();


                if (line == null)
                {
                    string e = "目录不存在";
                    return false;
                }
                reader.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
        }

        private  FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
            //提供身份验证信息
            if(username!="")
            result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }
    }


    public class FTP
    {
        public string hostname{get;set;}
        public string username { get; set; }
        public string password { get; set; }
        public string port { get; set; }
    }
}
