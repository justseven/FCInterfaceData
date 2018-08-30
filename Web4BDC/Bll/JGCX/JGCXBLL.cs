using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Web4BDC.Dal.JGCX;
using System.Linq;
using Web4BDC.Models.JGCXModel;

namespace Web4BDC.Bll.JGCX
{

    public class JGCXBLL
    {
        int colCount = Convert.ToInt32(ConfigurationManager.AppSettings["JGCXColCount"]);
        int qscfCount = 10;
        JGCXDAL dal = new JGCXDAL();
        public string GetCXLX(string slbh)
        {
            
            return dal.GetCXLX(slbh);
        }

        //internal object GetCFQKCX(string cxbh)
        //{
        //    CFQKCX dyqkcx = new CFQKCX();
        //    dyqkcx.CXBH = cxbh;
        //    dyqkcx.TITLE = "查封信息";
        //    dyqkcx.CFQK = new List<CFQK>();
        //    DataTable dt = dal.GetSLbhFrmCX(cxbh);
        //    if (null != dt && dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            string slbh = row["SLBH"].ToString();
        //            dyqkcx.CXRQ = dal.GetCXRQ(cxbh);
        //            GetSQR(dyqkcx, cxbh);
        //            dyqkcx.BDCZH = ConcatStr(dyqkcx.BDCZH, row["BDCZH"].ToString());
        //            dyqkcx.ZL = ConcatStr(dyqkcx.ZL, row["ZL"].ToString());

        //            CFQK dy = GetCFQKList(row["slbh"].ToString(), row["BDCZH"].ToString());
        //            dyqkcx.CFQK.Add(dy);
        //        }
        //    }
        //    return dyqkcx;
        //}

        //private CFQK GetCFQKList(string slbh, string bdczh)
        //{
        //    throw new NotImplementedException();
        //}
        private DYQK GetDYQKList(string slbh, string bdczh)
        {
            throw new NotImplementedException();
        }

        

       

        internal object GetQSQK(string cxbh)
        {
            QSQKCX qsqxcx = new QSQKCX();
            qsqxcx.CXBH = cxbh;
            qsqxcx.TITLE = "不动产登记信息";
            qsqxcx.QSQKList = new List<QSQK>();
            
            DataTable dt = dal.GetSLbhFrmCX(cxbh);
            if(null!=dt && dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row["SLBH"].ToString();
                    qsqxcx.CXRQ = dal.GetCXRQ(cxbh);

                    GetSQR(qsqxcx, cxbh);
                    qsqxcx.BDCZH = ConcatStr(qsqxcx.BDCZH, row["BDCZH"].ToString());
                    qsqxcx.ZL = ConcatStr(qsqxcx.ZL, row["ZL"].ToString());

                    List<QSQK> qs  = GetQSQKList(row["slbh"].ToString(),row["BDCZH"].ToString());
                    qsqxcx.QSQKList.AddRange(qs);
                }
            }
            return qsqxcx;
        }

        internal object GetZRZYQK(string cxbh)
        {
            ZRZYQKCX zrzyqkcx = new ZRZYQKCX();
            zrzyqkcx.TITLE = "不动产登记自然状况信息";
            zrzyqkcx.CXBH = cxbh;
            zrzyqkcx.ZRZYList = new List<ZRZYQK>();
            DataTable dt = dal.GetSLbhFrmCX(cxbh);
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                   
                    string slbh = row["SLBH"].ToString();
                    zrzyqkcx.CXRQ = dal.GetCXRQ(cxbh);

                    GetSQR(zrzyqkcx, cxbh);
                    zrzyqkcx.BDCZH = ConcatStr(zrzyqkcx.BDCZH, row["BDCZH"].ToString());
                    zrzyqkcx.ZL = ConcatStr(zrzyqkcx.ZL, row["ZL"].ToString());

