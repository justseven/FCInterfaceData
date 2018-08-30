using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using System.Web;
using DBCForFCWebService.Model.ZZDZ;
using System.IO;
using Config;

namespace DBCForFCWebService.Dal
{
    public class ZZDZ_DAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;
        private static string WDK = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ConnectionString;
        public DataTable GetCerQuery(string lzrzjh)
        {
            List<DbParameter> list = new List<DbParameter>();
            Oracle.DataAccess.Client.OracleParameter inputparm = new Oracle.DataAccess.Client.OracleParameter("LZRZJH", Oracle.DataAccess.Client.OracleDbType.NVarchar2);
            inputparm.Direction = ParameterDirection.Input;
            inputparm.Value = lzrzjh;

            Oracle.DataAccess.Client.OracleParameter outputparm = new Oracle.DataAccess.Client.OracleParameter("RETURNVAL", Oracle.DataAccess.Client.OracleDbType.RefCursor);
            outputparm.Direction = ParameterDirection.Output;
            list.Add(inputparm);
            list.Add(outputparm);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            
            DataTable dt= dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.StoredProcedure, "LND_PROC_GETCERTQUERY", list.ToArray());
            return GetDistinctSelf(dt, "BDCQZH");
        }

        internal DataTable GetCerInfo(string ywh, string qLRZJH, string bDCQZH)
        {
            //           nvl(fwqllx.itemname, zdqllx.itemname) AS QLLX,
            //nvl(fwqlxz.itemname, zdqlxz.itemname) AS QLXZ,
            //nvl(fwytlx.ytmc, dlb.dlmc) AS YT,
            //nvl(B.Zl,zd.tdzl) as ZL,
            string sql = @"select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczh as BDCQZH,
a.djrq as fzeq,
'' AS QLR,
item.itemname as GYQK,
sjd.zl as ZL,
nvl(a.BDCDYH,zd.bdcdyh) AS BDCDYH,
c.jzwmc as ZH,
B.dyh ||'-'|| B.FJH as HH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
zdqlxz.itemname ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
(case when tdxg.fttdmj is null then ('土地使用权面积: '|| to_char(tdxg.gytdmj,'fm9990.0099') ||  ' ㎡  /  '||  '房屋建筑面积: '||  fwxg.jzmj ||' ㎡') else ('土地使用权面积: ' || to_char(tdxg.fttdmj,'fm9990.0099')  ||  ' ㎡ /  房屋建筑面积: '||  fwxg.jzmj ||' ㎡') END) as MJ,
(case when tdxg.zzrq is null then U'' else (zdqllx.itemname || to_char(TDXG.zzrq,'yyyy')|| '年' ||to_char(TDXG.zzrq,'MM')||'月'||to_char(TDXG.zzrq,'dd')||'日'|| ' 止' ) END) as SYQX,
a.qt as QLQTQK,
nvl(a.fj,(select djjs from dj_djb_js djbjs where djbjs.slbh=a.slbh and djbjs.jslx = '登记记事' and djbjs.sfdy='是')) as FJ,
U'' as zdt,
U'' as FHT,
B.FCFHT as FHT1,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'不动产权证' as QLMC,
'' as YWR,
B.zdtybm || ' '||tdxg.jzzdmj as QRCODE
 from dj_djb A
left join dj_djb_js djbjs on a.slbh=djbjs.slbh
left join dj_sjd sjd on sjd.slbh=A.slbh
left join ql_tdxg tdxg on tdxg.slbh=A.slbh
left join ql_fwxg fwxg on fwxg.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join fc_z_qsdc C on c.tstybm=B.lsztybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)
LEFT JOIN (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '共有方式' AND ROWNUM<2) ) item on item.itemval=a.gyfs 
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join dic_dlb@xzbdcggk dlb on dlb.dlbm=tdxg.tdyt 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利类型' AND ROWNUM<2) ) fwqllx on fwqllx.itemval=b.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利性质' AND ROWNUM<2) ) fwqlxz on fwqlxz.itemval=b.qlxz 

