using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace Web4BDC.Dal
{
    public class BDCExtendDal
    {

        internal int ResetUploadFileCreateTime(DateTime createTime, string slbh, string CID)
        {
            string sql = "Update WFM_ATTACHLST set CreateDate=:CreateTime where PNODE=:SLBH and CID=:CID";
            string connectStr = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
            int num = DBHelper.ExecuteSql(connectStr,sql, new OracleParameter[]{
                new OracleParameter(){
                    OracleDbType=OracleDbType.Date,
                    Direction=ParameterDirection.Input,
                    ParameterName="CreateTime",
                    Value=createTime
                }, new OracleParameter(){
                    OracleDbType=OracleDbType.NVarchar2,
                    Direction=ParameterDirection.Input,
                    ParameterName="SLBH",
                    Value=slbh
                }, new OracleParameter(){
                    OracleDbType=OracleDbType.NVarchar2,
                    Direction=ParameterDirection.Input,
                    ParameterName="CID",
                    Value=CID
                }
            });
            return num;
        }

        internal string GetYCHouseIdBy(string tstybm) {
            string sql = string.Format("Select HYCID  from FC_H_QSDC Where Tstybm ='{0}'", tstybm);
            DataTable dt= DBHelper.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else {
                return string.Empty;
            }
        }
        internal string GetSCHouseIdBy(string tstybm)
        {
            string sql = string.Format("Select HSCID  from FC_H_QSDC Where Tstybm ='{0}'", tstybm);
            DataTable dt = DBHelper.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        internal string GetDJLX(string slbh) {
            string sql = string.Format("Select DJZL from DJ_tsgl Where slbh like'{0}%'", slbh);
            DataTable dt = DBHelper.GetDataTable(sql);
           
            if (dt != null && dt.Rows.Count > 0)
            {
                string djlx = dt.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(djlx))
                {
                    sql = string.Format("Select Count(1) from DJ_DJB Where SLBH like'{0}%'", slbh);
                    if (DBHelper.GetScalar(sql) > 0)
                    {
                        return "权属";
                    }
                    sql = string.Format("Select Count(1) from DJ_YG Where SLBH like'{0}%'", slbh);
                    if (DBHelper.GetScalar(sql) > 0)
                    {
                        return "预告";
                    }
                    sql = string.Format("Select Count(1) from DJ_DY Where SLBH like'{0}%'", slbh);
                    if (DBHelper.GetScalar(sql) > 0)
                    {
                        return "抵押";
                    }
                    sql = string.Format("Select Count(1) from DJ_CF Where SLBH like'{0}%'", slbh);
                    if (DBHelper.GetScalar(sql) > 0)
                    {
                        return "查封";
                    }
                    else {
                        return string.Empty;
                    }
                }
                else {
                    return djlx;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        internal IList<string> GetHouseIdsBySLBH(string slbh) {
            string sql = string.Format("Select nvl(FC_H_QSDC.ORACLE_WB_HOUSEID,FC_H_QSDC.TSTYBM) as TSTYBM From DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where SLBH like '{0}%'", slbh);
            DataTable dt= DBHelper.GetDataTable(sql);
            IList<string> hs = new List<string>();
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    hs.Add(dt.Rows[i][0].ToString());
                }
            }
            return hs;
        }

        internal DataTable GetHousesBySLBH(string slbh)
        {
            string sql = string.Format("Select nvl(FC_H_QSDC.ORACLE_WB_HOUSEID,FC_H_QSDC.TSTYBM) as TSTYBM,FC_H_QSDC.ZL From DJ_TSGL Left join FC_H_QSDC on DJ_TSGL.TSTYBM=FC_H_QSDC.TSTYBM Where SLBH like '{0}%'", slbh);
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }

        internal DataTable GetTZRXMBySLBH(string slbh) {
            string sql = string.Format("Select TZRXM From DJ_SJD Where SLBH like '{0}%'", slbh);
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        
        internal DataTable GetDYRBySLBH(string slbh)
        { 
            string sql = string.Format("Select DJ_QLR.QLRMC From DJ_QLRGL Left join DJ_QLR on DJ_QLRGL.QLRID=DJ_QLR.QLRID Where SLBH like '{0}%' and QLRLX='抵押人'", slbh);
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
        internal DataTable GetQLRBySLBH(string slbh)
        {
            string sql = string.Format("Select DJ_QLR.QLRMC From DJ_QLRGL Left join DJ_QLR on DJ_QLRGL.QLRID=DJ_QLR.QLRID Where SLBH like '{0}%' and QLRLX='权利人'", slbh);
            DataTable dt = DBHelper.GetDataTable(sql);
            return dt;
        }
    }
}