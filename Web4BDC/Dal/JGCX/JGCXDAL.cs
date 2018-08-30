using Config;
using System;
using System.Collections.Generic;
using System.Data;
using Web4BDC.Models.BDCModel;
using Web4BDC.Models.JGCXModel;
using Web4BDC.Tools;

namespace Web4BDC.Dal.JGCX
{
    public class JGCXDAL
    {
        public static object lockKey = new object();

        internal string GetCXLX(string slbh)
        {
            string sql = @"select cxlx from dj_jgcx where jgbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                    {
                        return o.ToString();
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal string GetCXRQ(string cxbh)
        {
            string sql = @"SELECT distinct cxrq from dj_jgcx where jgbh='{0}'";
            sql = string.Format(sql, cxbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o= dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if(null!=o)
                    {
                        return o.ToString();
                    }
                    return "";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal DataTable GetCXSQR(string slbh)
        {
            string sql = @"SELECT N'' AS XZ,B.SXH,B.QLRMC,bdcdj.f_ParseDic('证件类型',A.ZJLB) AS ZJLX,A.ZJHM,N'' AS GYFE,A.DH,B.QLRID,B.GX FROM DJ_QLR_CX A INNER JOIN DJ_QLRGL_CX B ON A.QLRID = B.QLRID WHERE B.QLRLX IN ('权利人') AND B.SLBH = '{0}' ORDER BY B.SXH";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    return  dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal DataTable getBDCDYH_slbh(string slbh)
        {
            string sql = @"select nvl(tsgl.bdcdyh,nvl(h.bdcdyh,zd.bdcdyh)) as bdcdyh,tsgl.bdclx as bdclx,tsgl.tstybm as tstybm from dj_tsgl tsgl
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join zd_qsdc zd on zd.tstybm=tsgl.tstybm
 where tsgl.slbh='{0}' ";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal DataTable GetTstybm(string slbh)
        {
            string sql = "select tstybm from dj_tsgl where slbh='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal ZRZYXX GetZRZYXX(string slbh,string tstybm)
        {
            string sql = @"select bdczh,bdcdyh,zl,jzmj,jzmj,ftjzmj as ftmj,gytdmj as zdmj,tdsymj as fttdmj,tdsyqx,tdyt as tdghyt,fwyt as fwghyt,fwxz,fwjg,szc,zcs,djrq as djsj  from (

SELECT nvl(h.zl,zd.tdzl) as zl,DJB.BDCZH,DJB.DJRQ,h.bdcdyh,tdxg.gytdmj,  tdxg.fttdmj as tdsymj,bdcdj.f_ParseDic('土地权利类型',tdxg.qllx) as tdqllx,bdcdj.f_ParseDic('土地权利性质',tdxg.qlxz) as tdqlxz, bdcdj.f_parsetdyt(tdxg.tdyt) as tdyt,tdxg.syqx as tdsyqx,

bdcdj.f_ParseFWYT(h.ghyt) as fwyt,bdcdj.f_ParseDic('房屋权利性质',fwxg.qlxz) as fwxz,'' as fwjg,nvl(h.myc,h.sjc) as szc,z.zcs as zcs,nvl2(fwxg.jzmj,h.jzmj,h.ycjzmj) as jzmj,nvl2(fwxg.tnjzmj,h.tnjzmj,h.yctnjzmj) as tnmj,nvl2(fwxg.ftjzmj,h.ftjzmj,h.ycftjzmj) as ftjzmj
FROM DJ_DJB DJB
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
left join zd_qsdc zd on zd.tstybm=tsgl.tstybm
left join ql_fwxg fwxg on fwxg.slbh=djb.slbh
left join ql_tdxg tdxg on tdxg.slbh=djb.slbh
where (djb.lifecycle is null or djb.lifecycle='0')  and djb.slbh='{0}' and tsgl.tstybm='{1}'
union all
SELECT nvl(h.zl,zd.tdzl) as zl,DJB.BDCZMH as bdczh,DJB.DJRQ,h.bdcdyh,tdxg.gytdmj,  tdxg.fttdmj as tdsymj,bdcdj.f_ParseDic('土地权利类型',tdxg.qllx) as tdqllx,bdcdj.f_ParseDic('土地权利性质',tdxg.qlxz) as tdqlxz, bdcdj.f_parsetdyt(tdxg.tdyt) as tdyt,tdxg.syqx as tdsyqx,

bdcdj.f_ParseFWYT(h.ghyt) as fwyt,bdcdj.f_ParseDic('房屋权利性质',fwxg.qlxz) as fwxz,'' as fwjg,nvl(h.myc,h.sjc) as szc,z.zcs as zcs,nvl2(fwxg.jzmj,h.jzmj,h.ycjzmj) as jzmj,nvl2(fwxg.tnjzmj,h.tnjzmj,h.yctnjzmj) as tnmj,nvl2(fwxg.ftjzmj,h.ftjzmj,h.ycftjzmj) as ftjzmj
FROM dj_yg DJB
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
left join zd_qsdc zd on zd.tstybm=tsgl.tstybm
left join ql_fwxg fwxg on fwxg.slbh=djb.slbh
left join ql_tdxg tdxg on tdxg.slbh=djb.slbh
where (djb.lifecycle is null or djb.lifecycle='0')  and djb.slbh='{0}' and tsgl.tstybm='{1}'
) a
group by BDCZH,bdcdyh,DJRQ,gytdmj,tdsymj,tdqllx,tdqlxz,tdyt,tdsyqx,fwyt,fwxz,fwjg,szc,zcs,jzmj,tnmj,ftjzmj,zl";

            sql = string.Format(sql, slbh,tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<ZRZYXX> list = ModelHelper<ZRZYXX>.FillModel(dt);
                    if (null != list && list.Count > 0)
                        return list[0];
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal int GetDJCount(string tstybm, string v)
        {
            string sql = "select count(1) from dj_tsgl  t where tstybm='{0}' and djzl='{1}' and (t.lifecycle is null or t.lifecycle=0)";
            sql = string.Format(sql, tstybm, v);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    object o = dbHelper.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != o)
                        return Convert.ToInt32(o);
                    return 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal List<DJDY> getQS_DYXX_tstybm(string tstybm)
        {
            string sql = @"select distinct qlr.qlrmc AS DYQR, NVL(TSGL.BDCDYH,H.BDCDYH) as bdcdyh,tsgl.bdclx AS BDCLX,dy.slbh,dy.djlx,dy.bdczmh,bdcdj.f_ParseDic('抵押方式',dy.dyfs) as dyfs,dy.dymj AS DYFW,dy.bdbzzqse as dyje,dy.qljssj as dyqx,dy.djrq as dyrq from dj_dy dy 
LEFT JOIN DJ_QLRGL GL ON GL.SLBH=DY.SLBH
LEFT JOIN DJ_QLR QLR ON QLR.QLRID=GL.QLRID
LEFT JOIN DJ_TSGL TSGL ON TSGL.SLBH=DY.SLBH
LEFT JOIN FC_H_QSDC H ON H.TSTYBM=TSGL.TSTYBM
left join dj_xgdjgl djgl on djgl.zslbh=dy.slbh
where (dy.lifecycle is null or dy.lifecycle='0') and gl.qlrlx='抵押权人' and (tsgl.tstybm = '{0}' or h.tstybm='{0}') order by dyrq";

            sql = string.Format(sql, tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJDY> list = ModelHelper<DJDY>.FillModel(dt);
                    if (null != list && list.Count > 0)
                        return list;
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        public QSXX GetQSXX(string slbh,string tstybm)
        {
            string sql = @"select DJLX,DAH,BDCZH,DJRQ,'现实' as qszt,wm_concat(to_char(syqr)) as syqr,wm_concat(to_char(zjhm)) as zjhm,wm_concat(to_char(zjlx)) as zjlx,BDCDYH,gyqk,tdsymj,tdsyqr,tdqllx,tdqlxz,tdyt,tdsyqx,fwyt,fwxz,fwjg,szc,zcs,jzmj,tnmj,ftjzmj,jgrq,zl from (

SELECT DJB.DJLX,'' AS DAH,DJB.BDCZH,DJB.DJRQ,djb.lifecycle as qszt,qlr.qlrmc as syqr,qlr.zjhm as zjhm,bdcdj.f_ParseDic('证件类型',qlr.zjlb) as zjlx,h.bdcdyh,bdcdj.f_ParseDic('共有方式',djb.gyfs) as gyqk,
nvl(tdxg.fttdmj,tdxg.dytdmj) as tdsymj,tdxg.tdsyqr as tdsyqr,bdcdj.f_ParseDic('土地权利类型',tdxg.qllx) as tdqllx,bdcdj.f_ParseDic('土地权利性质',tdxg.qlxz) as tdqlxz, bdcdj.f_parsetdyt(tdxg.tdyt) as tdyt,tdxg.syqx as tdsyqx,
fwxg.ghytms as fwyt,bdcdj.f_ParseDic('房屋权利性质',fwxg.qlxz) as fwxz,'' as fwjg,nvl(h.myc,h.sjc) as szc,z.zcs as zcs,nvl2(fwxg.jzmj,h.jzmj,h.ycjzmj) as jzmj,nvl2(fwxg.tnjzmj,h.tnjzmj,h.yctnjzmj) as tnmj,nvl2(fwxg.ftjzmj,h.ftjzmj,h.ycftjzmj) as ftjzmj,
z.jgrq as jgrq,nvl(h.zl,zd.tdzl) as zl 
FROM DJ_DJB DJB
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
left join zd_qsdc zd on zd.tstybm=tsgl.tstybm
left join ql_fwxg fwxg on fwxg.slbh=djb.slbh
left join ql_tdxg tdxg on tdxg.slbh=djb.slbh
where (djb.lifecycle is null or djb.lifecycle='0') and qlrgl.qlrlx in('权利人','抵押人') and djb.slbh='{0}' and tsgl.tstybm='{1}'
union all
SELECT DJB.DJLX,'' AS DAH,DJB.BDCZMH as bdczh,DJB.DJRQ,djb.lifecycle as qszt,qlr.qlrmc as syqr,qlr.zjhm as zjhm,bdcdj.f_ParseDic('证件类型',qlr.zjlb) as zjlx,h.bdcdyh,bdcdj.f_ParseDic('共有方式',djb.gyfs) as gyqk,
nvl(tdxg.fttdmj,tdxg.dytdmj) as tdsymj,tdxg.tdsyqr as tdsyqr,bdcdj.f_ParseDic('土地权利类型',tdxg.qllx) as tdqllx,bdcdj.f_ParseDic('土地权利性质',tdxg.qlxz) as tdqlxz,bdcdj.f_parsetdyt(tdxg.tdyt) as tdyt,tdxg.syqx as tdsyqx,
fwxg.ghytms as fwyt,bdcdj.f_ParseDic('房屋权利性质',fwxg.qlxz) as fwxz,'' as fwjg,nvl(h.myc,h.sjc) as szc,z.zcs as zcs,nvl2(fwxg.jzmj,h.jzmj,h.ycjzmj) as jzmj,nvl2(fwxg.tnjzmj,h.tnjzmj,h.yctnjzmj) as tnmj,nvl2(fwxg.ftjzmj,h.ftjzmj,h.ycftjzmj) as ftjzmj,
z.jgrq as jgrq,nvl(h.zl,zd.tdzl) as zl 
FROM dj_yg DJB
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join fc_z_qsdc z on z.tstybm=h.lsztybm
left join zd_qsdc zd on zd.tstybm=tsgl.tstybm
left join ql_fwxg fwxg on fwxg.slbh=djb.slbh
left join ql_tdxg tdxg on tdxg.slbh=djb.slbh
where (djb.lifecycle is null or djb.lifecycle='0') and qlrgl.qlrlx in('权利人','抵押人') and djb.slbh='{0}' and tsgl.tstybm='{1}'
) a
group by DJLX,DAH,BDCZH,bdcdyh,DJRQ,qszt,gyqk,tdsymj,tdsyqr,tdqllx,tdqlxz,tdyt,tdsyqx,fwyt,fwxz,fwjg,szc,zcs,jzmj,tnmj,ftjzmj,jgrq,zl";

            sql = string.Format(sql, slbh,tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<QSXX> list = ModelHelper<QSXX>.FillModel(dt);
                    if (null != list && list.Count > 0)
                        return list[0];
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal List<CFXX> GetCFXXList(string qsSlbh)
        {
            List<CFXX> list = new List<CFXX>();
            
            List<DJCF> cfList = this.GetDJCF_By_QSSlbh(qsSlbh);
            
            if(null!=cfList && cfList.Count>0)
            {
                foreach (DJCF cf in cfList)
                {
                    CFXX cfxx = new CFXX();
                    cfxx.CF = cf;
                    cfxx.JF = new DJJF();
                    list.Add(cfxx);
                }
            }
            return list;
        }

        private List<DJCF> GetDJCF_By_QSSlbh(string qsSlbh)
        {
            string sql = @"select SLBH,CFJG,CFWH,CFLX,CFFW,CFWJ,CFQX,CFSX,DJSJ AS CFRQ,CFQSSJ,CFJSSJ from dj_cf cf where slbh in
(
select slbh from dj_tsgl where tstybm in 
(select tstybm from dj_tsgl where slbh='{0}') and djzl='查封'
) and (cf.lifecycle is null or cf.lifecycle='0') order by cfsx
";
            sql = string.Format(sql, qsSlbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJCF> list = ModelHelper<DJCF>.FillModel(dt);
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal List<DJCF> GetJFXX(string bdcdyh)
        {
            string sql = @"select CFJG,CFWH,CFLX,CFFW,CFWJ,t.cfjssj as CFQX,t.djsj as CFRQ  from dj_cf t where (t.lifecycle is null or t.lifecycle='0') and bdcdyh='{0}' and t.cflx='解封'";

            sql = string.Format(sql, bdcdyh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJCF> list = ModelHelper<DJCF>.FillModel(dt);
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        internal string GetDYR_Slbh(string sLBH)
        {
            string sql = @"select slbh,wm_concat(to_char(qlrmc)) AS QLRMC from (
select gl.slbh,qlr.qlrmc from dj_qlr qlr
left join dj_qlrgl gl on gl.qlrid=qlr.qlrid
where gl.slbh='{0}' AND GL.QLRLX='抵押人') a
group by slbh";
            sql = string.Format(sql, sLBH);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["QLRMC"].ToString();
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }

        }

        internal List<DJDY> GetQS_DYXX_tstybm(string tstybm)
        {
            string sql = @"select distinct qlr.qlrmc AS DYQR, NVL(TSGL.BDCDYH,H.BDCDYH) as bdcdyh,tsgl.bdclx AS BDCLX,dy.slbh,dy.djlx,dy.bdczmh,bdcdj.f_ParseDic('抵押方式',dy.dyfs) as dyfs,dy.dymj AS DYFW,dy.bdbzzqse as dyje,dy.qljssj as dyqx,dy.djrq as dyrq from dj_dy dy 
LEFT JOIN DJ_QLRGL GL ON GL.SLBH=DY.SLBH
LEFT JOIN DJ_QLR QLR ON QLR.QLRID=GL.QLRID
LEFT JOIN DJ_TSGL TSGL ON TSGL.SLBH=DY.SLBH
LEFT JOIN FC_H_QSDC H ON H.TSTYBM=TSGL.TSTYBM
left join dj_xgdjgl djgl on djgl.zslbh=dy.slbh
where (dy.lifecycle is null or dy.lifecycle='0') and gl.qlrlx='抵押权人' and (tsgl.tstybm = '{0}' or h.tstybm='{0}')";
            sql = string.Format(sql, tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJDY> list = ModelHelper<DJDY>.FillModel(dt);
                    if (null != list && list.Count > 0)
                        return list;
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        public DataTable GetSLbhFrmCX(string cxbh)
        {
            string sql = @"SELECT N'' AS XZ,ROWNUM AS RN,A.* FROM ( SELECT DISTINCT B.SLBH,N'' AS QLRMC,N'' AS QLRZJH,B.BDCZH,C.ZL,N'' AS BDCDYH,N'',A.DJZT,A.DYZT,N'否' AS DY FROM DJ_JGCX_FB A INNER JOIN DJ_DJB B ON A.SLBH = B.SLBH LEFT JOIN DJ_SJD C ON B.SLBH = C.SLBH WHERE A.DYZT = 1 AND A.JGBH = '{0}'  UNION ALL  SELECT DISTINCT B.SLBH,N'' AS QLRMC,N'' AS QLRZJH,B.BDCZMH AS BDCZH,C.ZL,N'' AS BDCDYH,N'',A.DJZT,A.DYZT,N'否' AS DY FROM DJ_JGCX_FB A INNER JOIN DJ_YG B ON A.SLBH = B.SLBH LEFT JOIN DJ_SJD C ON B.SLBH = C.SLBH WHERE A.DYZT = 1 AND A.JGBH = '{0}'  UNION ALL  SELECT DISTINCT B.SLBH,B.QLR AS QLRMC,B.ZJBH AS QLRZJH,B.CFBH AS BDCZH,C.ZL,N'' AS BDCDYH,N'',A.DJZT,A.DYZT,N'是' AS DY FROM DJ_JGCX_FB A INNER JOIN DJ_CF B ON A.SLBH = B.SLBH LEFT JOIN DJ_SJD C ON B.SLBH = C.SLBH WHERE A.DYZT = 1 AND A.JGBH = '{0}'  UNION ALL  SELECT DISTINCT B.SLBH,N'' AS QLRMC,N'' AS QLRZJH,B.BDCZMH AS BDCZH,C.ZL,N'' AS BDCDYH,N'',A.DJZT,A.DYZT,N'是' AS DY FROM DJ_JGCX_FB A INNER JOIN DJ_DY B ON A.SLBH = B.SLBH LEFT JOIN DJ_SJD C ON B.SLBH = C.SLBH WHERE A.DYZT = 1 AND A.JGBH = '{0}'  )A";
            sql = string.Format(sql, cxbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                   if(null!=dt && dt.Rows.Count>0)
                    {
                        return dt;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }

        }


        public List<DJDY> GetQS_DYXX(string bdcdyh)
        {
            string sql = @"select distinct qlr.qlrmc AS DYQR, NVL(TSGL.BDCDYH,H.BDCDYH) as bdcdyh,tsgl.bdclx AS BDCLX,dy.slbh,dy.djlx,dy.bdczmh,bdcdj.f_ParseDic('抵押方式',dy.dyfs) as dyfs,dy.dymj AS DYFW,dy.bdbzzqse as dyje,dy.qljssj as dyqx,dy.djrq as dyrq from dj_dy dy 
LEFT JOIN DJ_QLRGL GL ON GL.SLBH=DY.SLBH
LEFT JOIN DJ_QLR QLR ON QLR.QLRID=GL.QLRID
LEFT JOIN DJ_TSGL TSGL ON TSGL.SLBH=DY.SLBH
LEFT JOIN FC_H_QSDC H ON H.TSTYBM=TSGL.TSTYBM
left join dj_xgdjgl djgl on djgl.zslbh=dy.slbh
where (dy.lifecycle is null or dy.lifecycle='0') and gl.qlrlx='抵押权人' and (tsgl.bdcdyh = '{0}' or h.bdcdyh='{0}')";
           
            sql = string.Format(sql, bdcdyh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJDY> list = ModelHelper<DJDY>.FillModel(dt);
                    if(null!=list && list.Count>0)
                        return list;
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        public List<DJ_QLR> GetQLR(string slbh,string qlrlx)
        {
            string sql = @"
select qlr.* from dj_qlr qlr
left join dj_qlrgl gl on gl.qlrid=qlr.qlrid
where gl.qlrlx='{1}' and gl.slbh='{0}'";
            sql = string.Format(sql, slbh,qlrlx);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJ_QLR> list = ModelHelper<DJ_QLR>.FillModel(dt);
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }

        private string FormatZH(List<string> xgzhList)
        {
            string xgzh = string.Empty;
            if(null!= xgzhList && xgzhList.Count>0)
            {
                foreach (string zh in xgzhList)
                {
                    if(string.IsNullOrEmpty(xgzh))
                    {
                        xgzh = "'" + zh + "'";
                    }
                    else
                    {
                        xgzh+=",'" + zh + "'";
                    }
                }
            }
            return xgzh;
        }

        public List<DJCF> GetQS_CFXX(string tstybm)
        {
            string sql = @"select CFJG, CFWH, CFLX, CFFW, CFWJ,CFSX, t.cfqx as CFQX,t.CFQSSJ as CFQSSJ,t.CFJSSJ as CFJSSJ, t.djsj as CFRQ
  from dj_cf t
  left join dj_tsgl tsgl on tsgl.slbh=t.slbh
 where (t.lifecycle is null or t.lifecycle = '0')
   and tsgl.tstybm='{0}' and t.cflx <> '解封' order by CFSX";
            
            sql = string.Format(sql, tstybm);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);

                try
                {
                    dbHelper.SetProvider(MyDBType.Oracle);
                    DataTable dt = dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
                    List<DJCF> list = ModelHelper<DJCF>.FillModel(dt);
                    if (null != list && list.Count > 0)
                        return list;
                    else
                    {
                        list = new List<DJCF>();
                        list.Add(new DJCF());
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { if (null != dbHelper.Conn) { dbHelper.Conn.Close(); } }
            }
        }
    }
}