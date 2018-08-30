/****************************************************************************************
 *                              2017.7.17
 *                                 by seven
 * 
 * 
 * 
 * 
 * 
 * 
 * ***************************************************************************************/
using Config;
//using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using Web4BDC.FC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Tools;

namespace Web4BDC.Dal
{
    public class ImportDAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["bdcwdkConnection"].ConnectionString;

        private static readonly object lockKey = new object();
            
        public static DataTable GetPushedSLBH()
        {
            string sql = "select distinct slbh from FC_DA_TAG t where t.issuccess='1'";
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                return dbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            }

        }



        internal static void InsertDoc_binfile(Web4BDC.Models.BDCModel.DOC_BINFILE doc_bin)
        {
           
                string sql = "insert into doc_binfile (binid, fileid, filename, extname, filesize, isencrypted, iscompressed, ftpath)values (:binid,:fileid,:filename,:extname,:filesize, :isencrypted,:iscompressed,:ftpath)";
                List<DbParameter> list = new List<DbParameter>();
                ListAdd(list, ":binid", doc_bin.BINID);
                ListAdd(list, ":fileid", doc_bin.FILEID);

                ListAdd(list, ":filename", doc_bin.FILENAME);
                ListAdd(list, ":extname", doc_bin.EXTNAME);
                ListAdd(list, ":filesize", doc_bin.FILESIZE);
                ListAdd(list, ":isencrypted", doc_bin.ISENCRYPTED);
                ListAdd(list, ":iscompressed", doc_bin.ISCOMPRESSED);
                ListAdd(list, ":ftpath", doc_bin.FTPATH);




                
                lock (lockKey)
                {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());
                dbHelper.Conn.Close();
                    UpdateWfm_attachlst(doc_bin.BINID);
                }
                


            
        }

        private static void UpdateWfm_attachlst(string p)
        {
            string sql = "update wfm_attachlst t set t.ckind='文件',t.isupload=1 where cid='{0}'";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);

                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                dbHelper.Conn.Close();
            }
        }

        internal static void InsertDoc_File(Web4BDC.Models.BDCModel.DOC_FILE doc_file)
        {
            throw new NotImplementedException();
        }

        internal static void InsertLog(FC_REWRITE_TAG tag)
        {
            string sql = "insert into FC_REWRITE_TAG (ID,SLBH,PUSHDATE,FILECOUNT,FILENAME,ISSUCCESS,MESSAGE) values(:ID,:SLBH,:PUSHDATE,:FILECOUNT,:FILENAME,:ISSUCCESS,:MESSAGE)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":ID", tag.ID);
            ListAdd(list, ":SLBH", tag.SLBH);

            ListAdd(list, ":PUSHDATE", tag.PUSHDATE);
            ListAdd(list, ":FILECOUNT", tag.FILECOUNT);
            ListAdd(list, ":FILENAME", tag.FILENAME);
            ListAdd(list, ":ISSUCCESS", tag.ISSUCCESS);
            ListAdd(list, ":MESSAGE", tag.MESSAGE);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
            }
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new Oracle.DataAccess.Client.OracleParameter(paraName, value));
        }

        internal static List<VolEleArcDtl> GetVolEleArcDtl(string slbh)
        {
            string sql = "select a.* from [dbo].[VolEleArcDtl] as a left join [dbo].[VolEleArc] as b on a.VolEleArc_ID=b.EleArcVol_ID left join [dbo].[ArchiveIndex] as c on c.ArchiveId=b.ArchiveId where c.BusiNO='{0}'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return Tools.ModelHelper<VolEleArcDtl>.FillModel(dt);
            }
        }

       

        internal static string GetCID(string slbh, string fileName,string fileType)
        {
            string sql = "select CID from wfm_attachlst where pnode = '{0}' and cname='{1}' and cname<>'流程附件' and CKIND='{2}'";
            sql = string.Format(sql, slbh, fileName, fileType);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                dbHelper.Conn.Close();
                if (null != o)
                    return o.ToString();
                return "";


            }
        }

        internal static string GetCID(string slbh)
        {
            string sql = "select CID from wfm_attachlst where pnode= '{0}' and cname='流程附件'";
            sql = string.Format(sql, slbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);


                dbHelper.SetProvider(MyDBType.Oracle);
                object o = dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                dbHelper.Conn.Close();
                if (null != o)
                    return o.ToString();
                return "";


            }
        }

        internal static bool CanImport(string p)
        {
            string sql = "select count(1) from [dbo].[VolEleArcDtl] as a left join [dbo].[VolEleArc] as b on a.VolEleArc_ID=b.EleArcVol_ID left join [dbo].[ArchiveIndex] as c on c.ArchiveId=b.ArchiveId where c.BusiNO='{0}' and a.imgName is not null";
            sql = string.Format(sql, p);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int count = Convert.ToInt32(dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null));
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static string InserAttachlst(WFM_ATTACHLST att,string fileType,string user)
        {

            string sql = "insert into wfm_attachlst (cid, pid, cname, pnode, ptype, ctype, ckind, csort, createdate, createby, isupload) " +
                "values ('{0}', '{1}', '{2}', '{3}', '流程实例', '必选', '{4}', 0,sysdate , '{5}', 0)";
                    //string.Concat(new string[]  
            sql = string.Format(sql, att.CID, att.PID, att.CNAME, att.PNODE, fileType, user);
                lock (lockKey)
                {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                dbHelper.Conn.Close();
                    return att.CID;
                }
           
           
        }

        internal static string InserAttachlst(WFM_ATTACHLST att)
        {

            string sql = "insert into wfm_attachlst (cid, pid, cname, pnode, ptype, ctype, ckind, csort, createdate, createby, isupload) " +
                "values ('{0}', '{1}', '{2}', '{3}', '流程实例', '必选', '文件', 0,sysdate , 'ADMIN', 0)";
            //string.Concat(new string[]  
            sql = string.Format(sql, att.CID, att.PID, att.CNAME, att.PNODE);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, null);
                dbHelper.Conn.Close();
                return att.CID;
            }


        }

        internal static void UpDateDoc_binfile(DOC_BINFILE doc_bin)
        {
            string sql = " update doc_binfile set fileid=:fileid, filename=:filename, extname=:extname, filesize=:filesize, isencrypted=:isencrypted, iscompressed=:iscompressed, ftpath=:ftpath where binid=:binid";

            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list, ":fileid", doc_bin.FILEID);

            ListAdd(list, ":filename", doc_bin.FILENAME);
            ListAdd(list, ":extname", doc_bin.EXTNAME);
            ListAdd(list, ":filesize", doc_bin.FILESIZE);
            ListAdd(list, ":isencrypted", doc_bin.ISENCRYPTED);
            ListAdd(list, ":iscompressed", doc_bin.ISCOMPRESSED);
            ListAdd(list, ":ftpath", doc_bin.FTPATH);
            ListAdd(list, ":binid", doc_bin.BINID);



            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);
                dbHelper.SetProvider(MyDBType.Oracle);
                dbHelper.ExecuteNonQuery(dbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());
                dbHelper.Conn.Close();
            }

        }

        internal static bool ExistDoc_binfile(DOC_BINFILE item)
        {
            string sql = "select count(1) from doc_binfile where binid='{0}'";
            sql = string.Format(sql, item.BINID);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(WDK);
                dbHelper.SetProvider(MyDBType.Oracle);
                int count = Convert.ToInt32(dbHelper.ExecuteScalar(dbHelper.Conn, System.Data.CommandType.Text, sql, null));
                dbHelper.Conn.Close();
                if (count > 0)
                    return true;
                return false;
            }
        }

        internal static VolEleArc GetVolEleArc(Guid? vid)
        {
            string sql = "select * from [dbo].[VolEleArc]  where EleArcVol_ID='{0}'";
            sql = string.Format(sql, vid);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return Tools.ModelHelper<VolEleArc>.FillModel(dt.Rows[0]);
            }
        }

        internal static List<VolEleArc> GetVolEleArc_list(string FCslbh)
        {
            string sql = "select b.* from  [dbo].[VolEleArc] as b left join [dbo].[ArchiveIndex] as c on c.ArchiveId=b.ArchiveId where c.BusiNO='{0}'";
            sql = string.Format(sql, FCslbh);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return Tools.ModelHelper<VolEleArc>.FillModel(dt);
            }
        }

        internal static List<VolEleArcDtl> GetVolEleArcDtlByVol(Guid guid)
        {
            string sql = "select * from [dbo].[VolEleArcDtl]  where [VolEleArc_ID]='{0}' order by [imgName]";
            sql = string.Format(sql, guid);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return Tools.ModelHelper<VolEleArcDtl>.FillModel(dt);
            }
        }
    }
}
