using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace DBCForFCWebService
{
    public class FC_DY_INFO_DAL
    { 
        public DataTable GetDYOpList(string strKeyWord, string sDate, string eDate,string strTXCertCode)
        {
            string sql = @"select dy.slbh 抵押业务宗号,dy.djyy 他项权利种类,h.qm 区名,z.fwzl 坐落,h.zl 坐落细则,dy.bdbzzqse 权利价值,dy.qlqssj 抵押起始日期,dy.qljssj 抵押结束日期,dy.bdczmh 他项权证号,dy.fj 他项权证附记,DECODE(dy.lifecycle,1,'已注销','现势') 注销状态  from dj_dy dy
left join dj_tsgl tsgl on dy.slbh = tsgl.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left join fc_z_qsdc z on z.tstybm = h.lsztybm
where dy.qlqssj between to_date('{1}', 'yyyy/mm/dd hh24:mi:ss') and to_date('{2}','yyyy/mm/dd hh24:mi:ss') 
and dy.slbh in (select qlrgl.slbh from dj_qlrgl qlrgl
 left join dj_qlr qlr on qlr.qlrid = qlrgl.qlrid
 where qlr.qlrmc like '%{0}%' and qlrgl.qlrlx='抵押权人') and tsgl.bdclx='房屋' {3} 
  group by dy.slbh,dy.djyy,h.qm,z.xmmc,z.fwzl,h.zl,dy.bdbzzqse,dy.qlqssj,dy.qljssj,dy.bdczmh,dy.fj,dy.lifecycle";
            string where1 = "";
            if(!string.IsNullOrEmpty(strTXCertCode))
            {
                where1 = string.Format("and dy.bdczmh='{0}'", strTXCertCode);
            }
            sql = string.Format(sql, strKeyWord, sDate, eDate, where1);

            try
            {
                if (IsHasSQLInject(sql))
                {
                    return null;
                }
                return OleDBHelper.GetDataTable(sql);
            }
            catch(Exception ex)
            {
                string str = ex.Message;
                return null;
            }
        }

        public DataTable GetDYOpList(string strKeyWord, string sDate, string eDate, string strTXCertCode, string fzSDate, string fzEDate)
        {
            string sql = @"select dy.slbh 抵押业务宗号,dy.djyy 他项权利种类,h.qm 区名,z.fwzl 坐落,h.zl 坐落细则,dy.bdbzzqse 权利价值,dy.qlqssj 抵押起始日期,dy.qljssj 抵押结束日期,dy.bdczmh 他项权证号,dy.fj 他项权证附记,dy.djrq as 登簿日期,DECODE(dy.lifecycle,1,'已注销','现势') 注销状态  from dj_dy dy
left join dj_tsgl tsgl on dy.slbh = tsgl.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left join fc_z_qsdc z on z.tstybm = h.lsztybm
where 1=1 {1}
and dy.slbh in (select qlrgl.slbh from dj_qlrgl qlrgl
 left join dj_qlr qlr on qlr.qlrid = qlrgl.qlrid
 where qlr.qlrmc like '%{0}%' and qlrgl.qlrlx='抵押权人') and tsgl.bdclx='房屋' {2} 
  group by dy.slbh,dy.djyy,h.qm,z.xmmc,z.fwzl,h.zl,dy.bdbzzqse,dy.qlqssj,dy.qljssj,dy.bdczmh,dy.fj,dy.lifecycle,dy.djrq";
            string where1 = "";
            string where = "";
            if(!string.IsNullOrEmpty(sDate) && !string.IsNullOrEmpty(eDate))
            {
                where+=string.Format(" and dy.qlqssj between to_date('{0}', 'yyyy/mm/dd hh24:mi:ss') and to_date('{1}','yyyy/mm/dd hh24:mi:ss') ",sDate,eDate);
            }
            if(!string.IsNullOrEmpty(fzSDate) && !string.IsNullOrEmpty(fzEDate))
            {
                where += string.Format(" and dy.djrq between to_date('{0}', 'yyyy/mm/dd hh24:mi:ss') and to_date('{1}','yyyy/mm/dd hh24:mi:ss') ", fzSDate, fzEDate);
            }
            if (!string.IsNullOrEmpty(strTXCertCode))
            {
                where1 = string.Format("and dy.bdczmh='{0}'", strTXCertCode);
            }
            sql = string.Format(sql, strKeyWord, where,where1);

            try
            {
                if (IsHasSQLInject(sql))
                {
                    return null;
                }
                return OleDBHelper.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        internal DataTable GetQLRByFSLBH(string slbh)
        {
            string sql = @"select distinct qlr.qlrmc from dj_qlr qlr
left join dj_qlrgl qlrgl on qlrgl.qlrid=qlr.qlrid
left join dj_xgdjgl djgl on djgl.fslbh=qlrgl.slbh
where djgl.zslbh='{0}' and qlrgl.qlrlx='权利人'";
            sql = string.Format(sql, slbh);
            return OleDBHelper.GetDataTable(sql);
        }

        internal string GetZXRQ(string slbh)
        {
            string sql = @"select zx.djrq from dj_xgdjzx zx
left join dj_xgdjgl djgl on zx.slbh = djgl.zslbh
left join dj_dy dy on dy.slbh=djgl.fslbh
where dy.slbh = '{0}'";

            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            object o= OleDBHelper.ExScalar(sql);
            if(null!=o)
            {
                return o.ToString();
            }
            return "";
        }

        internal DataTable GetQLRMCBySLBH(string slbh,string IsDYQR)
        {
            string qlrlx = IsDYQR;//? "抵押权人" : "抵押人";
            string sql = @"select qlr.qlrmc from dj_qlr qlr
left join dj_qlrgl qlrgl on qlrgl.qlrid = qlr.qlrid
where qlrgl.slbh = '{0}' and qlrgl.qlrlx = '{1}'";

            sql = string.Format(sql, slbh, qlrlx);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable getDYOpFlowFromBDC(string ywzh, string txqzh, string cqr, string zjhm, string fwzl, string yhmc)
        {
            string sql = @"select dy.slbh 业务宗号,dy.djyy 业务流程,qlr.qlrmc 产权人,qlr.zjhm 证件号码,h.zl 房屋坐落,DECODE(dy.lifecycle,1,'已注销','现势') 注销状态,dy.qlqssj as 抵押起始时间,dy.qljssj 抵押结束时间 from dj_dy dy
left join dj_tsgl tsgl on tsgl.slbh = dy.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left join dj_qlrgl qlrgl on qlrgl.slbh = dy.slbh
left join dj_qlr qlr on qlr.qlrid = qlrgl.qlrid
where qlrgl.qlrlx = '抵押人' and tsgl.bdclx='房屋'   ";
            if(!string.IsNullOrEmpty(ywzh))
            {
                sql += string.Format(" and dy.slbh = '{0}'", ywzh);
            }
            if(!string.IsNullOrEmpty(txqzh))
            {
                sql += string.Format(" and dy.bdczmh = '{0}'", txqzh);
            }
            if(!string.IsNullOrEmpty(cqr))
            {
                sql += string.Format(" and qlr.qlrmc = '{0}'", cqr);
            }
            if(!string.IsNullOrEmpty(zjhm))
            {
                sql += string.Format(" and qlr.zjhm = '{0}'", zjhm);
            }
            if(!string.IsNullOrEmpty(fwzl))
            {
                sql += string.Format(" and h.zl like '%{0}%'", fwzl);
            }
            if(!string.IsNullOrEmpty(yhmc))
            {
                sql += string.Format(@"and dy.slbh in (select qlrgl.slbh from dj_qlrgl qlrgl
 left join dj_qlr qlr on qlr.qlrid = qlrgl.qlrid
 where qlr.qlrmc like '%{0}%' and qlrgl.qlrlx = '抵押权人')", yhmc);
            }
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetQSInfoFromBDC(string slbh)
        {
            string sql = @"select djb.slbh 业务ID,djb.slbh 自然状况登记簿ID,nvl(nvl(h.oracle_wb_houseid,h.oracle_houseinfo_id),h.tstybm) 房屋ID,djb.slbh 业务宗号,2 as 状态,qm.qm 行政区,fwytlx.ytmc 产别,'' as 来源,h.zl 房屋坐落,
item.itemname 共有性质,djb.djlx 登记类型,djb.djyy 业务种类,djb.bdczh 所有权证号,djb.zsxlh 证书印刷号,djb.djrq 登簿时间 
 from dj_djb djb
left join dj_tsgl tsgl on tsgl.slbh = djb.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left join dic_fwytlx@xzbdcggk fwytlx on fwytlx.ytbm=h.Ghyt
LEFT JOIN dic_item@xzbdcggk item on item.itemval=djb.gyfs and item.diccode='150106151438XH6KL22QEP'
left join fc_qm qm on qm.id=h.qm
where djb.slbh = '{0}' and tsgl.bdclx='房屋' ";

            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetQLRZJHMBySLBH(string slbh, string isDyqr)
        {
            string sql = @"select qlr.zjhm from dj_djb djb
left join dj_qlrgl gl on gl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=gl.qlrid
where djb.slbh='{0}' and gl.qlrlx='{1}'";
            sql = string.Format(sql, slbh, isDyqr);

            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetDYFormBDC(string slbh)
        {
//            string sql = @"select  qlrgl.qlrmc 权利人,'房屋所有权' 权利种类,dj_dy.bdczmh 他项权证号,fc_h_qsdc.zh 幢号,fc_h_qsdc.dyh||'-'||fc_h_qsdc.fjh 房号,fc_h_qsdc.jzmj 建筑面积,
//dj_dy.qlqssj 设定日期,dj_dy.bdbzzqse 权利价值,dj_dy.pgje 评估值,dj_dy.dyqx 约定期限, dj_dy.djyy 他项种类,dj_xgdjzx.djrq 注销日
//from dj_dy left join dj_tsgl on dj_dy.slbh=dj_tsgl.slbh left join fc_h_qsdc on fc_h_qsdc.tstybm=dj_tsgl.tstybm 
//left join dj_xgdjzx on dj_xgdjzx.slbh=dj_xgdjgl.zslbh 
//left join dj_xgdjgl on dj_xgdjgl.zslbh=dj_dy.slbh 
//left join dj_djb djb on djb.slbh=dj_xgdjgl.fslbh
//left join dj_qlrgl qlrgl on qlrgl.slbh=dj_dy.slbh 
//left join ql_fwxg fwxg on fwxg.slbh=dj_dy.slbh
//Where dj_dy.slbh ='{0}' and  qlrgl.qlrlx='抵押权人' and dj_tsgl.bdclx='房屋' ";


            string sql = @"select A.*,dj_xgdjzx.Djrq 注销日期,'' as 注销原因 from (

select dj_dy.slbh, qlrgl.qlrmc 权利人,'房屋所有权' 权利种类,dj_dy.bdczmh 他项权证号,fc_h_qsdc.zh 幢号,fc_h_qsdc.dyh||'-'||fc_h_qsdc.fjh 房号,fc_h_qsdc.jzmj 建筑面积,
dj_dy.qlqssj 设定日期,dj_dy.bdbzzqse 权利价值,dj_dy.pgje 评估值,dj_dy.dyqx 约定期限, dj_dy.djyy 他项种类 from dj_dy
left join dj_xgdjgl gl on gl.zslbh=dj_dy.slbh
left join dj_djb djb on djb.slbh=gl.fslbh
left join dj_qlrgl qlrgl on qlrgl.slbh=dj_dy.slbh 
left join ql_fwxg fwxg on fwxg.slbh=dj_dy.slbh
left join dj_tsgl on dj_dy.slbh=dj_tsgl.slbh 
left join fc_h_qsdc on fc_h_qsdc.tstybm=dj_tsgl.tstybm 
Where djb.slbh ='{0}' and  qlrgl.qlrlx='抵押权人' and dj_tsgl.bdclx='房屋' ) A
left join dj_xgdjgl on dj_xgdjgl.fslbh=A.slbh
left join dj_xgdjzx on dj_xgdjzx.slbh=dj_xgdjgl.zslbh";

            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable getCFFromBDC(string hid)
        {
            string sql = @"SELECT DISTINCT ''被执行人,cfjg 执行人,cffw 执行项目,DJSJ 查封日期,NVL(CFQX,decode (ceil(TO_CHAR(CFJSSJ-CFQSSJ)/365),1,'1',2,'2',3,'3',null)) 查封期限,BGRQ 解封录入日期,''备注 ,CFYY  查封信息, DJ_XGDJZX.DJYY 解封信息
FROM DJ_CF left join dj_XGDJGL ON DJ_CF.SLBH=DJ_XGDJGL.FSLBH LEFT JOIN DJ_XGDJZX ON DJ_XGDJGL.ZSLBH=DJ_XGDJZX.SLBH 
left join dj_tsgl tsgl on tsgl.slbh=dj_cf.slbh left join fc_h_qsdc h on h.tstybm=tsgl.tstybm 
Where h.tstybm = '{0}' or h.oracle_houseinfo_id = '{0}' or h.oracle_wb_houseid = '{0}' and tsgl.bdclx='房屋' ";
            sql = string.Format(sql, hid);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetFZFromBDC(string hID)
        {
            string sql = @"select z.fwzl 项目坐落,z.xmmc 项目名称,z.jzwmc 楼盘名称,h.dyh 单元号,h.fjh 房间号,h.jzmj 建筑面积,fwjg.itemname 结构,ghyt.ytmc 用途,z.jcnf 建成年代,z.zcs 总层数,h.sjc 所在层数,h.bdcdyh as 不动产单元号,nvl(h.FTTDMJ,h.DYTDMJ) as 土地面积 ,'' as 土地取得方式 from fc_h_qsdc h
left join fc_z_qsdc z on h.lsztybm = z.tstybm
left join dic_item@xzbdcggk fwjg on fwjg.itemval=z.fwjg and fwjg.diccode='1505181711115COR1KAPMB'
left join DIC_FWYTLX@Xzbdcggk ghyt on ghyt.ytbm=h.ghyt
where h.tstybm = '{0}' or h.oracle_houseinfo_id = '{0}' or h.oracle_wb_houseid = '{0}'";
            sql = string.Format(sql, hID);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable getBDCDYH(string zid)
        {
            string sql = @"select tstybm as 不动产户ID，bdcdyh as 不动产单元号,zl as 坐落,ycjzmj as 预测建筑面积,yctnjzmj as 预测套内面积,jzmj as 实测建筑面积,tnjzmj as 实测套内面积,HSCID as 原实测ID,FJBM as 房间编码 from fc_h_qsdc
where lsztybm = '{0}' ";
            sql = string.Format(sql, zid);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetQLRFromBDC(string slbh)
        {
            string sql = @"select qlrgl.qlrlx 权利人类型,qlr.qlrmc 权利人姓名,qlr.zjlb 证件类型,qlr.zjhm 证件号码,qlr.dz 联系地址,qlr.dh 联系电话,qlrgl.gyfe 份额,qlr.frdbxm 法定代表人 from dj_qlr qlr
left join dj_qlrgl qlrgl on qlrgl.qlrid=qlr.qlrid
where qlrgl.slbh='{0}'";
            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable GetQSInfoListFromBDC(string bdczh, string qlrmc, string zjhm, string zl)
        {
            string sql = @"select distinct djb.slbh 业务ID,djb.slbh 自然状况登记簿ID,nvl(nvl(h.oracle_wb_houseid,h.oracle_houseinfo_id),h.tstybm) 房屋ID,djb.slbh 业务宗号,h.zl 房屋坐落,djb.djlx 登记类型,djb.djyy 业务种类,djb.bdczh 证号,
djb.djrq 登簿时间,2 as 状态
 from dj_djb djb
left join dj_tsgl tsgl on tsgl.slbh=djb.slbh
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm 
left join fc_h_qsdc h on h.tstybm=tsgl.tstybm
left join dj_qlrgl gl on gl.slbh=djb.slbh
left join dj_qlr qlr on gl.qlrid=qlr.qlrid 
where gl.qlrlx='权利人' and  tsgl.bdclx='房屋' and (djb.lifecycle=0 or djb.lifecycle is null) ";
            if(!string.IsNullOrEmpty(bdczh))
            {
                sql += string.Format(" and djb.bdczh='{0}'", bdczh);
            }
            if(!string.IsNullOrEmpty(qlrmc))
            {
                sql += string.Format(" and gl.qlrmc='{0}'", qlrmc);
            }
            if(!string.IsNullOrEmpty(zjhm))
            {
                sql += string.Format(" and qlr.zjhm='{0}'", zjhm);
            }
            if(!string.IsNullOrEmpty(zl))
            {
                sql += string.Format(" and h.zl like '%{0}%'", zl);
            }
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);

        }

        internal DataTable getREInfoFromBDC(string strHouseSite, string strCertCode)
        {
            string sql = @"select h.tstybm as HID,tsgl.bdcdyh,h.zl,h.jzmj,ghyt.ytmc as ghyt,fwjg.itemname as fwjg from dj_dy dy
left join dj_tsgl tsgl on tsgl.slbh = dy.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
left join fc_z_qsdc z on z.tstybm = h.lsztybm
left join dic_item @xzbdcggk fwjg on fwjg.itemval = z.fwjg and fwjg.diccode = '1505181711115COR1KAPMB'
left join DIC_FWYTLX @Xzbdcggk ghyt on ghyt.ytbm = h.ghyt
where 1=1 ";

            if(!string.IsNullOrEmpty(strHouseSite))
            {
                sql += string.Format(" and h.zl like '%{0}%'", strHouseSite);
            }
            if(!string.IsNullOrEmpty(strCertCode))
            {
                sql += string.Format("and dy.bdczmh = '{0}'", strCertCode);
            }

            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);

        }

        internal DataTable GetDYBDCDYH(string slbh)
        {
            string sql = @"select tsgl.bdcdyh from dj_dy dy
left join dj_tsgl tsgl on tsgl.slbh = dy.slbh
left join fc_h_qsdc h on tsgl.tstybm = h.tstybm
where dy.slbh = '{0}' ";
            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }

        internal DataTable getCertInfoFromBDC(string strBusNo, string stringstrCertCode)
        {
            string sql = @"select slbh 抵押业务宗号, bdczmh 不动产证明号,'苏'  不动产证明省份,'徐州市' 不动产证明市级,FZND 不动产证明年份,bdczmh 不动产证明序号,'抵押权' as 证明权利或事项,qt 其他,fj 附记,djrq 登簿日期,zsxlh 证书印刷号,DYLX AS LX,BDCDJ.f_ParseDic('预告登记种类',YGDJZL) AS YGDJZL from dj_dy 
where 1=1  ";

            if(!string.IsNullOrEmpty(strBusNo))
            {
                sql += string.Format(" and dj_dy.slbh = '{0}'", strBusNo);
            }
            if(!string.IsNullOrEmpty(stringstrCertCode))
            {
                sql += string.Format(" and bdczmh = '{0}'", stringstrCertCode);
            }
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return  OleDBHelper.GetDataTable(sql);
            
        }


        internal DataTable GetDYZL(string slbh)
        {
//            string sql = @"select h.zl from dj_dy dy
//left join dj_tsgl tsgl on tsgl.slbh = dy.slbh
//left join fc_h_qsdc h on tsgl.tstybm = h.tstybm
//where dy.slbh = '{0}' ";

            string sql = @"select sjd.zl from dj_sjd sjd where sjd.slbh='{0}'";
            sql = string.Format(sql, slbh);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }



        internal DataTable GetCFInfoFromBDC(string hoID)
        {
            string sql = @"select cf.cfjg 执行法院,cf.cfwh 协助执行书号,cf.cfqssj 查封日期,NVL(cf.CFQX,decode (ceil(TO_CHAR(cf.CFJSSJ-cf.CFQSSJ)/365),1,'1',2,'2',3,'3',null)) 查封期限,cf.cfyy 查封原因 from dj_cf cf 
left join  dj_tsgl tsgl on cf.slbh = tsgl.slbh
left join fc_h_qsdc h on h.tstybm = tsgl.tstybm
where h.tstybm = '{0}' or h.ORACLE_HOUSEINFO_ID = '{0}' or h.ORACLE_WB_HOUSEID = '{0}'  and tsgl.bdclx='房屋'";

            sql = string.Format(sql, hoID);
            if (IsHasSQLInject(sql))
            {
                return null;
            }
            return OleDBHelper.GetDataTable(sql);
        }
    }
}