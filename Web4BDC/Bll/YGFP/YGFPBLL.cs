using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.YGFP;
using Web4BDC.Tools;

namespace Web4BDC.Bll.YGFP
{
    public class YGFPBLL
    {
        static string InterFaceUrl= ConfigurationManager.AppSettings["YGFPInterfaceUrl"].ToString();
        static string YGFPKEY = ConfigurationManager.AppSettings["YGFPKEY"].ToString();
        static string PostToYGFP= ConfigurationManager.AppSettings["PostToYGFP"].ToString();
        static string YGFPExcelPath= ConfigurationManager.AppSettings["YGFPExcelPath"].ToString();
        internal static BDCFilterResult PushData(PageParams param)
        {

            List<BDCFilterResult> resList = new List<BDCFilterResult>();
            MZ_YGFP nonghu = YGFPDAL.CheckPerson(param.PrjId);
            //判断是否推送
            if (null!= nonghu)
            {
               
                try
                {
                    //需推送阳光扶贫接口
                    DataTable dt = GetYGFPModel(param.PrjId,nonghu.CardID);
                    
                    //判断是否直接推送接口
                    if (PostToYGFP.Equals("是"))
                    {


                        List<YGFPModel> models = ModelHelper<YGFPModel>.FillModel(dt);
                        foreach (YGFPModel model in models)
                        {
                            try
                            {
                                model.IDCard = nonghu.CardID;
                                string paramStr = GetParams(model);
                                ReturnValue rv=SendPost(paramStr);
                                if (rv.result == "1")
                                {
                                    resList.Add(new BDCFilterResult { IsSuccess = true, Message = "推送成功" });
                                }
                                else
                                {
                                    resList.Add(new BDCFilterResult { IsSuccess = false, Message = rv.message });
                                }
                            }
                            catch(Exception ex)
                            {
                                resList.Add(new BDCFilterResult { IsSuccess = false, Message = ex.Message });
                                
                                continue;
                            }
                        }

                        if(resList.Count==0)
                        {
                            
                        }
                    }
                    //无法推送直接导出EXCEL
                    else
                    {

                        if (File.Exists(YGFPExcelPath))
                        {
                            DataTable old_dt = DataTableRenderToExcel.RenderDataTableFromExcel(YGFPExcelPath);
                            dt.Merge(old_dt,true, MissingSchemaAction.Ignore);
                        }
                        int flag = DataTableRenderToExcel.RenderDataTableToExcel(dt, YGFPExcelPath);
                        if (flag == 1)
                        {
                            resList.Add(new BDCFilterResult { IsSuccess = true, Message = "推送成功" });
                        }
                        else
                        {
                            resList.Add(new BDCFilterResult { IsSuccess = false, Message = "推送失败" });
                        }
                    }
                }
                catch(Exception ex)
                {
                    resList.Add(new BDCFilterResult { IsSuccess = false, Message = ex.Message });
                }
                foreach (BDCFilterResult res in resList)
                {
                    MZ_YGFP_TAG tag = new MZ_YGFP_TAG();
                    tag.ID = Guid.NewGuid().ToString();
                    tag.ISSUCCESS = res.IsSuccess ? "1" : "0";
                    tag.MESSAGE = res.Message;
                    tag.PUSHDATE = DateTime.Now;
                    tag.PUSHUSER = param.UserName;
                    tag.SLBH = param.PrjId;
                    try
                    {
                        InsertLog(tag);
                    }
                    catch(Exception ex)
                    {
                        string str = ex.Message;
                    }
                }
                return resList[0];
            }

            return new BDCFilterResult { IsSuccess = true, Message = "OK" };
        }

        private static void InsertLog(MZ_YGFP_TAG tag)
        {
            if (YGFPDAL.ExistTag(tag))
            {
                YGFPDAL.UpdateTag(tag);
            }
            else
            {
                YGFPDAL.InsertLog(tag);
            }
        }

       

        private static ReturnValue SendPost(string paramStr)
        {
            try
            {
                HttpHelper hp = new HttpHelper();

                string date = paramStr;// JsonConvert.SerializeObject(m);
                byte[] data_byte = Encoding.UTF8.GetBytes(date);
                string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

                HttpItem item = new HttpItem()
                {
                    URL = InterFaceUrl,//URL     必需项
                    Method = "post",
                    UserAgent = UserAgent,
                    ContentType = "application/x-www-form-urlencoded",//"application/x-www-form-urlencoded",//"application/json;charset=utf-8",
                    //Accept = "application/json",
                    //ResultType = ResultType.String,
                    PostDataType = PostDataType.Byte,
                    PostdataByte = data_byte

                };

                //得到HTML代码

                HttpResult result = hp.GetHtml(item);


                return JsonConvert.DeserializeObject<ReturnValue>(result.Html);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetMd5(YGFPModel model)
        {
            string data = (model.UserID + model.ANum + model.IDCard + model.HouseArea + model.HousePosition + model.BuyDate.ToString("yyyy-MM-dd") + (model.HouseMoney == null ? "0" : model.HouseMoney) + YGFPKEY).Replace(" ", "");
            return  MyEncrypt.MD5Encrypt32(data);
        }

        private static string GetParams(YGFPModel model)
        {
            model.MD5 = GetMd5(model);
            string param = "parameters=Function={0}|UserID={1}|ANum={2}|IDCard={3}|HouseArea={4}|HousePosition={5}|BuyDate={6}|HouseMoney={7}|MD5={8}";
            param = string.Format(param, model.Function, model.UserID, model.ANum, model.IDCard, model.HouseArea, model.HousePosition, model.BuyDate.ToString("yyyy-MM-dd"), model.HouseMoney==null?"0":model.HouseMoney, model.MD5);
            return param.Replace(" ","");
        }

        private static DataTable GetYGFPModel(string prjId,string zjhm)
        {
            return YGFPDAL.GetBDCInfo(prjId,zjhm);
        }
    }
}