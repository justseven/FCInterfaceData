using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Web4BDC.Dal
{
    public class BDCInfo4WWWDal
    {
        public DataTable GetBDCStepInfo()
        {
            string sql = @"select p1.slbh,p1.cxmm, p1.Tzrxm, s1.stepname
  from DJ_SJD p1
  join 
 (select s.t1,
         s.stepname,
         s.prjid,
         prjname from (select t2.t1, t2.stepname, prjid
                    from (select row_number() over(partition by t.prjid order by t.accepttime desc) t1,
                                 t.stepname,t.prjid
                            from jwbdcggk.WFM_ACTINST t join jwbdcggk.wfm_procinst on  t.prjid=wfm_procinst.prjid where wfm_procinst.prjstate='处理中') t2
                   where t2.t1 = 1) s join jwbdcggk.wfm_procinst on s.prjid = jwbdcggk.wfm_procinst.prjid) s1
    on p1.slbh = s1.prjid 
    
    where p1.slbh not in (select slbh from dj_cf) and s1.prjname not like '%补录%' and s1.stepname not in('归档') 
     ";
            return DBHelper.GetDataTable(sql);
        }

        public DataTable GetBDCStepInfoT()
        {
            string sql = @"select a.slbh 受理编号,
       b.bdczh 不动产权证号,
       a.tzrxm 通知人,
      
       a.zl 坐落,
       a.lcmc 申请类型,
       a.sjsj 接件日期,
       case
         when b.dycs > 0 then
          '已经办结'
         else
          '办理中'
       end 项目状态,
       '320305' 行政区代码,
       '贾汪' 区域
  from dj_sjd a
 inner join (select slbh,bdczh,dycs from dj_djb
             union select slbh,bdczmh bdczh,dycs from dj_dy
             union select slbh,bdczmh bdczh,dycs from dj_yg
             union select slbh,bdczmh bdczh,dycs from dj_yy) b
    on a.slbh = b.slbh
where a.slbh like '20%' 

     ";
            return DBHelper.GetDataTable(sql);
        }


        public string GetPIDBySLBH(string slbh ) {
            string sql = string.Format(@"select PID from WFM_ACTINST t left join WFM_ACTIVITY on MDLID=AID
where prjid='{0}' and rownum=1", slbh);
            string connectStr = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
            DataTable dt= DBHelper.GetDataTable(connectStr,sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else {
                return string.Empty;
            }
        }
    }
}