using Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Web4BDC.Models.TAXInterface;

namespace Web4BDC.Dal
{
    public class TAXDAL
    {
        DbHelper dbHelper = new DbHelper();


        public TAXDAL()
        {
            dbHelper.SetProvider(MyDBType.Other);
        }
        public void Insert(TAXModels models)
        {
            dbHelper.CreateConn();
            if(dbHelper.Conn.State!=System.Data.ConnectionState.Open)
            {
                dbHelper.Conn.Open();
            }
            DbTransaction trs = dbHelper.Conn.BeginTransaction();

            try
            {
                InsertProofInfo(models.proofInfo, trs);
                InsertProofperson(models.personList, trs);
                InsertZFXX(models.zfxxList, trs);
                
                trs.Commit();
                
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                trs.Rollback();
            }
            
        }


        private void InsertZFXX(zfxxindex zfxxList, DbTransaction trs)
        {

            string sql = dbHelper.CreateInsertStr<zfxxindex>(zfxxList, "zfxxindex", MyDBType.Sql);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<zfxxindex>(zfxxList, MyDBType.Sql);
            dbHelper.ExecuteNonQuery(MyDBType.Other, trs, System.Data.CommandType.Text, sql, param);

        }

        private void InsertProofperson(List<proofperson> personList, DbTransaction trs)
        {
            if (null != personList && personList.Count > 0)
            {
                foreach (proofperson item in personList)
                {
                    string sql = dbHelper.CreateInsertStr<proofperson>(item, "proofperson", MyDBType.Sql);
                    sql = sql.Trim().Replace("\r\n", "");
                    DbParameter[] param = dbHelper.GetParamArray<proofperson>(item, MyDBType.Sql);
                    dbHelper.ExecuteNonQuery(MyDBType.Other, trs, System.Data.CommandType.Text, sql, param);

                }
               
            }
        }

        private void InsertProofInfo(proofinfo proofInfo, DbTransaction trs)
        {
            string sql = dbHelper.CreateInsertStr<proofinfo>(proofInfo, "proofinfo", MyDBType.Sql);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<proofinfo>(proofInfo, MyDBType.Sql);
            dbHelper.ExecuteNonQuery(MyDBType.Other, trs, System.Data.CommandType.Text, sql, param);
        }

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