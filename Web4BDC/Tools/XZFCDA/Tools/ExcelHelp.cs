using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Web4BDC.Tools
{
    public class ExcelHelp
    {
        public static DataTable LoadDataFromExcel(string filePath,string sheetName)
        {
            try
            {
                string strConn;
                if (filePath.Contains(".xlsx"))
                {
                    strConn = "Provider= Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                }
                else
                {
                    strConn = "Provider= Microsoft.JET.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                }
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  [" + sheetName + "$]";//可是更改Sheet名称，比如sheet2，等等    

                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, sheetName);
                OleConn.Close();

                return OleDsExcle.Tables[sheetName];
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        


    }
}
