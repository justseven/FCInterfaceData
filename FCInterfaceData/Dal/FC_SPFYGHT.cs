using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace FCInterfaceData.Dal
{
    /// <summary>
    /// 模拟  商品房预购合同信息
    /// </summary>
    public class FC_SPFYGHT:IGetDbData
    {
        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("HTID");
            dt.Columns.Add("HID");
            dt.Columns.Add("ZID");
            dt.Columns.Add("SPFHTBAH");
            dt.Columns.Add("HTBASJ");
            dt.Columns.Add("HTBH");
            dt.Columns.Add("HTZL");
            dt.Columns.Add("XSFS");
            dt.Columns.Add("HTQDRQ");
            dt.Columns.Add("HTQRRQ");
            dt.Columns.Add("HTQRR");
            dt.Columns.Add("JZMJ");
            dt.Columns.Add("TNJZMJ");
            dt.Columns.Add("FTJZMJ");
            dt.Columns.Add("HTJE");
            dt.Columns.Add("JJFS");
            dt.Columns.Add("FKLX");
            dt.Columns.Add("DKFS");
            dt.Columns.Add("FKSJ");
            dt.Columns.Add("PGJG");
            dt.Columns.Add("JYJG");
            dt.Columns.Add("BZ");
            dt.Columns.Add("SFYX");
            DataRow dr= dt.NewRow();
            dr["HTID"] = "20150001";
            dr["HID"] = "123456321";
            dr["ZID"] = "1234";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["HTID"] = "20150002";
            dr2["HID"] = "584811";
            dr2["ZID"] = "dsdsdsd";
            dt.Rows.Add(dr2);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }
}