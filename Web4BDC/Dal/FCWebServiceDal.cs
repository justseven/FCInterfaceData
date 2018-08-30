using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Web4BDC.Dal
{
    public class FCWebServiceDal
    {
        public string GetHouseIdByAddress(string add)
        {
            return "";
        }

        /// <summary>
        /// 根据受理编号去找ID
        /// </summary>
        /// <param name="SLBH"></param>
        /// <returns></returns>
        public DataTable GetHouseIdsBySLBH(string SLBH) {
            string sql = @" Select distinct nvl(fc_h_qsdc.ORACLE_WB_HOUSEID,fc_h_qsdc.TSTYBM) as TSTYBM from DJ_TSGL
 left join fc_h_qsdc on fc_h_qsdc.TSTYBM=DJ_TSGL.TSTYBM
  Where SLBH like '{0}%' and fc_h_qsdc.TSTYBM is not null";
            sql = string.Format(sql, SLBH);
            return DBHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 根据房屋的ID获取权利人信息
        /// </summary>
        /// <param name="TSTYBM"></param>
        /// <returns></returns>
        public DataTable GetQLRMCByHouseId(string TSTYBM) {
            string sql = @"Select DJ_QLR.QLRMC from DJ_TSGL Left join DJ_QLRGL on DJ_TSGL.SLBH=DJ_QLRGL.SLBH
Left join DJ_QLR on DJ_QLRGL.QLRID=DJ_QLR.QLRID
Where TSTYBM='{0}' And DJ_TSGL.Lifecycle<>1 and DJ_QLRGL.QLRLX='权利人'";
            return DBHelper.GetDataTable(string.Format(sql, TSTYBM));
        }


        public string GetCQZHByHouseId(string tstybm)
        {
            string sql = "select djb.bdczh from dj_tsgl tsgl left join dj_djb djb on djb.slbh = tsgl.slbh where tsgl.tstybm = '{0}' and tsgl.djzl = '权属' and(djb.lifecycle = 0 or djb.lifecycle = null)";
            object o=DBHelper.GetScalar_Object(string.Format(sql, tstybm));
            if (null != o)
                return o.ToString();
            return "";
        }

        public int HasQSOrYGRegister(string HouseInfoId, string slbh) {
            string sql = string.Format("Select Count(1) From dj_tsgl where nvl2(LifeCycle,LifeCycle,0)=0 and SLBH not like '{0}%' and TSTYBM='{1}' and (DJZL='权属' or DJZL='预告')", slbh, HouseInfoId);
            return DBHelper.GetScalar(sql);
        }

        public DataTable GetRegistState(string HouseInfoId, string slbh)
        {
            string sql = string.Format("Select djzl From dj_tsgl where nvl2(LifeCycle,LifeCycle,0)=0 and SLBH not like '{0}%' and TSTYBM='{1}' and (DJZL in ('权属','预告','抵押'))", slbh, HouseInfoId);
            return DBHelper.GetDataTable(sql);
        }

        public string GetHYCId(string tstybm) {
            string sql = string.Format("Select HYCID from FC_H_QSDC where TSTYBM='{0}'", tstybm);
            DataTable dt= DBHelper.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获得他的卖放信息
        /// </summary>
        /// <param name="zslbh"></param>
        public IList<XGZHAndQLR> GetFDJBInfo(string zslbh) {
            string sql = string.Format("Select FSLBH,XGZH From DJ_XGDJGL Where ZSLBH like '{0}%'", zslbh);
            DataTable dt= DBHelper.GetDataTable(sql);
            IList<XGZHAndQLR> xgzhAndQlrs = new List<XGZHAndQLR>();
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    XGZHAndQLR xgzhAndQlr = new XGZHAndQLR();
                    xgzhAndQlr.XGZH = dt.Rows[i]["XGZH"].ToString();
                    string qlrSql = string.Format("Select DJ_QLR.QLRMC From DJ_QLR left join DJ_QLRGL on DJ_QLR.QLRID=DJ_QLRGL.QLRID Where DJ_QLRGL.SLBH='{0}'", dt.Rows[i]["FSLBH"].ToString());
                    xgzhAndQlr.QLRS = new List<string>();
                    DataTable qlrdt = DBHelper.GetDataTable(qlrSql);
                    for (int j = 0; j < qlrdt.Rows.Count; j++) {
                        xgzhAndQlr.QLRS.Add(qlrdt.Rows[j]["QLRMC"].ToString());
                    }
                    xgzhAndQlrs.Add(xgzhAndQlr);
                }
            }
            return xgzhAndQlrs;
        }

        public IList<HouseInfoBase> GetHouseInfoByIds(string where) {
            string sql = string.Format("Select FC_H_QSDC.TSTYBM,LSZTYBM,ZL,FJH,JZMJ,XMMC,JZWMC,Oracle_WB_HouseId from FC_H_QSDC left join FC_Z_QSDC on FC_H_QSDC.LSZTYBM=FC_Z_QSDC.TSTYBM Where FC_H_QSDC.TSTYBM in ({0})", where);
            DataTable dt = DBHelper.GetDataTable(sql);
            IList<HouseInfoBase> houseinfos = new List<HouseInfoBase>();
            if (dt != null) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    HouseInfoBase info = new HouseInfoBase();
                    info.FJH = dt.Rows[i]["FJH"].ToString();
                    info.JZMJ = dt.Rows[i]["JZMJ"].ToString();
                    info.JZWMC = dt.Rows[i]["JZWMC"].ToString();
                    info.TSTYBM = dt.Rows[i]["TSTYBM"].ToString();
                    info.XMMC = dt.Rows[i]["XMMC"].ToString();
                    info.ZL = dt.Rows[i]["ZL"].ToString();
                    info.LSZTYBM = dt.Rows[i]["LSZTYBM"].ToString();
                    info.Oracle_WB_HouseId = dt.Rows[i]["Oracle_WB_HouseId"].ToString();
                    houseinfos.Add(info);
                }
            }
            return houseinfos;
        }


        public IList<HouseInfoBase> GetWBHouseInfoByZL(string zl) {
            zl = zl.Replace(" ", "").Replace("号楼", "#");
            string sql = string.Format(@"select WB_HOUSEINFO.ROOMNO as FJH,WB_HOUSEINFO.BUAREA as JZMJ,wb_buildinginfo.buildingname as JZWMC,WB_HOUSEINFO.HOUSEID as TSTYBM,wb_saleiteminfo.itemname as XMMC,
wb_saleiteminfo.itemname||wb_buildinginfo.buildingname||'#'||WB_HOUSEINFO.Unitno||'-'||WB_HOUSEINFO.Houseno as ZL
 from WB_HOUSEINFO  
left join wb_buildinginfo on WB_HOUSEINFO.BUILDINGID=wb_buildinginfo.buildinginfo_id
left join wb_saleiteminfo on wb_saleiteminfo.saleitemid=wb_buildinginfo.saleitemid Where wb_saleiteminfo.itemname||wb_buildinginfo.buildingname||'#'||WB_HOUSEINFO.Unitno||'-'||WB_HOUSEINFO.Houseno Like '%{0}%' ", zl);
            DataTable dt = DBHelper.GetDataTable(sql);
            IList<HouseInfoBase> houseinfos = new List<HouseInfoBase>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    HouseInfoBase info = new HouseInfoBase();
                    info.FJH = dt.Rows[i]["FJH"].ToString();
                    info.JZMJ = dt.Rows[i]["JZMJ"].ToString();
                    info.JZWMC = dt.Rows[i]["JZWMC"].ToString();
                    info.TSTYBM = dt.Rows[i]["TSTYBM"].ToString();
                    info.XMMC = dt.Rows[i]["XMMC"].ToString();
                    info.ZL = dt.Rows[i]["ZL"].ToString();
                    houseinfos.Add(info);
                }
            }
            return houseinfos;
        }

        public int SetOracle_WB(string tstybm, string houseid) {
            string sql = string.Format("Update FC_H_QSDC Set Oracle_WB_HouseId='{0}' where tstybm ='{1}'", houseid, tstybm);
            return DBHelper.ExecuteSql(sql);
        }
    }

    public class XGZHAndQLR {
        public string XGZH { get; set; }

        public IList<string> QLRS { get; set; }
    }

    public class HouseInfoBase
    {
        public string TSTYBM { get; set; }

        public string LSZTYBM { get; set; }
        public string ZL { get; set; }

        public string FJH { get; set; }

        public string JZMJ { get; set; }

        public string XMMC { get; set; }

        public string JZWMC { get; set; }

        public string Oracle_WB_HouseId { get; set; }
    }
}