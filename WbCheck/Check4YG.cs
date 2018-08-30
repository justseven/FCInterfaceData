using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WbCheck
{
    public class Check4YG
    {
        private DataTable CheckDataParper() //没有预告的
        {
            string sql = @"Select Houseid,PledgeSate,houseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from HouseInfo left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on  BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID where YGDJ <>1";
            SQLDBHelper db = SQLDBHelper.CreateInstance();
            return db.GetTable(sql);
        }

        /// <summary>
        ///  
        /// </summary>
        private void CheckWBInBdc(DataTable dt, Action<DataRow, string, string> Write, string pch)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string sql = @"Select distinct DJ_TSGL.TSTYBM from DJ_TSGL Join (Select TSTYBM from FC_H_QSDC where ComesFromCG=0) cgh on cgh.TSTYBM=DJ_TSGL.TSTYBM 
Where DJ_TSGL.DJZL='预告' and nvl2(dj_tsgl.Lifecycle,dj_tsgl.Lifecycle,0)=0";
                DataTable dtbdc = OleDBHelper.GetDataTable(sql);
                for (int i = 0; i < dtbdc.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dtbdc.Rows[i]["TSTYBM"].ToString().Equals(dt.Rows[j]["Houseid"].ToString()))
                        {
                            Write(dt.Rows[j], "不动产做过预告，网备没有预告记录", pch);
                            break;
                        }
                    }

                }
            }
        }
        public string Check()
        {
            string pch = Guid.NewGuid().ToString();
            DataTable dt = CheckDataParper();
            CheckWBInBdc(dt, (dr, errorType, p) => WriteLog.WriteCheckResult(dr, errorType, p), pch);
            return pch;
        }
    }
}
