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
using XZFCDA.FC.Models;
using XZFCDA.Models.BDCModel;
using XZFCDA.Tools;

namespace XZFCDA.Dal
{
    public class ImportDAL
    {
        private static string GGK = ConfigurationManager.ConnectionStrings["ZtgeoGGK"].ConnectionString;

        private static string WDK = ConfigurationManager.ConnectionStrings["ZtgeoWDK"].ConnectionString;

        public static DataTable GetPushedSLBH()
        {
            string sql = "select distinct slbh from FC_DA_TAG t where t.issuccess='1'";
            DbHelper.SetProvider(MyDBType.Oracle);
            return DbHelper.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

        }



        internal static void InsertDoc_binfile(XZFCDA.Models.BDCModel.DOC_BINFILE doc_bin)
        {
            try
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




                DbHelper.Conn = new OracleConnection(WDK);
                DbHelper.SetProvider(MyDBType.Oracle);
                DbHelper.ExecuteNonQuery(DbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());

                UpdateWfm_attachlst(doc_bin.BINID);

            }
            catch (Exception ex)
            {
                //WriteLog(path, "insertDOCBINFILE"+ex.Message);
                throw ex;
            }
        }

        private static void UpdateWfm_attachlst(string p)
        {
            string sql = "update wfm_attachlst t set t.ckind='文件',t.isupload=1 where cid='{0}'";
            sql = string.Format(sql, p);
            DbHelper.Conn = new OracleConnection(GGK);
            DbHelper.SetProvider(MyDBType.Oracle);
            DbHelper.ExecuteNonQuery(DbHelper.Conn, System.Data.CommandType.Text, sql, null);
        }

        internal static void InsertDoc_File(XZFCDA.Models.BDCModel.DOC_FILE doc_file)
        {
            throw new NotImplementedException();
        }

        internal static void InsertLog(FC_REWRITE_TAG tag)
        {
            string sql = "insert into FC_ReWrite_Tag (ID,SLBH,PUSHDATE,FILECOUNT,FILENAME,ISSUCCESS,MESSAGE) values(:ID,:SLBH,:PUSHDATE,:FILECOUNT,:FILENAME,:ISSUCCESS,:MESSAGE)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, ":ID", tag.ID);
            ListAdd(list, ":SLBH", tag.SLBH);

            ListAdd(list, ":PUSHDATE", tag.PUSHDATE);
            ListAdd(list, ":FILECOUNT", tag.FILECOUNT);
            ListAdd(list, ":FILENAME", tag.FILENAME);
            ListAdd(list, ":ISSUCCESS", tag.ISSUCCESS);
            ListAdd(list, ":MESSAGE", tag.MESSAGE);

            DbHelper.SetProvider(MyDBType.Oracle);
            DbHelper.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new OracleParameter(paraName, DBNull.Value));
            else
                list.Add(new OracleParameter(paraName, value));
        }

        internal static List<VolEleArcDtl> GetVolEleArcDtl(string slbh)
        {
            string sql = "select a.* from [dbo].[VolEleArcDtl] as a left join [dbo].[VolEleArc] as b on a.VolEleArc_ID=b.EleArcVol_ID left join [dbo].[ArchiveIndex] as c on c.ArchiveId=b.ArchiveId where c.BusiNO='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Sql);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            return Tools.ModelHelper<VolEleArcDtl>.FillModel(dt);
        }

        internal static VolEleArc GetVolEleArc(Guid? vid)
        {
            string sql = "select * from [dbo].[VolEleArc]  where EleArcVol_ID='{0}'";
            sql = string.Format(sql, vid);
            DbHelper.SetProvider(MyDBType.Sql);
            DataTable dt = DbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            return Tools.ModelHelper<VolEleArc>.FillModel(dt.Rows[0]);
        }

        internal static string GetCID(string slbh, string fileName)
        {
            string sql = "select CID from wfm_attachlst where pnode = '{0}' and cname='{1}' and cname<>'流程附件'";
            sql = string.Format(sql, slbh, fileName);
            DbHelper.Conn = new OracleConnection(GGK);

            try
            {
                DbHelper.SetProvider(MyDBType.Oracle);
                object o = DbHelper.ExecuteScalar(DbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return o.ToString();
                return "";


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static string GetCID(string slbh)
        {
            string sql = "select CID from wfm_attachlst where pnode= '{0}' and cname='流程附件'";
            sql = string.Format(sql, slbh);
            DbHelper.Conn = new OracleConnection(GGK);

            try
            {
                DbHelper.SetProvider(MyDBType.Oracle);
                object o = DbHelper.ExecuteScalar(DbHelper.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return o.ToString();
                return "";


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool CanImport(string p)
        {
            string sql = "select count(1) from [dbo].[VolEleArcDtl] as a left join [dbo].[VolEleArc] as b on a.VolEleArc_ID=b.EleArcVol_ID left join [dbo].[ArchiveIndex] as c on c.ArchiveId=b.ArchiveId where c.BusiNO='{0}' and a.imgName is not null";
            sql = string.Format(sql, p);
            DbHelper.SetProvider(MyDBType.Sql);
            int count = Convert.ToInt32(DbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null));
            if (count > 0)
                return true;
            return false;
        }

        internal static string InserAttachlst(WFM_ATTACHLST att)
        {
            try
            {
                string sql = string.Concat(new string[]
				{
					"insert into wfm_attachlst (cid, pid, cname, pnode, ptype, ctype, ckind, csort, createdate, createby, isupload) values ('",
					att.CID,
					"', '",
					att.PID,
					"', '",
					att.CNAME,
					"', '",
					att.PNODE,
					"', '流程实例', '必选', '文件', 0,sysdate , 'ADMIN', 1)"
				});
                DbHelper.Conn = new OracleConnection(GGK);
                DbHelper.SetProvider(MyDBType.Oracle);
                DbHelper.ExecuteNonQuery(DbHelper.Conn, System.Data.CommandType.Text, sql, null);
                return att.CID;
            }
            catch (Exception ex)
            {
                //WriteLog(path, "insertATTACHLST"+ex.Message);
                throw ex;
            }
            return "";
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




            DbHelper.Conn = new OracleConnection(WDK);
            DbHelper.SetProvider(MyDBType.Oracle);
            DbHelper.ExecuteNonQuery(DbHelper.Conn, System.Data.CommandType.Text, sql, list.ToArray());

        }

        internal static bool ExistDoc_binfile(DOC_BINFILE item)
        {
            string sql = "select count(1) from doc_binfile where binid='{0}'";
            sql = string.Format(sql, item.BINID);
            DbHelper.Conn = new OracleConnection(WDK);
            DbHelper.SetProvider(MyDBType.Oracle);
            int count = Convert.ToInt32(DbHelper.ExecuteScalar(DbHelper.Conn, System.Data.CommandType.Text, sql, null));
            if (count > 0)
                return true;
            return false;
        }
    }
}
