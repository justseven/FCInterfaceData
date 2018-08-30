using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Web4BDC.Dal.MergeIamge;
using Web4BDC.Tools;

namespace Web4BDC.Bll.MergeImage
{
    public class MergeImageBLL
    {
        private static string ftpDir ="MergeImg";
        private static string tmpDir="d:\\MergeTmp\\";
        internal string MergeImage(string slbh)
        {
           

            MergeImgHelp mih=new MergeImgHelp();
            List<string> tstybm = GetTstybm(slbh);
            List<string> fileIds = GetFiled(tstybm);
            List<string> imgPath = GetImagePath(fileIds);

            if (!Directory.Exists(tmpDir))
                Directory.CreateDirectory(tmpDir);

            GetIamgeFromFTP(imgPath);
            DirectoryInfo dir = new DirectoryInfo(tmpDir);
            FileInfo[] fileNames = dir.GetFiles();
            if (null != fileNames && fileNames.Length > 0)
            {
                foreach (FileInfo item in fileNames)
                {
                    FileStream stream =   new FileStream(item.FullName, FileMode.Open);
                    mih.ZoomAuto(stream, item.FullName, 1287, 1059, "", "");
                    stream.Close();
                    //item.Delete();
                }
                string mergepath = Merge(fileNames, slbh);
                
                string path = UploadToFTP(slbh, mergepath);
                DeleteDirFile(tmpDir);
                return path;
            }
            return string.Empty;
        }

        private void DeleteDirFile(string tmpDir)
        {
            DirectoryInfo dir = new DirectoryInfo(tmpDir);
            FileInfo[] fileNames = dir.GetFiles();
            if(null!=fileNames && fileNames.Length>0)
            {
                foreach (var item in fileNames)
                {
                    item.Delete();
                }
            }
        }

        private string UploadToFTP(string slbh, string source)
        {
            FTPHelper ftpHelper = new FTPHelper();
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FtpAddr"],
                username = ConfigurationManager.AppSettings["FtpUser"],
                password = ConfigurationManager.AppSettings["FtpPwd"],
                port = ConfigurationManager.AppSettings["FtpPort"],
            };
            string yearDir = slbh.Substring(0, 4);
            string monthDir = slbh.Substring(4, 2);
            string dayDir = slbh.Substring(6, 2);
            string sortDir = slbh.Substring(8);

            string path = ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir;

            if (!ftpHelper.DirectoryExist(ftpDir , souFTp))
            {
                ftpHelper.MakeDir(ftpDir , souFTp);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir, souFTp))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir, souFTp);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir, souFTp))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir, souFTp);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, souFTp))
            {
                ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir, souFTp);
            }
            if (!ftpHelper.DirectoryExist(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, souFTp))
            {
                try
                {
                    ftpHelper.MakeDir(ftpDir + "/" + yearDir + "/" + monthDir + "/" + dayDir + "/" + sortDir, souFTp);
                }
                catch { }
            }
           
            FileInfo fi = new FileInfo(source);
            string FTPATH = ftpHelper.UploadFtp("", source, path, souFTp);
            fi.Delete();
            return FTPATH;
        }

        private string Merge(FileInfo[] fileNames, string slbh)
        {
            string source = tmpDir + slbh + ".jpg";
            MergeImgHelp mih = new MergeImgHelp();
            mih.CombineImages(fileNames, source);
            return source;
        }

        private void GetIamgeFromFTP(List<string> imgPath)
        {
            FTPHelper ftpHelper = new FTPHelper();
            FTP souFTp = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FtpAddr"],
                username = ConfigurationManager.AppSettings["FtpUser"],
                password = ConfigurationManager.AppSettings["FtpPwd"],
                port = ConfigurationManager.AppSettings["FtpPort"],
            };
            if(null!= imgPath && imgPath.Count>0)
            {
                foreach (var item in imgPath)
                {
                    try
                    {
                        string source = ftpHelper.DownloadFtp(tmpDir + "\\" + GetFileName(item), "", item, souFTp);
                    }
                    catch
                    {
                        continue;
                    }
                    
                }
            }
            
        }

        private static string GetFileName(string filePath)
        {
            int index = filePath.LastIndexOf('/') + 1;
            if (index == 0)
            {
                index = filePath.LastIndexOf('\\') + 1;
            }
            return filePath.Substring(index, filePath.Length - index);
        }

        private List<string> GetImagePath(List<string> fileIds)
        {
            MergeImageDAL dal = new MergeImageDAL();
            List<string> imgPath = dal.GetImagePath(fileIds);
            return imgPath;
        }

        private List<string> GetFiled(List<string> tstybm)
        {
            MergeImageDAL dal = new MergeImageDAL();
            List<string> fileIds = dal.GetFileIds(tstybm);
            return fileIds;
        }

        private List<string> GetTstybm(string slbh)
        {
            MergeImageDAL dal = new MergeImageDAL();
            List<string> tstybm = dal.GetTstybms(slbh);
            return tstybm;
        }
    }
}