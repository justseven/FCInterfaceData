 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Geo.Plug.DataExchange.XZFCPlug.Dal;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    /// <summary>
    /// 此类存在的意义，可以从bdc数据库中数据
    /// </summary>
    public class GetBDCData : IGetDataBase
    {
        /// <summary>
        /// 将数据传入中间库
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="paramNeeded"></param>
        /// <param name="connection"></param>
        /// <param name="pch"></param>
        /// <returns></returns>
        public IDictionary<string, string> Data2DBAndReturnId(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            string pchr = Data2DB(ps, paramNeeded, connection, pch);
            if (string.IsNullOrEmpty(pchr))
            {
                return new Dictionary<string, string>();
            }
            else {
                IDictionary<string, string> r = new Dictionary<string, string>();
                if (ps.ContainsKey("HID"))
                    r.Add("HID", ps["HID"]);
                if (ps.ContainsKey("ZID"))
                    r.Add("ZID", ps["ZID"]);
                return r;
            }
        }


        public string Data2DB(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch)
        {
            if (!ValidationParam(ps, paramNeeded)) {
                throw new ArgumentNullException();//抛出参数异常
            }
            int row = 0;
            string selectSql = string.Format("select LSZTYBM as ZID from FC_H_QSDC where tstybm='{0}'", ps["HID"]);
            object o = DBHelper.GetScalar(selectSql);
            string sql1 = string.Format(@"Insert into fc_h_tmp (HID,ZID,FWBH,ZH,HH, BDCDYH, QLLX, QLXZ,HX,HXJG, ZXCD,GHYT, ZL, SJC, MYC,DYH,FJH,LJZH,QDJG,
 QDFS, SHBW,YCJZMJ, YCTNJZMJ,YCFTJZMJ,YCDXBFJZMJ,YCQTJZMJ, YCFTXS,JZMJ, TNJZMJ, FTJZMJ, DXBFJZMJ,QTJZMJ,
 FTXS, TDZZRQ,TDYT, TDSYQR, GYTDMJ, FTTDMJ, DYTDMJ, TCJS,CG,ZT, FCFHT, FJSM,FJBM, HSCID, LPBH,SPFHID,PCH) select
TSTYBM as HID,LSZTYBM as ZID,LSFWBH as FWBH,ZH,HH,BDCDYH,QLLX,QLXZ,HX,HXJG,ZXCD,GHYT,ZL,SJC,MYC,DYH        
,FJH,LJZH,QDJG,QDFS,SHBW,YCJZMJ,YCTNJZMJ,YCFTJZMJ,YCDXBFJZMJ,YCQTJZMJ,YCFTXS,JZMJ,TNJZMJ,FTJZMJ,DXBFJZMJ,
QTJZMJ,FTXS,TDZZRQ,TDYT,TDSYQR,GYTDMJ,FTTDMJ,DYTDMJ,TCJS,CG,ZT,FCFHT,FJSM,FJBM,HSCID ,'' as LPBH,'' as SPFHID,'{1}' as PCH
from FC_H_QSDC Where TSTYBM ='{0}'", ps["HID"], pch);
            string sql2 = string.Empty;
            if (o != null)
            {
                sql2 = string.Format(@"insert into fc_z_tmp(ZID,ZH,BDCDYH,XMMC,JZWMC,ZTS,LPLB,LZXZ,LZTD
,FWZL,QLRZS,GHYT,FWJG,ZCS,YCJZMJ,SCJZMJ,ZZDMJ,ZYDMJ,JZWGD,JGRQ,DSCS,DXCS,DXSD
,BZ,MPH,LPBH,PCH)
select TSTYBM as ZID,ZH,BDCDYH,XMMC,JZWMC,ZTS,LPLB,LZXZ,LZTD,FWZL,QLRZS
,GHYT,FWJG,ZCS,YCJZMJ,SCJZMJ,ZZDMJ,ZYDMJ,JZWGD,JGRQ
,DSCS,DXCS,DXSD,BZ,MPH,LPBH,'{0}' as pch from FC_Z_QSDC where TSTYBM='{1}'", pch, o.ToString());
            }
            DbTransaction Tran = connection.BeginTransaction();
            DbCommand command = connection.CreateCommand();
            command.Transaction = Tran;
            try
            { 
                command.CommandType = CommandType.Text;
                command.Parameters.Clear();
                command.CommandText = sql1;
                row= command.ExecuteNonQuery();
                command.Parameters.Clear();
                if (!string.IsNullOrEmpty(sql2))
                {
                    command.CommandText = sql2;
                    row += command.ExecuteNonQuery();
                } 
                Tran.Commit();
            }
            catch (Exception ex)
            {
                Tran.Rollback(); 
                throw new Exception(ex.Message); 
            }
            finally
            {
               // connection.Close();
            }
            if (row > 0)
            { return pch; }
            else
                return string.Empty;
        }

        private bool ValidationParam(IDictionary<string, string> ps, IList<string> paramNeeded)
        {
            if (paramNeeded != null && paramNeeded.Count > 0)
            {
                if (paramNeeded.Any(p => !ps.ContainsKey(p) || string.IsNullOrWhiteSpace(ps[p])))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