left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利类型' AND ROWNUM<2) ) zdqllx on zdqllx.itemval=tdxg.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利性质' AND ROWNUM<2) ) zdqlxz on zdqlxz.itemval=tdxg.qlxz 
where (A.Lifecycle is null or A.Lifecycle=0) {0} 
UNION ALL
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
a.djrq as fzeq,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(a.BDCDYH,zd.bdcdyh) AS BDCDYH,
c.jzwmc as ZH,
B.dyh ||'-'|| B.FJH as HH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
zdqlxz.itemname ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
(case when tdxg.fttdmj is null then ('土地使用权面积: '|| to_char(tdxg.gytdmj,'fm9990.0099') ||  ' ㎡  /  '||  '房屋建筑面积: '||  fwxg.jzmj ||' ㎡') else ('土地使用权面积: ' || to_char(tdxg.fttdmj,'fm9990.0099')  ||  ' ㎡ /  房屋建筑面积: '||  fwxg.jzmj ||' ㎡') END) as MJ,
(case when tdxg.zzrq is null then U'' else (zdqllx.itemname || to_char(TDXG.zzrq,'yyyy')|| '年' ||to_char(TDXG.zzrq,'MM')||'月'||to_char(TDXG.zzrq,'dd')||'日'|| ' 止' ) END) as SYQX,
a.qt as QLQTQK,
nvl(a.fj,(select djjs from dj_djb_js djbjs where djbjs.slbh=a.slbh and djbjs.jslx = '登记记事' and djbjs.sfdy='是')) as FJ,
U'' as zdt,
U'' as FHT,B.FCFHT as FHT1,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
to_char(A.dylx) as QLMC,
'' as YWR ,
B.zdtybm || ' '||tdxg.jzzdmj as QRCODE
from dj_dy A
left join dj_djb_js djbjs on a.slbh=djbjs.slbh
left join dj_sjd sjd on sjd.slbh=A.slbh
left join ql_tdxg tdxg on tdxg.slbh=A.slbh
left join ql_fwxg fwxg on fwxg.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join fc_z_qsdc C on c.tstybm=B.lsztybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)

left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join (select DLMC,DLBM from dic_dlb@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地规划分类编码表' AND ROWNUM<2) ) dlb on dlb.dlbm=zd.sjtdyt 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利类型' AND ROWNUM<2) ) fwqllx on fwqllx.itemval=b.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利性质' AND ROWNUM<2) ) fwqlxz on fwqlxz.itemval=b.qlxz 

left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利类型' AND ROWNUM<2) ) zdqllx on zdqllx.itemval=tdxg.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利性质' AND ROWNUM<2) ) zdqlxz on zdqlxz.itemval=tdxg.qlxz 
where (A.Lifecycle is null or A.Lifecycle=0) {1}
union all 
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
a.djrq as fzeq,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(a.BDCDYH,zd.bdcdyh) AS BDCDYH,
c.jzwmc as ZH,
B.dyh ||'-'|| B.FJH as HH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
zdqlxz.itemname ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
(case when tdxg.fttdmj is null then ('土地使用权面积: '|| to_char(tdxg.gytdmj,'fm9990.0099') ||  ' ㎡  /  '||  '房屋建筑面积: '||  fwxg.jzmj ||' ㎡') else ('土地使用权面积: ' || to_char(tdxg.fttdmj,'fm9990.0099')  ||  ' ㎡ /  房屋建筑面积: '||  fwxg.jzmj ||' ㎡') END) as MJ,
(case when tdxg.zzrq is null then U'' else (zdqllx.itemname || to_char(TDXG.zzrq,'yyyy')|| '年' ||to_char(TDXG.zzrq,'MM')||'月'||to_char(TDXG.zzrq,'dd')||'日'|| ' 止' ) END) as SYQX,
a.qt as QLQTQK,
nvl(a.fj,(select djjs from dj_djb_js djbjs where djbjs.slbh=a.slbh and djbjs.jslx = '登记记事' and djbjs.sfdy='是')) as FJ,
U'' as zdt,
U'' as FHT,B.FCFHT as FHT1,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'预告登记' as QLMC,
'' as YWR ,
B.zdtybm || ' '||tdxg.jzzdmj as QRCODE
from dj_yg A
left join dj_djb_js djbjs on a.slbh=djbjs.slbh
left join dj_sjd sjd on sjd.slbh=A.slbh
left join ql_tdxg tdxg on tdxg.slbh=A.slbh
left join ql_fwxg fwxg on fwxg.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join fc_z_qsdc C on c.tstybm=B.lsztybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)
LEFT JOIN (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '共有方式' AND ROWNUM<2) ) item on item.itemval=a.gyfs 
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join (select DLMC,DLBM from dic_dlb@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地规划分类编码表' AND ROWNUM<2) ) dlb on dlb.dlbm=zd.sjtdyt 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利类型' AND ROWNUM<2) ) fwqllx on fwqllx.itemval=b.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利性质' AND ROWNUM<2) ) fwqlxz on fwqlxz.itemval=b.qlxz 

