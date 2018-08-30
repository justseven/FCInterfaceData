using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DBCForFCWebService 
{
    public class FC_CG_Data
    {
        /// <summary>
        /// 根据buildingId检查这个一栋房子是否有房屋做过登记
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns>true 表示做过登记</returns>
        public bool GetIsRegistedBuilding(string buildingId)
        {
            //string sql = string.Format("select count(1) from dj_tsgl gl left join fc_h_qsdc h on gl.tstybm=h.tstybm where h.lsztybm='{0}'", buildingId);
            //string sql2= string.Format("select count(1) from dj_tsgl gl left join fc_z_qsdc z on gl.tstybm=z.tstybm where z.tstybm='{0}'", buildingId);
            //int rowCount = OleDBHelper.GetScalar(sql);
            //if (rowCount == 0)
            //{
            //   int ret2=  OleDBHelper.GetScalar(sql2);
            //   return ret2>0;
            //}

            string sql = string.Format(@"Select Count(1) From DJ_TSGL 
Left Join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM 
Left Join FC_Z_QSDC on FC_H_QSDC.LSZTYBM=FC_Z_QSDC.TSTYBM Where Nvl2(DJ_TSGL.LifeCycle,DJ_TSGL.LifeCycle,0)=0 And FC_Z_QSDC.TSTYBM='{0}'", buildingId);
            int rowCount = OleDBHelper.GetScalar(sql);
            return true;
        }

        public bool GetIsRegistedHouse(string HouseId) {
            string sql = string.Format(@"Select Count(1) From DJ_TSGL 
Left Join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM
Where Nvl2(DJ_TSGL.LifeCycle,DJ_TSGL.LifeCycle,0)=0 And FC_H_QSDC.TSTYBM='{0}'", HouseId);
            int rowCount = OleDBHelper.GetScalar(sql);
            return true;
        }

        public DataSet GetFirstRegistedInfo(string HouseId) {
            string sql = string.Format(@"Select * from fc_djyz
Where Upper(HouseId)='{0}'", HouseId.ToUpper());
            return OleDBHelper.GetDataSet(sql);
        }
    }
}