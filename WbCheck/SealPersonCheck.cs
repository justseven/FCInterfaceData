using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    /// <summary>
    /// 查封被执行人检查
    /// </summary>
    public class SealPersonCheck
    {
        /// <summary>
        /// 所有没有领过权证的数据
        /// </summary>
        /// <returns></returns>
        private DataTable CheckDataParper() {
            string sql = @"select DJ_CF.SLBH,dj_tsgl.tstybm as HouseID,dj_cf.qlr,FC_H_QSDC.ZL,'' as HouseState   from dj_tsgl
join DJ_CF on dj_tsgl.SLBH=DJ_CF.SLBH
left join FC_H_QSDC on FC_H_QSDC.TSTYBM=dj_tsgl.TSTYBM
 where FC_H_QSDC.tstybm in (select TSTYBM
                    from dj_tsgl gl1
                   where not exists (select 1
                            from (select distinct tstybm
                                    from dj_tsgl
                                   where djzl = '权属') t
                           where t.tstybm = gl1.tstybm))
   and length(FC_H_QSDC.tstybm) = 36 
  "; 
            DataTable dtbdc = OleDBHelper.GetDataTable(sql);
            return dtbdc;
        } 

        private void CheckWBInBDC(DataTable dt, Action<DataRow, string, string> Write, string pch) {
            string sql = @"Select ContractBuyer.HouseId,ContractBuyer.BuyerName,PledgeSate,houseState,
SaleItemInfo.ItemName+BuildingInfo.BuildingName+'-'+HouseInfo.UnitNo+'-'+HouseInfo.HouseNo as zl
from ContractBuyer left join 
                ContractSign on ContractBuyer.ContractId=ContractSign.ContractId
left join  HouseInfo on ContractSign.HouseInfoID =HouseInfo.HouseId
left join BuildingInfo on HouseInfo.buildingid=BuildingInfo.BuildingInfo_ID 
left join SaleItemInfo on  BuildingInfo.SaleItemID=SaleItemInfo.SaleItemID  
where ContractBuyer.HouseId='{0}' and ContractSign.flag<>2";
            SQLDBHelper db = SQLDBHelper.CreateInstance();
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    DataTable wbdt = db.GetTable(string.Format(sql, dt.Rows[i]["HouseId"].ToString()));
                    if (wbdt != null && wbdt.Rows.Count > 0) {
                        if (!wbdt.Rows[0]["BuyerName"].ToString().Equals(dt.Rows[i]["QLR"])){
                            Write(dt.Rows[i], "网备中的网签人和查封登记被执行人不一致", pch);
                        }
                    }
                }
            }
        }

        public string Check() {
            string pch = Guid.NewGuid().ToString();
            DataTable dt = CheckDataParper();
            CheckWBInBDC(dt, (dr, errorType, p) => WriteLog.WriteCheckResult(dr, errorType, p), pch); 
            return pch;
        }

    }
}