left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利类型' AND ROWNUM<2) ) zdqllx on zdqllx.itemval=tdxg.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利性质' AND ROWNUM<2) ) zdqlxz on zdqlxz.itemval=tdxg.qlxz 
where (A.Lifecycle is null or A.Lifecycle=0) {1}
union all 
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
a.djrq as fzeq,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(a.BDCDYH,zd.bdcdyh) AS BDCDYH,
c.jzwmc as ZH,
B.dyh ||'-'|| B.FJH as HH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
zdqlxz.itemname ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
(case when tdxg.fttdmj is null then ('土地使用权面积: '|| to_char(tdxg.gytdmj,'fm9990.0099') ||  ' ㎡  /  '||  '房屋建筑面积: '||  fwxg.jzmj ||' ㎡') else ('土地使用权面积: ' || to_char(tdxg.fttdmj,'fm9990.0099')  ||  ' ㎡ /  房屋建筑面积: '||  fwxg.jzmj ||' ㎡') END) as MJ,
(case when tdxg.zzrq is null then U'' else (zdqllx.itemname || to_char(TDXG.zzrq,'yyyy')|| '年' ||to_char(TDXG.zzrq,'MM')||'月'||to_char(TDXG.zzrq,'dd')||'日'|| ' 止' ) END) as SYQX,
a.qt as QLQTQK,
nvl(a.fj,(select djjs from dj_djb_js djbjs where djbjs.slbh=a.slbh and djbjs.jslx = '登记记事' and djbjs.sfdy='是')) as FJ,
U'' as zdt,
U'' as FHT,B.FCFHT as FHT1,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'异议登记' as QLMC,
'' as YWR ,
B.zdtybm || ' '||tdxg.jzzdmj as QRCODE
from dj_yy A
left join dj_djb_js djbjs on a.slbh=djbjs.slbh
left join dj_sjd sjd on sjd.slbh=A.slbh
left join ql_tdxg tdxg on tdxg.slbh=A.slbh
left join ql_fwxg fwxg on fwxg.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join fc_z_qsdc C on c.tstybm=B.lsztybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)

left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join (select DLMC,DLBM from dic_dlb@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地规划分类编码表' AND ROWNUM<2) ) dlb on dlb.dlbm=zd.sjtdyt 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利类型' AND ROWNUM<2) ) fwqllx on fwqllx.itemval=b.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '房屋权利性质' AND ROWNUM<2) ) fwqlxz on fwqlxz.itemval=b.qlxz 

