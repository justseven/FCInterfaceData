using System; 
using System.Data;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    public class Check4DY
    {
        //数据准备 ------可销售的房屋的抵押状态一定要对到
        private DataTable CheckDataParper()
        {
            string sql = @"Select Houseid,PledgeSate,houseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from HouseInfo left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on  BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID where HouseState='可销售'";
            SQLDBHelper db = SQLDBHelper.CreateInstance();
            return db.GetTable(sql);
        }

        private void CheckWBInBDC(DataTable dt, Action<DataRow, string, string> Write, string pch)
        {
            if (dt != null && dt.Rows.Count > 0)
            { 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sql = "Select TSTYBM,Cunt from mortagetimes Where TSTYBM='{0}'";
                    if (dt.Rows[i]["Houseid"].ToString() == "0663B473-BAC4-0026-E053-C0A864060026") {

                    }
                    sql = string.Format(sql, dt.Rows[i]["Houseid"]);
                    DataTable dtbdc = OleDBHelper.GetDataTable(sql);
                    if (dtbdc != null && dtbdc.Rows.Count > 0)
                    {
                        int cfcsinbdc = Convert.ToInt32(dtbdc.Rows[0]["Cunt"].ToString());
                        int cfcsinwb = Convert.ToInt32(dt.Rows[i]["PledgeSate"].ToString());
                        if (cfcsinbdc != cfcsinwb)
                        {
                            Write(dt.Rows[i], "可销售房屋抵押次数对不到", pch);
                        }
                    }
                    else {//说明抵押次数是0
                        int cfcsinwb = Convert.ToInt32(dt.Rows[i]["PledgeSate"].ToString());
                        if (cfcsinwb != 0) {
                            Write(dt.Rows[i], "可销售房屋抵押次数对不到", pch);
                        }
                    }
                }
            }
        }

        public string Check()
        {
            string pch = Guid.NewGuid().ToString();
            DataTable dt = CheckDataParper();
            CheckWBInBDC(dt, (dr, errorType, p) => WriteLog.WriteCheckResult(dr, errorType, p), pch);
            return pch;
        }
    }
}
