using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using XZFCDA.Models.BDCModel;
using XZFCDA.Models;
using XZFCDA.Tools;
using XZFCDA.Dal;
using XZFCDA.FC.Models;

namespace XZFCDA.BLL
{
    public class ImportBLL
    {
        static string UseFtp = ConfigurationManager.AppSettings["UseFtp"];
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
                    if (CanImport(slbh))
                    {

                        try
                        {
                            List<DOC_BINFILE> doc_bin = GetDoc_Binfile(slbh);

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
            slbh = GetRealSLBH(slbh);
            //string slbh = row[0].ToString();
            if (CanImport(slbh))
            {

                try
                {
                    List<DOC_BINFILE> doc_bin = GetDoc_Binfile(slbh);

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
                                if (ImportDAL.ExistDoc_binfile(item))
                                    ImportDAL.UpDateDoc_binfile(item);
                                else
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



        private static string GetRealSLBH(string slbh)
        {
            slbh = slbh.Replace("FC", "");
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
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FCFtpIP"],
                username = ConfigurationManager.AppSettings["FCFtpUser"],
                password = ConfigurationManager.AppSettings["FCFtpPWD"]
            };
            FTP tagFTP = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["BDCFtpIP"],
                username = ConfigurationManager.AppSettings["BDCFtpUser"],
                password = ConfigurationManager.AppSettings["BDCFtpPWD"]
            };

            string tmpPath = ConfigurationManager.AppSettings["TmpPath"];
            string yearDir = slbh.Substring(0, 4);
            string monthDir = slbh.Substring(4, 2);
            string dayDir = slbh.Substring(6, 2);
            string sortDir = slbh.Substring(8);

            string path = yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir;

            if (!FTPHelper.DirectoryExist(yearDir, tagFTP))
            {
                FTPHelper.MakeDir(yearDir, tagFTP);
            }
            if (!FTPHelper.DirectoryExist(yearDir + "/" + monthDir, tagFTP))
            {
                FTPHelper.MakeDir(yearDir + "/" + monthDir, tagFTP);
            }
            if (!FTPHelper.DirectoryExist(yearDir + "/" + monthDir + "/" + dayDir, tagFTP))
            {
                FTPHelper.MakeDir(yearDir + "/" + monthDir + "/" + dayDir, tagFTP);
            }
            if (!FTPHelper.DirectoryExist(yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP))
            {
                FTPHelper.MakeDir(yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, tagFTP);
            }

            //if(!FTPHelper.DirectoryExist(path,tagFTP))
            //{
            //    FTPHelper.MakeDir(path, tagFTP);
            //}

            string source = FTPHelper.DownloadFtp(tmpPath + "\\" + GetFileName(item.FTPATH), "", item.FTPATH, souFTp);
            FileInfo fi = new FileInfo(source);
            FTPHelper.UploadFtp("", source, path, tagFTP);
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
        private static List<DOC_BINFILE> GetDoc_Binfile(string slbh)
        {

            //string[] args = new string[1];
            //args[0] = "China";
            //object result= WebServiceHelper.InvokeWebService(WebSrvUrl, WebSrvMethon, args);
            //List<Result> Res_list = (List<Result>)result;
            List<VolEleArcDtl> veads = ImportDAL.GetVolEleArcDtl(slbh);
            List<DOC_BINFILE> list = new List<DOC_BINFILE>();
            //if(null!=Res_list && Res_list.Count>0)
            //{
            //    foreach (Result res in Res_list)
            //    {
            foreach (VolEleArcDtl v in veads)
            {
                v.imgName = v.imgName.Replace(@"\\192.168.100.202 \", "").Replace(@"K:\", "");

                VolEleArc vea = GetVolEleArc(v.VolEleArc_ID);
                DOC_BINFILE doc_binfile = new DOC_BINFILE();
                doc_binfile.BINID = GetCID(slbh, vea.EleArcName);
                if (doc_binfile.BINID != "")
                {
                    doc_binfile.FTPATH = v.imgName;
                    doc_binfile.FILEID = Guid.NewGuid().ToString();
                    doc_binfile.FILENAME = vea.EleArcName;
                    doc_binfile.EXTNAME = "JPG";
                    doc_binfile.ISCOMPRESSED = "否";
                    doc_binfile.ISENCRYPTED = "否";
                    list.Add(doc_binfile);
                }
            }

            //    }
            //}

            return list;

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
        private static string GetCID(string slbh, string fileName)
        {

            string cid = ImportDAL.GetCID(slbh, fileName);
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