left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利类型' AND ROWNUM<2) ) zdqllx on zdqllx.itemval=tdxg.qllx 
left join (select itemval,itemname from dic_item@xzbdcggk where DICCODE = (SELECT DICCODE FROM DIC_MAIN@xzbdcggk WHERE DICNAME = '土地权利性质' AND ROWNUM<2) ) zdqlxz on zdqlxz.itemval=tdxg.qlxz 
where (A.Lifecycle is null or A.Lifecycle=0) {1}";

            string where1 = "";
            string where2 = "";

            bool flag = false;

            if(!string.IsNullOrEmpty(ywh))
            {
                where2=where1 += string.Format(" and A.SLBH='{0}'", ywh);
                flag = true;
            }
            if(!string.IsNullOrEmpty(bDCQZH))
            {
                where1 += string.Format(" and A.bdczh ='{0}'", bDCQZH);
                where2+= string.Format(" and A.bdczmh ='{0}'", bDCQZH);
                flag = true;
            }
            //where1 += " and djbjs.jslx = '登记记事'";
            //where2 += " and djbjs.jslx = '登记记事'";

            if (flag)
            {
                sql = string.Format(sql, where1, where2);
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                return CreateResultDataTable(dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null));
            }
            else
            {
                return GetCEerInfoByZJHM(qLRZJH);
            }
            
        }

        internal DataTable GetALLBDCDYH(string slbh)
        {
            string sql = "select bdcdyh from dj_tsgl where slbh='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            return dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
        }

        internal DataTable GetCerQuery_Bank(string bank)
        {
            List<DbParameter> list = new List<DbParameter>();
            Oracle.DataAccess.Client.OracleParameter inputparm = new Oracle.DataAccess.Client.OracleParameter("LZRZJH", Oracle.DataAccess.Client.OracleDbType.NVarchar2);
            inputparm.Direction = ParameterDirection.Input;
            inputparm.Value = bank;

            Oracle.DataAccess.Client.OracleParameter outputparm = new Oracle.DataAccess.Client.OracleParameter("RETURNVAL", Oracle.DataAccess.Client.OracleDbType.RefCursor);
            outputparm.Direction = ParameterDirection.Output;
            list.Add(inputparm);
            list.Add(outputparm);
            DbHelper dbHelper = new DbHelper();

            dbHelper.SetProvider(MyDBType.Oracle);

            DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.StoredProcedure, "LND_PROC_GETCERTQUERY_BANK", list.ToArray());
            return GetDistinctSelf(dt, "BDCQZH");
        }


        private DataTable GetDistinctSelf(DataTable SourceDt, string filedName)
        {
            if (SourceDt.Rows.Count > 1)
            {
                for (int i = SourceDt.Rows.Count - 1; i > 0; i--)
                {
                    DataRow[] rows = SourceDt.Select(string.Format("{0}='{1}'", filedName, SourceDt.Rows[i][filedName]));
                    if (rows.Length > 1)
                    {
                        SourceDt.Rows.RemoveAt(i);
                    }
                }
            }
            return SourceDt;


        }

        internal string GetFtpPath(string fileid, string v)
        {
            string sql = "select * from DOC_BINFILE t where  FILEID = '{0}'";
            sql = string.Format(sql, fileid, v);
            DbHelper dbHelper = new DbHelper();

            dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);
            dbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt = dbHelper.ExecuteTable(dbHelper.Conn, CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count == 1)
            {
                return dt.Rows[0]["FTPATH"].ToString();
            }


            if (null != dt && dt.Rows.Count > 1)
            {
                DataRow[] rows = dt.Select(string.Format("filename in ({0})", v));
                if (null != rows && rows.Length == 1)
                {
                    return rows[0]["FTPATH"].ToString();
                }
            }

            return "";
        }

        internal string GetFileID(string bdcdyh, string v)
        {
            string tstybm = GetTSTYBM(bdcdyh, v);
            string sql = string.Format("select FILEID from PUB_ATTACHLST t where parentnode in ({0}) ", tstybm);
            DbHelper dbHelper = new DbHelper();
            dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
            dbHelper.SetProvider(MyDBType.Oracle);
            object o= dbHelper.ExecuteScalar(dbHelper.Conn, CommandType.Text, sql, null);
            if (null != o)
                return o.ToString();
            return "";
        }

        private string GetTSTYBM(string bdcdyh, string v)
        {
            string sql = "";
            string fc_sql = "select tstybm from fc_h_qsdc where bdcdyh ='{0}'";
            string zd_sql = "select (zd.tstybm||''','''||h.lsztybm) as tstybm  from zd_qsdc zd left join fc_h_qsdc h on h.zdtybm=zd.zdtybm where h.bdcdyh='{0}'";
            //string zd_sql= "select lsztybm from fc_h_qsdc where bdcdyh='{0}'";
            switch (v)
            {
                case "宗地图":
                    sql = string.Format(zd_sql, bdcdyh);
                    break;
                default:
                    sql = string.Format(fc_sql, bdcdyh);
                    break;
            }
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            object o = dbHelper.ExecuteScalar(MyDBType.Oracle, CommandType.Text, sql, null);
            if (null != o)
                return "'"+o.ToString()+"'";
            return "";
        }

        internal void InsertLog(ZZDYLOG log)
        {
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            string sql = dbHelper.CreateInsertStr<ZZDYLOG>(log, "ZZDYLOG", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = dbHelper.GetParamArray<ZZDYLOG>(log, MyDBType.Oracle);

            dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
        }

        private DataTable GetCEerInfoByZJHM(string qLRZJH)
        {
            string sql = @"select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczh as BDCQZH,
'' AS QLR,
item.itemname as GYQK,
sjd.zl as ZL,
nvl(B.BDCDYH,zd.bdcdyh) AS BDCDYH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
dlb.dlmc ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
nvl(b.jzmj,zd.tdzmj) as MJ,
nvl(b.tdsyqr,zd.syqx) as SYQX,
a.qt as QLQTQK,
a.fj as FJ,
U'' as zdt,
U'' as FHT,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'不动产权证' as QLMC,
'' as YWR
 from dj_djb A
left join dj_sjd sjd on sjd.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)
LEFT JOIN dic_item@xzbdcggk item on item.itemval=a.gyfs and item.diccode='150106151438XH6KL22QEP'
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join dic_dlb@xzbdcggk dlb on dlb.dlbm=zd.sjtdyt and dlb.diccode='070820145140S02VG02538'
left join dic_item@xzbdcggk fwqllx on fwqllx.itemval=b.qllx and fwqllx.diccode='150106204424C42G0X65M4'
left join dic_item@xzbdcggk fwqlxz on fwqlxz.itemval=b.qlxz and fwqlxz.diccode='1412031055513P9CS92G08'

left join dic_item@xzbdcggk zdqllx on zdqllx.itemval=zd.qllx and zdqllx.diccode='150602150538D535T37215'
left join dic_item@xzbdcggk zdqlxz on zdqlxz.itemval=zd.qlxz and zdqlxz.diccode='150518145529SXO7V3CB83'
where  qlr.ZJHM='{0}' and (A.Lifecycle is null or A.Lifecycle=0)  AND (qlrgl.QLRLX IS NULL OR qlrgl.QLRLX IN ('权利人', '抵押权人'))
UNION ALL
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(B.BDCDYH,zd.bdcdyh) AS BDCDYH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
dlb.dlmc ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT, 
nvl(b.jzmj,zd.tdzmj) as MJ,
nvl(b.tdsyqr,zd.syqx) as SYQX,
a.qt as QLQTQK,
a.fj as FJ,
U'' as zdt,
U'' as FHT,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
to_char(A.dylx) as QLMC,
'' as YWR 
from dj_dy A
left join dj_sjd sjd on sjd.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)

left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join dic_dlb@xzbdcggk dlb on dlb.dlbm=zd.sjtdyt and dlb.diccode='070820145140S02VG02538'
left join dic_item@xzbdcggk fwqllx on fwqllx.itemval=b.qllx and fwqllx.diccode='150106204424C42G0X65M4'
left join dic_item@xzbdcggk fwqlxz on fwqlxz.itemval=b.qlxz and fwqlxz.diccode='1412031055513P9CS92G08'

