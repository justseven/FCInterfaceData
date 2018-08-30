using DBCForFCWebService.Dal;
using DBCForFCWebService.Model.ZZDZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Tools;

namespace DBCForFCWebService
{
    /// <summary>
    /// ZZDZ_SUINING 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ZZDZ_SUINING : System.Web.Services.WebService
    {

        bool IsPrintPicture = Convert.ToBoolean(ConfigurationManager.AppSettings["PrintPicture"]);
        string bank_person = ConfigurationManager.AppSettings["Bank_Person"].ToString();
        IDictionary<string, string> dic = null;
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public DataTable LND_PROC_GETCERTQUERY(string LZRZJH)
        {
            if (!string.IsNullOrEmpty(LZRZJH))
            {
                ZZDYLOG log = new ZZDYLOG();
                log.ID = Guid.NewGuid().ToString();
                log.CallTime = DateTime.Now;
                log.IntefaceName = "LND_PROC_GETCERTQUERY";
                log.keyValues = "@LZRZJH=" + LZRZJH;

                DataTable dt = null;
                ZZDZ_DAL dal = new ZZDZ_DAL();
                try
                {
                    if (null == dic)
                    {
                        dic = GetBanks(bank_person);
                    }
                    if (dic.Keys.Contains(LZRZJH))
                    {
                        string bank = dic[LZRZJH];
                        dt = dal.GetCerQuery_Bank(bank);

                    }
                    else
                    {
                        dt = dal.GetCerQuery(LZRZJH);
                    }
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        log.ReturnState = true.ToString();
                        log.RevurnVale = "返回" + dt.Rows.Count + "行";
                    }
                    else
                    {
                        log.ReturnState = false.ToString();
                        log.RevurnVale = "无数据返回";
                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    log.ReturnState = false.ToString();
                    log.RevurnVale = "错误：" + ex.Message;
                }
                finally
                {
                    dal.InsertLog(log);
                }
            }
            return null;
        }

        private IDictionary<string, string> GetBanks(string bank_person)
        {
            
            return JasonToDic(bank_person);
        }

        private IDictionary<string, string> JasonToDic(string bank_person)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, string>>(bank_person);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [WebMethod]
        public DataTable LND_PROC_GETCERTINFO(string ywh,string QLRZJH,string BDCQZH)
        {
            ZZDYLOG log = new ZZDYLOG();
            log.ID = Guid.NewGuid().ToString();
            log.CallTime = DateTime.Now;
            log.IntefaceName = "LND_PROC_GETCERTINFO";
            log.keyValues = "@ywh=" + ywh+ "|@QLRZJH="+ QLRZJH+ "|@BDCQZH="+ BDCQZH;

            if (string.IsNullOrEmpty(ywh) && string.IsNullOrEmpty(QLRZJH) && string.IsNullOrEmpty(BDCQZH))
            {
                log.ReturnState =false.ToString();
                log.RevurnVale = "所有参数都为空";
                return null;
            }//201709050055
            ZZDZ_DAL dal = new ZZDZ_DAL();
            try
            {
                DataTable dt =dal.GetCerInfo(ywh, QLRZJH, BDCQZH);
                if (null != dt && dt.Rows.Count > 0)
                {
                    if(IsPrintPicture)
                        InitBDCPicture(dt);
                    log.ReturnState =true.ToString();
                    log.RevurnVale = "返回" + dt.Rows.Count + "行";
                }
                else
                {
                    log.ReturnState =false.ToString();
                    log.RevurnVale = "无数据返回";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.ReturnState =false.ToString();
                log.RevurnVale = "错误：" + ex.Message;
            }
            finally
            {
                dal.InsertLog(log);
            }
            return null;
        }

        private void InitBDCPicture(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    string bdcdyh = row["BDCDYH"].ToString();
                    string ywh = row["YWH"].ToString();
                    string zdt = GetZDTBybdcdyh(bdcdyh,ywh);
                    if(string.IsNullOrEmpty(row["FHT1"].ToString()))
                    {
                        string fcfht = GetFCFHTBybdcdyh(bdcdyh, ywh);
                        row["FHT"] = fcfht;
                    }
                    else
                    {
                        byte[] bytes = (byte[])row["FHT1"];
                        row["FHT"]=Convert.ToBase64String(bytes);
                    }
                    
                    row["zdt"] = zdt;
                    
                }
                catch(Exception ex)
                {
                    string str = ex.Message;
                    continue;
                }
            }
            
        }

        private string GetFCFHTBybdcdyh(string bdcdyh, string ywh)
        {
            ZZDZ_DAL dal = new ZZDZ_DAL();
            string fileid = "";

            DataTable dt = GetALLBDCDYH(ywh);
            foreach (DataRow row in dt.Rows)
            {
                fileid = dal.GetFileID(row[0].ToString(), "分层分户草图");
                if (!string.IsNullOrEmpty(fileid))
                {
                    break;
                }
            }


            string ftppath = dal.GetFtpPath(fileid, "'分层分户草图'");
            //string filePath = GetFileFromFTP(ftppath);
            return GetFileFromFTP(ftppath);

        }

