#if !DEBUG
using Geo.Core;
#endif
using Geo.Plug.DataExchange.XZFCPlug.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
//#if DEBUG
//using Geo.Plug.DataExchange.XZFCPlug;
//using Geo.Core;
//using Geo.Core.Data;
//using Geo.Core.Interop;
//using Geo.Web;  
//#endif

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class DataExchange
#if DEBUG
        //X64
        : Geo.Plug.DataExchange.XZFCPlug.IDataExchange
#else
        //X32
         : Geo.Plug.DataExchange.IDataExchange 
#endif
    {
        //private DbConnection dbConnection {
        //    get {
        //        return DBHelper.Connection;
        //    }
        //}
#region webservice address
        private const string Z_Webservice_Address = "FC_Z";
        private const string H_Webservice_Address = "FC_H";
        private const string SPFYGHT_Webservice_Address = "FC_SPFYGHT";
        private const string CLMMHT_Webservice_Address = "CLF_FC_CLMMHT";
        private const string GFQLRXX_Webservice_Address = "FC_GFQLRXX";
        private const string SPF_FC_CLMMHT_Webservice_Address = "SPF_FC_CLMMHT";
        //public static string Webservice_Address = "http://192.168.100.80/BDCWSPub/BDCSrv.asmx";
        public static string Webservice_Address = ConfigurationManager.AppSettings["Webservice_Address"].ToString();
#endregion

        #region IDataExchange Extensions

        public string DigitalSign(string userName, string userKey) {
            return string.Empty; 
        }
        public string DataExtort(IDictionary<string, string> dicParam) {
            //return "200011220000";
            if (!dicParam.ContainsKey("ExecuteCode")) { 
                return string.Empty;
            } 
            string executeCode = dicParam["ExecuteCode"];
            string pch=string.Empty;
            dicParam = dicParam.Select(s => s).Where(s => !string.IsNullOrEmpty(s.Value)).ToDictionary(x => x.Key, y => y.Value); 
            try
            {
                switch (executeCode)
                {
                    case "0000"://批量获取楼盘信息
                        {
                            pch = GetBuildingInfos(dicParam);
                        }
                        break;
                    case "1000"://获取预告商品房交易信息
                        {
                            if (dicParam.ContainsKey("HTBH") && !dicParam.ContainsKey("SPFHTBAH"))
                            {
                                dicParam.Add("SPFHTBAH", dicParam["HTBH"]);
                            }
                             pch = GetForwardPurchasingCommodityHousesInfos(dicParam);
                        }
                        break;
                    case "2000":
                        {
                            if (dicParam.ContainsKey("HTBH") && !dicParam.ContainsKey("CLHTBAH"))
                            {
                                dicParam.Add("CLHTBAH", dicParam["HTBH"]);
                            }
                            pch = GetStockHousesInfos(dicParam);
                        }
                        break;
                    case "3000"://获取查封信息
                        {
                            if(dicParam.ContainsKey("YWZH"))
                            {
                                pch = GetCFInfo(dicParam);
                            }
                  
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                ILog log = new ErrorLog(typeof(DataExchange));
                log.WriteLog(ex);
                throw new Exception(ex.Message);
#endif
            }
            return pch;
        }

        private string GetCFInfo(IDictionary<string, string> dicParam)
        {
            throw new NotImplementedException();
        }

        public string DataDelivery(IDictionary<string, string> dicParam) {
            return string.Empty;
        }

        #endregion
        private string GetBuildingInfos(IDictionary<string, string> dicParam)
        {
            IGetDataBase Z_Data = new FC_Z_WSData(Z_Webservice_Address,"0000"); 
            if(dicParam.ContainsKey("ExecuteCode")){
                dicParam.Remove("ExecuteCode");
            }
            string pch= CreatePCH();
            using (DbConnection dbConnection= DBHelper.Connection) {
                dbConnection.Open();
                try
                {
                    if (!dicParam.ContainsKey("ZID"))//如果不是取单独幢
                    {
                        IList<string> ParamNeed = new List<string>();
                        Z_Data.Data2DB(dicParam, ParamNeed, dbConnection, pch);
                    }
                    else
                    {
                        IList<string> ParamNeed = new List<string>();
                        ParamNeed.Add("ZID");
                        IDictionary<string, string> ids = Z_Data.Data2DBAndReturnId(dicParam, ParamNeed, dbConnection, pch);
                        if (ids.ContainsKey("ZID"))//获取户的信息
                        {
                            ///zid用来在
                            string zid = GuidChange.Change2Without_(ids["ZID"]);
                            //string zid = ids["ZID"];
                            //string nowZid = ids.ContainsKey("NEWZID") && !string.IsNullOrEmpty(ids["NEWZID"]) ? ids["ZID"] : string.Empty;
                            IDictionary <string, string> hParam = new Dictionary<string, string>();
                            hParam.Add("ZID", zid);
                            //ILog log = new ErrorLog(typeof(FC_H_WSData));
                            //log.WriteLog(new Exception("ZID:" + zid));
                            IGetDataBase H_Data = new FC_H_WSData(H_Webservice_Address, Z_Webservice_Address, "0000");
                            H_Data.Data2DB(hParam, null, dbConnection, pch);
                        }
                    }
                }
                catch (Exception ex) { 
                    throw new Exception(ex.Message);
                } 
                finally {
                    dbConnection.Close();
                }
                
            } 
            return pch;
        }
        /// <summary>
        /// 获取新建商品房信息
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        private string GetForwardPurchasingCommodityHousesInfos(IDictionary<string, string> dicParam)
        {
            IGetDataBase ht_Data = new FC_SPFYGHT_WSData(SPFYGHT_Webservice_Address,"1000");
            IList<string> ParamNeed = new List<string>();
            ParamNeed.Add("HTBH");

            if (dicParam.ContainsKey("ExecuteCode"))
            {
                dicParam.Remove("ExecuteCode");
            }
            string pch = CreatePCH();

            using (DbConnection dbConnection = DBHelper.Connection)
            {
                dbConnection.Open();
                try
                {
                    IDictionary<string, string> ids = ht_Data.Data2DBAndReturnId(dicParam, ParamNeed, dbConnection, pch);
                    if (ids.ContainsKey("HTBAH") && !string.IsNullOrEmpty(ids["HTBAH"]))
                    {//获取权利人信息
                        string htid = ids["HTBAH"].Trim();
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("HTBAH", htid);
                        IGetDataBase H_Data = new FC_GFQLRXX_WSData(SPF_FC_CLMMHT_Webservice_Address, "1000");
                        H_Data.Data2DB(hParam, null, dbConnection, pch);
                    }
                    if (ids.ContainsKey("ZID") && !string.IsNullOrEmpty(ids["ZID"]) && ids.ContainsKey("HID") && !string.IsNullOrEmpty(ids["HID"]))
                    {
                        IDictionary<string, string> para4DataFromDB = new Dictionary<string, string>();
                        para4DataFromDB.Add("HID", GuidChange.Change2With_(ids["HID"]));
                        para4DataFromDB.Add("ZID", GuidChange.Change2With_(ids["ZID"]));
                        if (ids.ContainsKey("LPBH"))
                            para4DataFromDB.Add("LPBH", ids["LPBH"]);
                        if (ids.ContainsKey("FJBM"))
                            para4DataFromDB.Add("FJBM", ids["FJBM"]);
                        IGetDataBase DB_Data = new GetBDCData();//先从自己数据库中取数据
                        if (!string.IsNullOrEmpty(DB_Data.Data2DB(para4DataFromDB, null, dbConnection, pch)))
                        {
                            return pch;
                        }
                    }
                    if (ids.ContainsKey("ZID") && !string.IsNullOrEmpty(ids["ZID"]))
                    {//获取幢信息
                        string zid = GuidChange.Change2Without_(ids["ZID"]);
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("ZID", zid);
                        if (ids.ContainsKey("LPBH"))
                            hParam.Add("LPBH", ids["LPBH"]);
                        IGetDataBase Z_Data = new FC_Z_WSData(Z_Webservice_Address, "1000");
                        Z_Data.Data2DB(hParam, null, dbConnection, pch);
                    }
                    if (ids.ContainsKey("HID") && !string.IsNullOrEmpty(ids["HID"]))
                    {//获取户信息
                        string hid = GuidChange.Change2Without_(ids["HID"]);
                        string zid = GuidChange.Change2Without_(ids["ZID"]);
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("HID", hid);
                        hParam.Add("ZID", zid);
                        if (ids.ContainsKey("FJBM"))
                            hParam.Add("FJBM", ids["FJBM"]);
                        IGetDataBase H_Data = new FC_H_WSData(H_Webservice_Address, Z_Webservice_Address, "1000");
                        H_Data.Data2DB(hParam, null, dbConnection, pch);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    dbConnection.Close();
                } 
            }
            return pch;
        }
        /// <summary>
        /// 存量房
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        private string GetStockHousesInfos(IDictionary<string, string> dicParam)
        {
            IGetDataBase ht_Data = new FC_CLMMHT_WSData(CLMMHT_Webservice_Address,"2000");
            IList<string> ParamNeed = new List<string>();
             ParamNeed.Add("HTBH");
            //dicParam = new Dictionary<string, string>();
            //dicParam["CQZH"] = "159061";
            //dicParam["SYQR"] = "王建军";
            if (dicParam.ContainsKey("ExecuteCode"))
            {
                dicParam.Remove("ExecuteCode");
            }
            string pch = CreatePCH();

            using (DbConnection dbConnection = DBHelper.Connection)
            {
                dbConnection.Open();
                try {
                    IDictionary<string, string> ids = ht_Data.Data2DBAndReturnId(dicParam, ParamNeed, dbConnection, pch);
                    if (ids.ContainsKey("HTBAH") && !string.IsNullOrEmpty(ids["HTBAH"]))
                    {//HTBAH=201601110003 获取权利人信息
                        string htid = ids["HTBAH"];
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("HTBAH", htid); 
                        IGetDataBase H_Data = new FC_GFQLRXX_WSData(GFQLRXX_Webservice_Address, "2000");
                        H_Data.Data2DB(hParam, null, dbConnection, pch);
                    }

                    IDictionary<string, string> para4DataFromDB = new Dictionary<string, string>();
                    if (ids.ContainsKey("ZID") && !string.IsNullOrEmpty(ids["ZID"]))
                    {
                        para4DataFromDB.Add("HID", GuidChange.Change2With_(ids["HID"]));
                        para4DataFromDB.Add("ZID", GuidChange.Change2With_(ids["ZID"]));
                        IGetDataBase DB_Data = new GetBDCData();//先从自己数据库中取数据
                        if (!string.IsNullOrEmpty(DB_Data.Data2DB(para4DataFromDB, null, dbConnection, pch)))
                        {
                            return pch;
                        }
                        //获取Z信息
                        string zid = GuidChange.Change2Without_(ids["ZID"]);
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("ZID", zid);
                        IGetDataBase Z_Data = new FC_Z_WSData(Z_Webservice_Address, "2000");
                        Z_Data.Data2DB(hParam, null, dbConnection, pch);
                    }
                    if (ids.ContainsKey("HID") && !string.IsNullOrEmpty(ids["HID"]))
                    {//获取H信息
                        string hid = GuidChange.Change2Without_(ids["HID"]);
                        string zid = GuidChange.Change2Without_(ids["ZID"]);
                        IDictionary<string, string> hParam = new Dictionary<string, string>();
                        hParam.Add("HID", hid);
                        hParam.Add("ZID", zid);
                        IGetDataBase H_Data = new FC_H_WSData(H_Webservice_Address, Z_Webservice_Address, "2000");
                        H_Data.Data2DB(hParam, null, dbConnection, pch);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    dbConnection.Close();
                }
               
            }
            return pch;
        }

        /// <summary>
        /// 获取批次号
        /// </summary>
        /// <returns></returns>
        private string CreatePCH()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
