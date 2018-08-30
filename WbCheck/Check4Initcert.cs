using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WbCheck
{
    public class Check4Initcert
    {
        private DataTable CheckDataParper()
        {
            string sql = @"Select Houseid,PledgeSate,houseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from HouseInfo left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on  BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID where FirstRegNo is null";
            SQLDBHelper db = SQLDBHelper.CreateInstance();
            return db.GetTable(sql);
        }

        /// <summary>
        /// 网备里面是查封的
        /// </summary>
        private void CheckWBInBdc(DataTable dt, Action<DataRow, string, string> Write, string pch)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string sql = @"Select distinct DJ_TSGL.TSTYBM from DJ_TSGL Join (Select TSTYBM from FC_H_QSDC where ComesFromCG=0) cgh on cgh.TSTYBM=DJ_TSGL.TSTYBM 
Where DJ_TSGL.DJZL='权属' and nvl2(dj_tsgl.Lifecycle,dj_tsgl.Lifecycle,0)=0";
                DataTable dtbdc = OleDBHelper.GetDataTable(sql);
                for (int i = 0; i < dtbdc.Rows.Count; i++)
                {

                    for (int j = 0; j < dt.Rows.Count; j++) {
                        if (dtbdc.Rows[i]["TSTYBM"].ToString().Equals(dt.Rows[j]["Houseid"].ToString())) {
                            if (dtbdc.Rows[i]["TSTYBM"].ToString() == "0623059C-E886-0070-E053-C0A864060070") {

                            }
                            Write(dt.Rows[j], "不动产做过权属，网备没有首次记录", pch);
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