        private DataTable GetALLBDCDYH(string ywh)
        {
            ZZDZ_DAL dal = new ZZDZ_DAL();
            return dal.GetALLBDCDYH(ywh);
        }

        private string GetFileFromFTP(string ftppath)
        {
            FTPHelper ftpHelper = new FTPHelper();
            
            FTP tagFTP = new FTP()
            {
                hostname = ConfigurationManager.AppSettings["FtpAddr"],
                username = ConfigurationManager.AppSettings["FtpUser"],
                password = ConfigurationManager.AppSettings["FtpPwd"],
                port = ConfigurationManager.AppSettings["FtpPort"]
            };
            string tmpPath = ConfigurationManager.AppSettings["TmpPath"];
            string imgBase64 = ftpHelper.GetBase64StringFrmFtp(tmpPath + "\\" + GetFileName(ftppath), "", ftppath, tagFTP);

            return imgBase64;
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

        private string GetZDTBybdcdyh(string bdcdyh,string ywh)
        {
            ZZDZ_DAL dal = new ZZDZ_DAL();
            if(bdcdyh.Contains("等"))
            {
                bdcdyh = bdcdyh.Substring(0, bdcdyh.IndexOf("等"));
            }
            string fileid = dal.GetFileID(bdcdyh, "宗地图");
            //fileid="File-160511101022-CCJO0QOR";
            //string ftppath="/2016/05/11/Bin-160511101022-UX1AH5HK.PDF";
            string ftppath = dal.GetFtpPath(fileid, "'宗地图','宗地示意图','宗地草图'");
            if (!string.IsNullOrEmpty(ftppath))
            {
                //string filePath = GetFileFromFTP(ftppath);
                return GetFileFromFTP(ftppath);
            }
            return "";
        }
        [WebMethod]
        public string GetBase64StringByFilePath(string path)
        {
            return GetFileFromFTP(path);
        }

        [WebMethod]
        public int LND_PROC_UPDATECERTINFO(string YWH,string QLRZJH,string BDCQZH,string LZRXM,string LZRZJH,string YSXLH,string YSXLHP,string machineCode)
        {
            ZZDYLOG log = new ZZDYLOG();
            log.ID = Guid.NewGuid().ToString();
            log.CallTime = DateTime.Now;
            log.IntefaceName = "LND_PROC_UPDATECERTINFO";
            log.keyValues = "@ywh=" + YWH + "|@QLRZJH=" + QLRZJH + "|@BDCQZH=" + BDCQZH+ "|@LZRXM=" + LZRXM + "|@LZRZJH=" + LZRZJH + "|@YSXLH=" + YSXLH + "|@machineCode=" + machineCode;
           
            ZZDZ_DAL dal = new ZZDZ_DAL();
            try
            {
                if (string.IsNullOrEmpty(machineCode))
                {
                    log.ReturnState =false.ToString();
                    log.RevurnVale = "machineCode为空";
                    return -1;
                }
                int count= dal.UPDATECERTINFO(YWH, QLRZJH, BDCQZH, LZRXM, LZRZJH, YSXLH, YSXLHP, machineCode);
                if (count > 0)
                    log.ReturnState = true.ToString();
                else
                    log.ReturnState = false.ToString();
                log.RevurnVale = "返回值：" + count;

                return count;
            }
            catch(Exception ex)
            {
                log.ReturnState =false.ToString();
                log.RevurnVale = "错误：" + ex.Message;
            }
            finally
            {

                dal.InsertLog(log);
            }
            return -1;
        }
        [WebMethod]
        public int LND_PROC_UPDATECOST(string YWH, string QLRZJH, string BDCQZH,float cost)
        {
            return -1;
        }
        [WebMethod]
        public int LND_PROC_UPDATECERTSTATE(string YWH, string QLRZJH, string BDCQZH, int state,string machineCode)
        {
            ZZDYLOG log = new ZZDYLOG();
            log.ID = Guid.NewGuid().ToString();
            log.CallTime = DateTime.Now;
            log.IntefaceName = "LND_PROC_UPDATECERTSTATE";
            log.keyValues = "@ywh=" + YWH + "|@QLRZJH=" + QLRZJH + "|@BDCQZH=" + BDCQZH + "|@state=" + state + "|@machineCode=" + machineCode;
            ZZDZ_DAL dal = new ZZDZ_DAL();

            try
            {
                if (string.IsNullOrEmpty(machineCode))
                {
                    log.ReturnState =false.ToString();
                    log.RevurnVale = "machineCode为空";
                    return -1;
                }
                int count = dal.UPDATECERTSTATE(YWH, QLRZJH, BDCQZH, state, machineCode);
                if (count > 0)
                    log.ReturnState = true.ToString();
                else
                    log.ReturnState = false.ToString();
                log.RevurnVale = "返回值：" + count ;
                return count;
            }
            catch(Exception ex)
            {
                log.ReturnState =false.ToString();
                log.RevurnVale = "错误：" + ex.Message;
            }
            finally
            {
                dal.InsertLog(log);
            }
            return -1;
        }






    }
}
