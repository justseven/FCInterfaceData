using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text; 

namespace WbCheck
{
    public class WriteLog
    {
        public static string WriteCheckResult(DataRow dr, string errorType, string pch)
        {

            string sql = "Insert into WBCheck(HouseID,ZL,HouseState,ErrorType,PCH) Values(:HouseID,:ZL,:HouseState,:ErrorType,:PCH)";
            OleDbParameter[] cmdParms = new OleDbParameter[5];
            cmdParms[0] = new OleDbParameter("HouseID", OleDbType.VarChar);
            cmdParms[0].Value = dr["HouseID"].ToString();
            cmdParms[1] = new OleDbParameter("ZL", OleDbType.VarChar);
            cmdParms[1].Value = dr["ZL"].ToString();
            cmdParms[2] = new OleDbParameter("HouseState", OleDbType.VarChar);
            cmdParms[2].Value = dr["HouseState"].ToString();
            cmdParms[3] = new OleDbParameter("ErrorType", OleDbType.VarChar);
            cmdParms[3].Value = errorType;
            cmdParms[4] = new OleDbParameter("PCH", OleDbType.VarChar);
            cmdParms[4].Value = pch;
            OleDBHelper.ExecuteSql(sql, cmdParms);
            return pch;
        }
    }
}
