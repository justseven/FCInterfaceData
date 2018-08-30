using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace FCInterfaceData.Dal
{
    public class FC_H : IGetDbData
    {
        public DataTable GetDataTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("HID");
            dt.Columns.Add("ZID");
            dt.Columns.Add("FWBH");
            dt.Columns.Add("ZH");
            dt.Columns.Add("BDCDYH");
            dt.Columns.Add("QLLX");
            dt.Columns.Add("QLXZ");
            dt.Columns.Add("HX");
            dt.Columns.Add("HXJG");
            dt.Columns.Add("ZXCD");
            dt.Columns.Add("GHYT");
            dt.Columns.Add("ZL");
            dt.Columns.Add("SJC");
            dt.Columns.Add("MYC");
            dt.Columns.Add("DYH");
            dt.Columns.Add("FJH");
            dt.Columns.Add("LJZH");
            dt.Columns.Add("QDJG");
            dt.Columns.Add("QDFS");
            dt.Columns.Add("SHBW");
            dt.Columns.Add("YCJZMJ");
            dt.Columns.Add("YCTNJZMJ");
            dt.Columns.Add("YCDXBFJZMJ");
            dt.Columns.Add("YCFTJZMJ");
            dt.Columns.Add("YCQTJZMJ");
            dt.Columns.Add("YCFTXS");
            dt.Columns.Add("JZMJ");
            dt.Columns.Add("TNJZMJ");
            dt.Columns.Add("FTJZMJ");
            dt.Columns.Add("DXBFJZMJ");
            dt.Columns.Add("QTJZMJ");
            dt.Columns.Add("FTXS");
            dt.Columns.Add("TDZZRQ");
            dt.Columns.Add("TDYT");
            dt.Columns.Add("TDSYQR");
            dt.Columns.Add("GYTDMJ");
            dt.Columns.Add("FTTDMJ");
            dt.Columns.Add("DYTDMJ");
            dt.Columns.Add("TCJS");
            dt.Columns.Add("CG");
            dt.Columns.Add("ZT");
            dt.Columns.Add("FCFHT",typeof(byte[]));
            dt.Columns.Add("FJSM");
            DataRow dr = dt.NewRow();
            dr["HXJG"] = "201501";
            dr["GHYT"] = "123456321";
            dr["YCDXBFJZMJ"] = "25.663";
            string filename="D:\\Pictures\\6a63f6246b600c331be946091a4c510fd8f9a15b.jpg";
            FileStream  fs = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bs=new byte[fs.Length];
            br.Read(bs,0,bs.Length);
            br.Close();
            fs.Close();
            dr["FCFHT"] = bs;
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["HXJG"] = "201502";
            dr2["GHYT"] = "1234563233";
            dr2["YCDXBFJZMJ"] = "36.23";
            dt.Rows.Add(dr2);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }
}