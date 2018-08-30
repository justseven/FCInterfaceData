using Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace DBCForFCWebService.Dal
{
    public class TaxDal
    {
        internal DataTable GetHouseInfoByHTBH(string contrRecNum)
        {
            string sql = @"select distinct z.XMMC 项目名称,h.zl 项目座落,z.fwzl 楼盘名称,h.dyh 单元号,h.hh 房间号, h.ycjzmj 预测建筑面积,h.ycftjzmj 预测套内面积,h.yctnjzmj 预测分摊面积,h.jzmj 建筑面积,h.ftjzmj 套内面积,h.tnjzmj 分摊面积 from fc_spfyght_tmp ht
right join dj_sjd sjd on sjd.pch=ht.pch
left join dj_tsgl tsgl on sjd.slbh=tsgl.slbh
left join fc_h_qsdc h on tsgl.tstybm=h.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
where tsgl.bdclx='房屋' and ht.spfhtbah=:htbh
union all 
select distinct z.XMMC 项目名称,h.zl 项目座落,z.fwzl 楼盘名称,h.dyh 单元号,h.hh 房间号, h.ycjzmj 预测建筑面积,h.ycftjzmj 预测套内面积,h.yctnjzmj 预测分摊面积,h.jzmj 建筑面积,h.ftjzmj 套内面积,h.tnjzmj 分摊面积 from fc_clmmht_tmp clf
right join dj_sjd sjd on sjd.pch=clf.pch
left join dj_tsgl tsgl on sjd.slbh=tsgl.slbh
left join fc_h_qsdc h on tsgl.tstybm=h.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
where tsgl.bdclx='房屋' and clf.clhtbah=:htbh";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,":htbh", contrRecNum);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            return dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, list.ToArray());

        }


        private  void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }
    }
}