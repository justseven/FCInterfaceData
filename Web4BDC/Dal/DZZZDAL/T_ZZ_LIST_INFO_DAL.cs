using Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Web4BDC.Models;

namespace Web4BDC.Dal.DZZZDAL
{
    public class T_ZZ_LIST_INFO_DAL
    {
        private void Inser(T_ZZ_LIST_INFO zz)
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            string sql = dbHelper.CreateInsertStr<T_ZZ_LIST_INFO>(zz, "T_ZZ_LIST_INFO", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<T_ZZ_LIST_INFO>(zz, MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);

        }
    }
}