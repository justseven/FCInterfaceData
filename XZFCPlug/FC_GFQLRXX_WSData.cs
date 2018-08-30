using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class FC_GFQLRXX_WSData : WebServiceDataBase
    {
        public FC_GFQLRXX_WSData(string add, string excuteCode)
            : base(add, "FC_GFQLRXX_TMP", excuteCode)
        {
            
        }
        protected override void CreateParameters(DbCommand command, DataTable data, int index)
        {
            foreach (DataColumn dc in data.Columns)
            {
                DbType dbtype = DbType.String;
                DbParameter p = null;
                #region pp

                switch (dc.ColumnName) {
                    case "QLRID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QLRID";
                            p.Value = data.Rows[index]["QLRID"];
                            break;
                        }
                    case "HTID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTID";
                            p.Value = data.Rows[index]["HTID"].ToString().Trim();
                            break;
                        }
                    case "HTBAH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "HTBAH";
                            p.Value = data.Rows[index]["HTBAH"];
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
                    case "XGRMC":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "XGRMC";
                            p.Value = data.Rows[index]["XGRMC"];
                            break;
                        }
                    case "XGRLX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "XGRLX";
                            p.Value = data.Rows[index]["XGRLX"];
                            break;
                        }
                    case "ZJLX":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZJLX";
                            p.Value = data.Rows[index]["ZJLX"];
                            break;
                        }
                    case "ZJHM":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZJHM";
                            p.Value = data.Rows[index]["ZJHM"];
                            break;
                        }
                    case "XGRSX":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "XGRSX";
                            if (data.Rows[index]["XGRSX"] != null && data.Rows[index]["XGRSX"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["XGRSX"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["XGRSX"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                }

                #endregion
                command.Parameters.Add(p);
            }
        }
    }
}
