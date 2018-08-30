
using Geo.Plug.DataExchange.XZFCPlug;
using Geo.Plug.DataExchange.XZFCPlug.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public class FC_Z_WSData : WebServiceDataBase
    {
        public FC_Z_WSData(string add, string excuteCode)
            : base(add, "FC_Z_TMP", excuteCode)
        {

        }

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
                    case "ZID":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZID";
                            //Guid GZid = Guid.Parse(data.Rows[index]["ZID"].ToString());
                            //data.Rows[index]["ZID"] = GZid.ToString("D");
                            data.Rows[index]["ZID"] = GuidChange.Change2With_(data.Rows[index]["ZID"].ToString());
                            p.Value = data.Rows[index]["ZID"];
                            break;
                        }
                    case "ZH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZH";
                            p.Value = DBNull.Value;
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
                    case "XMMC":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "XMMC";
                            p.Value = data.Rows[index]["XMMC"];
                            break;
                        }
                    case "JZWMC":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JZWMC";
                            p.Value = data.Rows[index]["JZWMC"];
                            break;
                        }
                    case "ZTS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZTS";
                            if (data.Rows[index]["ZTS"] != null && data.Rows[index]["ZTS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZTS"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["ZTS"].ToString());
                                p.Value = val;
                            }
                            else
                                p.Value = DBNull.Value;
                            break;
                        }
                    case "LPLB":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "LPLB";
                            p.Value = data.Rows[index]["LPLB"];
                            break;
                        }
                    case "LZXZ":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "LZXZ";
                            p.Value = data.Rows[index]["LZXZ"];
                            break;
                        }
                    case "LZTD":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "LZTD";
                            p.Value = data.Rows[index]["LZTD"];
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
                    case "QLRZS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "QLRZS";
                            if (data.Rows[index]["QLRZS"] != null && data.Rows[index]["QLRZS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["QLRZS"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["QLRZS"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
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
                    case "FWJG":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "FWJG";
                            p.Value = data.Rows[index]["FWJG"];
                            break;
                        }
                    case "ZCS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZCS";
                            if (data.Rows[index]["ZCS"] != null && data.Rows[index]["ZCS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZCS"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["ZCS"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
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
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "SCJZMJ":
                        {
                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "SCJZMJ";
                            if (data.Rows[index]["SCJZMJ"] != null && data.Rows[index]["SCJZMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["SCJZMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["SCJZMJ"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "ZZDMJ":
                        {

                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZZDMJ";
                            if (data.Rows[index]["ZZDMJ"] != null && data.Rows[index]["ZZDMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZZDMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["ZZDMJ"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "ZYDMJ":
                        {


                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "ZYDMJ";
                            if (data.Rows[index]["ZYDMJ"] != null && data.Rows[index]["ZYDMJ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["ZYDMJ"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["ZYDMJ"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "JZWGD":
                        {

                            dbtype = DbType.Decimal;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JZWGD";
                            if (data.Rows[index]["JZWGD"] != null && data.Rows[index]["JZWGD"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["JZWGD"].ToString()))
                            {
                                double val = double.Parse(data.Rows[index]["JZWGD"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "JGRQ":
                        {
                            dbtype = DbType.DateTime;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "JGRQ";
                            if (data.Rows[index]["JGRQ"] != null && data.Rows[index]["JGRQ"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["JGRQ"].ToString()))
                            {
                                //DateTime val = DateTime.Parse(data.Rows[index]["JGRQ"].ToString());
                                //DateTime val = DateTime.ParseExact(data.Rows[index]["JGRQ"].ToString(), "yyyy", CultureInfo.InvariantCulture);
                                DateTime val;
                                if (DateTime.TryParseExact(data.Rows[index]["JGRQ"].ToString(), new string[] { "yyyy" },
                                    CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out val))
                                {
                                    p.Value = val;
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
                            break;
                        }
                    case "DSCS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DSCS";
                            if (data.Rows[index]["DSCS"] != null && data.Rows[index]["DSCS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["DSCS"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["DSCS"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "DXCS":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DXCS";
                            if (data.Rows[index]["DXCS"] != null && data.Rows[index]["DXCS"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["DXCS"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["DXCS"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "DXSD":
                        {
                            dbtype = DbType.Int32;
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "DXSD";
                            if (data.Rows[index]["DXSD"] != null && data.Rows[index]["DXSD"] != DBNull.Value && !string.IsNullOrEmpty(data.Rows[index]["DXSD"].ToString()))
                            {
                                int val = int.Parse(data.Rows[index]["DXSD"].ToString());
                                p.Value = val;
                            }
                            else
                            {
                                p.Value = DBNull.Value;
                            }
                            break;
                        }
                    case "BZ":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "BZ";
                            p.Value = data.Rows[index]["ZH"];
                            break;
                        }
                    case "MPH":
                        {
                            p = command.CreateParameter();
                            p.DbType = dbtype;
                            p.ParameterName = "MPH";
                            p.Value = data.Rows[index]["MPH"];
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
                }
                #endregion
                if (p != null)
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
            }
            return ret;
        }

        protected override string PushData2DB(BDC bdc, DbConnection conn, string pch)
        {
            //return base.PushData2DB(bdc, conn, pch);
            if (bdc == null || bdc.data == null) {

                return pch;
            }
            DataTable dt = bdc.data.dt;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (this._excuteCode == "0000")//如果是测管系统取数据，且有数据返回.用LPBH和XMMC去匹配，尽量用数据库中原有的ID。
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //dt.Rows[i]["ZID"] = GuidChange.Change2With_(dt.Rows[i]["ZID"].ToString());//GZid.ToString("D");
                        if (dt.Columns.Contains("LPBH")) {
                            string tm = LPBHFindExist(dt.Rows[i]["LPBH"].ToString(), dt.Rows[i]["XMMC"].ToString());//尽可能返回不动产的id
                            if (!string.IsNullOrEmpty(tm))
                            {
                                insertFC_ZIDGL(tm, dt.Rows[i]["ZID"].ToString());
                                dt.Rows[i]["ZID"] = tm;
                            }
                            else
                            {
                                dt.Rows[i]["ZID"] = GuidChange.Change2With_(dt.Rows[i]["ZID"].ToString());//GZid.ToString("D"); 
                            }
                        } 
                    }
                }
            }
            return base.PushData2DB(bdc, conn, pch);
        }

        public override string Data2DB(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            //return base.Data2DB(ps, paramNeeded, connection, pch); 
            //先看看zid在数据库中有无对应关系 
            if (ps.ContainsKey("ZID"))
            {
                string idInCG = GetCGZID(ps["ZID"]);
                if (!string.IsNullOrEmpty(idInCG))
                {
                    ps["ZID"] = GuidChange.Change2Without_(idInCG);//
                }
            }
            BDC bdc = null;
            try
            {
                string webServiceData = GetWebServiceData(ps, paramNeeded);//获取webservice数据
                bdc = XMLParsing(webServiceData);
            }
            catch (Exception ex) {
                bdc = new BDC();
                bdc.head = new Head { flag = 0, msg = ex.Message };
            } 
            if (_excuteCode != "0000" && ps.ContainsKey("ZID")&& bdc.data !=null&& bdc.data.dt!=null&& bdc.data.dt.Rows.Count>0)
            { //存量房和网备可能使用LPBH去获取，导致ZID不一致
                string CGZID = bdc.data.dt.Rows[0]["ZID"].ToString();
                if (!CGZID.Equals(ps["ZID"]))//如果返回的zid和参数里面的zid不一致。将zid对应关系保存，并修改返回的数据
                {
                    insertFC_ZIDGL(ps["ZID"], GuidChange.Change2With_(CGZID));
                    bdc.data.dt.Rows[0]["ZID"] = GuidChange.Change2With_(ps["ZID"]);
                }
            }
            return PushData2DB(bdc, connection, pch);
        }

        public override string GetWebServiceData(IDictionary<string, string> ps, IList<string> paramNeeded)
        {
            string re1 = base.GetWebServiceData(ps, paramNeeded);
            if (re1.Contains("没有符合条件的数据"))//用楼盘编号去获取
            {
                IDictionary<string, string> ps2 = new Dictionary<string, string>();
                //if (ps.ContainsKey("LPBH"))
                //{
                //    ps2.Add("LPBH", ps["LPBH"]);
                //}
                //if (ps2.ContainsKey("LPBH") && !string.IsNullOrEmpty(ps2["LPBH"]))
                //{
                //    string re2 = base.GetWebServiceData(ps2, null);
                //    if (!re2.Contains("没有符合条件的数据"))
                //    { //如果re2
                //        return re2;
                //    }
                //}
                if (ps.ContainsKey("ZID"))
                {
                    ps2.Add("ZID", ps["ZID"]);
                }
                if (ps2.ContainsKey("ZID") && !string.IsNullOrEmpty(ps2["ZID"]))
                {
                    string re2 = base.GetWebServiceData(ps2, null);
                    if (!re2.Contains("没有符合条件的数据"))
                    { //如果re2
                        return re2;
                    }
                }
            }
            return re1;
        }

        public override IDictionary<string, string> Data2DBAndReturnId(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            if (ps.ContainsKey("ZID"))//尽可能用测管的zid
            {
                string idInCG = GetCGZID(ps["ZID"]);
                if (!string.IsNullOrEmpty(idInCG))
                {
                    ps["ZID"] = GuidChange.Change2Without_(idInCG);
                }
            }
            return base.Data2DBAndReturnId(ps, paramNeeded, connection, pch);
        }
        public string LPBHFindExist(string lpbhPushed, string xmmc)//楼盘编号是否已经存在
        {
            if (string.IsNullOrEmpty(lpbhPushed))
            {
                return string.Empty;
            }
            string[] ss = lpbhPushed.Split('|');
            string sql = string.Empty;
            if (ss.Length == 1)
            {
                sql = string.Format("select tstybm from fc_z_qsdc where (LPBH='{0}' or LPBH='{0}'||'|' or LPBH='|'||'{0}') and xmmc='{1}'", lpbhPushed, xmmc);
            }
            else if (ss.Length == 2)
            {
                sql = string.Format("select tstybm from fc_z_qsdc where (LPBH='{2}' or LPBH='{0}' or  LPBH='{1}' or LPBH='{0}'||'|' or LPBH='{1}'||'|' or LPBH='|'||'{0}' or LPBH='|'||'{1}') and xmmc='{3}'", ss[0], ss[1], lpbhPushed, xmmc);
            }
            if (string.IsNullOrEmpty(sql))
            {
                return string.Empty;
            }
            object o = DBHelper.GetScalar(sql);
            if (o == null)
            {
                return string.Empty;
            }
            return o.ToString();
        }

        private int insertFC_ZIDGL(string tm, string cgzid)
        {
            string sql = string.Format("select 1 from FC_ZIDGL where DBCZID='{0}'", tm);
            object o = DBHelper.GetScalar(sql);
            int ret = 0;
            if (o == null)
            {
                string insertSql = string.Format("insert into FC_ZIDGL(DBCZID,ZIDINCG) values('{0}','{1}')", tm, cgzid);
                ret = DBHelper.ExecuteNonQuery(insertSql);
            }
            else
            {
                string updateSql = string.Format("Update FC_ZIDGL set ZIDINCG='{0}' where DBCZID='{1}'", cgzid, tm);
                ret = DBHelper.ExecuteNonQuery(updateSql);
            }
            return ret;
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
    }
}
