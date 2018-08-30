using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using XZFCPlug;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class FC_SPFYGHT_WSData : WebServiceDataBase
    {
        private string hid_temp = string.Empty; 
        private string zid_temp = string.Empty;
        public FC_SPFYGHT_WSData(string add,string excuteCode)
            : base(add, "FC_SPFYGHT_TMP", excuteCode)
        {
            
        }
        protected override void CreateParameters(DbCommand command, DataTable data, int index)
        {
            foreach (DataColumn dc in data.Columns)
            {
                DbType dbtype = DbType.String;
                DbParameter p = null;
                #region pp

                switch (dc.ColumnName)
                {
                    case "HTID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTID";
                            p.Value = data.Rows[index]["HTID"].ToString().Trim();
                            break;
                        }
                    case "HID"://把BuildingNo|FactBuildingNO_RoomNo转成hid    //TMD又改回去了
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HID";
                            //if (!string.IsNullOrEmpty(data.Rows[index]["HID"].ToString()))
                            //{
                            //    p.Value = GetHidTemp(data,index);
                            //}
                            //else
                            //    p.Value = DBNull.Value;
                            p.Value = data.Rows[index]["HID"];
                            break;
                        }
                    case "ZID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZID";
                            //if (!string.IsNullOrEmpty(data.Rows[index]["ZID"].ToString()))
                            //{
                            //    p.Value = GetZidTemp(data, index);
                            //}
                            //else
                            //    p.Value = DBNull.Value;
                            p.Value = data.Rows[index]["ZID"];
                            break;
                        }
                    case "SPFHTBAH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SPFHTBAH";
                            p.Value = data.Rows[index]["SPFHTBAH"];
                            break;
                        }
                    case "HTBASJ":
                        {
                            dbtype = DbType.DateTime;
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
                    case "HTBH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTBH";
                            p.Value = data.Rows[index]["HTBH"];
                            break;
                        }
                    case "HTZL":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTZL";
                            p.Value = data.Rows[index]["HTZL"];
                            break;
                        }
                    case "XSFS":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "XSFS";
                            p.Value = data.Rows[index]["XSFS"];
                            break;
                        }
                    case "HTQDRQ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTQDRQ";
                            if (data.Rows[index]["HTQDRQ"] != null && data.Rows[index]["HTQDRQ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["HTQDRQ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["HTQDRQ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "HTQRRQ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTQRRQ";
                            if (data.Rows[index]["HTQRRQ"] != null && data.Rows[index]["HTQRRQ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["HTQRRQ"].ToString()))
                            {
                                DateTime val = DateTime.Parse(data.Rows[index]["HTQRRQ"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "HTQRR":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTQRR";
                            p.Value = data.Rows[index]["HTQRR"];
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
                    case "HTJE":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTJE";
                            if (data.Rows[index]["HTJE"] != null && data.Rows[index]["HTJE"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["HTJE"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["HTJE"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "JJFS":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JJFS";
                            p.Value = data.Rows[index]["JJFS"];
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
                            if (data.Rows[index]["HTJE"] != null && data.Rows[index]["HTJE"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["HTJE"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["HTJE"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "BZ":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "BZ";
                            p.Value = data.Rows[index]["BZ"];
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
                    case "LPBH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "LPBH";
                            p.Value = data.Rows[index]["LPBH"];
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
                }
                if(p!=null)
                command.Parameters.Add(p);
                #endregion
            }
            //DbParameter SPFHID_P = command.CreateParameter();
            //SPFHID_P.DbType = DbType.String;
            //SPFHID_P.ParameterName = "SPFHID";
            //SPFHID_P.Value = data.Rows[index]["HID"];
            //command.Parameters.Add(SPFHID_P);
            //DbParameter LPBH_P = command.CreateParameter();
            //LPBH_P.DbType = DbType.String;
            //LPBH_P.ParameterName = "LPBH";
            //LPBH_P.Value = data.Rows[index]["ZID"];
            //command.Parameters.Add(LPBH_P);
        }

        protected override IDictionary<string,string> GetReturnId(BDC bdc)
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
                //Guid GZid = Guid.Parse(dt.Rows[0]["ZID"].ToString());
                //string zid = GZid.ToString("N");//GetZidTemp(dt, 0);
                string zid = dt.Rows[0]["ZID"].ToString();
                ret.Add("ZID", zid);
                //Guid GHid = Guid.Parse(dt.Rows[0]["HID"].ToString());
                //string hid = GHid.ToString("N");//GetZidTemp(dt, 0); 
                string hid = dt.Rows[0]["HID"].ToString();
                ret.Add("HID", hid);
                string htid = dt.Rows[0]["HTID"].ToString().Trim();
                ret.Add("HTID", htid);
                string htbahid = dt.Rows[0]["SPFHTBAH"].ToString();
                ret.Add("HTBAH", htbahid);
                if (dt.Columns.Contains("LPBH")) { 
                string lpbh = dt.Rows[0]["LPBH"].ToString();
                ret.Add("LPBH", lpbh);
                }
                if (dt.Columns.Contains("FJBM"))
                { 
                string fjbm = dt.Rows[0]["FJBM"].ToString();
                ret.Add("FJBM", fjbm);
                }
            }
            return ret;
        }

        //protected virtual string InsertSql(string pch, DataTable data, string tableName)
        //{
        //    StringBuilder sbCmdText = new StringBuilder();
        //    if (data.Columns.Count > 0)
        //    {
        //        IList<string> dbColumns = new List<string>();
        //        foreach (DataColumn dc in data.Columns)
        //        {
        //            dbColumns.Add(dc.ColumnName);
        //        }
        //        sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
        //        sbCmdText.Append(string.Join(",", dbColumns));
        //        sbCmdText.Append(",PCH,SPFHID,LPBH) VALUES (");
        //        sbCmdText.Append(":" + string.Join(",:", dbColumns));
        //        sbCmdText.AppendFormat(",'{0}',:SPFHID,:LPBH)", pch);
        //    }
        //    return sbCmdText.ToString();
        //}

        private string GetHidTemp(DataTable data,int index)
        {
            if (string.IsNullOrEmpty(hid_temp))
            {
                hid_temp = IDChange.ChangeHIDFromWS2HID(data.Rows[index]["HID"].ToString());
            }
            return hid_temp;
        }
        private string GetZidTemp(DataTable data, int index)
        { 
            if (string.IsNullOrEmpty(zid_temp))
            {
                zid_temp = IDChange.ChangeZIDFromWS2ZID(data.Rows[index]["ZID"].ToString());
            }
            return zid_temp;
        }
    }
}
