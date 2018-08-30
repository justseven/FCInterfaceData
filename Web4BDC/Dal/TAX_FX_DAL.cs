using Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.TAXInterface;

namespace Web4BDC.Dal
{
    public class TAX_FX_DAL
    {
        public DataTable GetHList(string qlrmc,string zjhm)
        {
            if(string.IsNullOrEmpty(qlrmc) || string.IsNullOrEmpty(zjhm))
            {
                return null;
            }
            string sql = @"select distinct djb.bdczh as 权证号, h.zl as 房屋坐落 from fc_h_qsdc h
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
left join dj_djb djb on djb.slbh=tsgl.slbh
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where  (djb.lifecycle is null or djb.lifecycle='0') and djb.djrq is not null and qlr.qlrmc ='{0}' and qlr.zjhm ='{1}'
union all
select distinct yg.bdczmh as 权证号, h.zl as 房屋坐落 from fc_h_qsdc h
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
left join dj_yg yg on yg.slbh=tsgl.slbh
left join dj_qlrgl qlrgl on qlrgl.slbh=yg.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where  (yg.lifecycle is null or yg.lifecycle='0') and yg.djrq is not null and qlr.qlrmc ='{0}' and qlr.zjhm ='{1}' 
";
           

            sql = string.Format(sql, qlrmc, zjhm);

            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            
            return dt;
        }


        public DataTable GetHListFrmOld(string qlrmc, string zjhm)
        {
            string sql = @"SELECT distinct ZH as 权证号, ZL as 房屋坐落 FROM QZXXINFO QZ  WHERE (FWSYQR LIKE '%{0}%' and zjhm like '%{1}%')";
            sql = string.Format(sql, qlrmc, zjhm);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            return dt;
        }

        private string GetQLRMC(ZMInfo info)
        {
            string sqlmc = string.Empty;
            if(!string.IsNullOrEmpty(info.SQRInfo.XM))
            {
                sqlmc += "'"+info.SQRInfo.XM+"'";
            }

            if (!string.IsNullOrEmpty(info.SQRPoInfo.XM))
            {

                sqlmc += ",'"+info.SQRPoInfo.XM + "'";
            }

            if (!string.IsNullOrEmpty(info.SQRZnInfo.XM))
            {
                sqlmc += ","+info.SQRZnInfo.XM + "'";
            }
            return sqlmc;
        }

        private string GetQLRZJH(ZMInfo info)
        {
            string sqlmc = string.Empty;
            if (!string.IsNullOrEmpty(info.SQRInfo.ZJH))
            {
                sqlmc += "'" + info.SQRInfo.ZJH + "'";
            }

            if (!string.IsNullOrEmpty(info.SQRPoInfo.ZJH))
            {

                sqlmc += ",'" + info.SQRPoInfo.ZJH + "'";
            }

            if (!string.IsNullOrEmpty(info.SQRZnInfo.ZJH))
            {
                sqlmc += "," + info.SQRZnInfo.ZJH + "'";
            }
            return sqlmc;
        }
    }
}