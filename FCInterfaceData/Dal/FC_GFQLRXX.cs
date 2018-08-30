using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FCInterfaceData.Dal
{
    public class FC_GFQLRXX:IGetDbData
    {
        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("QLRID");
            dt.Columns.Add("HTID");
            dt.Columns.Add("HTBAH");
            dt.Columns.Add("FWLX");
            dt.Columns.Add("XGRMC");
            dt.Columns.Add("XGRLX");
            dt.Columns.Add("ZJLX");
            dt.Columns.Add("ZJHM");
            dt.Columns.Add("XGRSX");
            DataRow dr = dt.NewRow();
            dr["HTID"] = "20150001";
            dr["QLRID"] = "123456321";
            dr["HTBAH"] = "sdsdsd";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["HTID"] = "20150002";
            dr2["QLRID"] = "584811";
            dr2["HTBAH"] = "dsdsds";
            dt.Rows.Add(dr2);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }
}