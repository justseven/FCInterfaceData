using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Web4BDC.Dal
{
    public class TaxInterfaceDal
    {
        public string GetTaxInterfaceBySLBH(string slbh) {
            OracleConnection connection=null;
            string content = string.Empty;
            int actual = 0;
            try
            {
                connection = DBHelper.Connection;
                connection.Open();
                OracleCommand cmd = new OracleCommand("", connection);
                cmd.CommandText = string.Format("Select XML From DJ_TaxInfo Where SLBH='{0}'",slbh);
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    OracleClob myOracleClob = reader.GetOracleClob(0); 
                    StreamReader streamreader = new StreamReader(myOracleClob, Encoding.Unicode);
                    char[] cbuffer = new char[100];
                    while ((actual = streamreader.Read(cbuffer, 0, cbuffer.Length)) > 0)
                    {
                         content += new string(cbuffer, 0, actual); 
                    } 
                    break;
                }
            }
            catch(Exception ex){
                throw ex;
            }
            finally {
                connection.Close();
            }
            return content;
        }


        public string GetTXMBySLBH(string SLBH) {
            string sql=string.Format("Select TXM From DJ_TaxInfo Where SLBH='{0}'", SLBH);
            DataTable dt = DBHelper.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else {
                return string.Empty;
            }
        }
        public void InsertTaxInfo(string xml, string slbh, string ry_id,string txm) { 
            OracleConnection connection = null;
            OracleCommand command = null;
            try
            {
                connection = DBHelper.Connection;
                connection.Open();  

                command = new OracleCommand("Insert into DJ_TaxInfo(SLBH,TXM,XML,RY_ID) values(:SLBH,:TXM,:XML,:RY_ID)", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("SLBH", OracleDbType.NVarchar2);
                command.Parameters["SLBH"].Direction = ParameterDirection.Input;
                command.Parameters["SLBH"].Value = slbh;
                command.Parameters.Add("TXM", OracleDbType.NVarchar2);
                command.Parameters["TXM"].Direction = ParameterDirection.Input;
                command.Parameters["TXM"].Value = txm;
                command.Parameters.Add("XML", OracleDbType.Clob);
                command.Parameters["XML"].Direction = ParameterDirection.Input;
                command.Parameters["XML"].Value = xml;
                command.Parameters.Add("RY_ID", OracleDbType.Clob);
                command.Parameters["RY_ID"].Direction = ParameterDirection.Input;
                command.Parameters["RY_ID"].Value = ry_id; 
                command.ExecuteNonQuery();
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Dispose();
                    connection.Close();
                }
            }
        }

        public void UpdateTaxInfo(string xml, string slbh, string ry_id, string txm)
        {
            OracleConnection connection = null;
            OracleCommand command = null;
            try
            {
                connection = DBHelper.Connection;
                connection.Open(); 
                command = new OracleCommand("Update DJ_TaxInfo Set TXM=:TXM,XML=:XML,RY_ID=:RY_ID where SLBH=:SLBH", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("TXM", OracleDbType.NVarchar2);
                command.Parameters["TXM"].Direction = ParameterDirection.Input;
                command.Parameters["TXM"].Value = txm;
                command.Parameters.Add("XML", OracleDbType.Clob);
                command.Parameters["XML"].Direction = ParameterDirection.Input;
                command.Parameters["XML"].Value = xml;
                command.Parameters.Add("RY_ID", OracleDbType.Clob);
                command.Parameters["RY_ID"].Direction = ParameterDirection.Input;
                command.Parameters["RY_ID"].Value = ry_id;
                command.Parameters.Add("SLBH", OracleDbType.NVarchar2);
                command.Parameters["SLBH"].Direction = ParameterDirection.Input;
                command.Parameters["SLBH"].Value = slbh;
                command.ExecuteNonQuery();
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public bool IfExistsSLBH(string slbh) {
            string sql = string.Format("Select Count(1) From DJ_TaxInfo Where SLBH='{0}'", slbh);
            return DBHelper.GetScalar(sql) > 0;
        }
    }
}