left join dic_item@xzbdcggk zdqllx on zdqllx.itemval=zd.qllx and zdqllx.diccode='150602150538D535T37215'
left join dic_item@xzbdcggk zdqlxz on zdqlxz.itemval=zd.qlxz and zdqlxz.diccode='150518145529SXO7V3CB83'
where  qlr.ZJHM='{0}' and (A.Lifecycle is null or A.Lifecycle=0) AND (qlrgl.QLRLX IS NULL OR qlrgl.QLRLX IN ('权利人', '抵押权人'))
union all 
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(B.BDCDYH,zd.bdcdyh) AS BDCDYH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
dlb.dlmc ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
nvl(b.jzmj,zd.tdzmj) as MJ,
nvl(b.tdsyqr,zd.syqx) as SYQX,
a.qt as QLQTQK,
a.fj as FJ,
U'' as zdt,
U'' as FHT,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'预告登记' as QLMC,
'' as YWR 
from dj_yg A
left join dj_sjd sjd on sjd.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)
LEFT JOIN dic_item@xzbdcggk item on item.itemval=a.gyfs and item.diccode='150106151438XH6KL22QEP'
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join dic_dlb@xzbdcggk dlb on dlb.dlbm=zd.sjtdyt and dlb.diccode='070820145140S02VG02538'
left join dic_item@xzbdcggk fwqllx on fwqllx.itemval=b.qllx and fwqllx.diccode='150106204424C42G0X65M4'
left join dic_item@xzbdcggk fwqlxz on fwqlxz.itemval=b.qlxz and fwqlxz.diccode='1412031055513P9CS92G08'

left join dic_item@xzbdcggk zdqllx on zdqllx.itemval=zd.qllx and zdqllx.diccode='150602150538D535T37215'
left join dic_item@xzbdcggk zdqlxz on zdqlxz.itemval=zd.qlxz and zdqlxz.diccode='150518145529SXO7V3CB83'
where  qlr.ZJHM='{0}' and (A.Lifecycle is null or A.Lifecycle=0) AND (qlrgl.QLRLX IS NULL OR qlrgl.QLRLX IN ('权利人', '抵押权人'))
union all 
select 
a.slbh as YWH,
'' as QLRZJH,
a.bdczmh as BDCQZH,
'' AS QLR,
U'' as GYQK,
sjd.zl as ZL,
nvl(B.BDCDYH,zd.bdcdyh) AS BDCDYH,
zdqllx.itemname ||' / '|| fwqllx.itemname AS QLLX,
dlb.dlmc ||' / '|| fwqlxz.itemname AS QLXZ,
dlb.dlmc ||' / '|| fwytlx.ytmc AS YT,
nvl(b.jzmj,zd.tdzmj) as MJ,
nvl(b.tdsyqr,zd.syqx) as SYQX,
a.qt as QLQTQK,
a.fj as FJ,
U'' as zdt,
U'' as FHT,
'' AS EWM,
a.ssjc as JC,
a.fznd as NF,
A.Jgjc as SX,
A.Zsh as XH,
'异议登记' as QLMC,
'' as YWR 
from dj_yy A
left join dj_sjd sjd on sjd.slbh=A.slbh
left join dj_tsgl tsgl on tsgl.slbh=A.Slbh
left join fc_h_qsdc B on B.Tstybm=tsgl.tstybm
left join zd_qsdc zd on (zd.zdtybm=b.Zdtybm or zd.tstybm=tsgl.tstybm)
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=b.Ghyt
left join dic_dlb@xzbdcggk dlb on dlb.dlbm=zd.sjtdyt and dlb.diccode='070820145140S02VG02538'
left join dic_item@xzbdcggk fwqllx on fwqllx.itemval=b.qllx and fwqllx.diccode='150106204424C42G0X65M4'
left join dic_item@xzbdcggk fwqlxz on fwqlxz.itemval=b.qlxz and fwqlxz.diccode='1412031055513P9CS92G08'

