using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    public class Check4Seal
    {
        //数据准备
        private DataTable CheckDataParper() {
            string sql = @"Select Houseid,SealState,houseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from HouseInfo left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on  BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID  
order by houseState ";
            SQLDBHelper db= SQLDBHelper.CreateInstance();
            return  db.GetTable(sql);
        }
        /// <summary>
        /// 网备里面是查封的
        /// </summary>
        private void CheckWBInBdc(DataTable dt, Action<DataRow, string, string> Write, string pch) {
            if (dt != null && dt.Rows.Count > 0) { 
                for (int i = 0; i < dt.Rows.Count; i++) {
                    string sql = "Select TSTYBM,Cunt from SEALTIMES Where TSTYBM='{0}'";
                    sql = string.Format(sql, dt.Rows[i]["Houseid"]);
                    DataTable dtbdc = OleDBHelper.GetDataTable(sql);
                    if (dtbdc != null && dtbdc.Rows.Count > 0)
                    {
                        int cfcsinbdc = Convert.ToInt32(dtbdc.Rows[0]["Cunt"].ToString());
                        int cfcsinwb = Convert.ToInt32(dt.Rows[i]["SealState"].ToString());
                        if (cfcsinbdc != cfcsinwb)
                        {
                            if (cfcsinbdc > cfcsinwb)
                            {//bdc多 
                                Write(dt.Rows[i], "不动产中的查封比网备的查封次数多", pch);
                            }
                            else
                            {
                                Write(dt.Rows[i], "网备中的查封比的不动产查封次数多", pch);
                            }
                        }
                    }
                    else {
                        int cfcsinwb = Convert.ToInt32(dt.Rows[i]["SealState"].ToString());
                        if (cfcsinwb != 0)
                        {
                            Write(dt.Rows[i], "网备中的查封比的不动产查封次数多", pch);
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
