using Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Web4BDC.Models.TAXInterface;

namespace Web4BDC.Dal.XZFCDA
{
    public class TAXLogModelDAL
    {
        internal void InserTaxLog(TaxLogModel tm)
        {
            DbHelper BDCHelper = new DbHelper();
            BDCHelper.SetProvider(MyDBType.Oracle);
            string sql = BDCHelper.CreateInsertStr<TaxLogModel>(tm, "TaxLogModel", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = BDCHelper.GetParamArray<TaxLogModel>(tm, MyDBType.Oracle);
            BDCHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
        }
    }
}