left join dic_item@xzbdcggk zdqllx on zdqllx.itemval=zd.qllx and zdqllx.diccode='150602150538D535T37215'
left join dic_item@xzbdcggk zdqlxz on zdqlxz.itemval=zd.qlxz and zdqlxz.diccode='150518145529SXO7V3CB83'
where qlr.ZJHM='{0}' and (A.Lifecycle is null or A.Lifecycle=0) AND (qlrgl.QLRLX IS NULL OR qlrgl.QLRLX IN ('权利人', '抵押权人'))";
            sql = string.Format(sql, qLRZJH);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            return CreateResultDataTable(dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null));
        }



        private DataTable CreateResultDataTable(DataTable dt)
        {
            if (null != dt && dt.Rows.Count > 0)
            {
                dt = DistinctTable(dt);
                if (null != dt && dt.Rows.Count > 0)
                {
                    string slbh = dt.Rows[0]["YWH"].ToString();
                    string qlrmc = "";
                    string zjhm = "";
                    string ywr = "";
                    GetXGRInfo(slbh, 0, out qlrmc, out zjhm);
                    dt.Rows[0]["QLRZJH"] = zjhm;
                    dt.Rows[0]["QLR"] = qlrmc;
                    GetXGRInfo(slbh, 1, out ywr, out zjhm);
                    dt.Rows[0]["YWR"] = ywr;
                    string qlmc = dt.Rows[0]["QLMC"].ToString();
                    if(qlmc.Contains("预告抵押"))
                    {
                        qlmc= "预售商品房抵押权预告登记";
                    }
                    else
                    {
                        qlmc = "抵押权";
                    }
                    dt.Rows[0]["QLMC"] = qlmc;
                    dt.Rows[0]["QRCODE"] = dt.Rows[0]["QRCODE"] +" "+qlrmc;
                }
                return dt;
            }
            return null;
        }

        private DataTable DistinctTable(DataTable dt)
        {
            DataTable colneDT = dt.Clone();
            DataRow colneRow = colneDT.NewRow();
            //colneRow["BDCDYH"] = GetBDCDYH(dt);
            colneDT.Rows.Add(colneRow);
            if (null!=dt && dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach(DataColumn col in dt.Columns)
                    {
                        if (colneRow[col.ColumnName].ToString().Contains(row[col.ColumnName].ToString()))// ||col.ColumnName.ToUpper().Equals("BDCDYH"))
                        {
                            continue;
                        }
                        else
                        {
                            if (col.ColumnName.ToUpper().Equals("MJ"))
                            {

                                string colneMJ = GetMJ(colneRow).Trim();
                                string rowMJ = GetMJ(row).Trim();
                                if (!colneMJ.Equals(rowMJ))
                                {
                                    double mj1 = 0;
                                    double mj2 = 0;
                                    if(!string.IsNullOrEmpty(colneMJ))
                                    {
                                        mj1 = Convert.ToDouble(colneMJ);
                                    }
                                    if (!string.IsNullOrEmpty(rowMJ))
                                    {
                                        mj2 = Convert.ToDouble(rowMJ);
                                    }
                                    string mj = (mj1 +mj2 ) + "";
                                    row[col] = GetReplaceMJ(row, mj);
                                }

                            }
                            colneRow[col.ColumnName] = row[col];
                        }
                    }
                }
            }
            
            return colneDT;
        }

        private string GetBDCDYH(DataTable dt)
        {
            string bdcdyh = "";
            DataRow[] rows = dt.Select("QLLX LIKE '%房屋所有权%'");
            if(rows.Length>1)
            {
                bdcdyh= rows[0]["BDCDYH"] + "等" + rows.Length + "个";
            }
            if(string.IsNullOrEmpty(bdcdyh))
            {
                bdcdyh= dt.Rows[0]["BDCDYH"] + (dt.Rows.Count >1?"等" + dt.Rows.Count + "个":"");
            }
            return bdcdyh;
        }

        private string GetMJ(DataRow colneRow)
        {
            string colneMJStr = colneRow["MJ"].ToString().Trim();
            if(string.IsNullOrEmpty(colneMJStr))
            {
                return "";
            }
            string colneMJ = colneMJStr.Substring(colneMJStr.IndexOf("房屋建筑面积:")+7).Replace("㎡","");

            return colneMJ;
        }

        private string GetReplaceMJ(DataRow row,string mj)
        {
            string oldMJ = row["MJ"].ToString();
            string source = oldMJ.Substring(oldMJ.LastIndexOf("/")+1).Trim();
            return oldMJ.Replace(source, "房屋建筑面积:" + mj+ " ㎡");
        }

        private object GetEWM()
        {
            throw new NotImplementedException();
        }

        private string GetXGRInfo(string slbh, int v,out string qlrmc,out string zjhm)
        {
            string sql = @"select wm_concat(to_char(qlr.qlrmc)) qlr, wm_concat(to_char(qlr.zjhm)) zjhm from dj_qlrgl qlrgl  
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid 
where qlrgl.slbh='{0}' and (qlrgl.QLRLX IS NULL OR qlrgl.QLRLX IN ('{1}', '{2}'))";
            switch(v)
            {
                case 0:
                    sql = string.Format(sql, slbh, "权利人", "抵押权人");
                    break;
                default:
                    sql = string.Format(sql, slbh, "义务人", "抵押人");
                    break;
            }
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            DataTable dt= dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
            if(null!=dt && dt.Rows.Count>0)
            {
                qlrmc = dt.Rows[0]["qlr"].ToString().Replace(",","、");
                zjhm = dt.Rows[0]["zjhm"].ToString().Replace(",", "、");
            }
            else
            {
                qlrmc = "";
                zjhm = "";
            }
            return qlrmc + "|" + zjhm;
        }

        internal int UPDATECERTINFO(string yWH, string qLRZJH, string bDCQZH, string lZRXM, string lZRZJH, string ySXLH, string ySXLHP, string machineCode)
        {

            string[] zjhArray = qLRZJH.Split('、');
            DataTable dt = GetSLBH(yWH, "", zjhArray[0], bDCQZH);
            if (null != dt && dt.Rows.Count > 0)
            {
                string slbh = dt.Rows[0]["SLBH"].ToString();
                string tableName = dt.Rows[0]["TABLENAME"].ToString();

                DJ_FJD fjd = CreateFJD(yWH, machineCode, 1);


                DJ_FJD_FB fb = CreateFJD_FB(yWH, bDCQZH, ySXLH);
                DJ_FJD_LJR ljr = null;
                if (!string.IsNullOrEmpty(ySXLHP))
                {
                    byte[] arr = Convert.FromBase64String(ySXLHP);
                    ljr = CreateFJD_LJR(yWH, lZRXM, "1", lZRZJH, arr);
                }
                else
                {
                    ljr = CreateFJD_LJR(yWH, lZRXM, "1", lZRZJH, null);
                }


                return UpdateFJD(fjd, fb, ljr, tableName, ySXLH);

            }


            return -1;
        }

        private string GetQLMC(string bDCQZH)
        {
            if(bDCQZH.Contains("证明"))
            {
                return "不动产登记证明";
            }
            return "不动产权证书";
        }

        private decimal GetQDXH(string yWH)
        {
            string sql = string.Format("select max(qdxh) from dj_fjd_fb where slbh='{0}'", yWH);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            object count = dbHelper.ExecuteScalar(MyDBType.Oracle, CommandType.Text, sql, null);

            try
            {
                decimal qdxh = Convert.ToDecimal(count);
                return qdxh + 1;
            }
            catch
            {
                return 1;
            }
        }

        internal int UPDATECERTSTATE(string yWH, string qLRZJH, string bDCQZH, int state,string machineCode)
        {
            DataTable dt = GetSLBH(yWH, "", qLRZJH, bDCQZH);
            if (null!=dt&& dt.Rows.Count>0)
            {
                string slbh = dt.Rows[0]["SLBH"].ToString();
                string tableName = dt.Rows[0]["TABLENAME"].ToString();
                if (state > 0)
                    return UpdateDYCS(slbh, tableName);
                else
                    return 0;
            }
            return -1;
        }


        private DJ_FJD CreateFJD(string ywh,string fjr,int state)
        {
            DJ_FJD fjd = new DJ_FJD();
            fjd.DYZT = GetDYZT(state);
            fjd.FJR = fjr;
            fjd.FJRQ = DateTime.Now;
            fjd.SLBH = ywh;
            return fjd;
        }

        private DJ_FJD_FB CreateFJD_FB(string ywh,string bdczh,string zsxlh)
        {
           
            DJ_FJD_FB fb = new DJ_FJD_FB();
            fb.FJQDID = Guid.NewGuid().ToString();
            fb.SLBH = ywh;
            fb.QDXH = GetQDXH(ywh); 
            fb.QDWJM= GetQLMC(bdczh);
            fb.QDBZ = string.Format("不动产证号：{0} 证书序列号：{1} ", bdczh, zsxlh);
            return fb;
        }


        private DJ_FJD_LJR CreateFJD_LJR(string ywh,string ljrmc,string zjlx,string zjhm,byte[] ljrzp)
        {
            DJ_FJD_LJR ljr = new DJ_FJD_LJR();
            ljr.LJRZP = ljrzp;
            ljr.LJBH = Guid.NewGuid().ToString();
            ljr.SLBH = ywh;
            ljr.LJR = ljrmc;
            ljr.LJRSFZJLX = zjlx;
            ljr.LJRSFZJH = zjhm;
            return ljr;
        }

        private int UpdateDYCS(string slbh,string tableName)
        {
            string updateSql = string.Format("update {0} set DYCS=nvl(DYCS,0)+1 where slbh='{1}'", tableName,slbh);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            int count = dbHelper.ExecuteNonQuery(MyDBType.Oracle, CommandType.Text, updateSql, null);

            return count > 0 ? 0 : -1;
        }

        private int UpdateFJD(DJ_FJD fjd,DJ_FJD_FB fb,DJ_FJD_LJR ljr,string tableName,string zsxlh)
        {

            try
            {
                //using (TransactionScope ts = new TransactionScope())
                //{

                    InsertFJD(fjd);
                    InsertFJD_FB(fb);
                    InsertFJD_LZR(ljr);
                    UpdateWFM_ACTINST(fjd.SLBH);
                    UpdateZSXLH(fjd.SLBH, tableName, zsxlh);
                    //ts.Complete();
                    return 0;
                //}
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                WriteLog("C:\\LOG.txt", ex.Message);
                return -1;
            }
        }

        private void UpdateZSXLH(string sLBH, string tableName, string zsxlh)
        {
            string sql = "update {0} set zsxlh='{1}' where slbh='{2}'";
            sql = string.Format(sql, tableName, zsxlh, sLBH);
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(MyDBType.Oracle, CommandType.Text, sql, null);
        }

        private void UpdateWFM_ACTINST(string sLBH)
        {
            string sql = "update WFM_ACTINST set stepstate='已完成',COMPLETETIME=sysdate where prjid='{0}' and STEPNAME like '%发证%'";
            sql = string.Format(sql, sLBH);
            DbHelper dbHelper = new DbHelper();
            dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
            dbHelper.SetProvider(MyDBType.Oracle);
            dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
        }

        private int InsertFJD_FB(DJ_FJD_FB fb)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                //DelteFB(fb);
                
                string sql = dbHelper.CreateInsertStr<DJ_FJD_FB>(fb, "DJ_FJD_FB", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<DJ_FJD_FB>(fb, MyDBType.Oracle);
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }
        }

        private void DelteFB(DJ_FJD_FB fb)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                string sql = string.Format("delete from DJ_FJD_FB where slbh='{0}'", fb.SLBH);
                
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }
        }

        private int InsertFJD_LZR(DJ_FJD_LJR ljr)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                DeleteLJR(ljr);
                
                string sql = dbHelper.CreateInsertStr<DJ_FJD_LJR>(ljr, "DJ_FJD_LJR", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<DJ_FJD_LJR>(ljr, MyDBType.Oracle);
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                WriteLog("C:\\LOG.txt", ex.Message);
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }
        }

        private static void WriteLog(string path, string logText)
        {

            //string catalogName = itemCatalog.FullName;
            FileStream flagStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(flagStream);
            sw.BaseStream.Seek(0L, SeekOrigin.End);
            sw.WriteLine(logText);
            sw.Flush();
            sw.Close();
        }

        private void DeleteLJR(DJ_FJD_LJR ljr)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                string sql = string.Format("delete from DJ_FJD_LJR where slbh='{0}'", ljr.SLBH);
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }
        }

        private int InsertFJD(DJ_FJD fjd)
        {
            DbHelper dbHelper = new DbHelper();
            try
            { 
           
                DeleteFJD(fjd);

                string sql = dbHelper.CreateInsertStr<DJ_FJD>(fjd, "DJ_FJD", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = dbHelper.GetParamArray<DJ_FJD>(fjd, MyDBType.Oracle);
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }
        }

        private int DeleteFJD(DJ_FJD fjd)
        {
            DbHelper dbHelper = new DbHelper();
            try
            {
                string sql = string.Format("delete from dj_fjd where slbh='{0}'", fjd.SLBH);
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbHelper.CloseConn();
            }

        }

        private string GetDYZT(int state)
        {
            switch (state)
            {
                case -1:
                   return "未打印";
                    
                case 1:
                    return "已打印";
                    
                default:
                    return "打印中";
            }
        }

        private DataTable GetSLBH(string yWH,string qLRMC, string qLRZJH, string bDCQZH)
        {
            string sql = @"select a.slbh,'dj_djb' as tableName from dj_djb a 
left join dj_qlrgl b on b.slbh = a.slbh 
left join dj_qlr c on c.qlrid=b.qlrid 
where 1=1 {0}
union all
select a.slbh,'dj_yg' as tableName from dj_yg a
left
                      join dj_qlrgl b on b.slbh = a.slbh 
left join dj_qlr c on c.qlrid=b.qlrid 
where 1=1 {1}
union all
select a.slbh,'dj_dy' as tableName from dj_dy a
left
                      join dj_qlrgl b on b.slbh = a.slbh 
left join dj_qlr c on c.qlrid=b.qlrid 
where 1=1 {1}
union all
select a.slbh ,'dj_yy' as tableName from dj_yy a
left
                      join dj_qlrgl b on b.slbh = a.slbh 
left join dj_qlr c on c.qlrid=b.qlrid 
where 1=1 {1}";


            string where = "";
            string where1 = "";
            if (!string.IsNullOrEmpty(yWH))
            {
                where1 = where += string.Format(" and a.slbh = '{0}'", yWH);
                //where1 += string.Format(" and a.slbh = '{0}'", yWH);
            }
            if(!string.IsNullOrEmpty(qLRMC))
            {
                where1 = where += string.Format(" and b.qlrmc='{0}'", qLRMC);
            }
            if (!string.IsNullOrEmpty(qLRZJH))
            {
                where1 = where += string.Format(" and c.zjhm = '{0}'", qLRZJH);
                //where1 += string.Format(" and a.zjhm = '{0}'", qLRZJH);
            }
            if (!string.IsNullOrEmpty(bDCQZH))
            {
                where += string.Format(" and a.bdczh = '{0}'", bDCQZH);
                where1 += string.Format(" and a.bdczmh = '{0}'", bDCQZH);
            }

            sql = string.Format(sql, where, where1);

            if (IsHasSQLInject(sql))
            {
                return null;
            }
            DbHelper dbHelper = new DbHelper();
            dbHelper.SetProvider(MyDBType.Oracle);
            return dbHelper.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        private bool IsHasSQLInject(string str)
        {
            bool isHasSQLInject = false;

            //字符串中的关键字更具需要添加
            //string inj_str = "'|and|exec|union|create|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|xp_|or|--|+";
            string inj_str = "'exec|create|insert|delete|update|count|chr|mid|master|truncate|declare|xp_|--|+";
            str = str.ToLower().Trim();
            string[] inj_str_array = inj_str.Split('|');
            foreach (string sql in inj_str_array)
            {
                if (str.IndexOf(sql) > -1)
                {
                    isHasSQLInject = true;
                    break;
                }
            }
            return isHasSQLInject;
        }
    }
}