using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using DBCForFCWebService.Dal;
using DBCForFCWebService.Model;
using DBCForFCWebService.Model.ZZSF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Tools;

namespace DBCForFCWebService
{
    /// <summary>
    /// SFInfoWS 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SFInfoWS : System.Web.Services.WebService
    { 
        private string URL= ConfigurationManager.AppSettings["InterfaceUrl"].ToString();
        private bool IsSubmit = Convert.ToBoolean(ConfigurationManager.AppSettings["SFSubmit"]);

       
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetSFInfo(string slbh)
        {
            
            return CreateResult(slbh);
            
        }
        [WebMethod]
        public string Submit(string json)
        {
            ResutlModel res = new ResutlModel();

            string errStr = string.Empty;
            try
            {
                WriteLog("进入程序");
                MessageBody msg = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageBody>(json);
                if (CheckToken(msg))
                {
                    WriteLog("通过TOKEN检查");
                    res.Token = msg.Token;
                    if (msg.IsSuccess)
                    {
                        if (msg.Count == msg.JFInfoList.Count)
                        {
                            errStr = SendSubmit(msg);
                        }
                        else
                        {
                            errStr = "缴费项目个数不一致";
                        }
                    }
                    else
                    {
                        errStr = "缴费失败，请查询缴费信息。";
                    }
                }
                else
                {
                    errStr = "非法请求";
                }
                if (string.IsNullOrEmpty(errStr))
                {
                    res.IsSuccess = true;
                    UpDateSFDMODEL(res.Token, msg.UserID);

                }
                else
                {
                    res.IsSuccess = false;
                }
                
                res.Message = errStr;
                

            }
            catch (Exception ex)
            {
                WriteLog("出现异常:"+ex.Message);
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            try
            {
                InsertRequestLog(json, res);
            }
            catch(Exception ex)
            {
                WriteLog("插入日志异常"+ex.Message);
            }
            return ToXml(res);
        }


        private void WriteLog(string str)
        {
            string path = "d:\\SFINFOASMX.txt";
            StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            wr.WriteLine(str);
            wr.WriteLine("--------------------------------------------------------------------");
            wr.Close();

        }


        private void InsertRequestLog(string json, ResutlModel res)
        {
            SFInfo_DAL dal = new SFInfo_DAL();
            SF_Submit_Request ssr = new SF_Submit_Request();
            ssr.UUID = Guid.NewGuid().ToString();
            ssr.RequestContent = json;
            ssr.RequestTime = DateTime.Now;
            ssr.Result = res.Message;
            ssr.ResFlag = res.IsSuccess?1:-1;
            dal.InsertRequestLog(ssr);
        }

        private void UpDateSFDMODEL(string token,string userid)
        {
            SFInfo_DAL dal = new SFInfo_DAL();
            dal.UpDateSDFModel(token,userid);
        }

        private string SendSubmit(MessageBody msg)
        {
            if (IsSubmit)
            {
                WriteLog("发送提交请求");
                string postData = GetSLbh(msg, "|");
                postData += "|" + msg.UserID;
                HttpResult res = SendPost(postData);
                WriteLog("发送完成");
                if (res.StatusCode == System.Net.HttpStatusCode.OK && res.Html.Contains("OK"))
                {
                    return "";
                }
                else
                {
                    throw new Exception(res.StatusDescription + "  " + res.Html);
                }
            }
            else
            {
                return "";
            }
        }

        private HttpResult SendPost(string data)
        {
            string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            try
            {
                HttpHelper hp = new HttpHelper();
                byte[] data_byte = Encoding.UTF8.GetBytes(data);
                HttpItem item = new HttpItem()
                {
                    URL = URL,//URL必需项
                    Method = "post",
                    UserAgent = UserAgent,
                    ContentType = "application/x-www-form-urlencoded",//"application/x-www-form-urlencoded",//"application/json;charset=utf-8",
                                                                      //Accept = "application/json",
                                                                      //ResultType = ResultType.String,
                    PostDataType = PostDataType.Byte,
                    PostdataByte = data_byte
                };

                //得到HTML代码

                return hp.GetHtml(item);
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckToken(MessageBody msg)
        {
            SFInfo_DAL dal = new SFInfo_DAL();

            List<SFDModel> list = dal.QuerySfdModel(msg.Token);
            if(list.Count>0)
            {
                return true;
            }
            return false;
        }

        private string MD5(string v)
        {
            return MyEncrypt.MD5Encrypt32(v);
        }

        private string GetSLbh(MessageBody msg,string chr)
        {
            string slbh = string.Empty;
            if(msg.JFInfoList!=null && msg.JFInfoList.Count>0)
            {
                foreach (JFInfoModel m in msg.JFInfoList)
                {
                    if (string.IsNullOrEmpty(slbh))
                        slbh += m.SLBH;
                    else
                        slbh += chr + m.SLBH;
                }
            }
            return slbh;
        }

        private string CreateResult(string slbh)
        {
            SFInfo_DAL dal = new SFInfo_DAL();
            ResutlModel res = new ResutlModel();

            try
            {
                if (dal.CheckSFState(slbh))
                {
                    List<DJ_SFD> sfd = dal.GetSFData(slbh);
                    if (null != sfd)
                    {
                        
                        res.Data = sfd;
                        res.IsSuccess = true;
                        res.Token = Guid.NewGuid().ToString();

                        UpdateSFDZT(sfd, res.Token);
                        
                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Message = "无需收费信息";
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Message = "尚未确定收费金额或已缴费"; 
                }
            }
            catch(Exception ex)
            {
                res.IsSuccess = false;
                res.Message = "异常:" + ex.Message;
            }
            return ToXml(res);
            
        }

       

       


        private void UpdateSFDZT(MessageBody body)
        {
            SFInfo_DAL dal = new SFInfo_DAL();

            List<DJ_SFD> list = new List<DJ_SFD>();
            if(body.JFInfoList!=null && body.JFInfoList.Count>0)
            {
                foreach (JFInfoModel m in body.JFInfoList)
                {
                    //DJ_SFD sfd = GetSFD(m.SLBH);
                    
                }
            }
        }

        private void UpdateSFDZT(List<DJ_SFD> sfd_list,string token)
        {
            List<SFDModel> modList = new List<SFDModel>();
            SFInfo_DAL dal = new SFInfo_DAL();
            foreach (DJ_SFD sfd in sfd_list)
            {
                SFDModel mod = new SFDModel();
                mod.ID = Guid.NewGuid().ToString();
                mod.Slbh = sfd.SLBH;
                mod.Uuid = token;
                mod.Money = sfd.YSJE;
                mod.State = "查询";
                modList.Add(mod);
                sfd.ZZSFZT = "-1";
            }
            dal.UpDateSfd_ModList(sfd_list, modList);
        }

        public string ToXml(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, obj);
            return sw.ToString();
        }
    }
}
