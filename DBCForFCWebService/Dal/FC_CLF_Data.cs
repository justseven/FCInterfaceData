using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DBCForFCWebService
{
    /// <summary>
    /// 获取存量房所需的数据
    /// </summary>
    public class FC_CLF_Data
    {
        public DataTable GetCLF_ZTXX(string syqr, string qzbh)
        {
            string sql = string.Format("select * from CLF_ZTXX where syqr ='{0}' and QZBH like '%{1}%'", syqr, qzbh);
            DataTable dt= OleDBHelper.GetDataTable(sql);
            DataTable dtCopy= dt.Copy();
            dtCopy.TableName = "HouseOwnerInfo";
            return dtCopy;
        }

        public DataTable GetCLF_ZTXX(string ywzh)
        {
            string sql = string.Format("select * from CLF_ZTXX where  YWZH='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "HouseOwnerInfo";
            return dtCopy;
        }

        public DataTable GetCLF_FZXX(string ywzh)
        {
            string sql = string.Format("select * from CLF_FZXX where ywzh ='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "houseInfo";
            return dtCopy;
        }

        public DataTable GetCLF_DYXX(string ywzh)
        {
            string sql = string.Format("select * from CLF_DYXX where ywzh ='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "houseHockInfo";
            return dtCopy;
        }
        public DataTable GetCLF_CFXX(string ywzh)
        {
            string sql = string.Format("select * from CLF_CFXX where ywzh ='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "houseChaFenInfo";
            return dtCopy;
        }
        /// <summary>
        /// 领证信息权证编号信息
        /// </summary>
        /// <param name="ywzh"></param>
        /// <returns></returns>
        public DataTable GetCLF_LZXX_QZBH(string ywzh)
        {
//            string sql = string.Format(@"select DJB.BDCZH AS QZBH FROM (SELECT * FROM  dj_xgdjgl WHERE BGLX='房屋变更') xgdjgl
//left join dj_djb djb on xgdjgl.zslbh=djb.slbh where xgdjgl.fslbh='{0}'", ywzh);
            string sql = string.Format("select * from CLF_LZXX_QZBH where ywzh='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "FZXX";
            return dtCopy;
        }
        /// <summary>
        /// 领证信息收费发证信息
        /// </summary>
        /// <param name="ywzh"></param>
        /// <returns></returns>
        public DataTable GetCLF_LZXX_SFFZ(string ywzh) {
            string sql = string.Format(@"select State,FZState from CLF_LZXX_SFXX where ywzh='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "SFFZXX";
            return dtCopy;
        }

        public DataTable GetCLF_LZXX_QTXX(string ywzh)
        {
            string sql = string.Format(@"select PrintState,SFState,FZState,DYQZBH from CLF_LZXX_DYXX where ywzh='{0}'", ywzh);
            DataTable dt = OleDBHelper.GetDataTable(sql);
            DataTable dtCopy = dt.Copy();
            dtCopy.TableName = "QTXX";
            return dtCopy;
        }
    }
}