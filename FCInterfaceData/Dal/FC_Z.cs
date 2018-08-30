using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FCInterfaceData.Dal
{
    public class FC_Z : IGetDbData
    {
        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("ZID");
            dt.Columns.Add("ZH");
            dt.Columns.Add("BDCDYH");
            dt.Columns.Add("XMMC");
            dt.Columns.Add("JZWMC");
            dt.Columns.Add("ZTS");
            dt.Columns.Add("LPLB");
            dt.Columns.Add("LZXZ");
            dt.Columns.Add("LZTD");
            dt.Columns.Add("FWZL");
            dt.Columns.Add("QLRZS");
            dt.Columns.Add("GHYT");
            dt.Columns.Add("FWJG");
            dt.Columns.Add("ZCS");
            dt.Columns.Add("YCJZMJ");
            dt.Columns.Add("SCJZMJ");
            dt.Columns.Add("ZZDMJ");
            dt.Columns.Add("ZYDMJ");
            dt.Columns.Add("JZWGD");
            dt.Columns.Add("JGRQ");
            dt.Columns.Add("DSCS");
            dt.Columns.Add("DXCS");
            dt.Columns.Add("DXSD");
            dt.Columns.Add("BZ");
            dt.Columns.Add("MPH");
            DataRow dr = dt.NewRow();
            dr["ZID"] = "20150001";
            dr["XMMC"] = "123456321";
            dr["JGRQ"] = DateTime.Now.AddDays(50);
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
             dr2["ZID"]  = "20150002";
             dr2["XMMC"] = "1234563233";
             dr2["JGRQ"] = DateTime.Now;
            dt.Rows.Add(dr2);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }
}