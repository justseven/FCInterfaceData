using Geo.Plug.DataExchange.XZFCPlug.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XZFCPlug
{
    /// <summary>
    /// 参数准备
    /// </summary>
    public class IDChange
    {
        /// <summary>
        /// 准备户ID
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public static string HIDPrepare(string hid)
        {
            string sql=string.Format("select z.LPBH||'_'||h.FJBM as HID from fc_h_qsdc h left join fc_z_qsdc z on h.lsztybm=z.tstybm where h.TSTYBM='{0}'",hid);
            DataTable dt= DBHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["HID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
         
        /// <summary>
        /// 把BuildingNo|FactBuildingNO_RoomNo转成hid
        /// </summary>
        public static string ChangeHIDFromWS2HID(string hid)
        {
            string[] hs = hid.Split('_');
            string code8="", zs1="", zs2="";
            if (hs.Length == 2) {
                code8 = hs[1];
                string[] zs = hs[0].Split('|');
                if (zs.Length == 2)
                { 
                    zs1=zs[0];
                    zs2 = zs[1];
                }
                else
                {
                    throw new Exception("传入的HZID=" + hid);
                }
            }
            else
            {
                throw new Exception("传入的HZID=" + hid);
            }
            string sql = "";
            if (!string.IsNullOrEmpty(zs1)&&!string.IsNullOrEmpty(zs2)) {
               sql= string.Format("select h.tstybm HID from fc_h_qsdc h left join fc_z_qsdc z on h.lsztybm=z.tstybm where 1=1 and (z.LPBH like '{0}|%' or z.LPBH like '%|{1}') and h.FJBM='{2}'", zs1, zs2, code8);
            }
            else if (!string.IsNullOrEmpty(zs1) && string.IsNullOrEmpty(zs2))
            {
                sql = string.Format("select h.tstybm HID from fc_h_qsdc h left join fc_z_qsdc z on h.lsztybm=z.tstybm where 1=1 and (z.LPBH like '{0}|%') and h.FJBM='{1}'", zs1,  code8);
            }
            else if (!string.IsNullOrEmpty(zs2) && string.IsNullOrEmpty(zs1))
            {
                sql = string.Format("select h.tstybm HID from fc_h_qsdc h left join fc_z_qsdc z on h.lsztybm=z.tstybm where 1=1 and (z.LPBH like '%|{0}') and h.FJBM='{1}'", zs2, code8);
            }
            DataTable dt = DBHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["HID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ChangeZIDFromWS2ZID(string zid)
        {
            string zs1="", zs2="";
            string[] zs = zid.Split('|');
            if (zs.Length == 2)
            {
                zs1 = zs[0]; zs2 = zs[1];
            }
            else
            {
                throw new Exception("传入的ZID=" + zid);
            }
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(zs1) && !string.IsNullOrEmpty(zs2))
                sql = string.Format("select z.tstybm ZID from fc_z_qsdc z where z.lpbh like '{0}|%' or z.lpbh like '%|{1}'", zs1, zs2);
            else if (!string.IsNullOrEmpty(zs1) && string.IsNullOrEmpty(zs2))
                sql = string.Format("select z.tstybm ZID from fc_z_qsdc z where z.lpbh like '{0}|%'", zs1);
            else if (!string.IsNullOrEmpty(zs2) && string.IsNullOrEmpty(zs1))
                sql = string.Format("select z.tstybm ZID from fc_z_qsdc z where z.lpbh like '%|{0}'", zs2);
            else
                return string.Empty;
            DataTable dt = DBHelper.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ZID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