                    List<ZRZYQK> qs = GetZRZYQKList(row["slbh"].ToString(), row["BDCZH"].ToString());
                    zrzyqkcx.ZRZYList.AddRange(qs);
                }
            }
            return zrzyqkcx;

        }

        private List<ZRZYQK> GetZRZYQKList(string slbh, string bdczh)
        {
            List<ZRZYQK> list = new List<ZRZYQK>();
            DataTable dt = GetTstybm(slbh);

            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string tstybm = row[0].ToString();
                    ZRZYQK zrzyqk = new ZRZYQK();
                    zrzyqk.bdczh = bdczh;
                    zrzyqk.zrzyxx = dal.GetZRZYXX(slbh, tstybm);
                    zrzyqk.zrzyxx.ygqk = GetZRZQYGQK(tstybm);
                    zrzyqk.zrzyxx.dyqk = GetZRZQDYQK(tstybm);
                    zrzyqk.zrzyxx.cfqk = GetZRZQCFQK(tstybm);
                    zrzyqk.zrzyxx.yyqk = GetZRZQYYQK(tstybm);
                    list.Add(zrzyqk);
                }

            }

            return list;
        }

        private string GetZRZQYYQK(string tstybm)
        {
            int count = dal.GetDJCount(tstybm, "异议");
            if (count > 0)
                return "有";
            return "无";
        }

        private string GetZRZQCFQK(string tstybm)
        {
            int count = dal.GetDJCount(tstybm, "查封");
            if (count > 0)
                return "有";
            return "无";
        }

        private string GetZRZQDYQK(string tstybm)
        {
            int count = dal.GetDJCount(tstybm, "抵押");
            if (count > 0)
                return "有";
            return "无";
        }

        private string GetZRZQYGQK(string tstybm)
        {
            int count = dal.GetDJCount(tstybm,"预告");
            if (count > 0)
                return "有";
            return "无";
        }

        internal object GetYGQK(string slbh)
        {
            throw new NotImplementedException();
        }

        private void GetZL(QSQKCX qsqxcx, string zl)
        {
            if (string.IsNullOrEmpty(qsqxcx.ZL))
                qsqxcx.ZL = zl;
            else
                qsqxcx.ZL += "、" + zl;
        }

        private void GetBDCZH(QSQKCX qsqxcx, string bdczh)
        {
            if (string.IsNullOrEmpty(qsqxcx.BDCZH))
                qsqxcx.BDCZH = bdczh;
            else
                qsqxcx.BDCZH +="、"+ bdczh;
        }

        private List<QSQK> GetQSQKList(string slbh,string bdczh)
        {

            List<QSQK> list = new List<QSQK>();

            DataTable dt = GetTstybm(slbh);

            if(null!=dt&& dt.Rows.Count>0)
            {
                    foreach (DataRow row in dt.Rows)
                    {
                        QSQK qsqk = new QSQK();
                        qsqk.Qsxx = dal.GetQSXX(slbh, row[0].ToString());
                        qsqk.DYXX = dal.getQS_DYXX_tstybm(row[0].ToString());
                        if (null == qsqk.DYXX)
                        {
                            qsqk.DYXX = new List<DJDY>();
                            qsqk.DYXX.Add(new DJDY());
                        }
                        List<DJCF> cfxx = dal.GetQS_CFXX(row[0].ToString());
                    if (cfxx.Count > qscfCount)
                    {
                        int index = cfxx.Count / qscfCount;
                        int modeNumber = cfxx.Count % qscfCount;
                        for (int i = 0; i < index; i++)
                        {
                            QSQK newqsqk = new QSQK();
                            newqsqk.Qsxx = qsqk.Qsxx;
                            newqsqk.DYXX = qsqk.DYXX;
                            newqsqk.CFXX = GetListFromList(cfxx, i * qscfCount, (i + 1) * qscfCount);
                            CreateCFQX(newqsqk.CFXX);
                            list.Add(newqsqk);
                        }
                        if (modeNumber > 0)
                        {
                            QSQK newqsqk = new QSQK();
                            newqsqk.Qsxx = qsqk.Qsxx;
                            newqsqk.DYXX = qsqk.DYXX;
                            newqsqk.CFXX = GetListFromList(cfxx, index * qscfCount, index * qscfCount + modeNumber);
                            CreateCFQX(newqsqk.CFXX);
                            list.Add(newqsqk);
                        }
                    }
                    else
                    {
                        qsqk.CFXX = cfxx;
                        CreateCFQX(qsqk.CFXX);
                        list.Add(qsqk);
                    }
                    }
                

            }

            return list;
        }

        private List<DJCF> GetListFromList(List<DJCF> source,int start, int end)
        {
            List<DJCF> list = new List<DJCF>();
            for (int i = start; i < end; i++)
            {
                list.Add(source[i]);
            }
            return list;
        }

        private void CreateCFQX(List<DJCF> list)
        {
            if (null != list && list.Count>0)
            {
                foreach (DJCF cf in list)
                {
                    
                    try
                    {
                        if (string.IsNullOrEmpty(cf.CFQX))
                        {
                            if (null != cf.CFJSSJ)
                            {
                                int years = 3;
                                if (null != cf.CFQSSJ)
                                {
                                    years = Convert.ToDateTime(cf.CFJSSJ).Year - Convert.ToDateTime(cf.CFQSSJ).Year;

                                }
                                else
                                {
                                    years = Convert.ToDateTime(cf.CFJSSJ).Year - Convert.ToDateTime(cf.CFRQ).Year;
                                }
                                cf.CFQX = years.ToString();
                            }
                            
                        }
                    }
                    catch
                    {
                        continue;
                    }
                   
                }
            }
        }

        private DataTable GetTstybm(string slbh)
        {
            return dal.GetTstybm(slbh);
        }

        private void GetSQR(CX_Head qsqxcx, string slbh)
        {
            DataTable sqrArray = dal.GetCXSQR(slbh);
            if (null != sqrArray && sqrArray.Rows.Count > 0)
            {
                foreach (DataRow sqrRow in sqrArray.Rows)
                {
                    qsqxcx.SQR = ConcatStr(qsqxcx.SQR, sqrRow["QLRMC"].ToString());
                    qsqxcx.SQRZJHM = ConcatStr(qsqxcx.SQRZJHM, sqrRow["ZJHM"].ToString());
                    qsqxcx.SQRZJLX = ConcatStr(qsqxcx.SQRZJLX, sqrRow["ZJLX"].ToString());
                }
            }
        }

        private string ConcatStr(string str,string value)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = value;
            }
            else
            {
                if(!str.Contains(value))
                str += "、" + value;
            }
            return str;
        }

        internal object GetCFQK(string cxbh)
        {
            CFQKCX cfqkcx = new CFQKCX();
            cfqkcx.CXBH = cxbh;
            cfqkcx.TITLE = "不动产查封情况";
            cfqkcx.CFQK = new List<CFQK>();
            DataTable dt = dal.GetSLbhFrmCX(cxbh);
            List<string> xgzh = new List<string>();
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row["SLBH"].ToString();
                    cfqkcx.CXRQ = dal.GetCXRQ(cxbh);
                    GetSQR(cfqkcx, cxbh);
                    cfqkcx.BDCZH = ConcatStr(cfqkcx.BDCZH, row["BDCZH"].ToString());
                    cfqkcx.ZL = ConcatStr(cfqkcx.ZL, row["ZL"].ToString());

                    DataTable bdcdyhList = dal.getBDCDYH_slbh(slbh);
                    if(null!= bdcdyhList && bdcdyhList.Rows.Count>0)
                    {
                        foreach (DataRow dyh in bdcdyhList.Rows)
                        {
                            List<CFQK> list = CreateCFQK(row, slbh, dyh[0].ToString());

                            cfqkcx.CFQK.AddRange(list);
                        }
                    }
                }
            }
            
            return cfqkcx;




        }

        private List<CFQK> CreateCFQK(DataRow row, string slbh, string dyh)
        {
            List<CFQK> list = new List<CFQK>();
            List<CFXX> cfList=  dal.GetCFXXList(slbh);
           
            
            if (cfList.Count < colCount && cfList.Count>0)
            {
                CFQK cfqk = new CFQK();
                cfqk.BDCZH = row["BDCZH"].ToString();
                cfqk.BDCDYH = dyh;
                cfqk.ZL = row["ZL"].ToString();
                for (int i = cfList.Count; i < colCount; i++)
                {
                    cfList.Add(new CFXX());
                }
                cfqk.CFXXList = cfList;
                list.Add(cfqk);
            }
            if(cfList.Count> colCount)
            {
                int index = cfList.Count / colCount;
                int count= cfList.Count % colCount;

                for (int i = 0; i < index; i++)
                {
                    CFQK cfqk = new CFQK();
                    cfqk.BDCZH = row["BDCZH"].ToString();
                    cfqk.BDCDYH = dyh;
                    cfqk.ZL = row["ZL"].ToString();
                    for (int j = 0; j < colCount; j++)
                    {
                        cfqk.CFXXList.Add(cfList[i * colCount + j]);
                    }
                    list.Add(cfqk);
                }
                if (count>0)
                {
                    CFQK cfqk = new CFQK();
                    cfqk.BDCZH = row["BDCZH"].ToString();
                    cfqk.BDCDYH = dyh;
                    cfqk.ZL = row["ZL"].ToString();
                    for (int m = 1; m < count+1; m++)
                    {
                        cfqk.CFXXList.Add(cfList[cfList.Count-m]);
                        //cfqk.CFXXList.Sort();
                    }
                    
                    if (cfqk.CFXXList.Count < colCount)
                    {
                        for (int i = cfqk.CFXXList.Count; i < colCount; i++)
                        {
                            cfqk.CFXXList.Add(new CFXX());
                        }
                    }
                    list.Add(cfqk);
                }




            }
            return list;
        }

        internal object GetDYQK(string cxbh)
        {
            DYQKCX dycqcx = new DYQKCX();
            DataTable dt = dal.GetSLbhFrmCX(cxbh);
            dycqcx.dyqk = new List<DYQK>();
            dycqcx.CXBH = cxbh;
            dycqcx.TITLE = "不动产抵押情况";
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string slbh = row["SLBH"].ToString();
                    dycqcx.CXRQ = dal.GetCXRQ(cxbh);
                    GetSQR(dycqcx, cxbh);
                    dycqcx.BDCZH = ConcatStr(dycqcx.BDCZH, row["BDCZH"].ToString());
                    dycqcx.ZL = ConcatStr(dycqcx.ZL, row["ZL"].ToString());

                    DataTable bdcdyhList = dal.getBDCDYH_slbh(slbh);
                    if (null != bdcdyhList && bdcdyhList.Rows.Count > 0)
                    {
                        foreach (DataRow dyh in bdcdyhList.Rows)
                        {
                            DYQK dyqk = new DYQK();
                            dyqk.bdczh = row["BDCZH"].ToString();
                            dyqk.bdcdyh = dyh[0].ToString();
                            dyqk.bdclx = dyh["BDCLX"].ToString();
                            dyqk.zl = row["ZL"].ToString();
                            if (string.IsNullOrEmpty(dyqk.bdcdyh))
                            {
                                dyqk.dyxx = dal.GetQS_DYXX_tstybm(dyh["tstybm"].ToString());
                            }
                            else
                            {
                                dyqk.dyxx = dal.GetQS_DYXX(dyqk.bdcdyh);
                            }
                            if (null != dyqk.dyxx && dyqk.dyxx.Count > 0)
                            {
                                
                                foreach (DJDY djdy in dyqk.dyxx)
                                {
                                    djdy.DYR = GetDYR(djdy.SLBH);
                                }
                                if (dyqk.dyxx.Count < colCount)
                                {
                                    for (int i = dyqk.dyxx.Count; i < colCount; i++)
                                    {
                                        dyqk.dyxx.Add(new DJDY());
                                    }

                                }
                                dycqcx.dyqk.Add(dyqk);
                            }
                            else
                            {
                                if (dyqk.bdclx.Equals("房屋"))
                                {
                                    dyqk.dyxx = new List<DJDY>();
                                    dyqk.dyxx = new List<DJDY>();
                                    if (dyqk.dyxx.Count < colCount)
                                    {
                                        for (int i = dyqk.dyxx.Count; i < colCount; i++)
                                        {
                                            dyqk.dyxx.Add(new DJDY());
                                        }

                                    }
                                    dycqcx.dyqk.Add(dyqk);
                                }
                                    
                            }
                           
                        }
                    }
                    
                    
                }
            }
            
            return dycqcx;
        }

        private string GetDYR(string sLBH)
        {
            return dal.GetDYR_Slbh(sLBH);
        }
    }
}