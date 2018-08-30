/****************************************************************************************
 *                              2017.7.17
 *                                 by seven
 * 
 * 
 * 
 * 
 * 
 * 
 * ***************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models;
using Web4BDC.Tools;
using Web4BDC.Dal;
using Web4BDC.FC.Models;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace Web4BDC.Bll
{
    public class ImportBLL
    {
        static string UseFtp = ConfigurationManager.AppSettings["FtpPassive"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsCL"></param>
        /// <returns></returns>
        public static BDCFilterResult ImportFromFC(bool IsCL)
        {
            int fileCount = 0;
            bool IsSuccess = false;
            BDCFilterResult res = new BDCFilterResult();

            DataTable dt = null;
            if (IsCL)
            {
                dt = ImportBLL.GetCLSLBH();
            }
            else
            {
                dt = ImportDAL.GetPushedSLBH();
            }
            if (null != dt && dt.Rows.Count > 0)
            {
                FC_REWRITE_TAG tag = new FC_REWRITE_TAG();
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row[0].ToString();
                    string fcslbh = "";
                    if (CanImport(slbh))
                    {

                        try
                        {
                            List<DOC_BINFILE> doc_bin = GetDoc_Binfile(slbh, fcslbh);

                            if (UseFtp.Equals("是") || UseFtp.ToLower().Equals("true"))
                            {
                                if (null != doc_bin && doc_bin.Count > 0)
                                {
                                    fileCount = doc_bin.Count;
                                    foreach (DOC_BINFILE item in doc_bin)
                                    {
                                        tag.FILENAME += tag.FILENAME != null ? item.FILENAME : ("," + item.FILENAME);
                                        //using (TransactionScope ts = new TransactionScope())
                                        //{
                                        UploadToFtp(slbh, item);
                                        item.FTPATH = ChangeFTPPath(item.FTPATH);
                                        ImportDAL.InsertDoc_binfile(item);

                                        //ts.Complete();
                                        //}
                                    }
                                    tag.FILECOUNT = fileCount.ToString();
                                    //tag.FILENAME =doc_bin[0].BINID
                                    tag.ID = Guid.NewGuid().ToString();
                                    tag.SLBH = slbh;
                                    tag.PUSHDATE = DateTime.Now;
                                    tag.ISSUCCESS = "1";
                                    tag.MESSAGE = "成功";
                                    IsSuccess = true;
                                    ImportDAL.InsertLog(tag);
                                }
                                else
                                {
                                    tag.FILECOUNT = fileCount.ToString();
                                    //tag.FILENAME =doc_bin[0].BINID
                                    tag.ID = Guid.NewGuid().ToString();
                                    tag.SLBH = slbh;
                                    tag.PUSHDATE = DateTime.Now;
                                    tag.ISSUCCESS = "0";
                                    tag.MESSAGE = "失败！不动产库中无此受理编号记录。";
                                    //IsSuccess = true;
                                    ImportDAL.InsertLog(tag);
                                }

                            }
                            else
                            {
                                DOC_FILE doc_file = GetDoc_file(slbh);
                                ImportDAL.InsertDoc_File(doc_file);
                            }
                        }
                        catch (Exception ex)
                        {
                            tag.FILECOUNT = fileCount.ToString();
                            //tag.FILENAME =doc_bin[0].BINID
                            tag.ID = Guid.NewGuid().ToString();
                            tag.SLBH = slbh;
                            tag.PUSHDATE = DateTime.Now;
                            tag.ISSUCCESS = "0";
                            tag.MESSAGE = "失败!" + ex.Message;
                            ImportDAL.InsertLog(tag);
                            continue;
                        }


                    }


                }
                res.ConfirmType = 0;
                res.IsSuccess = IsSuccess;
                res.Message = tag.MESSAGE;

            }
            return res;
        }


        public static BDCFilterResult ImportFromFC(string slbh)
        {
            int fileCount = 0;
            bool IsSuccess = false;
            BDCFilterResult res = new BDCFilterResult();


            FC_REWRITE_TAG tag = new FC_REWRITE_TAG();
            string FCslbh = GetRealSLBH(slbh);
            //string slbh = row[0].ToString();
            if (CanImport(FCslbh))
            {

                try
                {
                    List<DOC_BINFILE> doc_bin = GetDoc_Binfile(slbh, FCslbh);

                    if (UseFtp.Equals("是") || UseFtp.ToLower().Equals("true"))
                    {
                        if (null != doc_bin && doc_bin.Count > 0)
                        {
                            fileCount = doc_bin.Count;
                            //using (TransactionScope ts = new TransactionScope())
                            //{
                                foreach (DOC_BINFILE item in doc_bin)
                                {
                                    

                                    try
                                    {
                                        tag.FILENAME += tag.FILENAME != null ? item.FILENAME : ("," + item.FILENAME);
                                        
                                        UploadToFtp(FCslbh, item);

                                        //item.FTPATH = ChangeFTPPath(item.FTPATH);
                                        if (ImportDAL.ExistDoc_binfile(item))
                                            ImportDAL.UpDateDoc_binfile(item);
                                        else
                                            ImportDAL.InsertDoc_binfile(item);
                                    }
                                    catch
                                    {
                                        continue;
                                    }



                                }
                                //ts.Complete();
                            //}
                            tag.FILECOUNT = fileCount.ToString();
                            //tag.FILENAME =doc_bin[0].BINID
                            tag.ID = Guid.NewGuid().ToString();
                            tag.SLBH = slbh;
                            tag.PUSHDATE = DateTime.Now;
                            tag.ISSUCCESS = "1";
                            tag.MESSAGE = "成功";
                            IsSuccess = true;
                            ImportDAL.InsertLog(tag);
                        }
                        else
                        {
                            tag.FILECOUNT = fileCount.ToString();
                            //tag.FILENAME =doc_bin[0].BINID
                            tag.ID = Guid.NewGuid().ToString();
                            tag.SLBH = slbh;
                            tag.PUSHDATE = DateTime.Now;
                            tag.ISSUCCESS = "0";
                            tag.MESSAGE = "失败！不动产库中无此受理编号记录。";
                            //IsSuccess = true;
                            ImportDAL.InsertLog(tag);
                        }

                    }
                    
                }
                catch (Exception ex)
                {
                    tag.FILECOUNT = fileCount.ToString();
                    //tag.FILENAME =doc_bin[0].BINID
                    tag.ID = Guid.NewGuid().ToString();
                    tag.SLBH = slbh;
                    tag.PUSHDATE = DateTime.Now;
                    tag.ISSUCCESS = "0";
                    tag.MESSAGE = "失败!" + ex.Message;
                    ImportDAL.InsertLog(tag);

                }






                res.ConfirmType = 0;
                res.IsSuccess = IsSuccess;
                res.Message = tag.MESSAGE;

            }
            else
            {
                res.ConfirmType = 0;
                res.IsSuccess = false;
                res.Message = "失败！图像尚未扫描入库！";
            }
            return res;
        }

        internal static BDCFilterResult ImportQSPic(string base64Str,string sLBH, string userName)
        {
            BDCFilterResult res = new BDCFilterResult();

            int count = 0;
            FC_REWRITE_TAG tag = new FC_REWRITE_TAG();
            //string slbh = row[0].ToString();
            try
            {
                if (CanImportPic(sLBH))
                {

                    ImportQS(base64Str, sLBH, userName);

                    tag.FILECOUNT = count.ToString();
                    //tag.FILENAME =doc_bin[0].BINID
                    tag.ID = Guid.NewGuid().ToString();
                    tag.SLBH = sLBH;
                    tag.PUSHDATE = DateTime.Now;
                    tag.ISSUCCESS = "1";
                    tag.MESSAGE = "成功";

                    res.ConfirmType = 0;
                    res.IsSuccess = true; ;
                    res.Message = tag.MESSAGE;
                }
                else
                {
                    res.ConfirmType = 0;
                    res.IsSuccess = false;
                    res.Message = "无完税信息！";
                }
            }
            catch (Exception ex)
            {
                res.ConfirmType = 0;
                res.IsSuccess = false;
                res.Message = ex.Message;

                tag.FILECOUNT = count.ToString();
                //tag.FILENAME =doc_bin[0].BINID
                tag.ID = Guid.NewGuid().ToString();
                tag.SLBH = sLBH;
                tag.PUSHDATE = DateTime.Now;
                tag.ISSUCCESS = "0";
                tag.MESSAGE = "失败！" + ex.Message;
            }
            finally
            {
                if (null != tag && null != tag.ID)
                    ImportDAL.InsertLog(tag);
            }
            //IsSuccess = true;


            return res;
        }

        private static void base64ToImage(string base64str,string path)
        {
            MemoryStream ms = null;
            try
            {
                byte[] arr = Convert.FromBase64String(GetRealStr(base64str));
                ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                int w = bmp.Width;
                int h = bmp.Height;
                bmp.Save(path, ImageFormat.Png);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(null!=ms)
                {
                    ms.Close();
                    ms.Dispose();
                }
            }
        }

        private static string GetRealStr(string base64Str)
        {
            string[] str = base64Str.Split(',');
            return str[1];
        }

        private static void ImportQS(string base64Str, string sLBH, string userName)
        {
            string path = sLBH + "_完税凭证.jpg";
            string cid = GetCID(sLBH, "完税凭证", "文件夹", userName);
            DOC_BINFILE doc_binfile = new DOC_BINFILE();
            
            string binid = GetCID(sLBH, "完税凭证", "文件", userName, cid);//att.CID;
            doc_binfile.BINID = binid;
            doc_binfile.FTPATH = path;
            doc_binfile.FILEID = Guid.NewGuid().ToString();
            doc_binfile.FILENAME = "完税凭证";
            doc_binfile.EXTNAME = "jpg";//GetExtName(v.imgName);
            doc_binfile.ISCOMPRESSED = "否";
            doc_binfile.ISENCRYPTED = "否";

            QSPicUploadToFtp(sLBH,base64Str, doc_binfile);


            if (ImportDAL.ExistDoc_binfile(doc_binfile))
                ImportDAL.UpDateDoc_binfile(doc_binfile);
            else
                ImportDAL.InsertDoc_binfile(doc_binfile);
        }

        private static void QSPicUploadToFtp(string slbh,string base64Str, DOC_BINFILE doc_binfile)
        {
            FTPHelper ftpHelper = new FTPHelper();
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FCFtpIP"],
                username = ConfigurationManager.AppSettings["FCFtpUser"],
                password = ConfigurationManager.AppSettings["FCFtpPWD"],
                port = ConfigurationManager.AppSettings["FCFtpPort"],
            };
            FTP tagFTP = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FtpAddr"],
                username = ConfigurationManager.AppSettings["FtpUser"],
                password = ConfigurationManager.AppSettings["FtpPwd"],
                port = ConfigurationManager.AppSettings["FtpPort"]
            };

            string tmpPath = ConfigurationManager.AppSettings["TmpPath"];
            string yearDir = slbh.Substring(0, 4);
            string monthDir = slbh.Substring(4, 2);
            string dayDir = slbh.Substring(6, 2);
            string sortDir = slbh.Substring(8);

            string path = ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir;

            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP))
            {
                try
                {
                    ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP);
                }
                catch { }
            }

            //if(!FTPHelper.DirectoryExist(path,tagFTP))
            //{
            //    FTPHelper.MakeDir(path, tagFTP);
            //}

            string source = tmpPath + "\\" + GetFileName(doc_binfile.FTPATH);
            CreateImgByBase64(base64Str, source);
            FileInfo fi = new FileInfo(source);
            doc_binfile.FTPATH = ftpHelper.UploadFtp("", source, path, tagFTP);
            fi.Delete();

        }

        private static void CreateImgByBase64(string base64Str,string path)
        {
            base64ToImage(base64Str, path);
        }

        private static bool CanImportPic(string sLBH)
        {
            if(sLBH.Length<12)
            {
                return false;
            }
            return Finshed(sLBH);
            
        }

        private static bool Finshed(string slbh)
        {
            return new TaxInterfaceDal().IfExistsSLBH(slbh);
        }

        public static BDCFilterResult ImportFromFC(string slbh, string userName)
        {
             
            BDCFilterResult res = new BDCFilterResult();

            int count = 0;
            FC_REWRITE_TAG tag = new FC_REWRITE_TAG();
            string FCslbh = GetRealSLBH(slbh);
            //string slbh = row[0].ToString();
            try
            {
                if (CanImport(FCslbh))
                {

                    ImportFCDA(slbh, FCslbh, userName, ref count);

                    tag.FILECOUNT = count.ToString();
                    //tag.FILENAME =doc_bin[0].BINID
                    tag.ID = Guid.NewGuid().ToString();
                    tag.SLBH = slbh;
                    tag.PUSHDATE = DateTime.Now;
                    tag.ISSUCCESS = "1";
                    tag.MESSAGE = "成功";

                    res.ConfirmType = 0;
                    res.IsSuccess = true; ;
                    res.Message = tag.MESSAGE;
                }
                else
                {
                    res.ConfirmType = 0;
                    res.IsSuccess = false;
                    res.Message = "图像尚未扫描入库！";
                }
            }
            catch (Exception ex)
            {
                res.ConfirmType = 0;
                res.IsSuccess = false;
                res.Message = ex.Message;

                tag.FILECOUNT = count.ToString();
                //tag.FILENAME =doc_bin[0].BINID
                tag.ID = Guid.NewGuid().ToString();
                tag.SLBH = slbh;
                tag.PUSHDATE = DateTime.Now;
                tag.ISSUCCESS = "0";
                tag.MESSAGE = "失败！" + ex.Message;
            }
            finally
            {
                if(null!=tag && null!=tag.ID)
                ImportDAL.InsertLog(tag);
            }
            //IsSuccess = true;


            return res;
        }

        private static void ImportFCDA(string slbh, string FCslbh, string user,ref int count)
        {
            List<Result> list = new List<Result>();
            List<VolEleArc> vol_list = ImportDAL.GetVolEleArc_list(FCslbh);
            string errStr = "";
            foreach (VolEleArc vol in vol_list)
            {
                Result res = new Result();
                string cid = GetCID(slbh, vol.EleArcName, "文件夹", user);
                List<VolEleArcDtl> dtlList = ImportDAL.GetVolEleArcDtlByVol(vol.EleArcVol_ID);
                if (null != dtlList && dtlList.Count > 0)
                {
                    count = dtlList.Count;
                    for (int i = 0; i < dtlList.Count; i++)
                    {
                        try
                        {
                            VolEleArcDtl v = dtlList[i];
                            v.imgName = v.imgName.Replace(@"\\192.168.134.245\", "").Replace(@"\\192.168.100.202\", "").Replace(@"K:\", "").Replace(@"\\192.168.100.202 \", "");
                            v.imgName = v.imgName.Replace("\\", "/");
                            DOC_BINFILE doc_binfile = new DOC_BINFILE();
                            string EleArcName = "";
                            if (i == 0)
                                EleArcName = vol.EleArcName;
                            if (i > 0)
                                EleArcName = vol.EleArcName + "-" + i;
                            string binid = GetCID(slbh, EleArcName, "文件", user, cid);//att.CID;
                            doc_binfile.BINID = binid;
                            doc_binfile.FTPATH = v.imgName;
                            doc_binfile.FILEID = Guid.NewGuid().ToString();
                            doc_binfile.FILENAME = vol.EleArcName;
                            doc_binfile.EXTNAME = GetExtName(v.imgName);
                            doc_binfile.ISCOMPRESSED = "否";
                            doc_binfile.ISENCRYPTED = "否";

                            UploadToFtp(FCslbh, doc_binfile);


                            if (ImportDAL.ExistDoc_binfile(doc_binfile))
                                ImportDAL.UpDateDoc_binfile(doc_binfile);
                            else
                                ImportDAL.InsertDoc_binfile(doc_binfile);
                        }
                        catch(Exception ex)
                        {
                            if(!errStr.Contains(ex.Message))
                            {
                                errStr += ex.Message;
                            }
                            continue;
                        }
                    }
                }
            }

            if(!string.IsNullOrEmpty(errStr))
            {
                throw new Exception(errStr);
            }
        }

       



        private static string GetRealSLBH(string slbh)
        {
            slbh = slbh.Replace("FC", "").Replace("OFC","").Replace("OF","");
            if (slbh.Contains("_"))
                slbh = slbh.Substring(0, slbh.IndexOf('_'));
            return slbh;
        }


        private static DataTable GetCLSLBH()
        {
            throw new NotImplementedException();
        }

        private static string ftpDir = ConfigurationManager.AppSettings["FtpDir"];

        public static void UploadToFtp(string slbh, DOC_BINFILE item)
        {
            FTPHelper ftpHelper = new FTPHelper();
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FCFtpIP"],
                username = ConfigurationManager.AppSettings["FCFtpUser"],
                password = ConfigurationManager.AppSettings["FCFtpPWD"],
                port= ConfigurationManager.AppSettings["FCFtpPort"],
            };
            FTP tagFTP = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FtpAddr"],
                username = ConfigurationManager.AppSettings["FtpUser"],
                password = ConfigurationManager.AppSettings["FtpPwd"],
                port=ConfigurationManager.AppSettings["FtpPort"]
            };

            string tmpPath = ConfigurationManager.AppSettings["TmpPath"];
            string yearDir = slbh.Substring(0, 4);
            string monthDir = slbh.Substring(4, 2);
            string dayDir = slbh.Substring(6, 2);
            string sortDir = slbh.Substring(8);

            string path = ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir;

            if (!ftpHelper.DirectoryExist(ftpDir+"/"+yearDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, tagFTP))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, tagFTP);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP))
            {
                try
                {
                    ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP);
                }
                catch { }
            }

            //if(!FTPHelper.DirectoryExist(path,tagFTP))
            //{
            //    FTPHelper.MakeDir(path, tagFTP);
            //}

            string source = ftpHelper.DownloadFtp(tmpPath + "\\" + GetFileName(item.FTPATH), "", item.FTPATH, souFTp);
            FileInfo fi = new FileInfo(source);
            item.FTPATH=ftpHelper.UploadFtp("", source, path, tagFTP);
            fi.Delete();


        }

        private static string ChangeFTPPath(string filePath)
        {
            string path = GetFileName(filePath);

            path = ftpDir + "/" + path;

            return path;

        }

        private static string GetFileName(string filePath)
        {
            int index = filePath.LastIndexOf('/') + 1;
            if(index==0)
            {
                index = filePath.LastIndexOf('\\') + 1;
            }
            return filePath.Substring(index, filePath.Length - index);
        }

        private static DOC_FILE GetDoc_file(string slbh)
        {
            throw new NotImplementedException();
        }


        static string WebSrvUrl = ConfigurationManager.AppSettings["FCWebService"];
        static string WebSrvMethon = ConfigurationManager.AppSettings["FCWebServiceMethon"];
        /// <summary>
        /// 从房产库查询数据构建BDC插入实例
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        private static List<DOC_BINFILE> GetDoc_Binfile(string slbh,string fcslbh)
        {

            
            List<VolEleArcDtl> veads = ImportDAL.GetVolEleArcDtl(fcslbh);
            List<DOC_BINFILE> list = new List<DOC_BINFILE>();
            //if(null!=Res_list && Res_list.Count>0)
            //{
            //    foreach (Result res in Res_list)
            //    {
            Thread.Sleep(200);
            foreach (VolEleArcDtl v in veads)
            {
                v.imgName = v.imgName.Replace(@"\\192.168.134.245\","").Replace(@"\\192.168.100.202\", "").Replace(@"K:\", "").Replace(@"\\192.168.100.202 \","");
                v.imgName = v.imgName.Replace("\\", "/");
                VolEleArc vea = GetVolEleArc(v.VolEleArc_ID);
                DOC_BINFILE doc_binfile = new DOC_BINFILE();
                doc_binfile.BINID = GetCID(slbh, vea.EleArcName);
                if (doc_binfile.BINID != "")
                {
                    doc_binfile.FTPATH = v.imgName;
                    doc_binfile.FILEID = Guid.NewGuid().ToString();
                    doc_binfile.FILENAME = vea.EleArcName;
                    doc_binfile.EXTNAME = GetExtName(v.imgName);
                    doc_binfile.ISCOMPRESSED = "否";
                    doc_binfile.ISENCRYPTED = "否";
                    list.Add(doc_binfile);
                }
            }

            //    }
            //}

            return list;

        }

        private static string GetExtName(string fileName)
        {
            string extName= fileName.Substring(fileName.LastIndexOf(".") + 1, (fileName.Length - fileName.LastIndexOf(".") - 1)); //扩展名
            return extName.ToUpper();
        }

        private static VolEleArc GetVolEleArc(Guid? vid)
        {
            return ImportDAL.GetVolEleArc(vid);
        }

        /// <summary>
        /// 从BDC库获得CID
        /// </summary>
        /// <param name="slbh"></param>
        /// <returns></returns>
        private static string GetCID(string slbh, string fileName,string fileType,string user)
        {

            string cid = ImportDAL.GetCID(slbh, fileName, fileType);
            if (cid == "")
            {
                WFM_ATTACHLST att = new WFM_ATTACHLST();
                att.CID = Guid.NewGuid().ToString();
                att.CNAME = fileName;
                if (fileType == "文件夹")
                    att.PID = ImportDAL.GetCID(slbh);
                else
                    att.PID = "";
                att.PNODE = slbh;
                cid = ImportDAL.InserAttachlst(att, fileType,user);
            }
            return cid;
        }

        private static string GetCID(string slbh, string fileName, string fileType, string user,string pid)
        {

            string cid = ImportDAL.GetCID(slbh, fileName, fileType);
            if (cid == "")
            {
                WFM_ATTACHLST att = new WFM_ATTACHLST();
                att.CID = Guid.NewGuid().ToString();
                att.CNAME = fileName;
                if (fileType == "文件夹")
                    att.PID = ImportDAL.GetCID(slbh);
                else
                    att.PID = pid;
                att.PNODE = slbh;
                cid = ImportDAL.InserAttachlst(att, fileType, user);
            }
            return cid;
        }

        private static string GetCID(string slbh, string fileName)
        {

            string cid = ImportDAL.GetCID(slbh);
            if (cid == "")
            {
                WFM_ATTACHLST att = new WFM_ATTACHLST();
                att.CID = Guid.NewGuid().ToString();
                att.CNAME = fileName;
                att.PID = ImportDAL.GetCID(slbh);
                att.PNODE = slbh;
                cid = ImportDAL.InserAttachlst(att);
            }
            return cid;
        }
        /// <summary>
        /// 判断是否需要导入
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool CanImport(string p)
        {
            return ImportDAL.CanImport(p);
        }

      
    }
}
