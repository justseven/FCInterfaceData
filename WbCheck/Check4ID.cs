using System; 
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    public class Check4ID
    {
        /// <summary>
        /// 数据准备
        /// </summary>
        private DataTable CheckDataParper() {
            string sql = @"select distinct HouseId,HouseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from  HouseInfo
left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID   
order by houseState ";
            SQLDBHelper dbhelper = SQLDBHelper.CreateInstance();
            DataTable dt=  dbhelper.GetTable(sql);
            return dt;
        }

        private void CheckWBInBdc(DataTable dt,Action<DataRow,string,string> Write,string pch) {
            if (dt != null && dt.Rows.Count > 0) {
                string sqlTmp = "Select Count(1) From FC_H_QSDC Where TSTYBM='{0}'";
                for (int i = 0; i < dt.Rows.Count; i++) {
                   int C= OleDBHelper.GetScalar(string.Format(sqlTmp, dt.Rows[i]["HouseId"]));
                    if (C <=0) {//网备库中不存在
                        Write(dt.Rows[i],"网备系统中的ID在不动产系统中不存在",pch);
                    }
                }
            }
        }
        private void CheckBdcInWB(DataTable dt, Action<DataRow, string, string> Write, string pch)
        {
            string getHIdsInBdc = "Select TSTYBM as HouseId,ZL,'' as HouseState from FC_H_QSDC Where ComesFromCG=0";
            DataTable dtbdc= OleDBHelper.GetDataTable(getHIdsInBdc);
            if (dt!=null&&dtbdc != null && dtbdc.Rows.Count > 0) {
                for (int i = 0; i < dtbdc.Rows.Count; i++) {
                    bool has = false;
                    for (int j = 0; j < dt.Rows.Count; j++) {
                        if (dt.Rows[j]["HouseId"].ToString().Equals(dtbdc.Rows[i]["HouseId"])) {
                            has = true;
                            break;
                        }
                    }
                    if (!has) {
                        Write(dtbdc.Rows[i], "不动产系统中的ID在中网备系统不存在", pch);
                    }
                }
            }
        }
        /// <summary>
        /// 将检查结果写入数据库
        /// </summary>
        /// <returns></returns>
        private string WriteCheckResult2DB(DataRow dr,string errorType,string pch) {
           
            string sql = "Insert into WBIDCheck(HouseID,ZL,HouseState,ErrorType,PCH) Values(:HouseID,:ZL,:HouseState,:ErrorType,:PCH)";
            OleDbParameter[] cmdParms = new OleDbParameter[4];
            cmdParms[0] = new OleDbParameter("HouseID",OleDbType.VarChar);
            cmdParms[0].Value = dr["HouseID"].ToString();
            cmdParms[1] = new OleDbParameter("ZL", OleDbType.VarChar);
            cmdParms[1].Value = dr["ZL"].ToString();
            cmdParms[2] = new OleDbParameter("HouseState", OleDbType.VarChar);
            cmdParms[2].Value = dr["HouseState"].ToString(); 
            cmdParms[3] = new OleDbParameter("ErrorType", OleDbType.VarChar);
            cmdParms[3].Value = errorType;
            cmdParms[4] = new OleDbParameter("PCH", OleDbType.VarChar);
            cmdParms[4].Value = dr["PCH"].ToString();
            OleDBHelper.ExecuteSql(sql, cmdParms);
            return pch;
        }

        public string Check() {
            string pch = Guid.NewGuid().ToString();
            DataTable dt = CheckDataParper(); 
            CheckWBInBdc(dt,(dr, errorType,p) => WriteLog.WriteCheckResult(dr, errorType, p), pch);
            CheckBdcInWB(dt, (dr, errorType, p) => WriteLog.WriteCheckResult(dr, errorType, p), pch);
            return pch;
        }
    }
}
