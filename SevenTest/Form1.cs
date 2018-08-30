using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Web4BDC.Bll;
using Web4BDC.Bll.MKevaluate;
using Web4BDC.Dal;
using Web4BDC.FC.Models;
using Web4BDC.Models;
using Web4BDC.Models.BDCModel;
using Web4BDC.Tools;
using WorkflowMonitorXZFCPlug;
using WorkflowMonitorXZFCPlug.Dal;
using XMLHelper;

namespace SevenTest
{
    public partial class Form1 : Form
    {

        private static string GGK = ConfigurationManager.ConnectionStrings["bdcggkConnection"].ConnectionString;
        private static string DAK = ConfigurationManager.ConnectionStrings["bdcdakConnection"].ConnectionString;
        String viewName = "";
        string address = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = txtPath.Text;
            //if (radDA.Checked)
            //{
            //   UpdateFC();
            //     BDCRepush(str);
            //    //BDCRePushBySlbh(str.Trim());
            //}

            //CheckCnameInFC(str);
            //updatePerson(str);

            //UPDATEOLDpERSON();

             Reinsert();

            //UpdateIsHistroy();

            //updateFJ(str);

        }

        private void updateFJ(string str)
        {
            List<string> errList = new List<string>();
            DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str);
            int j = 0;
            if(null!=dt && dt.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            string yuan = (row["YUAN"].ToString()).Trim();
                            string xin = (row["XIN"].ToString()).Trim();

                            if (xin.Equals("x"))
                            {
                                deleteFJ(yuan);
                            }
                            else
                            {
                                UpdateFJ(yuan, xin);
                            }
                        }
                        catch (Exception ex)
                        {
                            errList.Add(ex.Message);
                            continue;
                        }
                        j++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - j);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }

        private void deleteFJ(string yuan)
        {
            string sql = @"delete from WFM_ATTACHLST att  where att.cname='{0}' and att.cid in (
select t.CID from WFM_ATTACHLST t 
INNER JOIN WFM_PROCESS PRO ON PRO.PID=T.PNODE
inner join WFM_MODEL ON WFM_MODEL.MID=PRO.MID
WHERE T.PTYPE='流程模版' AND T.CKIND='文件夹' AND PRO.PROCSTATE='启用' AND T.CNAME<>'流程附件' AND PRO.PROCTYPE IN ('土地使用权及房屋所有权登记','国有建设用地使用权及房屋所有权登记','土地使用权及项目内多幢房屋所有权登记')
 AND wfm_model.type not  like '%线上%'
  and  wfm_model.createtime > TO_DATE('20170911', 'yyyymmdd'))";
            sql = string.Format(sql, yuan);
            DbHelper db = new DbHelper();
            try
            {
                
                db.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                db.SetProvider(MyDBType.Oracle);
                db.ExecuteNonQuery(db.Conn, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                db.CloseConn();
            }

        }

        private void UpdateFJ(string yuan, string xin)
        {
            string sql = @"update WFM_ATTACHLST att set att.cname='{1}' where att.cname='{0}' and att.cid in (
select t.CID from WFM_ATTACHLST t 
INNER JOIN WFM_PROCESS PRO ON PRO.PID=T.PNODE
inner join WFM_MODEL ON WFM_MODEL.MID=PRO.MID
WHERE T.PTYPE='流程模版' AND T.CKIND='文件夹' AND PRO.PROCSTATE='启用' AND T.CNAME<>'流程附件' AND PRO.PROCTYPE IN ('土地使用权及房屋所有权登记','国有建设用地使用权及房屋所有权登记','土地使用权及项目内多幢房屋所有权登记')
 AND wfm_model.type not  like '%线上%'
  and  wfm_model.createtime > TO_DATE('20170911', 'yyyymmdd'))";
            sql = string.Format(sql, yuan, xin);
            DbHelper db = new DbHelper();
            try
            {
                db.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                db.SetProvider(MyDBType.Oracle);
                db.ExecuteNonQuery(db.Conn, System.Data.CommandType.Text, sql, null);
            }
            catch (Exception ex){ throw ex; }
            finally
            {
                db.CloseConn();
            }
        }

        private void Reinsert()
        {
            string errinfo = "";
            string str = "201710050366,201710050366-1,201710090024,201710090024-1,201710090063,201710090063-1,201710120326,201710120326-1,201710260009,201710260009-1";
            DataTable dt = GetSLBH();
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int j = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string busino = row["busino"].ToString();
                        if(str.Contains(busino))
                        {
                            continue;
                        }
                        Guid ArchiveId = new Guid(row["ArchiveId"].ToString());

                        string slbh = GetRealSLBH(busino);
                        //Guid ArchiveId2 = GetArchiveId(slbh);

                        deleteRecode(busino, ArchiveId);
                        //deleteRecode(slbh, ArchiveId2);





                        PageParams pg = new PageParams();
                        pg.PrjId = slbh;
                        FCDA_BLL.Insert_FCDA(pg);
                        j++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - j);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }

        private Guid GetArchiveId(string slbh)
        {
            string sql = @"select archiveid from archiveindex where busino ='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            object o= db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            return new Guid(o.ToString());
        }

        private  string GetRealSLBH(string slbh)
        {
            slbh = slbh.Replace("FC", "").Replace("O", "").Replace("OF", "").Replace("M", "").Replace("OCS", "").Replace("F", "").Replace("C", "").Replace("S", "");
            if (slbh.Contains("_"))
                slbh = slbh.Substring(0, slbh.IndexOf('_'));
            if (slbh.Contains("-"))
                slbh = slbh.Substring(0, slbh.IndexOf('-'));
            //if(slbh.Contains("-"))
            //{
            //    slbh = slbh.Substring(0, slbh.IndexOf('-'));
            //}
            return slbh;
        }

        private void deleteRecode(string busino, Guid archiveId)
        {
            string[] arr = new string[] { string.Format(@"DELETE FROM [dbo].ArchiveIndex where archiveid='{0}'",archiveId),
            //string.Format(@"DELETE FROM [dbo].HouseInfo WHERE BusiNo = '{0}'",busino),
             string.Format(@"DELETE FROM [dbo].HouseArchiveRelation WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].[Certificate] WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].PropArchiveRelation WHERE archiveid='{0}'", archiveId),
            string.Format(@"DELETE FROM [dbo].Person WHERE archiveid='{0}'", archiveId),
             string.Format(@"DELETE FROM [dbo].VolEleArc WHERE archiveid='{0}'", archiveId) };

            for (int i = 0; i < arr.Length; i++)
            {
                string sql = arr[i];
                DbHelper db = new DbHelper();
                db.SetProvider(MyDBType.Sql);
                db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            }


        }

        private void UPDATEOLDpERSON()
        {
            string errinfo = "";
            DataTable dt = GetSLBH();
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            string busino = row["busino"].ToString();
                            Guid ArchiveId = new Guid(row["ArchiveId"].ToString());

                            ArchiveIndex arch = new ArchiveIndex();
                            arch.ArchiveId = ArchiveId;
                            arch.BusiNO = busino;
                            List<Person> person_list = FCDA_BLL.GetPerson(arch, busino);//(busino, ArchiveId);

                            delete_Person(ArchiveId);
                            foreach (Person person in person_list)
                            {
                                try
                                {

                                    Insert_Person(person);
                                }
                                catch (Exception ex)
                                {
                                    errinfo += "Person:" + ex.Message;
                                    continue;
                                }
                            }
                            i++;
                            this.Invoke(new Action(() =>
                            {
                                textBox1.Text = "剩余" + (dt.Rows.Count - i);
                            }));
                        }
                        catch { continue; }

                    }
                    MessageBox.Show("OK" + "  " + errinfo);
                });
            }
        }

        private void delete_Person(Guid archiveId)
        {
            string sql = @"DELETE FROM [dbo].Person WHERE ArchiveID='{0}'";
            sql = string.Format(sql, archiveId);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }

        private  string GetNumber(string str)
        {
            Regex r = new Regex("\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(str);
            MatchCollection mc = r.Matches(str);

            string result = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            {
                result += mc[i];//匹配结果是完整的数字，此处可以不做拼接的
            }
            return result;
        }

        private  string ReplaceProp(string prop)
        {
            string qz = "B{0}S";
            string tx = "B{0}T";
            for (int i = 1; i < 6; i++)
            {


                string tmp1 = string.Format(qz, i);
                string tmp2 = string.Format(tx, i);
                prop = prop.Replace(tmp1, "").Replace(tmp2, "");
            }
            if (prop.Contains("-"))
                prop = prop.Substring(0, prop.IndexOf("-"));
            return prop;
        }

        private void UpdateIsHistroy()
        {
            string sql = @"select djgl.zslbh,djgl.fslbh,djgl.xgzh,qlrgl.qlrmc,qlrgl.zjhm from dj_qlrgl qlrgl
left join dj_xgdjgl djgl on djgl.fslbh = qlrgl.slbh
where DJGL.FSLBH IN(select SLBH from dj_djb where bdczh like 'B%S%') AND DJGL.ZSLBH LIKE '201710%'and djgl.xgzlx <> '土地证' and djgl.xgzlx not like '%抵押%'";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            DataTable dt= db.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string name = null == row["QLRMC"] ? "" : row["QLRMC"].ToString();
                    string zjhm = null == row["ZJHM"] ? "" : row["ZJHM"].ToString();
                    string xgzh = null == row["XGZH"] ? "" : row["XGZH"].ToString();

                    xgzh = ReplaceProp(xgzh);
                    DataTable ywzh = GetYwzh(xgzh, name, zjhm);
                    if (null != ywzh && ywzh.Rows.Count > 0)
                    {
                        foreach (DataRow item in ywzh.Rows)
                        {
                            UpdateFCHistroy(item["业务宗号"].ToString());
                        }
                    }
                    i++;
                    this.Invoke(new Action(() =>
                    {
                        textBox1.Text = "剩余" + (dt.Rows.Count - i);
                    }));
                }

                MessageBox.Show("OK");
            });

        }

        private void UpdateFCHistroy(string v)
        {
            string sql1 = "Update ArchiveIndex set IsHistoray='1' where BusiNO='{0}'";
            sql1 = string.Format(sql1, v);
            DbHelper db = new DbHelper();
            db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql1, null);
        }

        private DataTable GetYwzh(string xgzh, string name, string zjhm)
        {

            string sql = "SELECT [业务宗号] FROM [dbo].[vw_档案信息查询] where 1=1 ";
            sql += GetSelectWhere(xgzh, name, zjhm);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }

        private  string GetSelectWhere(string prop, string name, string zjhm)
        {
            string str = "";
            
            if (prop.Contains("不动产权"))
            {
                str += " and 不动产证号 like '%" + prop + "%' ";
            }
            else
            {
                
                str += " and 权证号 like '%" + prop + "%'";
            }
            //if(!string.IsNullOrEmpty(name))
            //{
            str += " and 姓名='" + name + "' ";
            //}
            if (!string.IsNullOrEmpty(zjhm))
            {
                string oldCardID = Get15CardNo(zjhm);
                str += " and (证件号码='" + zjhm + "' or 证件号码='" + oldCardID + "' )";
            }
            return str;
        }


        private  string Get15CardNo(string cardID)
        {
            if (cardID.Length == 18)
            {
                cardID = cardID.Substring(0, 6) + cardID.Substring(8, 9);
            }
            return cardID;
        }



        private void updatePerson(string str)
        {
            string errinfo = "";
           // DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str);
            DataTable dt = GetSLBH_ywzh(str);
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    
                        int j = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            
                                string busino = row["busino"].ToString();
                                Guid ArchiveId = new Guid(row["ArchiveId"].ToString());

                                ArchiveIndex arch = new ArchiveIndex();
                                arch.ArchiveId = ArchiveId;
                                arch.BusiNO = busino;
                                delete_Person(ArchiveId);
                                List<Person> person_list = FCDA_BLL.GetPerson(arch, busino);//(busino, ArchiveId);

                                foreach (Person person in person_list)
                                {
                                    try
                                    {
                                        Insert_Person(person);
                                    }
                                    catch (Exception ex)
                                    {
                                        errinfo += "Person:" + ex.Message;
                                        continue;
                                    }
                                }
                            }
                    
                    MessageBox.Show("OK"+"  "+errinfo);
                });

            }
        }

        public  int Insert_Person(Person person)
        {
            string sql = "insert into Person(PersonID,ArchiveId,Name,CardNO,PersonType,IDCardType,RightMan_ID,Sex) " +
                "values(@PersonID,@ArchiveId,@Name,@CardNO,@PersonType,@IDCardType,@RightMan_ID,@Sex)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@PersonID", person.PersonID);
            ListAdd(list, "@ArchiveId", person.ArchiveId);
            ListAdd(list, "@Name", person.Name);
            ListAdd(list, "@CardNO", person.CardNO);
            ListAdd(list, "@PersonType", person.PersonType);
            ListAdd(list, "@IDCardType", person.IDCardType);
            ListAdd(list, "@RightMan_ID", person.RightMan_ID);
            ListAdd(list, "@Sex", person.Sex);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
                return db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
            
        }

        //private List<Person> GetPerson(string busino,Guid ArchiveId)
        //{
        //    List<Person> list = new List<Person>();

        //    List<DJ_QLR> qlr_list = GetQlr(archiveIndex.BusiNO);

        //    List<DJ_QLR> qlrList = InitQLR(qlr_list);

        //    //List<DJ_QLR> ywr_list = GetYWR(archiveIndex.BusiNO);

        //    if (null != qlr_list && qlrList.Count > 0)
        //    {
        //        foreach (DJ_QLR qlr in qlrList)
        //        {
        //            Person person = new Person();
        //            person.ArchiveId = archiveIndex.ArchiveId;
        //            person.PersonID = CreateGuid(32);
        //            person.PersonType = GetPersonType_New(qlr);//GetPersonType(qlr.QLRID, archiveIndex.BusiNO);
        //            person.Name = qlr.QLRMC;
        //            person.CardNO = string.IsNullOrEmpty(qlr.ZJHM) ? "无" : qlr.ZJHM;
        //            person.IDCardType = GetIDTYPE(qlr.ZJLB);
        //            person.Sex = qlr.XB;
        //            list.Add(person);
        //        }
        //    }

        //    return list;
        //}

        public static List<DJ_QLR> GetQlr(string p)
        {
            List<DJ_QLR> list = BDCDA_DAL.GetQLR(p);
            return list;
        }

        private string GetIDTYPE(object zJLB)
        {
            switch (zJLB)
            {
                case "1":
                    return "01";
                case "2":
                    return "17";
                case "3":
                    return "06";
                case "4":
                    return "07";
                case "5":
                    return "02";
                case "6":
                    return "04";
                case "7":
                    return "03";
                case "8":
                    return "99";
                default:
                    return "99";
            }
        }

        private  string GetPersonType_New(string qlrlx,int sxh)
        {
            switch (qlrlx)
            {
                case "义务人":
                case "抵押人":
                    if (qlrlx == "义务人" && sxh > 1)
                        return "4";
                    return "2";
                case "权利人代理人":
                    return "5";
                default:
                    if (qlrlx == "权利人" && sxh > 1)
                        return "3";
                    return "1";
            }
        }

        private  Guid CreateGuid(int length)
        {
            //string str = Guid.NewGuid().ToString().Substring(3);
            //return new Guid("BDC" + str);
            return Guid.NewGuid();
        }


        private DataTable GetSLBH()
        {
            string sql = @"SELECT [ArchiveId],busino
  FROM[dbo].[ArchiveIndex]
  where busino like '201710110438%' and isOld = 2";

            //            string sql = @"select * from archiveindex where archiveid in (
            //select archiveid from person where persontype=1 group by archiveid having(count(*)>1)
            //) and busino like '201710%'
            //order by busino";
            //sql = string.Format(sql, slbh);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }

        private void CheckCnameInFC(string str)
        {
            DataTable dt = GetCNAMEFROMFC();
            //DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str);
            if(null!=dt && dt.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string cname = row["EleArcName"].ToString();
                        //if (!IsNotExist(cname))
                        //{
                        //    this.Invoke(new Action(() =>
                        //    {
                        //        textBox1.Text += cname + "\r\n";
                        //    }));

                        //}
                        //else
                        //{
                            int count = 0;
                            string ordinal = Getordinal(cname);
                            if(!ordinal.Equals("-1"))
                                count=Updateordinal(cname, ordinal);
                            this.Invoke(new Action(() =>
                            {
                                textBox1.Text += "更新 "+cname + ":"+count+"行"+"\r\n";
                            }));
                        //}
                    }
                    MessageBox.Show("OK");
                });
                
            }
        }

        private DataTable GetCNAMEFROMFC()
        {
            string sql = @"SELECT distinct
      [EleArcName]
  FROM[dbo].[VolEleArc]
  where archiveid in (SELECT[ArchiveId]
  FROM[dbo].[ArchiveIndex]
  where isOld = 2)";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            
        }

        private int Updateordinal(string cname,string ordinal)
        {
            int count = 0;
            DbHelper db = new DbHelper();
            try
            {
                string sql = @"update [dbo].[VolEleArc] set ordinal={0} where [EleArcName]='{1}' and [EleArcCode] is null";
                sql = string.Format(sql, ordinal, cname);
               
                db.SetProvider(MyDBType.Sql);
                count= db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                
            }
            catch { }
            finally
            {
                db.CloseConn();
            }
            return count;
        }

        private string Getordinal(string cname)
        {
            DbHelper db = new DbHelper();
            try
            {
                string sql = @"select t2.ArcVolTypeNO+t1.ReceiveVolNO from ahms.dbo.ReceiveVol t1 
inner join ahms.dbo.ArcVolType t2 on t1.ArcVolTypeID = t2.ArcVolTypeID
where t1.ReceiveVolName like '%{0}%'";
                sql = string.Format(sql, cname);
                db.SetProvider(MyDBType.Sql);
                object o = db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (null != o)
                {
                    return o.ToString();
                }
                
            }
            catch { }
            finally
            {

                db.CloseConn();
                
            }
            return "2001";
        }

        private bool IsNotExist(string cname)
        {
            string sql = @"SELECT count(1)
  FROM [dbo].[ReceiveVol]
  where [ReceiveVolName] = '{0}'";
            sql = string.Format(sql,cname);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            object o= db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            if(null!=o)
            {
                int count = Convert.ToInt32(o);
                if (count > 0)
                    return true;
            }
            return false;
        }

        private void UpdateFC()
        {
            DataTable dt = new DataTable();//GetSLBH();
            if(null!=dt && dt.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string busino = row["busino"].ToString();
                        Guid ArchiveId = new Guid(row["ArchiveId"].ToString());
                        string slbh = busino.Substring(0, busino.IndexOf('-'));
                        List<VolEleArc> volEleArc = GetVolEleArc(ArchiveId, slbh);
                        foreach (VolEleArc item in volEleArc)
                        {
                            try
                            {
                                Insert_VolEleArc(item);
                            }
                            catch (Exception ex)
                            {
                                string str = "VolEleArc:" + ex.Message;
                                continue;
                            }
                        }
                        i++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - i);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }
        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new SqlParameter(paraName, DBNull.Value));
            else
                list.Add(new SqlParameter(paraName, value));
        }

        public static int Insert_VolEleArc(VolEleArc volEleArc)
        {
            string sql = "insert into VolEleArc(EleArcVol_ID,ArchiveId,Ordinal,EleArcName,IsShow) values(@EleArcVol_ID,@ArchiveId,@Ordinal,@EleArcName,@IsShow)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@EleArcVol_ID", volEleArc.EleArcVol_ID);
            ListAdd(list, "@ArchiveId", volEleArc.ArchiveId);
            ListAdd(list, "@Ordinal", volEleArc.Ordinal);
            ListAdd(list, "@EleArcName", volEleArc.EleArcName);
            ListAdd(list, "@IsShow", volEleArc.IsShow);

            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
            
        }

        private List<VolEleArc> GetVolEleArc(Guid archiveIndex, string prjId)
        {
            List<VolEleArc> list = new List<VolEleArc>();

            List<WFM_ATTACHLST> wfm_ATTACHLST_list = GetWFM_ATTACHLST_list(prjId);

            if (null != wfm_ATTACHLST_list && wfm_ATTACHLST_list.Count > 0)
            {

                foreach (WFM_ATTACHLST doc in wfm_ATTACHLST_list)
                {
                    if (doc.CNAME == "流程附件")
                    {
                        continue;
                    }
                    VolEleArc volEleArc = new VolEleArc();
                    volEleArc.ArchiveId = archiveIndex;//archiveIndex.ArchiveId;
                    volEleArc.EleArcVol_ID = Guid.NewGuid();
                    volEleArc.EleArcName = doc.CNAME;
                    volEleArc.PageNumber = Convert.ToInt32(doc.FILENUM);
                    volEleArc.IsShow = "1";
                    volEleArc.Ordinal = Convert.ToInt32(doc.CSORT);
                    list.Add(volEleArc);
                }
            }

            return list;
        }

        private List<WFM_ATTACHLST> GetWFM_ATTACHLST_list(string slbh)
        {
            List<WFM_ATTACHLST> list= BDCDA_DAL.GetWFM_ATTACHLST_SelectInfo(slbh);
            if (null != list && list.Count > 0)
                return list;
            return BDCDA_DAL.GetWFM_ATTACHLST_AllInfo(slbh);
        }

        private DataTable GetSLBH_ywzh(string str)
        {
            //string sql = @"SELECT [ArchiveId],busino
            //FROM[dbo].[ArchiveIndex]
            //where busino = '"+str+"' and isOld = 2";

            string sql = @"select  [ArchiveId],busino from archiveindex where archiveid in ( select archiveid from person where archiveid not in (select archiveid from person where personType=1)) and isold=2";
            //string sql = "select * from archiveindex where archiveid not in(select distinct archiveid from [dbo].[VolEleArc]) and busino like '201710%'";
            //string sql = string.Format(@"select [ArchiveId],busino from archiveindex where archiveid not in (select archiveid from person) and busino = '{0}'",str);
            //string sql = @"select [ArchiveId],busino from archiveindex where busino in (select busino from archiveindex where busino like '201710%' group by busino having(count(1)>1))";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }

        private void BDCRePushBySlbh(string slbh)
        {
            string userName = GetUserName(slbh);
            string userCode = GetUserCode(userName);
            PageParams param = new Web4BDC.Models.PageParams();
            param.PrjId = slbh;
            param.UserId = userCode;
            BDCFilterResult res = FCDA_BLL.Insert_FCDA(param);
        }

        private void UpdateCTSJ(string str)
        {
            DataTable totalTable = DataTableRenderToExcel.RenderDataTableFromExcel(str); //GetAllPushDAData();
            if (null != totalTable && totalTable.Rows.Count > 0)
            {
                int ThreadCount = 1;
                int mod = totalTable.Rows.Count / ThreadCount;
                count = totalTable.Rows.Count;

                ds = SplitDataTable(totalTable, mod);


                List<Thread> T_List = new List<Thread>();
                for (int i = 0; i < ThreadCount; i++)
                {
                    index = i;
                    Thread t = new Thread(new ThreadStart(() => {
                        if (index < ThreadCount)
                        {
                            GXJD(ds.Tables[index]);
                        }
                    }));
                    T_List.Add(t);

                }

                for (int j = 0; j < ThreadCount; j++)
                {
                    index = j;
                    T_List[j].Start();
                }
            }
        }

        

        private void GXJD(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                ArchiveIndex index = ModelHelper<ArchiveIndex>.FillModel(row);
                index.ArchiveId = new Guid(row["ArchiveId"].ToString());
                string needUpdateID = FCDA_DAL.GetArchive(index.BusiNO);
                if (!index.ArchiveId.ToString().ToUpper().Equals(needUpdateID.ToUpper()) && !string.IsNullOrEmpty(needUpdateID))
                {
                    UpDateGXJD(index, needUpdateID);
                    
                }
               // lock (lockKey)
               // {
                    count = count - 1;
               // }

                this.Invoke(new Action(() =>
                {
                    textBox1.Text = "剩余" + count;
                }));
            }
        }

        private void UpDateGXJD(ArchiveIndex index, string needUpdateID)
        {
            FCDA_DAL.UpdateGXJD(index, needUpdateID);
        }

        private void BDCRepush(string str)
        {
            DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str); //GetAllPushDAData();
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            int sqlcount = 0;
                            string slbh = row["PRJID"].ToString().Trim();
                            if (slbh.Contains("-"))
                            {
                                slbh = slbh.Substring(0, slbh.IndexOf('-'));
                            }
                            //string sql = "select count(1) from [dbo].[ArchiveIndex] where busino like '{0}%' ";// and isOld='2'";
                            //sql = string.Format(sql, slbh);
                            //DbHelper sqldb = new DbHelper();
                            //sqldb.SetProvider(MyDBType.Sql);
                            //object o = sqldb.ExecuteScalar(MyDBType.Sql, CommandType.Text, sql, null);
                            //if (null != o)
                            //{
                            //    sqlcount = Convert.ToInt32(o);
                            //}
                            //else
                            //{
                            //    sqlcount = 0;
                            //}

                            //if(sqlcount>0)
                            //{
                            //    MyDeleteRecode(slbh);

                               
                            //}

                            ReRecode(slbh);
                            //else
                            //{
                            //    this.Invoke(new Action(() =>
                            //    {
                            //        this.listBox1.Items.Add(slbh + "--无此受理编号");
                            //        this.listBox1.Items.Add("--------------------------------");
                            //        this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                            //    }));
                            //}



                            //List<ArchiveIndex> list = FCDA_BLL.GetArchiveIndex(slbh);

                            //if (null != list && list.Count > 0)
                            //{
                            //    if (list.Count > sqlcount)
                            //    {

                            //        MyDeleteRecode(slbh);

                            //        ReRecode(slbh);

                            //    }
                            //}
                            //else
                            //{
                            //    this.Invoke(new Action(() =>
                            //    {
                            //        this.listBox1.Items.Add(slbh + "--无业务");
                            //        this.listBox1.Items.Add("--------------------------------");
                            //        this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                            //    }));
                            //}
                            i++;
                            this.Invoke(new Action(() =>
                            {
                                this.listBox1.Items.Add("剩余" + (dt.Rows.Count - i));
                                this.listBox1.Items.Add("--------------------------------");
                                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                                
                            }));


                        }
                        catch
                        {
                            continue;
                        }

                    }

                    MessageBox.Show("OK");

                });

            }
        }

        private string GetUserCode(string userName)
        {
            string sql = "select usercode from SEC_EMPLOYEE t where usercode='{0}' or name='{0}'";
            sql = string.Format(sql, userName);
            object o = WorkflowMonitorXZFCPlug.Dal.DBHelper.GetScalar(sql, ConnectType.GGK);
            if(null!=o)
            {
                return o.ToString();
            }
            return "guidangren";

        }

        private string GetUserName(string slbh)
        {
            string userName = "";
            string sql = "select fjr from dj_fjd where slbh='{0}'";
            sql = string.Format(sql,slbh);
            object o=WorkflowMonitorXZFCPlug.Dal.DBHelper.GetScalar(sql, ConnectType.SXK);
            if(null!=o)
            {
                userName = o.ToString();
            }
            else
            {
                sql = @"select dbr as name
  from dj_djb
 where slbh = '{0}'
union
select dbr as name
  from dj_yg
 where slbh = '{0}'
union
select dbr as name
  from dj_dy
 where slbh = '{0}'
union
select dbr as name
  from dj_yy
 where slbh = '{0}'
 union
select dbr as name
  from dj_cf
 where slbh = '{0}' 
union
select dbr as name
  from dj_xgdjzx
 where slbh = '{0}'"
;
            
                sql = string.Format(sql, slbh);
                object od = WorkflowMonitorXZFCPlug.Dal.DBHelper.GetScalar(sql, ConnectType.SXK);
                if (null != od)
                {
                    userName = od.ToString();
                }
                else
                {
                    userName = "";
                }
            }
            return userName;
        }

        private DataSet GetPushData(string viewName)
        {
            string sql = string.Empty;
            sql = string.Format("select * from {0}",viewName);

            return WorkflowMonitorXZFCPlug.Dal.DBHelper.GetDataSet(sql, ConnectType.SXK);
        }

        private DataTable GetRePushData()
        {
            string sql = "select * from fc_da_tag where issuccess=0 order  by pushdate desc";
            return WorkflowMonitorXZFCPlug.Dal.DBHelper.GetDataTable(sql, ConnectType.SXK);
        }

        private DataTable GetAllPushDAData()
        {
            string sql = "select * from fc_da_tag where pushuser is null or pushuser='guidangren' order   by pushdate desc";
            return WorkflowMonitorXZFCPlug.Dal.DBHelper.GetDataTable(sql, ConnectType.SXK);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void rdbDY_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rab = (RadioButton)sender;
            if (rab.Checked)
            {
                string[] pars = rab.Tag.ToString().Split('|');
                viewName = pars[0];
                address = pars[1];
            }
        }

        /// <summary>
        /// 分解数据表
        /// </summary>
        /// <param name="originalTab">需要分解的表</param>
        /// <param name="rowsNum">每个表包含的数据量</param>
        /// <returns></returns>
        public DataSet SplitDataTable(DataTable originalTab, int rowsNum)
        {
            //获取所需创建的表数量
            int tableNum = originalTab.Rows.Count / rowsNum;

            //获取数据余数
            int remainder = originalTab.Rows.Count % rowsNum;

            DataSet ds = new DataSet();

            //如果只需要创建1个表，直接将原始表存入DataSet
            if (tableNum == 0)
            {
                ds.Tables.Add(originalTab);
            }
            else
            {
                DataTable[] tableSlice = new DataTable[tableNum];

                //Save orginal columns into new table.            
                for (int c = 0; c < tableNum; c++)
                {
                    tableSlice[c] = new DataTable();
                    foreach (DataColumn dc in originalTab.Columns)
                    {
                        tableSlice[c].Columns.Add(dc.ColumnName, dc.DataType);
                    }
                }
                //Import Rows
                for (int i = 0; i < tableNum; i++)
                {
                    // if the current table is not the last one
                    if (i != tableNum - 1)
                    {
                        for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                        {
                            tableSlice[i].ImportRow(originalTab.Rows[j]);
                        }
                    }
                    else
                    {
                        for (int k = i * rowsNum; k < ((i + 1) * rowsNum + remainder); k++)
                        {
                            tableSlice[i].ImportRow(originalTab.Rows[k]);
                        }
                    }
                }



                //add all tables into a dataset                
                foreach (DataTable dt in tableSlice)
                {
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        private void radDA_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnLL_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(DialogResult.OK==ofd.ShowDialog())
            {
                txtPath.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private DataTable GetDeleteInfo(string slbh)
        {
            string sql = @"SELECT [ArchiveId],busino
  FROM [dbo].[ArchiveIndex]
  where busino like '{0}%'  and ([SerialNo] is null or [SerialNo]='')";//and isOld = 2;
            sql = string.Format(sql, slbh);

            //            string sql = @"select * from archiveindex where archiveid in (
            //select archiveid from person where persontype=1 group by archiveid having(count(*)>1)
            //) and busino like '201710%'
            //order by busino";
            //sql = string.Format(sql, slbh);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }
       


        private void MyDeleteRecode(string slbh)
        {
            DataTable dt = GetDeleteInfo(slbh);
            if (null != dt && dt.Rows.Count > 0)
            {
                    foreach (DataRow row in dt.Rows)
                    {
                        string busino = row["busino"].ToString();
                       
                        Guid ArchiveId = new Guid(row["ArchiveId"].ToString());

                        

                        deleteRecode(busino, ArchiveId);
                        
                    }
                  
               
            }
        }

        private void MyDeleteRecode(string slbh,string actid)
        {
            if (!string.IsNullOrEmpty(actid) && !string.IsNullOrEmpty(slbh))
            {
                Guid ArchiveId = new Guid(actid);

                deleteRecode(slbh, ArchiveId);
            }
            else
            {
                DataTable dt = GetDeleteInfo(slbh);
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string busino = row["busino"].ToString();

                        Guid ArchiveId = new Guid(row["ArchiveId"].ToString());



                        deleteRecode(busino, ArchiveId);

                    }


                }
            }
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string slbh = txtSLBH.Text.Trim();

            if (IsNumeric(slbh) || slbh.Contains("-"))
            {

                //MyDeleteRecode(slbh);

                ReRecode(slbh);
                MessageBox.Show("OK");
            }
            else
            {
                //UpdateCTSJ(slbh);
                BDCRepush(slbh);
            }
            
        }

        private void ReRecode(string slbh)
        {

            PageParams pg = new PageParams();
            pg.PrjId = slbh;
            //pg.UserId = FCDA_BLL.GetUserID(pg.PrjId.Trim());
           
            FCDA_BLL.Insert_FCDA(pg);
        }
        object lockKey = new object();
        int count = 0;
        private void ReInsert_ByThread(DataTable dt)
        {
            int t = 0;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    int sqlcount = 0;
                    string slbh = row["slbh"].ToString();
                    if (slbh.Contains("-"))
                    {
                        slbh = slbh.Substring(0, slbh.IndexOf('-'));
                    }
                    string sql = "select count(1) from [dbo].[ArchiveIndex] where busino like '{0}%' and isOld='2'";
                    sql = string.Format(sql, slbh);
                    DbHelper sqldb = new DbHelper();
                    sqldb.SetProvider(MyDBType.Sql);
                    object o = sqldb.ExecuteScalar(MyDBType.Sql, CommandType.Text, sql, null);
                    if (null != o)
                    {
                        sqlcount = Convert.ToInt32(o);
                    }
                    else
                    {
                        sqlcount = 0;
                    }
                    List<ArchiveIndex> list = FCDA_BLL.GetArchiveIndex(slbh);

                    if (null != list && list.Count > 0)
                    {
                        if (list.Count > sqlcount)
                        {

                            MyDeleteRecode(slbh);

                            ReRecode(slbh);

                            //if (!list[0].ArchiveType.Equals("Z"))
                            //{
                            //    this.Invoke(new Action(() =>
                            //    {
                            //        this.listBox1.Items.Add(slbh + "--无号");
                            //        this.listBox1.Items.Add("--------------------------------");
                            //        this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                            //    }));
                            //}
                        }
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.listBox1.Items.Add(slbh + "--无业务");
                            this.listBox1.Items.Add("--------------------------------");
                            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                        }));
                    }
                    lock (lockKey)
                    {
                        count = count - 1;
                    }

                    this.Invoke(new Action(() =>
                    {
                        textBox1.Text = "剩余" + count;
                    }));


                }
                catch
                {
                    continue;
                }

            }

           
        }

        DataSet ds = null;
        int index = 0;

        private void button1_Click_1(object sender, EventArgs e)
        {

            int ThreadCount = 1;
            DataTable totalTable = GetALLTable();
            int mod = totalTable.Rows.Count / ThreadCount;
            count = totalTable.Rows.Count;
           
            ds = SplitDataTable(totalTable, mod);
            

            List<Thread> T_List = new List<Thread>();
            for (int i = 0; i < ThreadCount; i++)
            {
                index = i;
                Thread t = new Thread(new ThreadStart(() => {
                    if (index < ThreadCount)
                    {
                        ReInsert_ByThread(ds.Tables[index]);
                    }
                }));
                T_List.Add(t);
                
            }

            for (int j = 0; j < ThreadCount; j++)
            {
                index = j;
                T_List[j].Start();
            }


            //List<Task> taskList = new List<Task>();

            //for (int i = 0; i < ThreadCount; i++)
            //{
            //    //Task t = new Task(() => { ReInsert_ByThread(ds.Tables[i]); });
            //    //taskList.Add(t);

            //    taskList.Add(Task.Factory.StartNew(() =>
            //   {
            //       ReInsert_ByThread(ds.Tables[i]);
            //   }));
            //    Thread.Sleep(1000);

            //}

           
            //TaskFactory taskFactory = new TaskFactory();
            //taskList.Add(taskFactory.ContinueWhenAll(taskList.ToArray(), tArray =>
            //{ Thread.Sleep(200); MessageBox.Show("OK"); }));

           

        }

        private DataTable GetALLTable()
        {
            string or_sql = "select a.[busino] as slbh from [dbo].[ArchiveIndex_bak_seven] as a where a.[Busino] not in ( select busino from [dbo].[ArchiveIndex] as b)  order by busino";
            //string or_sql = "select slbh from fc_da_tag order by slbh";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            DataTable dt = db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, or_sql, null);
            return dt;
        }

        private void ChongHao()
        {
            string sql = "select busino from [dbo].[ArchiveIndex] where isold=2  group by busino having(count(busino)>1)";
            //string sql = "select busino from [dbo].[ArchiveIndex] where [ArchiveId] in ( select [ArchiveId] from [dbo].[Person] where persontype= 1 group by [ArchiveId] having(count(1)>1)) and isold=2";
            DbHelper sqldb = new DbHelper();
            sqldb.SetProvider(MyDBType.Sql);
            DataTable dt = sqldb.ExecuteTable(MyDBType.Sql, CommandType.Text, sql, null);
            if(null!=dt && dt.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string slbh = row["busino"].ToString();
                        MyDeleteRecode(slbh);

                        ReRecode(slbh);
                        i++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - i);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChongHao();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "select slbh from ARCH_GLDAXX t where GDR IS NULL AND slbh not like '%-%'";
            //string sql = "select busino from [dbo].[ArchiveIndex] where [ArchiveId] in ( select [ArchiveId] from [dbo].[Person] where persontype= 1 group by [ArchiveId] having(count(1)>1)) and isold=2";
            DbHelper sqldb = new DbHelper();
                
            sqldb.Conn = new Oracle.DataAccess.Client.OracleConnection(DAK);
            sqldb.SetProvider(MyDBType.Oracle);
            DataTable dt = sqldb.ExecuteTable(sqldb.Conn, CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string slbh = row["slbh"].ToString();
                        MyDeleteRecode(slbh);

                        ReRecode(slbh);
                        i++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - i);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }

        private void txtSLBH_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(DialogResult.OK==ofd.ShowDialog())
            {
                txtSLBH.Text = ofd.FileName;
            }
        }

        /// <summary>
        /// 幢整理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            DataTable dt = GetZtstybm();
            if(null!=dt && dt.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        string tstybm = row["tsbm"].ToString();
                        List<FC_Z_QSDC> zList = GetZ_By_tstybm(tstybm);
                        FC_Z_QSDC mainZ = zList.Find(p => p.TSTYBM.Equals(p.TSTYBM.ToUpper()));
                        List<FC_Z_QSDC> childZ = zList.FindAll(p => p.TSTYBM.Equals(p.TSTYBM.ToLower()));
                        foreach (FC_Z_QSDC z in childZ)
                        {
                            foreach (PropertyInfo propertyInfo in typeof(FC_Z_QSDC).GetProperties())
                            {
                                try
                                {
                                    object mainV = propertyInfo.GetValue(mainZ, null);
                                    object childV = propertyInfo.GetValue(z, null);
                                    if (null == mainV && null != childV)
                                    {
                                        propertyInfo.SetValue(mainZ, childV, null);
                                    }

                                }
                                catch { continue; }
                            }
                            DeleteZ(z);
                        }
                        UpdateZ(mainZ);
                        i++;
                        this.Invoke(new Action(() =>
                        {
                            textBox1.Text = "剩余" + (dt.Rows.Count - i);
                        }));
                    }
                    MessageBox.Show("OK");
                });
            }
        }

        private void UpdateZ(FC_Z_QSDC mainZ)
        {
            DbHelper db = new DbHelper();
            string sql = db.CreateUpdateStr<FC_Z_QSDC>(mainZ, "FC_Z_QSDC", "tstybm='" + mainZ.TSTYBM + "'", MyDBType.Oracle);
            sql = sql.Trim().Replace("\r\n", "");
            DbParameter[] param = db.GetParamArray<FC_Z_QSDC>(mainZ, MyDBType.Oracle);
            db.SetProvider(MyDBType.Oracle);
            db.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, param);
        }

        private void DeleteZ(FC_Z_QSDC z)
        {
            string sql = "delete from fc_z_qsdc where tstybm='{0}'";
            sql = string.Format(sql, z.TSTYBM);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            db.ExecuteNonQuery(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private List<FC_Z_QSDC> GetZ_By_tstybm(string tstybm)
        {
            string sql = @"select * from fc_z_qsdc where lower(tstybm)='{0}'";
            sql = string.Format(sql, tstybm);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            DataTable dt = db.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            return ModelHelper<FC_Z_QSDC>.FillModel(dt);
        }

        private DataTable GetZtstybm()
        {
            string sql = @"SELECT
lower(tstybm) tsbm 
FROM
fc_z_qsdc
WHERE
tstybm IS NOT NULL
GROUP BY
lower(tstybm)
HAVING
COUNT (*) >1";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            DataTable dt = db.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            return dt;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //BDCSrv.BDCSrvSoapClient client = new BDCSrv.BDCSrvSoapClient();
            //BDCSrv.BDC bdc = client.CLF_FC_CLMMHT("CQZH=0272715")
        }

        private void txtSLBH_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string str = txtSLBH.Text.Trim();

            if (IsNumeric(str))
            {
                PageParams param = new Web4BDC.Models.PageParams();
                param.PrjId = str;
                //param.UserId = FCDA_BLL.GetUserID(p.PrjId.Trim());//"guidangren";
                BDCFilterResult res = FCDA_BLL.Insert_ARCH(param);
                if (res.IsSuccess)
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show(res.Message);
                }
            }
            else
            {

                DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str); //GetAllPushDAData();
                if (null != dt && dt.Rows.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(delegate (object obj)
                    {
                        int i = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                string slbh = row["slbh"].ToString().Trim();
                                PageParams param = new Web4BDC.Models.PageParams();
                                param.PrjId = slbh.Trim();
                                param.UserId = FCDA_BLL.GetUserID(param.PrjId.Trim());//"guidangren";
                            BDCFilterResult res = FCDA_BLL.Insert_ARCH(param);


                                i++;
                                this.Invoke(new Action(() =>
                                {
                                    textBox1.Text = "剩余" + (dt.Rows.Count - i);
                                }));


                            }
                            catch
                            {
                                continue;
                            }

                        }

                        MessageBox.Show("OK");

                    });

                }
            }


            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ArchiveIndex a = new ArchiveIndex();
            a.ArchiveId = new Guid(txtArchiveId.Text.Trim());
            a.BusiNO = txtSLBH.Text.Trim();
            List<Person> list= FCDA_BLL.GetPerson(a, a.BusiNO);
            foreach (Person item in list)
            {
                if (!Youren(item.Name, item.CardNO, a.ArchiveId.ToString()))
                {
                    FCDA_DAL.Insert_Person(item);
                }
            }
            
        }

        private bool Youren(string name,string hm,string archID)
        {
            string sql = "select count(1) from [dbo].[Person] where [Name]='{0}' and [CardNO]='{1}' and [ArchiveId]='{2}'";
            sql = string.Format(sql, name, hm, archID);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            object o = db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            if (null != o)
            {
                return Convert.ToInt32(o) > 0 ? true : false;
            }
            return false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DataTable dt= HOUSE();
            UpdateHouseSit(dt);
            //deleteRecode(txtSLBH.Text.Trim(), new Guid(txtArchiveId.Text.Trim()));
            //MessageBox.Show("OK");
        }

        private DataTable HOUSE()
        {
            string sql = @" select h.[HouseInfo_ID],h.[HoSite] from [dbo].[HouseInfo] as h 
  left join [dbo].[HouseArchiveRelation] as hr on hr.[HouseInfo_ID]=h.[HouseInfo_ID]
  left join [dbo].[ArchiveIndex] as ai on ai.[ArchiveId]=hr.[ArchiveId]
  where ai.[IsOld]='2' and ai.[ArchiveType]='C'";

  //          string sql = @" select h.[HouseInfo_ID],h.[HoSite] from  [HouseInfo] as h
  //where h.[HoSite] in (select[HoSite] from [HouseInfo] where CHID is not null and busino like '201%' group by[HoSite] having(count([HoSite])>1))
  //and CHID is not null
  //order by h.busino";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            DataTable dt = db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            if(null!=dt&& dt.Rows.Count>0)
            {
                return dt;
            }
            return null;

        }

        private DataTable houseOrcle()
        {
            string sql = @" select tstybm,zl from fc_h_qsdc where tstybm in(

select tstybm
  from dj_tsgl
 where bdclx = '房屋'
   and djzl = '权属'
   and slbh in (select slbh
                  from dj_tsgl
                 where slbh like '201%'
                 group by slbh
                having(count(slbh) > 1))
                )";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            DataTable dt = db.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != dt && dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }


        DataTable colnedt = null;
        private void UpdateHouseSit(DataTable dt)
        {
            colnedt = dt.Clone();
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                int count = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    string hid = row[0].ToString();
                    string zl = row[1].ToString();
                    object o = GetZL(hid);
                    if (null != o)
                    {
                        if (!zl.Equals(o.ToString()))
                        {
                            string zltmp = zl.Replace(" ", "").Replace("#", "");

                            string otmp = o.ToString().Replace("号楼", "-").Replace(" ", "");
                            if (!zltmp.Equals(otmp.ToString()))
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.listBox1.Items.Add(zl + " :  " + o);
                                    this.listBox1.Items.Add("--------------------------------");
                                }));
                                DataRow colnerow = colnedt.NewRow();
                                colnerow[0] = hid;
                                colnerow[1] = o.ToString();
                                colnedt.Rows.Add(colnerow);

                                // UpdateFCH(hid, zl);
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        label4.Text = "剩余:" + ( --count);
                    }));
                }
                MessageBox.Show("OK");
            });
        }

        private static object GetZL(string hid)
        {
            object o = GetSJDZL(hid);
            if(null!=o)
            {
                return o;
            }
            return GetHZLFrmBDC(hid);
        }

        private static object GetHZLFrmBDC(string hid)
        {
            string sql = string.Format("select zl from fc_h_qsdc where (tstybm='{0}' or tstybm='{1}') or (OLDCGHOUSEID='{0}' or OLDCGHOUSEID='{1}')", hid, hid.ToUpper());
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            object o = db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            return o;
        }

        private static object GetSJDZL(string hid)
        {
            string sql = string.Format(@"select distinct sjd.zl from dj_sjd sjd
left join dj_tsgl gl on gl.slbh = sjd.slbh
left join fc_h_qsdc h on h.tstybm = gl.tstybm
where  h.tstybm='{0}' and sjd.djdl in ('200', '100', '700') and sjd.slbh like '201%'", hid.ToUpper());
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            object o = db.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if(null!=o)
            {
                return o;
            }
            else
            {
                sql = string.Format(@"select distinct sjd.zl from dj_sjd sjd
left join dj_tsgl gl on gl.slbh = sjd.slbh
left join fc_h_qsdc h on h.tstybm = gl.tstybm
where   h.OLDCGHOUSEID='{0}' and sjd.djdl in ('200', '100', '700') and sjd.slbh like '201%'", hid.ToUpper());
               // DbHelper db = new DbHelper();
                db.SetProvider(MyDBType.Oracle);
                 return db.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            }
        }

        private static object GetHZLFrmFC(string hid)
        {
            string sql = string.Format("select [HoSite] from [dbo].[HouseInfo] where [HouseInfo_ID]='{0}'",hid);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            object o = db.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            return o;
        }

        private int UpdateFCH(string hid, string zl)
        {
            string sql = @"update [dbo].[HouseInfo] set [I_ItSite]='{1}',[HoSite]='{1}' where [HouseInfo_ID]='{0}'";
            sql = string.Format(sql, hid, zl);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                int count = colnedt.Rows.Count;
                if (null != colnedt && colnedt.Rows.Count > 0)
                {
                    foreach (DataRow row in colnedt.Rows)
                    {
                        string hid = row[0].ToString();
                        string zl = row[1].ToString();
                        int i = UpdateFCH(hid, zl);
                        this.Invoke(new Action(() =>
                        {
                            this.listBox1.Items.Add("影响" + i + "行");
                            this.listBox1.Items.Add("--------------------------------");
                            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                            this.label4.Text = "剩余" + (--count) + "条";
                        }));
                    }
                    MessageBox.Show("OK");
                }
            });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ArchiveIndex a = new ArchiveIndex();
            a.ArchiveId = new Guid(txtArchiveId.Text.Trim());
            a.BusiNO = txtSLBH.Text.Trim();
            deleteRecode(a.BusiNO,a.ArchiveId);
            MessageBox.Show("OK");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DataTable dt = GetHBUSINO();
            foreach (DataRow row in dt.Rows)
            {
                string busino = row["BUSINO"].ToString();
                DataTable mjTable = GetHMJ(busino);
                if (null != mjTable && mjTable.Rows.Count>0)
                {
                    foreach (DataRow hrow in mjTable.Rows)
                    {
                        decimal jzmj = hrow["jzmj"] == DBNull.Value ? 0 : Convert.ToDecimal(hrow["jzmj"]);
                        decimal ycjzmj = hrow["ycjzmj"] == DBNull.Value ? 0 : Convert.ToDecimal(hrow["ycjzmj"]);
                        string hid = hrow["tstybm"].ToString().ToUpper();
                        if (jzmj == 0 && ycjzmj>0)
                        {
                            UpdateHINFO(ycjzmj, busino);
                        }
                        if(jzmj > 0)
                        {
                            UpdateHINFO(jzmj, busino);
                        }
                    }
                    
                }
            }
            MessageBox.Show("OK");
        }

        private void UpdateHINFO(decimal ycjzmj, string busino)
        {
            string sql = "update [dbo].[HouseInfo] set [h_conacre]='{1}' where [busino]='{0}'";
            sql = string.Format(sql, busino, ycjzmj);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }

        private DataTable GetHMJ(object v)
        {
            string sql = @"select h.tstybm, round( h.jzmj,2) jzmj,round( h.ycjzmj,2) ycjzmj  from fc_h_qsdc h
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
where tsgl.slbh='{0}'";
            sql = string.Format(sql, v);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Oracle);
            return db.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private DataTable GetHBUSINO()
        {
            string sql = @"SELECT BUSINO FROM [dbo].[HouseInfo] where (h_conacre is null OR h_conacre=0) and busino>'201710010000' and busino not like 'B%' 
  order by busino ";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DataTable dt = GetAllZHbdc();//GetALLZH();
            int count = dt.Rows.Count;
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string res = CheckDiff(row);
                    if (!res.Equals(""))
                    {
                        this.Invoke(new Action(() =>
                        {
                            
                            this.listBox3.Items.Add(res);
                            this.listBox3.Items.Add("--------------------------------");
                            this.listBox3.SelectedIndex= this.listBox3.Items.Count - 1;

                            this.label6.Text = "总计:" + this.listBox3.Items.Count/2 + "条";
                        }));
                    }
                    --count;
                    this.Invoke(new Action(() =>
                    {

                        label5.Text = "剩余" + count + "条";
                    }));
                }
                MessageBox.Show("OK");
            });
        }

        private string CheckDiff(DataRow row)
        {
            string slbh = row["SLBH"] == null ? "" : row["SLBH"].ToString();
            string bdczh = row["bdczh"] == null ? "" : row["bdczh"].ToString();
            string lifecycle = "";
            if (null == row["lifecycle"] || DBNull.Value == row["lifecycle"])
            {
                lifecycle = "0";
            }
            else
            {
                lifecycle = row["lifecycle"].ToString();
            }

            string realSLBH = GetRealSLBH(slbh);
            DataTable dt = GetZT_fc(bdczh,realSLBH);
            if (null != dt && dt.Rows.Count > 0)
            {
                
                string ywzh = dt.Rows[0]["业务宗号"] == null ? "" : dt.Rows[0]["业务宗号"].ToString();
                string fczt = dt.Rows[0]["档案状态"] == null ? "" : dt.Rows[0]["档案状态"].ToString();
                //switch(fczt)
                //{
                //    case "历史":
                //        fczt = "1";
                //        break;
                //    case "现实":
                //        fczt = "0";
                //        break;
                //}
                //string zh = bdc_zh == "" ? qzh : bdc_zh;
                if (!fczt.Equals(lifecycle))
                {
                    UpdateFCZT(ywzh, lifecycle);
                    return "房产：" + ywzh + "|"  + fczt + "  不动产:" + slbh + "|" + bdczh + "|" + lifecycle;
                }

            }
            return "";
        }

        private DataTable GetZT_fc(string zh,string realSLBH)
        {
//            string sql1 = @"  SELECT [姓名]
//      ,[证件号码]
//      ,[房屋座落]
//      ,[建筑面积]
//      ,[房屋用途]
//      ,[档案状态]
//      ,[业务宗号]
//      ,[权证号]
//      ,[不动产证号]
//      ,[日期]
//  FROM [AHMS].[dbo].[vw_档案信息查询]
//where [不动产证号]='{0}' and [档案状态]='历史'";






//            string sql2 = @"SELECT [姓名]
//      ,[证件号码]
//      ,[房屋座落]
//      ,[建筑面积]
//      ,[房屋用途]
//      ,[档案状态]
//      ,[业务宗号]
//      ,[权证号]
//      ,[不动产证号]
//      ,[日期]
//  FROM [AHMS].[dbo].[vw_档案信息查询]
//where [权证号] like '%{0}%' and [业务宗号]='{1}' and [档案状态]='历史' ";


            string sql1 = @"select distinct a.busino as 业务宗号,a.ishistoray as 档案状态 
  from [AHMS].[dbo].[ArchiveIndex] as a
  left join [AHMS].[dbo].[Certificate] as b on b.[ArchiveId]=a.[ArchiveId]
 where   a.archivetype='C'  and b.bdczh='{0}' 
order by 业务宗号";

            string sql2 = @"select distinct a.busino as 业务宗号,a.ishistoray as 档案状态 
  from [AHMS].[dbo].[ArchiveIndex] as a
  left join [AHMS].[dbo].[Certificate] as b on b.[ArchiveId]=a.[ArchiveId]
 where   a.archivetype='C'   and b.prop like '%{0}%' and a.busino='{1}'
order by 业务宗号";










            string sql = "";
            if(zh.Contains("不动产"))
            {
                sql = string.Format(sql1, zh);
            }
            else
            {
                
                sql = string.Format(sql2, GetNumber(zh), realSLBH);
            }
            DbHelper db = new DbHelper();
            try
            {
                //sql = string.Format(sql, realSLBH);
               
                db.SetProvider(MyDBType.Sql);
                return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            }
            catch
            { }
            finally
            {
                db.CloseConn();
            }
            return null;
        }

        private string CheckDIffInBDC(DataRow row)
        {
            string qzh = row["权证号"] == null ? "" : row["权证号"].ToString();
            string bdc_zh = row["不动产证号"] == null ? "" : row["不动产证号"].ToString();
            string ywzh = row["业务宗号"] == null ? "" : row["业务宗号"].ToString();
            string fczt = row["历史状态"] == null ? "" : row["历史状态"].ToString();
            string zh = bdc_zh == "" ? qzh : bdc_zh;
            if (!string.IsNullOrEmpty(zh))
            {
                DataTable dt = GetZT_BDC(zh);
                if (null != dt && dt.Rows.Count == 1)
                {
                    string slbh = dt.Rows[0]["SLBH"].ToString();
                    string bdczh = dt.Rows[0]["bdczh"].ToString();
                    string lifecycle = "";
                    if (null == dt.Rows[0]["lifecycle"] || DBNull.Value == dt.Rows[0]["lifecycle"])
                    {
                        lifecycle = "0";
                    }
                    else
                    {
                        lifecycle = dt.Rows[0]["lifecycle"].ToString();
                    }
                    if (!fczt.Equals(lifecycle))
                    {
                        UpdateFCZT(ywzh, lifecycle);
                        return "房产：" + ywzh + "|" + zh + "|" + fczt + "  不动产:" + slbh + "|" + bdczh + "|" + lifecycle;
                    }


                }
            }
            return "";
        }

        private void UpdateFCZT(string ywzh, string lifecycle)
        {
            if(string.IsNullOrEmpty(lifecycle))
            {
                lifecycle = "0";
            }
            string sql = "update [dbo].[ArchiveIndex] set ishistoray='{0}' where busino='{1}'";
            sql = string.Format(sql, lifecycle, ywzh);
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            db.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }
        DbHelper BDCdb = new DbHelper();
        //
        private  DataTable GetZT_BDC(string zh)
        {
            string sql = "";
            //if (zh.Contains("不动产"))
            //{
                sql = "select slbh,bdczh,lifecycle from dj_djb where bdczh= '{0}'";
            //}
            //else
            //{
            //    zh = GetNumber(zh);
            //    sql = "select slbh,bdczh,lifecycle from dj_djb where bdczh  like '%{0}%' and zslx='房产证'";
            //}
            
            sql = string.Format(sql, zh);
            BDCdb.SetProvider(MyDBType.Oracle);
            return BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private DataTable GetAllZHbdc()
        {
            string sql = @"select djb.slbh,djb.bdczh,djb.lifecycle from dj_xgdjgl djxg 
left join dj_djb djb on djb.slbh=djxg.fslbh
where bglx='抵押注销' and xgzlx<>'房屋抵押证明'  and xgzlx<>'土地证'";
            BDCdb.SetProvider(MyDBType.Oracle);
            return BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

        }

        private DataTable GetALLZH()
        {
            string sql = @"  select distinct a.busino as 业务宗号,b.prop as 权证号,b.BDCZH as 不动产证号,a.ishistoray as 历史状态 
  from [AHMS].[dbo].[ArchiveIndex] as a
  left join [AHMS].[dbo].[Certificate] as b on a.[ArchiveId]=b.[ArchiveId]
 where a.ishistoray=1 and b.prop not like 'CS%' and a.archivetype='C' and a.busino>'201506080001'    
order by 业务宗号";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string rootPath = txtFilePath.Text;
            this.listBox2.Items.Clear();
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                getDirectory(rootPath, txtFindStr.Text);
                MessageBox.Show("OK");
            });
        }

        

        /*
        * 获得指定路径下所有文件名
        * StreamWriter sw  文件写入流
        * string path      文件路径
        * int indent       输出时的缩进量
        */
        public void getFileName(string path,string str)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (FileInfo f in root.GetFiles())
            {
                //if (f.Extension.ToLower().Contains("asp"))
                //{
                try
                {
                    StreamReader sr = new StreamReader(f.FullName);
                    string content = sr.ReadToEnd();
                    content = content.Replace("\\r", "").Replace("\\n", "");
                    Regex r = new Regex(str); // 定义一个Regex对象实例

                    Match m = r.Match(content); // 在字符串中匹配

                    if (m.Success)

                    {

                        this.Invoke(new Action(() =>
                        {
                            this.listBox2.Items.Add(f.FullName);
                            this.listBox2.Items.Add("--------------------------------");
                            this.listBox2.SelectedIndex = this.listBox3.Items.Count - 1;
                        }));

                    }
                    // }
                }
                catch { continue; }
                
            }
        }

        //获得指定路径下所有子目录名
        public void getDirectory(string path,string str)
        {
            getFileName(path,str);
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                getDirectory(d.FullName,str);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f1 = new FolderBrowserDialog();
            if(DialogResult.OK==f1.ShowDialog())
            {
                this.txtFilePath.Text = f1.SelectedPath;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
           
        }

        private DataTable GetNoGLDT()
        {
            string sql = @"select [ArchiveId] from [AHMS].[dbo].[ArchiveIndex] where [ArchiveId] not in (select [ArchiveId] from [AHMS].[dbo].[PropArchiveRelation])";
            DbHelper db = new DbHelper();
            db.SetProvider(MyDBType.Sql);
            return db.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string str = txtSLBH.Text.Trim();
            DataTable res = new DataTable();
            res.Columns.Add("BDC_SLBH");
            res.Columns.Add("BDC_BDCZH");
            res.Columns.Add("BDC_LIF");
            res.Columns.Add("FC_SLBH");
            res.Columns.Add("FC_PROP");
            res.Columns.Add("FC_HIS");

            DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(str); //GetAllPushDAData();
            if (null != dt && dt.Rows.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            int sqlcount = 0;
                            string slbh = row["PRJID"].ToString();
                            if (slbh.Contains("-"))
                            {
                                slbh = slbh.Substring(0, slbh.IndexOf('-'));
                            }
                            //string sql = "select count(1) from [dbo].[ArchiveIndex] where busino like '{0}%' ";// and isOld='2'";
                            //sql = string.Format(sql, slbh);
                            //DbHelper sqldb = new DbHelper();
                            //sqldb.SetProvider(MyDBType.Sql);
                            //object o = sqldb.ExecuteScalar(MyDBType.Sql, CommandType.Text, sql, null);
                            //if (null != o)
                            //{
                            //    sqlcount = Convert.ToInt32(o);
                            //}
                            //else
                            //{
                            //    sqlcount = 0;
                            //}

                            //if(sqlcount>0)
                            //{
                            //    MyDeleteRecode(slbh);


                            //}

                            DataTable bdcDT = GetQZSLBH(slbh);
                            if(null!=bdcDT && bdcDT.Rows.Count>0)
                            {
                                foreach (DataRow bdcRow in bdcDT.Rows)
                                {
                                    string bdcslbh = bdcRow["SLBH"].ToString();
                                    string lif = bdcRow["lifecycle"].ToString();
                                    string qlrmc= bdcRow["qlrmc"].ToString();
                                    string zjhm= bdcRow["zjhm"].ToString();
                                    string qzh= bdcRow["bdczh"].ToString();

                                    if (string.IsNullOrEmpty(lif))
                                    {
                                        lif = "0";
                                    }
                                    DataTable fcdt = GetFCLif(qlrmc, zjhm, qzh);
                                    if(null!=fcdt && fcdt.Rows.Count>0)
                                    {
                                        string fcLif = (fcdt.Rows[0]["档案状态"].ToString().Equals("历史") ? "1" : "0");
                                        if (!lif.Trim().Equals(fcLif.Trim()))
                                        {
                                            DataRow newRow = res.NewRow();
                                            newRow["BDC_SLBH"] = slbh;
                                            newRow["BDC_BDCZH"] = qzh;
                                            newRow["BDC_LIF"] = lif;
                                            newRow["FC_SLBH"] = fcdt.Rows[0]["业务宗号"].ToString();
                                            newRow["FC_PROP"] = fcdt.Rows[0]["权证号"].ToString();
                                            newRow["FC_HIS"] = fcLif;

                                            res.Rows.Add(newRow);

                                            UpdateFCZT(fcdt.Rows[0]["业务宗号"].ToString(), lif);
                                            this.Invoke(new Action(() =>
                                            {
                                                this.listBox1.Items.Add("证号:" +qzh+";lif="+lif+"|房产:"+ fcdt.Rows[0]["业务宗号"]+"|房产证号:"+ fcdt.Rows[0]["权证号"]+"|"+ fcLif);
                                                this.listBox1.Items.Add("--------------------------------");
                                                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;

                                            }));
                                        }

                                    }
                                }
                            }
                            i++;
                            this.Invoke(new Action(() =>
                            {
                                this.label7.Text = "剩余" + (dt.Rows.Count - i);
                            }));


                        }
                        catch(Exception ex)
                        {
                            this.Invoke(new Action(() =>
                            {
                                this.listBox1.Items.Add(ex.Message);
                                this.listBox1.Items.Add("--------------------------------");
                                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;

                            }));
                            continue;
                        }

                    }
                    DataTableRenderToExcel.RenderDataTableToExcel(res, "E:\\RES.xls");
                    MessageBox.Show("OK");

                });

            }
        }


        private void WriteLog(string str)
        {
            string path = "E:\\ZX.txt";
            StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            wr.WriteLine(str);
            wr.WriteLine("--------------------------------------------------------------------");
            wr.Close();
        }

        private void WriteLog(string str,string path)
        {
            //string path = "E:\\ZX.txt";
            StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
            wr.WriteLine(str);
            wr.WriteLine("--------------------------------------------------------------------");
            wr.Close();
        }


        private DataTable GetQZSLBH(string slbh)
        {
            string sql = @"select djb.slbh,qlr.qlrmc,qlr.zjhm,djb.bdczh,djb.lifecycle from dj_djb djb
left join dj_xgdjgl djgl on djgl.fslbh=djb.slbh and djgl.xgzlx in ('房屋不动产证','房产证','预告证明')
left join dj_qlrgl qlrgl on qlrgl.slbh=djb.slbh
left join dj_qlr qlr on qlr.qlrid=qlrgl.qlrid
where djgl.zslbh='{0}' ";
            sql = string.Format(sql, slbh);
            BDCdb.SetProvider(MyDBType.Oracle);
            return BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private DataTable GetFCLif(string name, string cardNO, string prop)
        {
            if (!prop.Contains("不动产"))
            {
                string propTmp = ReplaceProp(prop);
                prop = GetNumber(propTmp);
            }
            DataTable dt = GetCountFromFC(prop, name);
            if (null != dt && dt.Rows.Count == 1)
            {
                return dt;
            }
            else
            {
                return GetZXFmbusino(prop, name, cardNO);
            }
        }

        private  DataTable GetCountFromFC(string prop, string qlrmc)
        {
            string sql = @"select [姓名]
      ,[证件号码]
      ,[房屋座落]
      ,[建筑面积]
      ,[房屋用途]
      ,[档案状态]
      ,[业务宗号]
      ,[权证号]
      ,[不动产证号]
      ,[日期] from [dbo].[vw_档案信息查询] WITH (nolock)  where 1=1  ";
            if (prop.Contains("不动产"))
            {
                sql += " and 不动产证号 = '" + prop + "' ";
            }
            else
            {
                string propTmp = ReplaceProp(prop);
                sql += " and 权证号 like '%" + GetNumber(propTmp) + "%'";
            }
            sql += " and 姓名='" + qlrmc + "' ";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                return dt;
            }

        }

        private  DataTable GetZXFmbusino(string prop, string name, string cardNO)
        {
            
            string sql = @"select [姓名]
      ,[证件号码]
      ,[房屋座落]
      ,[建筑面积]
      ,[房屋用途]
      ,[档案状态]
      ,[业务宗号]
      ,[权证号]
      ,[不动产证号]
      ,[日期] from [dbo].[vw_档案信息查询] WITH (nolock) where 1=1  ";

            sql += GetSelectWhere( prop, name, cardNO);

            //sql = string.Format(sql, prop);
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
               DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
               if(null!=dt && dt.Rows.Count>0)
                {
                    return dt;
                }
                return null;
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            string sql = txtSQL.Text;
            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                int i = dbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                MessageBox.Show("i=" + i);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
        //    SOAP.Bdc2Fc_CLFSoapClient client = new SOAP.Bdc2Fc_CLFSoapClient();
        //    DataSet ds = client.FC_CLF_YZXX(textBox2.Text.Trim());


            //BDCSrv.BDCSrvSoapClient c = new BDCSrv.BDCSrvSoapClient();
            //string mc = textBox2.Text.Trim();
            //string hm = textBox3.Text.Trim();
            //string zl = textBox4.Text.Trim();
            //DataSet ds = c.SPF_WFZMCX(mc, hm, zl);


            //this.dataGridView1.DataSource = ds;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("HOUSEID");
            //dt.Columns.Add("TYPE");
            //dt.Columns.Add("MORTGAGESTATE");
            //dt.Columns.Add("MORTGAGETIME");

            //DataRow row = dt.NewRow();
            //row["HOUSEID"] = "46BB780B-B09D-0074-E053-C0A864060074";
            //row["TYPE"] = "Less";
            //row["MORTGAGESTATE"] = 0;
            //row["MORTGAGETIME"] = DateTime.Now;

            //dt.Rows.Add(row);

            //DataSet sou = new DataSet();
            //sou.Tables.Add(dt);


            //BDCSrv.BDCSrvSoapClient soap = new BDCSrv.BDCSrvSoapClient();
            //BDCSrv.BDC bdc=soap.FC_SPFYGHT("SPFHTBAH=2018070910001");
            //this.dataGridView2.DataSource = bdc.data;

            SHIQU.BDCSrvSoapClient cli = new SHIQU.BDCSrvSoapClient();
            SHIQU.BDC source = cli.FC_SPFYGHT("SPFHTBAH=2018070910001");
            this.dataGridView2.DataSource = source.data;




        }
        int upcount = 0;
        StringBuilder sb = new StringBuilder();
        private void button20_Click(object sender, EventArgs e)
        {
            DataTable FCDT = GetFCNoProp();
            if(null!=FCDT && FCDT.Rows.Count>0)
            {
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    upcount=FCDT.Rows.Count;
                    foreach (DataRow row in FCDT.Rows)
                    {
                        string slbh = row["BUSINO"] == DBNull.Value ? "" : row["BUSINO"].ToString();
                        string bdcdyh = row["CHID"] == DBNull.Value ? "" : row["CHID"].ToString();
                        string CID = row["CertificateID"] == DBNull.Value ? "" : row["CertificateID"].ToString();
                        DataTable BDCDT = GetBDCZH(slbh, bdcdyh);
                        if (null != BDCDT && BDCDT.Rows.Count > 0)
                        {
                            foreach (DataRow bdcrow in BDCDT.Rows)
                            {
                                string prjid = bdcrow["SLBH"] == DBNull.Value ? "" : bdcrow["SLBH"].ToString();
                                string qzh = bdcrow["QZH"] == DBNull.Value ? "" : bdcrow["QZH"].ToString();
                                string xlh = bdcrow["ZSXLH"] == DBNull.Value ? "" : bdcrow["ZSXLH"].ToString();
                                if (prjid.Contains(slbh))
                                {
                                    if (!string.IsNullOrEmpty(qzh))
                                        CreateUpdateSQL(CID, qzh, xlh);
                                    else
                                        CreatepPropIsNull(prjid);

                                }
                            }

                        }
                        this.Invoke(new Action(() =>
                        {
                            this.listBox1.Items.Add("剩余" + (--upcount));
                            this.listBox1.Items.Add("--------------------------------");
                            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                        }));


                    }
                    //WriteSql(sb.ToString());
                    MessageBox.Show("完成");
                });
               
            }
        }

        private void CreatepPropIsNull(string prjid)
        {
            string str = string.Format("{0} 证号为空",prjid);
            WriteNoProp(str);
        }

        private void CreateUpdateSQL(string cID, string qzh, string xlh)
        {
                string sql = "update [Certificate] set prop='{0}',[BDCZH]='{0}',[PrintNO]='{1}' where [CertificateID]='{2}' ;";
                sql = string.Format(sql, qzh, xlh, cID);
            WriteSql(sql);



        }

        private void WriteSql(string str)
        {
            lock (lockKey)
            {
                string path = "d:\\sql.sql";
                StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                wr.Flush();
                wr.WriteLine(str);
                wr.Close();
            }
        }

        private void WriteNoProp(string str)
        {
            lock (lockKey)
            {
                string path = "d:\\noProp.sql";
                StreamWriter wr = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                wr.Flush();
                wr.WriteLine(str);
                wr.Close();
            }
        }


        private DataTable GetFCNoProp()
        {
            string sql = @"select a.BusiNO,a.ArchiveType,c.CertificateID,c.Prop,c.PrintNO,d.CHID from ArchiveIndex a
left join [Certificate] c on c.ArchiveId=a.ArchiveId
left join HouseInfo d on d.HouseInfo_ID=c.HouseInfo_ID
where a.IsOld=2 and c.Prop='' and a.ArchiveType in ('D','C') order by a.BusiNO";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
        }



        private DataTable GetBDCZH(string slbh,string bdcdyh)
        {
            /*
            union all
            string sql = @"select a.slbh,a.bdczh as qzh,a.zsxlh as zsxlh from fc_h_qsdc h
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
left join dj_djb a on a.slbh=tsgl.slbh
where a.slbh like '{0}%' and {1}
union all

select a.slbh, a.bdczmh as qzh,a.zsxlh as zsxlh from fc_h_qsdc h
left join dj_tsgl tsgl on tsgl.tstybm=h.tstybm
left join dj_yg a on a.slbh=tsgl.slbh
where a.slbh like '{0}%' and {1}";*/

            string sql = @"select a.slbh, a.bdczmh as qzh,a.zsxlh as zsxlh from fc_h_qsdc h
            left join dj_tsgl tsgl on tsgl.tstybm = h.tstybm
left join dj_dy a on a.slbh = tsgl.slbh
where a.slbh like '{0}%' and {1}
union all 
select a.slbh, a.bdczh as qzh,a.zsxlh as zsxlh from fc_h_qsdc h
            left join dj_tsgl tsgl on tsgl.tstybm = h.tstybm
left join dj_djb a on a.slbh = tsgl.slbh
where a.slbh like '{0}%' and {1}
";
            if (string.IsNullOrEmpty(bdcdyh))
            {
                sql = string.Format(sql, slbh, "(h.bdcdyh='' or h.bdcdyh is null)");
            }
            else
            {
                sql = string.Format(sql, slbh, "h.bdcdyh='"+bdcdyh+"'");
            }
            BDCdb.SetProvider(MyDBType.Oracle);
            return BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                this.Invoke(new Action(() =>
                {
                    this.listBox1.Items.Add("开始获取所有数据" );
                    this.listBox1.Items.Add("--------------------------------");
                    this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                }));
                DataTable fcdt = GetAllINFO();
                this.Invoke(new Action(() =>
                {
                    this.listBox1.Items.Add("获取成功");
                    this.listBox1.Items.Add("--------------------------------");
                    this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                }));
                upcount = fcdt.Rows.Count;
                this.Invoke(new Action(() =>
                {
                    this.listBox1.Items.Add("总计:"+upcount);
                    this.listBox1.Items.Add("--------------------------------");
                    this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                }));
                foreach (DataRow fcrow in fcdt.Rows)
                {
                    string busino = fcrow["BusiNO"] == DBNull.Value ? "" : fcrow["BusiNO"].ToString();
                    string qlrmc = fcrow["Name"] == DBNull.Value ? "" : fcrow["Name"].ToString();
                    string zjhm = fcrow["CardNO"] == DBNull.Value ? "" : fcrow["CardNO"].ToString();
                    string qzh = fcrow["Prop"] == DBNull.Value ? "" : fcrow["Prop"].ToString();
                    string bdczh = fcrow["BDCZH"] == DBNull.Value ? "" : fcrow["BDCZH"].ToString();
                    string fclif = fcrow["IsHistoray"] == DBNull.Value ? "" : fcrow["IsHistoray"].ToString();
                    DataTable bdcdt = GetBDCInfo(qzh, bdczh, qlrmc, zjhm);
                    if (null != bdczh && bdcdt.Rows.Count == 1)
                    {
                        string lif = bdcdt.Rows[0]["lifecycle"] == DBNull.Value ? "" : bdcdt.Rows[0]["lifecycle"].ToString();

                        if (string.IsNullOrEmpty(lif))
                        {
                            lif = "0";
                        }
                        if (!fclif.Equals(lif))
                        {
                            string zslbh = GetZslbh(bdcdt.Rows[0]["slbh"]);
                            if (!IsFinish(zslbh))
                            {
                                continue;
                            }
                            if (InTagErr(zslbh))
                            {
                                continue;
                            }
                            if (InFCDA(zslbh))
                            {
                                if (!InTag(zslbh))
                                {
                                    WriteLog(zslbh, "d:\\已完成未推送.txt");
                                    continue;
                                }
                               
                            }

                            string log = "busino={0},Prop={1},bdczh={2},IsHistoray={3}|slbh={4},bdczh={5},lif={6}";
                            log = string.Format(log, busino, qzh, bdczh, fclif, bdcdt.Rows[0]["slbh"], bdcdt.Rows[0]["qzh"], lif);
                            WriteLog(log, "d:\\upLog.log");
                            CreateUpdateLifSql(busino, lif);

                        }

                    }
                    this.Invoke(new Action(() =>
                    {
                        this.listBox1.Items.Add("剩余" + (--upcount));
                        this.listBox1.Items.Add("--------------------------------");
                        this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                    }));
                }
                MessageBox.Show("OK");
            });
        }

        private bool InTag(string zslbh)
        {
            string sql = @"select count(1) from fc_da_tag where  slbh='{0}'";
            sql = string.Format(sql, zslbh);
            BDCdb.SetProvider(MyDBType.Oracle);
            object o = BDCdb.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != o)
                return Convert.ToInt32(o) > 0;
            return false;
        }

        private bool InFCDA(string slbh)
        {
            try
            {
                bool flag = false;
                flag = FCDA_DAL.CanPust(slbh);
                if (!flag)
                    return flag;
                return FCDA_DAL.CanPust(slbh);
            }
            catch
            {
                return true;
            }
            //return true;
        }

        private bool InTagErr(object slbh)
        {
            string sql = @"select count(1) from fc_da_tag where issuccess='0' and message like '%失败%' and slbh='{0}'";
            sql = string.Format(sql, slbh);
            BDCdb.SetProvider(MyDBType.Oracle);
            object o = BDCdb.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != o)
                return Convert.ToInt32(o) > 0;
            return false;
        }

        private bool IsFinish(string slbh)
        {
            string sql = @"select count(1) from WFM_PROCINST t where prjid='{0}' and t.prjstate='已完成'";
            sql = string.Format(sql, slbh);
            DbHelper db = new DbHelper();
            try
            {

                db.Conn = new Oracle.DataAccess.Client.OracleConnection(GGK);
                db.SetProvider(MyDBType.Oracle);
                object o=db.ExecuteScalar(db.Conn, System.Data.CommandType.Text, sql, null);
                if (null != o)
                    return Convert.ToInt32(o) > 0;
                return false;
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                db.CloseConn();
            }
        }

        private string GetZslbh(object v)
        {
            string sql = @"select zslbh from dj_xgdjgl gl where gl.fslbh='{0}' and gl.bglx like '%权属%'";
            sql = string.Format(sql, v);
            BDCdb.SetProvider(MyDBType.Oracle);
            object o = BDCdb.ExecuteScalar(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
            if (null != o)
                return o.ToString();
            return string.Empty;
        }

        private void CreateUpdateLifSql(string busino, string lif)
        {
            string sql = "update [ArchiveIndex] set [IsHistoray]='{0}' where [busino]='{1}' ;";
            sql = string.Format(sql, lif, busino);
            WriteSql(sql);
        }

        private DataTable GetAllINFO()
        {
            string sql = @"select a.BusiNO,a.ArchiveType,c.Prop,c.[BDCZH],a.[IsHistoray], p.[Name],[CardNO] from ArchiveIndex a
left join [Certificate] c on c.ArchiveId=a.ArchiveId
left join [Person] p on p.[ArchiveId]=a.ArchiveId
where c.prop<>''
order by a.BusiNO";

            lock (lockKey)
            {
                DbHelper dbHelper = new DbHelper();
                dbHelper.SetProvider(MyDBType.Sql);
                DataTable dt = dbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (null != dt && dt.Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
        }

        private DataTable GetBDCInfo(string qzh,string bdczh,string qlrmc,string zjhm)
        {
            string sql = @"select a.slbh as slbh,a.bdczh as qzh,a.lifecycle,qlr.qlrmc,qlr.zjhm from DJ_DJB a
left join dj_qlrgl gl on gl.slbh=a.slbh
left join dj_qlr qlr on qlr.qlrid=gl.qlrid
where qlr.qlrmc='{0}' and qlr.zjhm='{1}' and {2}
union all
select a.slbh as slbh,a.bdczmh as qzh,a.lifecycle,qlr.qlrmc,qlr.zjhm from dj_yg a
left join dj_qlrgl gl on gl.slbh=a.slbh
left join dj_qlr qlr on qlr.qlrid=gl.qlrid
where qlr.qlrmc='{0}' and qlr.zjhm='{1}' and {3} ";

            string orsql =string.Empty;
            string orsql1 = string.Empty;
            if(!string.IsNullOrEmpty(bdczh))
            {
                orsql = string.Format("a.bdczh = '{0}'", bdczh);
                orsql1 = string.Format("a.bdczmh='{0}'", bdczh);
               
            }
            else
            {
                orsql = string.Format("a.bdczh like '%{0}%'", qzh);
                orsql1 = string.Format("a.bdczmh like '%{0}%'", qzh);
                
            }
            sql = string.Format(sql, qlrmc, zjhm, orsql, orsql1);
            BDCdb.SetProvider(MyDBType.Oracle);
            return BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

        }

        private void textBox5_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                textBox5.Text = ofd.FileName;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            DataTable dt = DataTableRenderToExcel.RenderDataTableFromExcel(textBox5.Text);
            foreach (DataRow row in dt.Rows)
            {
                string slbh = row["YWBH"].ToString();
                string sqr = GetSQRXM(slbh);
                string sqrlxfs = GetSQRLXFS(slbh);
                row["SQRXM"] = sqr;
                row["SQRLXFS"] = sqrlxfs;
            }

            DataTableRenderToExcel.RenderDataTableToExcel(dt, "C:\\1.xls");
        }
        private string GetSQRXM(string yWBH)
        {
            MKevaluateBLL bll = new MKevaluateBLL();
            return bll.GetSQRXM(yWBH);
        }
        private string GetSQRLXFS(string yWBH)
        {
            MKevaluateBLL bll = new MKevaluateBLL();
            return bll.GetSQRLXFS(yWBH);
        }

        private void textBox6_DoubleClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(DialogResult.OK==fbd.ShowDialog())
            {
                textBox6.Text = fbd.SelectedPath;
            }
        }
        private static string tmpDir = "d:\\MergeTmp\\";
        private void button23_Click(object sender, EventArgs e)
        {
            MergeImgHelp mih = new MergeImgHelp();
            string path = textBox6.Text.Trim();
            List<Image> list = new List<Image>();
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fileNames = dir.GetFiles("*.jpg");
            foreach (FileInfo item in fileNames)
            {
                string zoomPath = tmpDir + "201807270001" + "_" + index + item.Extension;
                FileStream stream = new FileStream(item.FullName, FileMode.Open);
                mih.ZoomAuto(stream, zoomPath, 1287, 1059, "", "");
                stream.Close();
                index++;
            }

            DirectoryInfo dirs = new DirectoryInfo(tmpDir);
            FileInfo[] fileNamess = dirs.GetFiles("*.jpg");


            Merge(fileNamess, "201807270001");
            
           
            foreach (FileInfo item in fileNamess)
            {
                File.Delete(item.FullName);
            }


                MessageBox.Show("OK");
        }

        private void Merge(FileInfo[] fileNames, string slbh)
        {
            
            string source = tmpDir + slbh + ".jpg";
            MergeImgHelp mih = new MergeImgHelp();
            mih.CombineImages(fileNames, source);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string sql = "select * from DJ_TaxInfo order by slbh";
            BDCdb.SetProvider(MyDBType.Oracle);
            DataTable taxDT= BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);

            if (null != taxDT && taxDT.Rows.Count > 0)
            {
                DataTable resDT = CreateTAXResDT();
                ThreadPool.QueueUserWorkItem(delegate (object obj)
                {
                    int i = 0;
                    foreach (DataRow row in taxDT.Rows)
                    {
                        i++;
                        
                        string slbh = row["slbh"]==DBNull.Value?"": row["slbh"].ToString();
                        string txm = row["TXM"] == DBNull.Value ? "" : row["TXM"].ToString();
                        string content = row["XML"] == DBNull.Value ? "" : row["XML"].ToString();
                        
                        try
                        {
                            XmlHelper xmlHelper = new XmlHelper(content);
                            string nsr = xmlHelper.GetXMLNodeText("taxXML/houseList/qsVO/qs_mc");
                            string sbh = xmlHelper.GetXMLNodeText("taxXML/houseList/qsVO/qs_nsrsbm");

                            if(string.IsNullOrEmpty(slbh))
                            {
                                AddNewRow(resDT, txm, nsr, sbh, slbh, "", "","不动产受理编号为空");
                                this.Invoke(new Action(() =>
                                {
                                    this.listBox4.Items.Add("剩余" + (taxDT.Rows.Count - i));
                                    this.listBox4.Items.Add("--------------------------------");
                                    this.listBox4.SelectedIndex = this.listBox4.Items.Count - 1;

                                }));
                                continue;
                            }
                            DataTable bdcQLRs = GetBDCQLR(slbh);
                            if (null != bdcQLRs && bdcQLRs.Rows.Count > 0)
                            {
                                if (!CheckCnameInFC(bdcQLRs, nsr))
                                {
                                    AddNewRow(resDT, txm, nsr, sbh, slbh, MemStr(bdcQLRs,"QLRMC"), MemStr(bdcQLRs,"ZJHM"), "权利人不一致");
                                }
                            }
                            else
                            {
                                AddNewRow(resDT, txm, nsr, sbh, slbh, "", "", "不动产权利人为空");
                            }
                           

                           
                            
                        }
                        catch(System.Xml.XmlException xmlex)
                        {
                            AddNewRow(resDT, txm, "", "", slbh, "", "", xmlex.Message);
                        }
                        catch(System.NullReferenceException nullex)
                        {
                            AddNewRow(resDT, txm, "", "", slbh, "", "", nullex.Message);
                        }
                        catch(System.Xml.XPath.XPathException pathex) 
                        {
                            AddNewRow(resDT, txm, "", "", slbh, "", "", pathex.Message);
                        }
                        this.Invoke(new Action(() =>
                        {
                            this.listBox4.Items.Add("剩余" + (taxDT.Rows.Count - i));
                            this.listBox4.Items.Add("--------------------------------");
                            this.listBox4.SelectedIndex = this.listBox4.Items.Count - 1;

                        }));
                    }
                    if(null!=resDT && resDT.Rows.Count>0)
                    {
                        DataTableRenderToExcel.RenderDataTableToExcel(resDT, "c:\\res.xls");
                    }
                    MessageBox.Show("OK");
                });
            }

           
        }

        private string MemStr(DataTable bdcQLRs, string v)
        {
            string str = string.Empty;
            foreach (DataRow row in bdcQLRs.Rows)
            {
                if(string.IsNullOrEmpty(str))
                {
                    str = row[v].ToString();
                }
                else
                {
                    str+=" "+ row[v].ToString();
                }
            }
            return str;
        }

        private bool CheckCnameInFC(DataTable bdcQLRs, string nsr)
        {
            string[] nsrs = nsr.Split(' ');
            foreach (string str in nsrs)
            {
                DataRow[] rows = bdcQLRs.Select(string.Format("qlrmc ='{0}'", str.Trim()));
                if (rows.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private DataTable GetBDCQLR(string slbh)
        {
            string sql = @"select qlr.qlrmc,qlr.zjhm,gl.qlrlx from dj_qlr qlr
left join dj_qlrgl gl on gl.qlrid = qlr.qlrid
where gl.slbh = '{0}' and gl.qlrlx = '权利人'";
            sql = string.Format(sql, slbh);
            BDCdb.SetProvider(MyDBType.Oracle);
            return  BDCdb.ExecuteTable(MyDBType.Oracle, System.Data.CommandType.Text, sql, null);
        }

        private DataTable CreateTAXResDT()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("条形码");
            DT.Columns.Add("纳税人");
            DT.Columns.Add("纳税人识别号");
            DT.Columns.Add("不动产受理编号");
            DT.Columns.Add("不动产权利人");
            DT.Columns.Add("权利人证件号");
            DT.Columns.Add("异常信息");
            return DT;
        }

        private void AddNewRow(DataTable dt,string txm,string nsr,string sbh,string slbh,string qlr,string zjhm,string message)
        {
            DataRow row = dt.NewRow();
            row["条形码"] = txm;
            row["纳税人"] = nsr;
            row["纳税人识别号"] = sbh;
            row["不动产受理编号"] = slbh;
            row["不动产权利人"] = qlr;
            row["权利人证件号"] = zjhm;
            row["异常信息"] = message;
            dt.Rows.Add(row);
        }
    }
}
