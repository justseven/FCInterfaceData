 
using Geo.Plug.DataExchange.XZFCPlug.Dal;
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
using System.Text.RegularExpressions;
using System.Xml;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class FC_H_WSData : WebServiceDataBase
    {
        string ftpAdd = ConfigurationManager.AppSettings["FtpAddr"].ToString();
        string ftpUser = ConfigurationManager.AppSettings["FtpUser"].ToString();
        string ftpPwd = ConfigurationManager.AppSettings["FtpPwd"].ToString();
        string ftpPort = ConfigurationManager.AppSettings["FtpPort"].ToString();
        string arear= ConfigurationManager.AppSettings["Area"].ToString();
        /// <summary>
        /// 可能传如新的幢ID
        /// </summary>
        /// <param name="add"></param>
        /// <param name="NEWZID"></param>
        public FC_H_WSData(string add, string zadd, string excuteCode)
            : base(add, "FC_H_TMP", excuteCode)
        {
            _zadd = zadd;
        }
        string _zadd;
        protected override void CreateParameters(DbCommand command, DataTable data, int index)
        {
            
            //OleDbParameterCollection ps = new OleDbParameterCollection();
            foreach (DataColumn dc in data.Columns)
            {
                DbType dbtype = DbType.String;
                DbParameter p = null;
                #region pp
                switch (dc.ColumnName)
                {
                    case "HH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HH";
                            p.Value = GetHH(data.Rows[index]);
                            break;
                        }

                    case "HID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HID";
                            p.Value = GetHID(data.Rows[index]);
                            break;
                        }
                    case "HYCID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HYCID";
                            p.Value = data.Rows[index]["HYCID"];
                            break;
                        }
                    case "HSCID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HSCID";
                            p.Value = data.Rows[index]["HSCID"];
                            break;
                        }
                    case "ZID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZID";
                            p.Value = data.Rows[index]["ZID"];
                            break;
                        }
                    case "FWBH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FWBH";
                            p.Value = data.Rows[index]["FWBH"];
                            break;
                        }
                    case "ZH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZH";
                            p.Value = data.Rows[index]["ZH"];
                            break;
                        }
                    case "BDCDYH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "BDCDYH";
                            p.Value = data.Rows[index]["BDCDYH"];
                            break;
                        }
                    case "QLLX":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QLLX";

                            if (data.Rows[index]["QLLX"] != null && data.Rows[index]["QLLX"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QLLX"].ToString()))
                            {
                                //int val = int.Parse(data.Rows[index]["QLLX"].ToString());
                                //p.Value = val;
                                int v = -1;
                                if (int.TryParse(data.Rows[index]["QLLX"].ToString(), out v))
                                {
                                    p.Value = v;
                                }
                                else
                                {
                                    p.Value = DBNull.Value;
                                }
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            if (p.Value == DBNull.Value) {//填入默认值
                                p.Value = 4;
                            }
                            break;
                        }
                    case "QLXZ":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QLXZ";
                            if (arear.Contains("贾汪"))
                            {
                                p.Value = DBNull.Value;
                            }
                            else
                            {
                                p.Value = data.Rows[index]["QLXZ"];
                                if (p.Value == DBNull.Value || string.IsNullOrEmpty(p.Value.ToString()))
                                {
                                    p.Value = "0";
                                }
                            }
                            
                            break;
                        }
                    case "HX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HX";
                            p.Value = data.Rows[index]["HX"];
                            break;
                        }
                    case "HXJG":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HXJG";
                            p.Value = data.Rows[index]["HXJG"];
                            break;
                        }
                    case "ZXCD":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZXCD";
                            p.Value = data.Rows[index]["ZXCD"];
                            break;
                        }
                    case "QLRZS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QLRZS";
                            if (data.Rows[index]["QLRZS"] != null && data.Rows[index]["QLRZS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QLRZS"].ToString()))
                            {
                                //int val = int.Parse(data.Rows[index]["QLRZS"].ToString());
                                //p.Value = val;
                                int v = -1;
                                if (int.TryParse(data.Rows[index]["QLRZS"].ToString(), out v))
                                {
                                    p.Value = v;
                                }
                                else
                                {
                                    p.Value = DBNull.Value;
                                }
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "GHYT":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "GHYT";
                            p.Value = data.Rows[index]["GHYT"];
                            break;
                        }
                    case "ZL":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZL";
                            p.Value = data.Rows[index]["ZL"];
                            break;
                        }
                    case "SJC":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SJC";
                            if (data.Rows[index]["SJC"] != null && data.Rows[index]["SJC"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["SJC"].ToString()))
                            {
                                //int val = int.Parse(data.Rows[index]["SJC"].ToString());
                                //p.Value = val;
                                int v = -1;
                                if (int.TryParse(data.Rows[index]["SJC"].ToString(), out v))
                                {
                                    p.Value = v;
                                }
                                else
                                {
                                    p.Value = DBNull.Value;
                                }
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "MYC":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "MYC";
                            if (data.Rows[index]["MYC"] == null || data.Rows[index]["MYC"] == DBNull.Value || string.IsNullOrEmpty(data.Rows[index]["MYC"].ToString()))
                            {
                                p.Value = data.Rows[index]["SJC"];
                            }
                            else
                                p.Value = data.Rows[index]["MYC"];
                            break;
                        }
                    case "DYH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DYH";
                            p.Value = data.Rows[index]["DYH"];
                            break;
                        }
                    case "FJH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FJH";
                            p.Value = data.Rows[index]["FJH"];
                            break;
                        }
                    case "LJZH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "LJZH";
                            p.Value = data.Rows[index]["LJZH"];
                            break;
                        }
                    case "QDJG":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QDJG";
                            if (data.Rows[index]["QDJG"] != null && data.Rows[index]["QDJG"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QDJG"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["QDJG"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "QDFS":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QDFS";
                            p.Value = data.Rows[index]["QDFS"];
                            break;
                        }
                    case "SHBW":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SHBW";
                            p.Value = data.Rows[index]["SHBW"];
                            break;
                        }
                    case "YCJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCJZMJ";
                            if (data.Rows[index]["YCJZMJ"] != null && data.Rows[index]["YCJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "YCTNJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCTNJZMJ";
                            if (data.Rows[index]["YCTNJZMJ"] != null && data.Rows[index]["YCTNJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCTNJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCTNJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "YCDXBFJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCDXBFJZMJ";
                            if (data.Rows[index]["YCDXBFJZMJ"] != null && data.Rows[index]["YCDXBFJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCDXBFJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCDXBFJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "YCFTJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCFTJZMJ";
                            if (data.Rows[index]["YCFTJZMJ"] != null && data.Rows[index]["YCFTJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCFTJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCFTJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "YCQTJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCQTJZMJ";
                            if (data.Rows[index]["YCQTJZMJ"] != null && data.Rows[index]["YCQTJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCQTJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCQTJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "YCFTXS":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "YCFTXS";
                            if (data.Rows[index]["YCFTXS"] != null && data.Rows[index]["YCFTXS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["YCFTXS"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["YCFTXS"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "JZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JZMJ";
                            if (data.Rows[index]["JZMJ"] != null && data.Rows[index]["JZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["JZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["JZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "TNJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "TNJZMJ";
                            if (data.Rows[index]["TNJZMJ"] != null && data.Rows[index]["TNJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["TNJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["TNJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "FTJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FTJZMJ";
                            if (data.Rows[index]["FTJZMJ"] != null && data.Rows[index]["FTJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FTJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["FTJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "DXBFJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DXBFJZMJ";
                            if (data.Rows[index]["DXBFJZMJ"] != null && data.Rows[index]["DXBFJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["DXBFJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["DXBFJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "QTJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QTJZMJ";
                            if (data.Rows[index]["QTJZMJ"] != null && data.Rows[index]["QTJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QTJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["QTJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FTXS":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FTXS";
                            if (data.Rows[index]["FTXS"] != null && data.Rows[index]["FTXS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FTXS"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["FTXS"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "TDZZRQ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "TDZZRQ";
                            if (data.Rows[index]["TDZZRQ"] != null && data.Rows[index]["TDZZRQ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["TDZZRQ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["TDZZRQ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "TDYT":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "TDYT";
                            p.Value = data.Rows[index]["TDYT"];
                            break;
                        }
                    case "TDSYQR":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "TDSYQR";
                            p.Value = data.Rows[index]["TDSYQR"];
                            break;
                        }
                    case "GYTDMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "GYTDMJ";
                            if (data.Rows[index]["GYTDMJ"] != null && data.Rows[index]["GYTDMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["GYTDMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["GYTDMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FTTDMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FTTDMJ";
                            if (data.Rows[index]["FTTDMJ"] != null && data.Rows[index]["FTTDMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FTTDMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["FTTDMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "DYTDMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DYTDMJ";
                            if (data.Rows[index]["DYTDMJ"] != null && data.Rows[index]["DYTDMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["DYTDMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["DYTDMJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "TCJS":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "TCJS";
                            if (data.Rows[index]["TCJS"] != null && data.Rows[index]["TCJS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["TCJS"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["TCJS"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "CG":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "CG";
                            if (data.Rows[index]["CG"] != null && data.Rows[index]["CG"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["CG"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["CG"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "ZT":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZT";
                            if (data.Rows[index]["ZT"] != null && data.Rows[index]["ZT"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZT"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["ZT"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FCFHT":
                        {
                            dbtype = DbType.Binary;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FCFHT";
                            //只有实测时导入楼盘
                            if (data.Rows[index]["HSCID"] != null&&data.Rows[index]["FCFHT"] != null && data.Rows[index]["FCFHT"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FCFHT"].ToString()))
                            {
                                //Byte[] val = Encoding.UTF8.GetBytes(data.Rows[index]["FCFHT"].ToString());  
                               
                                Byte[] buffer = Convert.FromBase64String(data.Rows[index]["FCFHT"].ToString());
                                if (string.IsNullOrEmpty(ftpAdd))
                                {
                                    p.Value = buffer;
                                }
                                else
                                {

                                    string fileName = getNeedFileName(data.Rows[index]["HID"].ToString());
                                    /************************************测试用**************************************************/
                                    InsertFCFHT(fileName, ftpAdd, ftpUser, ftpPwd, ftpPort, buffer, data.Rows[index]["HID"].ToString(), data.Rows[index]["ZID"].ToString());
                                }
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FJSM":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FJSM";
                            if (data.Rows[index]["FJSM"] != null && data.Rows[index]["FJSM"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FJSM"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["FJSM"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FJBM":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FJBM";
                            p.Value = data.Rows[index]["FJBM"];
                            break;
                        }
                    case "DYMC":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DYMC";
                            p.Value = data.Rows[index]["DYMC"];
                            break;
                        }
                    case "QM":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QM";
                            p.Value = data.Rows[index]["QM"];
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                #endregion
                if (p != null)
                    command.Parameters.Add(p);
            }
        }

        private object GetHH(DataRow row)
        {
            if(arear.Contains("贾汪"))
            {
                return "";
            }
            if (!string.IsNullOrEmpty(row["HH"].ToString()))
            {
                return row["HH"].ToString();
            }
            return row["FJH"].ToString();
        }

        private object GetHID(DataRow row)
        {
            if(!string.IsNullOrEmpty(row["HID"].ToString()))
            {
                return row["HID"].ToString();
            }
            return !string.IsNullOrEmpty(row["HYCID"].ToString())
                                ? row["HYCID"].ToString() : row["HSCID"].ToString();
        }

        protected override string PushData2DB(BDC bdc, DbConnection conn, string pch)
        {
            //return base.PushData2DB(bdc, conn, pch);
            if (bdc.data == null|| bdc.data.dt==null) {
                return pch;
            }
            DataTable dt = bdc.data.dt;
            if (dt != null && dt.Rows.Count > 0)
            {
                string zidinbdc = TransZID(dt.Rows[0]["ZID"].ToString());

                //if(!dt.Columns.Contains("QM"))
                //{
                //    dt.Columns.Add("QM");
                //}
                if (!dt.Columns.Contains("HH"))
                {
                    dt.Columns.Add("HH");
                }
                if (!dt.Columns.Contains("HSCID"))
                {
                    dt.Columns.Add("HSCID");
                }
                if (!dt.Columns.Contains("HID"))
                {
                    dt.Columns.Add("HID");
                }
                if (!dt.Columns.Contains("DYMC")) {
                    dt.Columns.Add("DYMC");
                }
                if (!dt.Columns.Contains("QLXZ"))
                {
                    dt.Columns.Add("QLXZ");
                }
                //HH ,bdcdyh,
                if (string.IsNullOrEmpty(zidinbdc))
                    FtpCheckDirectoryExist("ftp://" + ftpAdd + ":" + ftpPort, "FC_H/" + GuidChange.Change2With_(dt.Rows[0]["ZID"].ToString()) + "/", ftpUser, ftpPwd);
                else
                {
                    FtpCheckDirectoryExist("ftp://" + ftpAdd + ":" + ftpPort, "FC_H/" + zidinbdc + "/", ftpUser, ftpPwd);
                }
                //获取FC_H_QSDC里HH最大值
                int hh_tmp = GetMaxHH(string.IsNullOrEmpty(zidinbdc) ? GuidChange.Change2With_(dt.Rows[0]["ZID"].ToString()):zidinbdc);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Check_H_InBDC(dt.Rows[i], zidinbdc))
                    {
                        DataTable H_QSDC = Get_H_InBDC(dt.Rows[i], zidinbdc);
                        foreach (DataColumn col in H_QSDC.Columns)
                        {
                            try
                            {
                                if (col.ColumnName.ToUpper().Equals("TSTYBM"))
                                {
                                    dt.Rows[i]["HID"] = H_QSDC.Rows[0][col];
                                }
                                if (col.ColumnName.ToUpper().Equals("LSZTYBM"))
                                {
                                    dt.Rows[i]["ZID"] = H_QSDC.Rows[0][col];
                                }
                                if (col.ColumnName.ToUpper().Equals("TSTYBM"))
                                {
                                    dt.Rows[i]["HID"] = H_QSDC.Rows[0][col];
                                }
                                if (col.ColumnName.ToUpper().Equals("HH"))
                                {
                                    if (arear.Contains("贾汪"))
                                    {
                                        dt.Rows[i][col.ColumnName] = "";
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(H_QSDC.Rows[0][col].ToString()))
                                        {
                                            dt.Rows[i][col.ColumnName] = ++hh_tmp;
                                        }
                                        else
                                        {
                                            dt.Rows[i][col.ColumnName] = H_QSDC.Rows[0][col];
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                continue;
                            }
                            
                        }
                        if(dt.Rows[i]["ZL"].ToString().Contains("3-123"))
                        {
                            string s = "";
                        }
                        dt.Rows[i]["GHYT"] = ChangeYT(dt.Rows[i]["GHYT"].ToString());
                    }
                    else
                    {
                        Guid GHid = Guid.Parse(dt.Rows[i]["HID"].ToString());
                        dt.Rows[i]["HID"] = GuidChange.Change2With_(dt.Rows[i]["HID"].ToString());//GHid.ToString("D");
                        Guid GZid = Guid.Parse(dt.Rows[i]["ZID"].ToString());
                        if (string.IsNullOrEmpty(zidinbdc))
                            dt.Rows[i]["ZID"] = GuidChange.Change2With_(dt.Rows[i]["ZID"].ToString());//GZid.ToString("D"); 
                        else
                            dt.Rows[i]["ZID"] = zidinbdc;

                        if (dt.Columns.Contains("HH"))
                        {
                            if (arear.Contains("贾汪"))
                            {
                                dt.Rows[i]["HH"] = "";
                            }
                            else
                            {
                                dt.Rows[i]["HH"] = ++hh_tmp;
                            }
                            /*
                            string hh = GetHH_FromQSDC(dt.Rows[i]["ID"].ToString());
                            if (string.IsNullOrEmpty(hh))
                            {
                                dt.Rows[i]["HH"] = ++hh_tmp;
                            }
                            else
                            {
                                dt.Rows[i]["HH"] = hh;
                            }
                            */
                        }

                        if (dt.Columns.Contains("HYCID"))
                        {
                            dt.Rows[i]["HYCID"] = string.IsNullOrEmpty(dt.Rows[i]["HYCID"].ToString()) ? "" : GuidChange.Change2With_(dt.Rows[i]["HYCID"].ToString());//GHid.ToString("D");
                        }
                        else
                        {
                            dt.Columns.Add("HYCID");
                            dt.Rows[i]["HYCID"] = "";
                        }
                        if (dt.Columns.Contains("HSCID"))
                        {
                            dt.Rows[i]["HSCID"] = dt.Columns.Contains("HSCID") && string.IsNullOrEmpty(dt.Rows[i]["HSCID"].ToString()) ? "" : GuidChange.Change2With_(dt.Rows[i]["HSCID"].ToString());//GHid.ToString("D");

                        }
                        else
                        {
                            dt.Columns.Add("HSCID");
                            dt.Rows[i]["HSCID"] = "";
                        }
                        dt.Rows[i]["DYMC"] = dt.Columns.Contains("DYH") && string.IsNullOrEmpty(dt.Rows[i]["DYH"].ToString()) ? "" : dt.Rows[i]["DYH"].ToString() + "单元";
                        //string hid = dt.Rows[i]["HID"].ToString();
                        if (HIDFindExist(dt.Rows[i]["HYCID"].ToString()))//如果预测ID已经到数据库中
                        { //如果不存在先判断坐落，再判断八位编码 
                          //string ycId = YCIDFind(dt.Rows[i]["ZID"].ToString(), dt.Rows[i]["FJBM"].ToString(), dt.Rows[i]["ZL"].ToString(), pch);
                          //if (!string.IsNullOrEmpty(ycId))
                          //{ //无预测信息
                          //    dt.Rows[i]["HSCID"] = dt.Rows[i]["HID"];
                          //    dt.Rows[i]["HID"] = ycId;
                          //}  
                            dt.Rows[i]["HID"] = dt.Rows[i]["HYCID"];
                            if (HIDFindExist(dt.Rows[i]["HSCID"].ToString()))
                            {//说明预测和实测都在数据库中，应该提示出来
                                setHouseForbidInfo(dt.Rows[i]["HSCID"].ToString(), "该房屋的预测信息和实测信息分为两条记录存储在数据库中。请联系管理员");
                                setHouseForbidInfo(dt.Rows[i]["HYCID"].ToString(), "该房屋的预测信息和实测信息分为两条记录存储在数据库中。请联系管理员");
                            }
                        }
                        else if (HIDFindExist(dt.Rows[i]["HSCID"].ToString()))
                        {//如果实测ID已经到数据库中
                            dt.Rows[i]["HID"] = dt.Rows[i]["HSCID"];
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["HYCID"].ToString()))
                            {
                                dt.Rows[i]["HID"] = dt.Rows[i]["HYCID"];
                            }

                            if (string.IsNullOrEmpty(dt.Rows[i]["HID"].ToString()))
                            {
                                //如果不存在先判断坐落，再判断八位编码 ，进而判断是不是重复数据
                                if (dt.Columns.Contains("FJBM"))
                                {
                                    string ycId = YCIDFind(dt.Rows[i]["ZID"].ToString(), dt.Rows[i]["FJBM"].ToString(), dt.Rows[i]["ZL"].ToString(), pch);
                                    dt.Rows[i]["HID"] = ycId;
                                    setHouseForbidInfo(ycId, "该房屋的ID可能和测管不一致。请联系管理员");
                                }
                                else
                                {
                                    dt.Columns.Add("FJBM");
                                }
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["HID"].ToString()))
                            {
                                dt.Rows[i]["HID"] = dt.Rows[i]["HSCID"].ToString();
                            }
                        }
                        dt.Rows[i]["GHYT"] = ChangeYT(dt.Rows[i]["GHYT"].ToString());
                    }
                }
            }
            return base.PushData2DB(bdc, conn, pch);
        }

        private DataTable Get_H_InBDC(DataRow dataRow, string zid)
        {

            string hid = GetHIDString(dataRow);
            string sql = "select * from fc_h_qsdc where (tstybm='{0}' or tstybm='{1}') or (lsztybm='{2}' and fjbm='{3}') ";
            sql = string.Format(sql, GuidChange.Change2With_(dataRow["HID"].ToString()), hid, string.IsNullOrEmpty(zid) ? GuidChange.Change2With_(dataRow["ZID"].ToString()) : zid, dataRow["FJBM"].ToString());
            return DBHelper.GetDataTable(sql);
        }

        private bool Check_H_InBDC(DataRow dataRow,string zid)
        {
            string hid = GetHIDString(dataRow);
            string sql = "select count(1) from fc_h_qsdc where (tstybm='{0}' or tstybm='{1}') or (lsztybm='{2}' and fjbm='{3}') ";
            sql = string.Format(sql, GuidChange.Change2With_(dataRow["HID"].ToString()), hid, string.IsNullOrEmpty(zid)?GuidChange.Change2With_(dataRow["ZID"].ToString()):zid,dataRow["FJBM"].ToString());
            object o = DBHelper.GetScalar(sql);
            try { 
                int count= Convert.ToInt32(o);
                if (count > 0)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        private string GetHIDString(DataRow row)
        {
            try
            {
                return GuidChange.Change2With_(row["HYCID"].ToString());
            }
            catch
            {
                try
                {
                    return GuidChange.Change2With_(row["HSCID"].ToString());
                }
                catch
                {
                    return GuidChange.Change2With_(row["HID"].ToString());
                }
            }
        }

        private int GetMaxHH(string zidinbdc)
        {
            if (string.IsNullOrEmpty(zidinbdc))
                return 0;
            string sql = string.Format("select max(hh) from fc_h_qsdc where lsztybm='{0}'", zidinbdc);
            object o = DBHelper.GetScalar(sql);
            if(!(o == DBNull.Value || o == null))
            {
                return Convert.ToInt32(o);
            }
            return 0;
        }

        private string GetHH_FromQSDC(string hid)
        {
            if (string.IsNullOrEmpty(hid))
                return "";
            string sql = string.Format("select hh from fc_h_qsdc where tstybm='{0}'", hid);
            object o = DBHelper.GetScalar(sql);
            if (!(o == DBNull.Value || o == null))
            {
                return o.ToString();
            }
            return "";
        }

        public override IDictionary<string, string> Data2DBAndReturnId(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            if (ps.ContainsKey("ZID"))
            {
                string idInCG = GetCGZID(ps["ZID"]);
                if (!string.IsNullOrEmpty(idInCG))
                {
                    ps["ZID"] = GuidChange.Change2Without_(idInCG);
                }
            }
            return base.Data2DBAndReturnId(ps, paramNeeded, connection, pch);
        }

        public override string Data2DB(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            //先看看zid在对应关系中有无
            if (ps.ContainsKey("ZID"))
            {
                string idInCG = GetCGZID(ps["ZID"]);
                if (!string.IsNullOrEmpty(idInCG))
                {
                    ps["ZID"] = GuidChange.Change2Without_(idInCG);
                }
            }
            BDC bdc = null;
            try
            {
                string webServiceData = GetWebServiceData(ps, paramNeeded);//获取webservice数据
                bdc = XMLParsing(webServiceData);
            }
            catch (Exception ex)
            {
                bdc = new BDC();
                bdc.head = new Head { flag = 0, msg = ex.Message };
            }
            return PushData2DB(bdc, connection, pch);
        }

        /// <summary>
        /// 如果数据库中已经有幢ID，将其转成数据库中的幢ID
        /// </summary>
        /// <returns></returns>
        private string TransZID(string zidpush)
        {
            FC_Z_WSData z = new FC_Z_WSData(_zadd, this._excuteCode);
            IDictionary<string, string> ps = new Dictionary<string, string>();
            ps.Add("ZID", zidpush);
            string webServiceData = z.GetWebServiceData(ps, null);//获取webservice数据
            BDC bdc = XMLParsing(webServiceData);
            DataTable dt = bdc.data.dt;
            if (dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("LPBH")) {
                    string tm = z.LPBHFindExist(dt.Rows[0]["LPBH"].ToString(), dt.Rows[0]["XMMC"].ToString());
                    return tm;
                } 
            }
            return string.Empty;
        }


        /// <summary>
        /// 查看hid是否在数据库的FC_H_QSDC中
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        private bool HIDFindExist(string hid)
        {
            if (string.IsNullOrEmpty(hid)) {
                return false;
            }
            string sql = string.Format("select 1 from fc_h_qsdc where tstybm='{0}'", hid);
            object o = DBHelper.GetScalar(sql);
            return !(o == DBNull.Value || o == null);
        }
        /// <summary>
        /// 根据幢id和八位编码查询
        /// </summary>
        /// <param name="zid"></param>
        /// <param name="code8"></param>
        /// <returns></returns>
        private string YCIDFind(string zid, string code8, string zl, string pch)
        {
            string sql = string.Format(@"select h.tstybm from fc_h_qsdc h 
                left join fc_z_qsdc z on h.lsztybm=z.tstybm where h.FJBM ='{0}' and z.TSTYBM='{1}'", code8, zid);
            object o = DBHelper.GetScalar(sql);
            if (o != null && o != DBNull.Value)
            {
                return o.ToString();
            }

            string zxmmcsql = string.Format("select XMMC from FC_Z_TMP where pch='{0}' and Rownum=1", pch);
            object xmmco = DBHelper.GetScalar(zxmmcsql);
            string xmmcs = xmmco.ToString();
            string szl = zl.Trim().Replace(" ", "").Replace("号楼", "#").Replace("号", "#").Replace("单元", "-").Replace("贾汪区", "").Replace("室", "").Replace("##", "#");
            if (!string.IsNullOrEmpty(xmmcs) && szl.Contains(xmmcs))
            {
                szl = szl.Substring(szl.IndexOf(xmmcs));
            }
            string zlsql = string.Format(@"select h.tstybm from fc_h_qsdc h where replace(replace(replace(replace(replace(replace(replace(zl,'号楼','#'),'号','#'),'单元','-'),'贾汪区',''),'室',''),'##','#'),' ','') like '%{0}'", szl);
            object zlo = DBHelper.GetScalar(zlsql);
            if (zlo != null && zlo != DBNull.Value)
            {
                return zlo.ToString();
            }

            return string.Empty;
        }

        private string getNeedFileName(string hid)
        {
            //DateTime dt = DateTime.Now;
            //string nowYear = dt.Year.ToString();
            //string nowMonth = dt.Month.ToString();
            //string nowDay = dt.Day.ToString();
            //string nowHour = dt.Hour.ToString();
            //string nowMinute = dt.Minute.ToString();
            //string nowSecond = dt.Second.ToString();
            //if (dt.Month < 10)
            //{
            //    nowMonth = "0" + dt.Month.ToString();
            //}
            //if (dt.Day < 10)
            //{
            //    nowDay = "0" + dt.Day.ToString();
            //}
            //if (dt.Hour < 10)
            //{
            //    nowHour = "0" + dt.Hour.ToString();
            //}
            //if (dt.Minute < 10)
            //{
            //    nowMinute = "0" + dt.Minute.ToString();
            //}
            //if (dt.Second < 10)
            //{
            //    nowSecond = "0" + dt.Second.ToString();
            //}
            //string newFileName = nowYear + nowMonth + nowDay + nowHour + nowMinute + nowSecond;
            return hid + ".jpg";
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        private string ChangeYT(string ghyt)
        {
            string input = ghyt.Trim();
            string ytmc = string.Empty;

            string sql = @" SELECT TO_CHAR(MC)  FROM ( 
          SELECT WM_CONCAT(TO_CHAR(YTBM)) AS MC FROM DIC_FWYTLX WHERE YTMC IN ( 
              SELECT DLBM2 FROM ( 
                  SELECT DISTINCT REGEXP_SUBSTR('{0}','[^,]+',1,LEVEL) AS DLBM2 
                  FROM DUAL CONNECT BY REGEXP_SUBSTR('{0}','[^,]+',1,LEVEL) IS NOT NULL ORDER BY 1 
              ) 
          ) AND DICCODE IN (SELECT DICCODE FROM DIC_MAIN WHERE DICTYPE = '房屋用途类型' AND DICNAME = '房屋用途类型') 
      )";
            try
            {
                sql = string.Format(sql, input);
                object o = DBHelper.GetScalar(sql);
                if (o != null && o != DBNull.Value)
                {
                    ytmc = o.ToString();
                }
                else
                {
                    ytmc = "80";
                }

                if (arear.Contains("贾汪"))
                {
                    if (!ytmc.Equals("10"))
                    {
                        ytmc = "";
                    }
                }
            }
            catch
            {
                ytmc = "";
            }

            return ytmc;
           
        }

        /// <summary>
        /// 根据不动产这边的幢id去获取测管的幢id
        /// </summary>
        /// <param name="bdcZID"></param>
        /// <returns></returns>
        private string GetCGZID(string bdcZID)
        {
            bdcZID = GuidChange.Change2With_(bdcZID);
            string sql = string.Format("select ZIDINCG from FC_ZIDGL where DBCZID='{0}'", bdcZID);
            object o = DBHelper.GetScalar(sql);
            if (o == null)
            {
                return string.Empty;
            }
            else
            {
                return o.ToString();
            }
        }
        /// <summary>
        /// 插入分层分户图
        /// </summary>
        private void InsertFCFHT(string fileName, string ftpAdd, string ftpUser, string ftpPassword, string ftpPort, byte[] buffer, string hid, string ZID)
        {
            string ftpHead;
            if (string.IsNullOrEmpty(ftpPort))
                ftpHead = "ftp://" + ftpAdd;
            else
            {
                ftpHead = "ftp://" + ftpAdd + ":" + ftpPort;
            }
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
                                                                             values(:attachid,:fileid,'户','{0}','分层分户草图','jpg','权集',sysdate,0)", hid);
#if DEBUG
            string connectionstring = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
            DbConnection connection = new Oracle.DataAccess.Client.OracleConnection(connectionstring);
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
                DbConnection wdconnection = new Oracle.DataAccess.Client.OracleConnection(connectionwdstring);

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
                DbConnection wdconnection2 = new Oracle.DataAccess.Client.OracleConnection(connectionwdstring);
                DbCommand wdcommand2 = wdconnection2.CreateCommand();
                wdcommand2.Connection = wdconnection2;
                DBHelper.ExecuteNonQuery(insertWDKSql2, wdcommand2);
            }
        }


        /// <summary
        /// 文件存在检查
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool fileCheckExist(string url, string ftpName, string ftpUser, string ftpPassword)
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
                    catch (Exception ex)
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
            FtpWebRequest req = GetRequest(ftphead + curDir, ftpUserID, ftpPassword);//(FtpWebRequest)WebRequest.Create(ftphead + curDir);
            //req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        private static FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(URI);
            //提供身份验证信息
            if (username != "")
                result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }

        /// <summary>
        /// 设置房屋的禁止信息
        /// </summary>
        public static void setHouseForbidInfo(string hid,string xx) {
            string sql = string.Format("Update FC_H_QSDC Set BGJZXX='{1}' where TSTYBM='{0}'",hid,xx);
            DBHelper.ExecuteNonQuery(sql);
        }
    }
}
