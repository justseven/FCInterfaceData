using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FCInterfaceData.Dal
{
    public class FC_CLMMHT : IGetDbData
    {
        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("HTID");
            dt.Columns.Add("CQZH");
            dt.Columns.Add("HID");
            dt.Columns.Add("ZID");
            dt.Columns.Add("CLHTBAH");
            dt.Columns.Add("HTBASJ");
            dt.Columns.Add("FWZL");
            dt.Columns.Add("FWLX");
            dt.Columns.Add("FWJG");
            dt.Columns.Add("HX");
            dt.Columns.Add("JZMJ");
            dt.Columns.Add("TNJZMJ");
            dt.Columns.Add("FTJZMJ");
            dt.Columns.Add("PGJG");
            dt.Columns.Add("JYJG");
            dt.Columns.Add("FKLX");
            dt.Columns.Add("DKFS");
            dt.Columns.Add("FKSJ");
            dt.Columns.Add("QDSJ");
            dt.Columns.Add("QRSJ");
            dt.Columns.Add("QRQZSJ");
            dt.Columns.Add("ZHBGTGSJ");
            dt.Columns.Add("CXSJ");
            dt.Columns.Add("JSSJ");
            dt.Columns.Add("SFYX");
            DataRow dr = dt.NewRow();
            dr["HTID"] = "20150001";
            dr["CQZH"] = "123456321";
            dr["ZID"] = "sdsdsd";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr["HTID"] = "20150002";
            dr["CQZH"] = "121332222";
            dr["ZID"] = "sdsdsd";
            dt.Rows.Add(dr2);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }
}