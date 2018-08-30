using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBCForFCWebService
{
    public class FC_SPF_Data
    {
        public DataSet GetBuildRightInfo(string BuNo, string Area)
        {
            string sql = string.Format("select \"HouseID\",\"HCSID\",\"HYCID\",\"BuildingID\",\"RoomNo\",\"权证号\",\"登记坐落\",\"登记用途\" from BuildingRightInfo where BuNo ='{0}'", BuNo);
            DataSet ds = OleDBHelper.GetDataSet(sql); 
            return ds;
        }

        public DataSet GetHouseState(string HouseID)
        {
            string sql = string.Format("select * from GetHouseState where \"HouseID\" ='{0}'", HouseID);
            DataSet ds = OleDBHelper.GetDataSet(sql);
            return ds;
        }


        public DataSet GetPrprtcertInfo(string HouseID, string Area) {
            string sql = string.Format(@"select * from GetPrprtcertInfo where houseid='{0}'", HouseID);
            DataSet ds = OleDBHelper.GetDataSet(sql);
            return ds;
        }

        public DataSet GetRegistesByHouseId(string HouseId) {
            string sql = string.Format(@"Select '权属' as RegistType,Count(1) as Times from DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where (DJ_TSGL.TSTYBM='{0}' or HSCID='{0}'  or HYCID='{0}') and nvl2(DJ_TSGL.Lifecycle,DJ_TSGL.Lifecycle,0)=0 and exists(Select 1 from DJ_DJB where DJ_TSGL.SLBH=DJ_DJB.SLBH)
                               union all Select '抵押' as RegistType,Count(1) as Times from DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where (DJ_TSGL.TSTYBM='{0}' or HSCID='{0}'  or HYCID='{0}') and nvl2(DJ_TSGL.Lifecycle,DJ_TSGL.Lifecycle,0)=0 and exists(Select 1 from DJ_Dy where DJ_TSGL.SLBH=DJ_Dy.SLBH)
                               union all Select '预告' as RegistType,Count(1) as Times from DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where (DJ_TSGL.TSTYBM='{0}' or HSCID='{0}'  or HYCID='{0}') and nvl2(DJ_TSGL.Lifecycle,DJ_TSGL.Lifecycle,0)=0 and exists(Select 1 from DJ_YG where DJ_TSGL.SLBH=DJ_YG.SLBH)
                               union all Select '查封' as RegistType,Count(1) as Times from DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where (DJ_TSGL.TSTYBM='{0}' or HSCID='{0}'  or HYCID='{0}') and nvl2(DJ_TSGL.Lifecycle,DJ_TSGL.Lifecycle,0)=0 and exists(Select 1 from DJ_CF where DJ_TSGL.SLBH=DJ_CF.SLBH)
", HouseId);
            DataSet ds = OleDBHelper.GetDataSet(sql);
            return ds;
        }
    }
}
