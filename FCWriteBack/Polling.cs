using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWriteBack
{

    public class Polling
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Polling));
        /// <summary>
        /// 获取需要推送的数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetPushDataInfos()
        {
            string sql = "select SLBH,DJLX from FC_SPFHX_TAG where SFTS<>1";
            return OleDBHelper.GetDataTable(sql);
        } 
        private Dictionary<string, string> DispatchData(DataTable dt)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            IList<string> cf = new List<string>(), dy = new List<string>(), yg = new List<string>(), cs = new List<string>();
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DJLX"].ToString() == "查封登记" || dt.Rows[i]["DJLX"].ToString() == "解封登记")
                    {
                        cf.Add("'"+dt.Rows[i]["SLBH"].ToString()+"'");
                    }
                    else if (dt.Rows[i]["DJLX"].ToString() == "抵押登记" || dt.Rows[i]["DJLX"].ToString() == "抵押注销")
                    {
                        dy.Add("'"+dt.Rows[i]["SLBH"].ToString()+"'");
                    }
                    else if (dt.Rows[i]["DJLX"].ToString() == "预告登记" || dt.Rows[i]["DJLX"].ToString() == "预告注销")
                    {
                        yg.Add("'"+dt.Rows[i]["SLBH"].ToString()+"'");
                    }
                    else if (dt.Rows[i]["DJLX"].ToString() == "首次登记")
                    {
                        cs.Add("'"+dt.Rows[i]["SLBH"].ToString()+"'");
                    }
                }
            }
            if (cf.Count > 0) {
                ret.Add("CF", string.Join(",", cf.ToArray()));
            }
            if (dy.Count > 0)
            {
                ret.Add("DY", string.Join(",", dy.ToArray()));
            }
            if (yg.Count > 0)
            {
                ret.Add("YG", string.Join(",", yg.ToArray()));
            }
            if (cs.Count > 0)
            {
                ret.Add("CS", string.Join(",", cs.ToArray()));
            }
            return ret;
        }
        /// <summary>
        /// 一次轮询执行的操作
        /// </summary>
        public bool GetOnce()
        {
            try
            {
                DataTable dt = GetPushDataInfos();
                Dictionary<string, string> patchdata = DispatchData(dt);
                foreach (var d in patchdata)
                {
                    if (!string.IsNullOrEmpty(d.Value))
                    {
                        VisitWebService v = new VisitWebService();
                        switch (d.Key)
                        {
                            case "CF":
                                DataSet ds_CF = GetPushData("CF", d.Value);
                                string r_CF = v.UpdateSealStateForSPF(ds_CF, "3");
                                ResetPushResult(d.Value, r_CF);
                                break;
                            case "DY":
                                DataSet ds_DY = GetPushData("DY", d.Value);
                                string r_DY = v.UpdateMortgageStateForSPF(ds_DY, "3");
                                ResetPushResult(d.Value, r_DY);
                                break;
                            case "YG":
                                DataSet ds_YG = GetPushData("YG", d.Value);
                                string r_YG = v.UpdateYGDJStateForSPF(ds_YG, "3");
                                ResetPushResult(d.Value, r_YG);
                                break;
                            case "CS":
                                DataSet ds_CS = GetPushData("CS", d.Value);
                                string r_CS = v.UpdateCSDJStateForSPF(ds_CS, "3");
                                ResetPushResult(d.Value, r_CS);
                                break;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Debug("执行错误", ex);
                return false;
            }
        }  
        private DataSet GetPushData(string flag,string  where)
        {
            string sql = string.Empty;
            if (flag == "CF") {
                sql = string.Format("select * from fc_updateseal where slbh in({0})", where);
            }
            else if (flag == "DY") {
                sql = string.Format("select * from fc_updatemortage where slbh in({0})", where);
            }
            else if (flag == "YG")
            {
                sql = string.Format("select * from fc_updateygdj where slbh in({0})", where);
            }
            else if (flag == "CS")
            {
                sql = string.Format("select * from fc_updateinitcert where slbh in({0})", where);
            }
            return OleDBHelper.GetDataSet(sql);
        }
        /// <summary>
        /// 在标记表中返回
        /// </summary>
        private void ResetPushResult(string slbhs,string ret)
        {
            string sql =string.Empty;
            if(ret=="1")//返回成功
                sql = string.Format("update FC_SPFHX_TAG set SFTS =-1 where slbh in({0})",slbhs);
            else
                sql = string.Format("update FC_SPFHX_TAG set SFTS =SFTS+1 where slbh in({0})", slbhs);
            OleDBHelper.ExecuteCommand(sql);
        }

        public void RePushSF()
        {
            Web4BDC.Bll.FCSF.FCSFBLL.RePushSF();
        }
    }
}
