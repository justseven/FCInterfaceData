using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace DBCForFCWebService
{
    /// <summary>
    /// Bdc2DY 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Bdc2DY : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        ///获取抵押办理情况
        ///功能要求：获取一个时间段（最小单位：天）内的抵押业务信息
        ///查询条件：抵押权人关键字、起始日期、结束日期
        /// </summary>
        /// <param name="strKeyWord"> 抵押权人</param>
        /// <param name="sDate">起始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns>获取一个时间段（最小单位：天）内的抵押业务信息</returns>
        
        public DataTable getDYOpListFromBDC(string strKeyWord, string sDate, string eDate, string strTXCertCode)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable resDT = dal.GetDYOpList(strKeyWord, sDate, eDate, strTXCertCode);
            if(null!=resDT && resDT.Rows.Count>0)
            {
                resDT = InitDyInfoDT(resDT);
            }
            return resDT;
        }
        [WebMethod]
        public DataTable getDYOpListFromBDC(string strKeyWord, string sDate, string eDate, string strTXCertCode,string fzSDate,string fzEDate)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable resDT = dal.GetDYOpList(strKeyWord, sDate, eDate, strTXCertCode,fzSDate,fzEDate);
            if (null != resDT && resDT.Rows.Count > 0)
            {
                resDT = InitDyInfoDT(resDT);
            }
            return resDT;
        }

        private DataTable InitDyInfoDT(DataTable resDT)
        {
            
            resDT.Columns.Add("抵押权人");
            resDT.Columns.Add("抵押人");
            resDT.Columns.Add("注销日期");

            foreach (DataRow row in resDT.Rows)
            {
                row["抵押人"] = GetQLRMCFromBDC(row["抵押业务宗号"].ToString(),"抵押人");
                row["抵押权人"] = GetQLRMCFromBDC(row["抵押业务宗号"].ToString(),"抵押权人");
                
                if (row["注销状态"].ToString().Equals("已注销"))
                {
                    row["注销日期"] = GetZXRQ(row[0].ToString());
                }
            }
            return resDT;
        }

        private DataTable InitZXRQ(DataTable resDT)
        {

            
            resDT.Columns.Add("注销日期");

            foreach (DataRow row in resDT.Rows)
            {
                if (row["注销状态"].ToString().Equals("已注销"))
                {
                    row["注销日期"] = GetZXRQ(row[0].ToString());
                }
            }
            return resDT;
        }

        private string GetZXRQ(string slbh)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            string zxrq = dal.GetZXRQ(slbh);
            return zxrq;
        }

        private string GetQLRMCFromBDC(string slbh,string IsDyqr)
        {
            string qlrmc = "";
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = null;
            dt = dal.GetQLRMCBySLBH(slbh,IsDyqr);
            if(null!=dt && dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string name = row[0].ToString();
                    if(!qlrmc.Equals(name))
                    {
                        qlrmc += " " + name;
                    }
                }
                qlrmc = qlrmc.Trim();
            }
            else
            {
                if (IsDyqr.Equals("抵押人"))
                {
                    dt = dal.GetQLRByFSLBH(slbh);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row[0].ToString();
                            if (!qlrmc.Equals(name))
                            {
                                qlrmc += " " + name;
                            }
                        }
                        qlrmc = qlrmc.Trim();
                    }
                }
            }
            return qlrmc;
        }

        /// <summary>
        ///抵押登记流程查询
        ///功能要求：抵押办理流程查询
        ///查询条件：业务宗号、他项权证号、产权人姓名、身份证号、房屋坐落
        /// </summary>
        /// <param name="ywzh">业务宗号</param>
        /// <param name="txqzh">他项权证号</param>
        /// <param name="cqr">产权人</param>
        /// <param name="zjhm">产权人证件号码</param>
        /// <param name="fwzl">房屋坐落</param>
        /// <param name="yhmc">抵押权人名称</param>
        /// <returns>如出现除产权人不同，其他相同，表示此不动产产权人为共有人</returns>
        [WebMethod]
        public DataTable getDYOpFlowFromBDC(string ywzh, string txqzh, string cqr, string zjhm, string fwzl, string yhmc)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = InitZXRQ( dal.getDYOpFlowFromBDC(ywzh, txqzh, cqr, zjhm, fwzl, yhmc));

            return dt;
        }
        /// <summary>
        ///查封情况查询
        ///功能要求：根据房屋ID获取房屋查封情况
        ///查询条件：房屋ID
        /// </summary>
        /// <param name="HoID">户ID</param>
        /// <returns>根据房屋ID获取房屋查封情况</returns>
        [WebMethod]
        
        public DataTable getCFInfoFromBDC(string HoID)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetCFInfoFromBDC(HoID);
            return dt;
        }

        /// <summary>
        ///获取权属信息索引
        ///功能要求：
        ///查询条件：权证编号、所有权人、证件号码、房屋坐落 
        /// </summary>
        /// <param name="bdczh">不动产证号</param>
        /// <param name="qlrmc">权利人名称</param>
        /// <param name="zjhm">权利人你证件号码</param>
        /// <param name="zl">不动产坐落</param>
        /// <returns>根据条件获取不动产权属信息</returns>
        [WebMethod]
        
        public DataTable getQSInfoListFromBDC(string bdczh, string qlrmc, string zjhm, string zl)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = InitQLRInfoDT(dal.GetQSInfoListFromBDC(bdczh, qlrmc, zjhm, zl));

            return dt;
        }

        private DataTable InitQLRInfoDT(DataTable resDT)
        {

            resDT.Columns.Add("权利人");
            resDT.Columns.Add("证件号码");

            foreach (DataRow row in resDT.Rows)
            {
                row["权利人"] = GetQLRMCFromBDC(row[0].ToString(), "权利人");
                row["证件号码"] = GetZJHMFromBDC(row[0].ToString(),"权利人");
            }
            return resDT;
        }

        private object GetZJHMFromBDC(string slbh, string qlrlx)
        {
            string qlrmc = "";
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = null;
            dt = dal.GetQLRZJHMBySLBH(slbh, qlrlx);
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string name = row[0].ToString();
                    if (!qlrmc.Equals(name))
                    {
                        qlrmc += " " + name;
                    }
                }
                qlrmc = qlrmc.Trim();
            }
            return qlrmc;
        }


        /// <summary>
        ///获取权属详细信息
        ///功能要求：根据条件获取不动产权属信息
        ///查询条件：业务ID 
        /// </summary>
        /// <param name="slbh">业务宗号</param>
        /// <returns>根据条件获取不动产权属信息</returns>
        [WebMethod]
        
        public DataTable getQSInfoFromBDC(string slbh)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetQSInfoFromBDC(slbh);
            return dt;
        }

        /// <summary>
        ///获取权利人信息
        ///功能要求：根据条件获取不动产权利人信息
        ///查询条件：业务ID 
        /// </summary>
        /// <param name="slbh">业务宗号</param>
        /// <returns>根据条件获取不动产权利人信息</returns>
        [WebMethod]
        
        public DataTable getQLRFromBDC(string slbh)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetQLRFromBDC(slbh);
            return dt;
        }

        /// <summary>
        ///获取分幢（房屋）信息
        ///功能要求：根据条件获取分幢信息
        ///查询条件：房屋ID 
        /// </summary>
        /// <param name="HID">户ID</param>
        /// <returns>根据条件获取分幢信息</returns>
        [WebMethod]
       
        public DataTable getFZFromBDC(string HID)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetFZFromBDC(HID);
            return dt;
        }

        /// <summary>
        ///获取抵押信息
        ///功能要求：根据条件获取不动产抵押信息
        ///查询条件：业务ID 
        /// </summary>
        /// <param name="slbh">业务宗号</param>
        /// <returns>根据条件获取不动产抵押信息</returns>
        [WebMethod]
        
        public DataTable getDYFromBDC(string slbh)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetDYFormBDC(slbh);
            return dt;
        }

        /// <summary>
        ///获取查封信息
        ///功能要求：根据条件获取查封信息
        ///查询条件：房屋ID
        /// </summary>
        /// <param name="hid">户ID</param>
        /// <returns>根据条件获取查封信息</returns>
        [WebMethod]
        
        public DataTable getCFFromBDC(string hid)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.getCFFromBDC(hid);
            return dt;
        }

        /// <summary>
        /// 获取不动产证明信息
        ///功能要求：获取指定条件的不动产证明信息
        ///查询条件：业务宗号（抵押）、不动产证明号
        /// </summary>
        /// <param name="strBusNo">抵押业务宗号</param>
        /// <param name="stringstrCertCode">不动产证明号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable getCertInfoFromBDC(string strBusNo, string strCertCode)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.getCertInfoFromBDC(strBusNo, strCertCode);
            if (null != dt && dt.Rows.Count > 0)
            {
                dt = InitDYCerInfoDT(dt);
            }
            return dt;
        }


        /***********************************************************
         * 
         * 2017/11/24 增加
         * 
         * ********************************************************/
        /// <summary>
        /// 获取指定条件的不动产单元（房屋）信息
        /// </summary>
        /// <param name="strHouseSite">坐落</param>
        /// <param name="strCertCode">不动产证明号</param>
        /// <returns></returns>
        [WebMethod]
        public DataTable getREInfoFromBDC(string strHouseSite, string strCertCode)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            return dal.getREInfoFromBDC(strHouseSite, strCertCode);
        }

        [WebMethod]
        public DataTable getBDCDYH_by_ZID(string zid)
        {
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            try
            {
                zid = new Guid(zid).ToString("D").ToUpper();
            }
            catch
            { }
            return dal.getBDCDYH(zid);
        }



        private DataTable InitDYCerInfoDT(DataTable resDT)
        {



            resDT.Columns.Add("抵押权人");
            resDT.Columns.Add("抵押人");
            resDT.Columns.Add("坐落");
            resDT.Columns.Add("不动产单元号");


            //DataTable dt = resDT.Clone();
            //DataRow newRow = dt.NewRow();

            foreach (DataRow row in resDT.Rows)
            {
                row["证明权利或事项"] = GetZMQL(row);

                row["抵押人"] = GetQLRMCFromBDC(row["抵押业务宗号"].ToString(), "抵押人");
                row["抵押权人"] = GetQLRMCFromBDC(row["抵押业务宗号"].ToString(), "抵押权人");
                row["坐落"] = GetDYZL(row["抵押业务宗号"].ToString());
                row["不动产单元号"] = GetDYBDCDYH(row["抵押业务宗号"].ToString());
                row["不动产证明序号"] = GetNumber(row["不动产证明序号"].ToString(),4);
                
            }
            //dt.Rows.Add(newRow);
            return resDT;
        }

        private string GetZMQL(DataRow row)
        {
            string dylx = row["LX"].ToString();
            if(!string.IsNullOrEmpty(dylx))
            {
                if(dylx.Contains("预告抵押"))
                {
                    return row["YGDJZL"].ToString();
                }
            }
            return "抵押权";
        }

        private  string GetNumber(string str,int startIndex)
        {
            Regex r = new Regex("\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(str);
            MatchCollection mc = r.Matches(str);

            string result = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                result += mc[i];//匹配结果是完整的数字，此处可以不做拼接的
            }
            return result.Substring(startIndex);
        }

        private string GetDYBDCDYH(string slbh)
        {
            string bdcdyh = "";
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetDYBDCDYH(slbh);
            if (null != dt)
            {
                if (dt.Rows.Count > 1)
                {
                    bdcdyh = dt.Rows[0]["BDCDYH"].ToString() + "等共" + dt.Rows.Count + "个";
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    if(string.IsNullOrEmpty(bdcdyh))
                    //    {
                    //        bdcdyh = row["BDCDYH"].ToString();
                    //    }
                    //    else
                    //    {
                    //        bdcdyh+=" "+ row["BDCDYH"].ToString();
                    //    }
                    //}
                }
                if (dt.Rows.Count==1)
                {
                    bdcdyh = dt.Rows[0]["BDCDYH"].ToString();
                }
            }
            return bdcdyh;
        }



        private string GetDYZL(string slbh)
        {
            //string bdcdyh = "";
            FC_DY_INFO_DAL dal = new FC_DY_INFO_DAL();
            DataTable dt = dal.GetDYZL(slbh);
            if(null!=dt && dt.Rows.Count>0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
            

            //if (null != dt && dt.Rows.Count > 0)
            //{
            //    //foreach (DataRow row in dt.Rows)
            //    //{
            //    //    if (string.IsNullOrEmpty(bdcdyh))
            //    //    {
            //    //        bdcdyh = row["ZL"].ToString();
            //    //    }
            //    //    else
            //    //    {
            //    //        bdcdyh += " " + row["ZL"].ToString();
            //    //    }

            //    //}

            //}
            //return bdcdyh;
        }
    }
}
