using Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.YGFP;
using Web4BDC.Tools;

namespace Web4BDC.Dal
{
    public class YGFPDAL
    {
        private static readonly object lockKey = new object();
        internal static MZ_YGFP CheckPerson(string prjId)
        {
            string sql = "select CARDID from MZ_YGFP t where t.cardid in (SELECT QLR.ZJHM FROM DJ_QLRGL GL LEFT JOIN DJ_QLR QLR on QLR.QLRID = GL.QLRID and gl.qlrlx='权利人' WHERE gl.SLBH LIKE '%{0}%' )";
            sql = string.Format(sql, prjId);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt&&dt.Rows.Count>0)
                {
                    return ModelHelper<MZ_YGFP>.FillModel(dt.Rows[0]);
                }
                return null;
            }
        }

        internal static DataTable GetBDCInfo(string prjId,string zjhm)
        {
            string sql = @"SELECT
       B.BDCDYH as ANum,
       nvl(h.jzmj, h.ycjzmj) as HouseArea,
       h.zl as HousePosition,
       a.djrq as BuyDate,
       nvl(fwxg.qdjg,0) as HouseMoney,
        '{1}' as IDCard
  FROM DJ_DJB A
  left join ql_fwxg fwxg
  on fwxg.slbh = a.slbh
  LEFT JOIN DJ_TSGL B
    ON A.SLBH = B.SLBH
  LEFT JOIN FC_H_QSDC h
    on h.tstybm = b.tstybm
 WHERE B.TSTYBM IS NOT NULL
   AND A.SLBH LIKE '%{0}%'
UNION ALL
SELECT B.BDCDYH as ANum,
       nvl(h.jzmj, h.ycjzmj) as HouseArea,
       h.zl as HousePosition,
       a.djrq as BuyDate,
       nvl(fwxg.qdjg,0) as HouseMoney,
'{1}' as IDCard
  FROM DJ_YG A
  left join ql_fwxg fwxg
  on fwxg.slbh = a.slbh
  LEFT JOIN DJ_TSGL B
    ON A.SLBH = B.SLBH
  LEFT JOIN FC_H_QSDC h
    on h.tstybm = b.tstybm
 WHERE B.TSTYBM IS NOT NULL
   AND A.SLBH LIKE '%{0}%'";

            sql = string.Format(sql, prjId,zjhm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return dt;
                return null;
            }
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        internal static void InsertLog(Models.BDCModel.MZ_YGFP_TAG tag)
        {

            string sql = "insert into MZ_YGFP_TAG (ID,SLBH,PUSHUSER,PUSHDATE,ISSUCCESS,MESSAGE) values(:ID,:SLBH,:PUSHUSER,:PUSHDATE,:ISSUCCESS,:MESSAGE)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":ID", tag.ID);
            ListAdd(list, ":SLBH", tag.SLBH);
            ListAdd(list, ":PUSHUSER", tag.PUSHUSER);
            ListAdd(list, ":PUSHDATE", tag.PUSHDATE);
            ListAdd(list, ":ISSUCCESS", tag.ISSUCCESS);
            ListAdd(list, ":MESSAGE", tag.MESSAGE);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
            }


        }

        internal static void UpdateTag(MZ_YGFP_TAG tag)
        {
            //string sql = "update FC_SF_TAG set ID=@ID,PUSHUSER=@PUSHUSER,PUSHDATE=@PUSHDATE,ISSUCCESS=@ISSUCCESS,MESSAGE=@MESSAGE where SLBH=@SLBH";

            //List<DbParameter> list = new List<DbParameter>();
            //ListAdd(list, "@ID", tag.ID);
            //ListAdd(list, "@PUSHUSER", tag.PUSHUSER);
            //ListAdd(list, "@PUSHDATE", tag.PUSHDATE);
            //ListAdd(list, "@ISSUCCESS", tag.ISSUCCESS);
            //ListAdd(list, "@MESSAGE", tag.MESSAGE);
            //ListAdd(list, "@SLBH", tag.SLBH);

            string sql = "update MZ_YGFP_TAG set ID='{0}',PUSHUSER='{1}',PUSHDATE=to_date('{2}','yyyy-mm-dd hh24:mi:ss'),ISSUCCESS={3},MESSAGE='{4}' where SLBH='{5}'";
            sql = string.Format(sql, tag.ID, tag.PUSHUSER, tag.PUSHDATE.ToString(), tag.ISSUCCESS, tag.MESSAGE, tag.SLBH);

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                //dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
        }

        internal static bool ExistTag(MZ_YGFP_TAG tag)
        {
            string sql = "select count(1) from MZ_YGFP_TAG where slbh='{0}'";
            sql = string.Format(sql, tag.SLBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                //object o = dbHelper.ExecuteScalar("FCSF",true, System.Data.CommandType.Text, sql, null);
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static List<MZ_YGFP_TAG> GetFailTag()
        {
            string sql = "select ID,SLBH,PUSHUSER,PUSHDATE,ISSUCCESS,MESSAGE from MZ_YGFP_TAG where ISSUCCESS<>'1'";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                    return ModelHelper<Models.BDCModel.MZ_YGFP_TAG>.FillModel(dt);
                return null;
            }
        }
    }
}