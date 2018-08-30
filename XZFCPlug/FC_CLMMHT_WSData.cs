using Geo.Plug.DataExchange.XZFCPlug;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class FC_CLMMHT_WSData : WebServiceDataBase
    {
        public FC_CLMMHT_WSData(string add, string excuteCode) : base(add, "FC_CLMMHT_TMP", excuteCode) { }

        protected override void CreateParameters(DbCommand command, DataTable data, int index)
        {
            foreach (DataColumn dc in data.Columns)
            {
                DbType dbtype = DbType.String;
                DbParameter p = null;
                #region pp

                switch (dc.ColumnName) {
                    case "HTID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTID";
                            p.Value = data.Rows[index]["HTID"].ToString().Trim();
                            break;
                        }
                    case "SYQR":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SYQR";
                            p.Value = data.Rows[index]["SYQR"];
                            break;
                        }
                    case "BARPASSNO":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "BARPASSNO";
                            p.Value = data.Rows[index]["BARPASSNO"];
                            break;
                        }
                    case "CQZH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "CQZH";
                            p.Value = data.Rows[index]["CQZH"];
                            break;
                        }
                    case "HID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HID";
                            p.Value = data.Rows[index]["HID"];
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
                    case "CLHTBAH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "CLHTBAH";
                            p.Value = data.Rows[index]["CLHTBAH"];
                            break;
                        }
                    case "HTBASJ":
                        {
                            dbtype =DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTBASJ";
                            if (data.Rows[index]["HTBASJ"] != null && data.Rows[index]["HTBASJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["HTBASJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["HTBASJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FWZL":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FWZL";
                            p.Value = data.Rows[index]["FWZL"];
                            break;
                        }
                    case "FWLX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FWLX";
                            p.Value = data.Rows[index]["FWLX"];
                            break;
                        }
                    case "FWJG":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FWJG";
                            p.Value = data.Rows[index]["FWJG"];
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
                                p.Value = DBNull.Value;
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
                    case "PGJG":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "PGJG";
                            if (data.Rows[index]["PGJG"] != null && data.Rows[index]["PGJG"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["PGJG"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["PGJG"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "JYJG":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JYJG";
                            if (data.Rows[index]["JYJG"] != null && data.Rows[index]["JYJG"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["JYJG"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["JYJG"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "FKLX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FKLX";
                            p.Value = data.Rows[index]["FKLX"];
                            break;
                        }
                    case "DKFS":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DKFS";
                            p.Value = data.Rows[index]["DKFS"];
                            break;
                        }
                    case "FKSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FKSJ";
                            if (data.Rows[index]["FKSJ"] != null && data.Rows[index]["FKSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["FKSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["FKSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "QDSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QDSJ";
                            if (data.Rows[index]["QDSJ"] != null && data.Rows[index]["QDSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QDSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["QDSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "QRSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QRSJ";
                            if (data.Rows[index]["QRSJ"] != null && data.Rows[index]["QRSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QRSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["QRSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "QRQZSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QRQZSJ";
                            if (data.Rows[index]["QRQZSJ"] != null && data.Rows[index]["QRQZSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QRQZSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["QRQZSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "ZHBGTGSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZHBGTGSJ";
                            if (data.Rows[index]["ZHBGTGSJ"] != null && data.Rows[index]["ZHBGTGSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZHBGTGSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["ZHBGTGSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "CXSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "CXSJ";
                            if (data.Rows[index]["CXSJ"] != null && data.Rows[index]["CXSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["CXSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["CXSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "JSSJ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JSSJ";
                            if (data.Rows[index]["JSSJ"] != null && data.Rows[index]["JSSJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["JSSJ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["JSSJ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "SFYX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SFYX";
                            p.Value = data.Rows[index]["SFYX"];
                            break;
                        }
                }
                #endregion
                command.Parameters.Add(p);
            }
        }

        protected override IDictionary<string, string> GetReturnId(BDC bdc)
        {
            //return base.GetReturnId(bdc);
            IDictionary<string, string> ret = new Dictionary<string, string>();
            if (bdc.head.flag != 1)
            {
                return ret;
            }
            if (bdc.data != null && bdc.data.dt != null && bdc.data.dt.Rows.Count > 0)
            {
                DataTable dt = bdc.data.dt;
                string zid = dt.Rows[0]["ZID"].ToString();
                ret.Add("ZID", zid);
                string hid = dt.Rows[0]["HID"].ToString();
                ret.Add("HID", hid);
                string htbah = dt.Rows[0]["CLHTBAH"].ToString();
                ret.Add("HTBAH", htbah);
            }
            return ret;
        }
    }
}
