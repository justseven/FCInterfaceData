using Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using XZFCDA.FC.Models;

namespace XZFCDA.Dal
{
    public class FCDA_DAL
    {


        public static void Insert(ArchiveIndex archiveIndex, List<VolEleArc> volEleArc, HouseInfo houseInfo, List<Person> persons, Certificate certificate, HouseArchiveRelation houseArchiveRelation, PropArchiveRelation propArchiveRelation, List<VolEleArcDtl> volEleArcDtl)
        {
           
                using (TransactionScope ts = new TransactionScope())
                {
                    Insert_ArchiveIndex(archiveIndex);
                    foreach (VolEleArc item in volEleArc)
                    {
                        Insert_VolEleArc(item);
                    }
                    
                    Insert_HouseInfo(houseInfo);
                    foreach (Person person in persons)
                    {
                        Insert_Person(person);
                    }
                    
                    Insert_Certificate(certificate);
                    Insert_HouseArchiveRelation(houseArchiveRelation);
                    Insert_PropArchiveRelation(propArchiveRelation);

                    if (null != volEleArcDtl)
                    {
                        foreach (VolEleArcDtl v in volEleArcDtl)
                        {
                            Insert_VolEleArcDtl(v);
                        }
                    }

                    //trans.Commit();
                    ts.Complete();

                }
            
           
        }

        private static int Insert_VolEleArcDtl(VolEleArcDtl volEleArcDtl, DbTransaction trans)
        {
            string sql = "insert into VolEleArcDtl(VolEleArcDtl_id,VolEleArc_ID,imgName,PageNo,ScanDate) " +
                "values(@VolEleArcDtl_id,@VolEleArc_ID,@imgName,@PageNo,@ScanDate)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@VolEleArcDtl_id", volEleArcDtl.VolEleArcDtl_id);
            ListAdd(list, "@VolEleArc_ID", volEleArcDtl.VolEleArc_ID);
            ListAdd(list, "@imgName", volEleArcDtl.imgName);
            ListAdd(list, "@PageNo", volEleArcDtl.PageNo);
            ListAdd(list, "@ScanDate", volEleArcDtl.ScanDate);


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_PropArchiveRelation(PropArchiveRelation propArchiveRelation, DbTransaction trans)
        {
            string sql = "insert into PropArchiveRelation(RelationID,ArchiveId,CertificateID) "+
                "values(@RelationID,@ArchiveId,@CertificateID)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", propArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", propArchiveRelation.ArchiveId);
            ListAdd(list,"@CertificateID", propArchiveRelation.CertificateID);
            

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        
        }

        private static int Insert_HouseArchiveRelation(HouseArchiveRelation houseArchiveRelation, DbTransaction trans)
        {
            string sql = "insert into HouseArchiveRelation(RelationID,ArchiveId,HouseInfo_ID,BusiNO) " +
                "values(@RelationID,@ArchiveId,@HouseInfo_ID,@BusiNO)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@RelationID", houseArchiveRelation.RelationID);
            ListAdd(list, "@ArchiveId", houseArchiveRelation.ArchiveId);
            ListAdd(list, "@HouseInfo_ID", houseArchiveRelation.HouseInfo_ID);
            ListAdd(list, "@BusiNO", houseArchiveRelation.BusiNO);


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_Certificate(Certificate certificate, DbTransaction trans)
        {
            string sql = "insert into Certificate(CertificateID,HouseInfo_ID,Prop,PrintNO,CertificateType,GrantDate,PersonID,ArchiveId) " +
                 "values(@CertificateID,@HouseInfo_ID,@Prop,@PrintNO,@CertificateType,@GrantDate,@PersonID,@ArchiveId)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@CertificateID", certificate.CertificateID);
            ListAdd(list, "@HouseInfo_ID", certificate.HouseInfo_ID);
            ListAdd(list, "@Prop", certificate.Prop);
            ListAdd(list, "@PrintNO", certificate.PrintNO);
            ListAdd(list, "@CertificateType", certificate.CertificateType);
            ListAdd(list, "@GrantDate", certificate.GrantDate);
            ListAdd(list, "@PersonID", certificate.PersonID);
            ListAdd(list, "@ArchiveId", certificate.ArchiveId);


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_Person(Person person, DbTransaction trans)
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


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_HouseInfo(HouseInfo houseInfo, DbTransaction trans)
        {
            string sql = "insert into HouseInfo(HouseInfo_ID,H_HoUse,H_ConAcre,I_ItSite,I_ItName,BuNum,BuName,H_CeCode,H_RoNum,H_CurLay,H_HoStru,CHID,BusiNo) " +
               "values(@HouseInfo_ID,@H_HoUse,@H_ConAcre,@I_ItSite,@I_ItName,@BuNum,@BuName,@H_CeCode,@H_RoNum,@H_CurLay,@H_HoStru,@CHID,@BusiNo)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@HouseInfo_ID", houseInfo.HouseInfo_ID);
            //ListAdd(list,"@HoSite", houseInfo.HoSite);
            ListAdd(list, "@H_HoUse", houseInfo.H_HoUse);
            ListAdd(list, "@H_ConAcre", houseInfo.H_ConAcre);
            ListAdd(list, "@I_ItSite", houseInfo.I_ItSite);
            ListAdd(list, "@I_ItName", houseInfo.I_ItName);
            ListAdd(list, "@BuNum", houseInfo.BuNum);
            ListAdd(list, "@BuName", houseInfo.BuName);
            ListAdd(list, "@H_CeCode", houseInfo.H_CeCode);

            ListAdd(list, "@H_RoNum", houseInfo.H_RoNum);
            ListAdd(list, "@H_CurLay", houseInfo.H_CurLay);
            ListAdd(list, "@H_HoStru", houseInfo.H_HoStru);
            ListAdd(list, "@CHID", houseInfo.CHID);
            ListAdd(list, "@BusiNo", houseInfo.BusiNo);


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_VolEleArc(VolEleArc volEleArc, DbTransaction trans)
        {
            string sql = "insert into VolEleArc(EleArcVol_ID,ArchiveId,Ordinal,EleArcName,IsShow) values(@EleArcVol_ID,@ArchiveId,@Ordinal,@EleArcName,@IsShow)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list, "@EleArcVol_ID", volEleArc.EleArcVol_ID);
            ListAdd(list, "@ArchiveId", volEleArc.ArchiveId);
            ListAdd(list, "@Ordinal", volEleArc.Ordinal);
            ListAdd(list, "@EleArcName", volEleArc.EleArcName);
            ListAdd(list, "@IsShow", volEleArc.IsShow);


            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static int Insert_ArchiveIndex(ArchiveIndex archiveIndex, DbTransaction trans)
        {
            string sql = "insert into ArchiveIndex(ArchiveId,ArchiveType,BusiNO,IsHistoray,ArchiveDate,Source,ReqType,HousePropertyType,FmBusiNo) values(@ArchiveId,@ArchiveType,@BusiNO,@IsHistoray,@ArchiveDate,@Source,@ReqType,@HousePropertyType,@FmBusiNo)";
            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list, "@ArchiveId", archiveIndex.ArchiveId);
            //list.Add(new SqlParameter);
            //ListAdd(list,"@DaCode", archiveIndex.DaCode);
            ListAdd(list, "@ArchiveType", archiveIndex.ArchiveType);
            ListAdd(list, "@BusiNO", archiveIndex.BusiNO);
            ListAdd(list, "@IsHistoray", archiveIndex.IsHistoray);
            ListAdd(list, "@ArchiveDate", archiveIndex.ArchiveDate);

            ListAdd(list, "@Source", archiveIndex.Source);
            ListAdd(list, "@ReqType", archiveIndex.ReqType);
            ListAdd(list, "@HousePropertyType", archiveIndex.HousePropertyType);
            ListAdd(list, "@FmBusiNo", archiveIndex.FmBusiNo);

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());
        }
        public static int Insert_ArchiveIndex(ArchiveIndex archiveIndex)
        {
            string sql = "insert into ArchiveIndex(ArchiveId,ArchiveType,BusiNO,IsHistoray,ArchiveDate,Source,ReqType,HousePropertyType,FmBusiNo) values(@ArchiveId,@ArchiveType,@BusiNO,@IsHistoray,@ArchiveDate,@Source,@ReqType,@HousePropertyType,@FmBusiNo)";
            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list,"@ArchiveId", archiveIndex.ArchiveId);
            //list.Add(new SqlParameter);
            //ListAdd(list,"@DaCode", archiveIndex.DaCode);
            ListAdd(list,"@ArchiveType", archiveIndex.ArchiveType);
            ListAdd(list,"@BusiNO", archiveIndex.BusiNO);
            ListAdd(list,"@IsHistoray", archiveIndex.IsHistoray);
            ListAdd(list,"@ArchiveDate", archiveIndex.ArchiveDate);
            
            ListAdd(list,"@Source", archiveIndex.Source);
            ListAdd(list,"@ReqType", archiveIndex.ReqType);
            ListAdd(list,"@HousePropertyType", archiveIndex.HousePropertyType);
            ListAdd(list, "@FmBusiNo", archiveIndex.FmBusiNo);

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        private static void ListAdd(List<DbParameter> list,string paraName,object value)
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
            ListAdd(list,"@EleArcVol_ID", volEleArc.EleArcVol_ID);
            ListAdd(list,"@ArchiveId", volEleArc.ArchiveId);
            ListAdd(list,"@Ordinal", volEleArc.Ordinal);
            ListAdd(list,"@EleArcName", volEleArc.EleArcName);
            ListAdd(list,"@IsShow", volEleArc.IsShow);
  

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_HouseInfo(HouseInfo houseInfo)
        {
            string sql = "insert into HouseInfo(HouseInfo_ID,H_HoUse,H_ConAcre,I_ItSite,I_ItName,BuNum,BuName,H_CeCode,H_RoNum,H_CurLay,H_HoStru,CHID,BusiNo) "+
                "values(@HouseInfo_ID,@H_HoUse,@H_ConAcre,@I_ItSite,@I_ItName,@BuNum,@BuName,@H_CeCode,@H_RoNum,@H_CurLay,@H_HoStru,@CHID,@BusiNo)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@HouseInfo_ID", houseInfo.HouseInfo_ID);
            //ListAdd(list,"@HoSite", houseInfo.HoSite);
            ListAdd(list,"@H_HoUse", houseInfo.H_HoUse);
            ListAdd(list,"@H_ConAcre", houseInfo.H_ConAcre);
            ListAdd(list,"@I_ItSite", houseInfo.I_ItSite);
            ListAdd(list,"@I_ItName", houseInfo.I_ItName);
            ListAdd(list,"@BuNum", houseInfo.BuNum);
            ListAdd(list,"@BuName", houseInfo.BuName);
            ListAdd(list,"@H_CeCode", houseInfo.H_CeCode);

            ListAdd(list,"@H_RoNum", houseInfo.H_RoNum);
            ListAdd(list,"@H_CurLay", houseInfo.H_CurLay);
            ListAdd(list,"@H_HoStru", houseInfo.H_HoStru);
            ListAdd(list,"@CHID", houseInfo.CHID);
            ListAdd(list,"@BusiNo", houseInfo.BusiNo);
      

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_Person(Person person)
        {
            string sql = "insert into Person(PersonID,ArchiveId,Name,CardNO,PersonType,IDCardType,RightMan_ID,Sex) " +
                "values(@PersonID,@ArchiveId,@Name,@CardNO,@PersonType,@IDCardType,@RightMan_ID,@Sex)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@PersonID", person.PersonID);
            ListAdd(list,"@ArchiveId", person.ArchiveId);
            ListAdd(list,"@Name", person.Name);
            ListAdd(list,"@CardNO", person.CardNO);
            ListAdd(list,"@PersonType", person.PersonType);
            ListAdd(list,"@IDCardType", person.IDCardType);
            ListAdd(list,"@RightMan_ID", person.RightMan_ID);
            ListAdd(list,"@Sex", person.Sex);
            

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_Certificate(Certificate certificate)
        {
            string sql = "insert into Certificate(CertificateID,HouseInfo_ID,Prop,PrintNO,CertificateType,GrantDate,PersonID,ArchiveId) "+
                "values(@CertificateID,@HouseInfo_ID,@Prop,@PrintNO,@CertificateType,@GrantDate,@PersonID,@ArchiveId)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@CertificateID", certificate.CertificateID);
            ListAdd(list,"@HouseInfo_ID", certificate.HouseInfo_ID);
            ListAdd(list,"@Prop", certificate.Prop);
            ListAdd(list,"@PrintNO", certificate.PrintNO);
            ListAdd(list,"@CertificateType", certificate.CertificateType);
            ListAdd(list,"@GrantDate", certificate.GrantDate);
            ListAdd(list,"@PersonID", certificate.PersonID);
            ListAdd(list,"@ArchiveId", certificate.ArchiveId);
   

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_HouseArchiveRelation(HouseArchiveRelation houseArchiveRelation)
        {
            string sql = "insert into HouseArchiveRelation(RelationID,ArchiveId,HouseInfo_ID,BusiNO) " +
                "values(@RelationID,@ArchiveId,@HouseInfo_ID,@BusiNO)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", houseArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", houseArchiveRelation.ArchiveId);
            ListAdd(list,"@HouseInfo_ID", houseArchiveRelation.HouseInfo_ID);
            ListAdd(list,"@BusiNO", houseArchiveRelation.BusiNO);
            

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_PropArchiveRelation(PropArchiveRelation propArchiveRelation)
        {
            string sql = "insert into PropArchiveRelation(RelationID,ArchiveId,CertificateID) "+
                "values(@RelationID,@ArchiveId,@CertificateID)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@RelationID", propArchiveRelation.RelationID);
            ListAdd(list,"@ArchiveId", propArchiveRelation.ArchiveId);
            ListAdd(list,"@CertificateID", propArchiveRelation.CertificateID);
            

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

        public static int Insert_VolEleArcDtl(VolEleArcDtl volEleArcDtl)
        {
            string sql = "insert into VolEleArcDtl(VolEleArcDtl_id,VolEleArc_ID,imgName,PageNo,ScanDate) "+
                "values(@VolEleArcDtl_id,@VolEleArc_ID,@imgName,@PageNo,@ScanDate)";
            List<DbParameter> list = new List<DbParameter>();
            ListAdd(list,"@VolEleArcDtl_id", volEleArcDtl.VolEleArcDtl_id);
            ListAdd(list,"@VolEleArc_ID", volEleArcDtl.VolEleArc_ID);
            ListAdd(list,"@imgName", volEleArcDtl.imgName);
            ListAdd(list,"@PageNo", volEleArcDtl.PageNo);
            ListAdd(list,"@ScanDate", volEleArcDtl.ScanDate);
           

            DbHelper.SetProvider(MyDBType.Sql);
            return DbHelper.ExecuteNonQuery(MyDBType.Sql, System.Data.CommandType.Text, sql, list.ToArray());
        }

       

        private String ArchiveId(int length)
        {
            return Guid.NewGuid().ToString().Substring(0, length);
        }



        internal static bool CanPust(string slbh)
        {
            string sql = "select count(1) from ArchiveIndex where BusiNO='{0}'";
            sql = string.Format(sql, slbh);
            DbHelper.SetProvider(MyDBType.Sql);
            object count=DbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
            if (Convert.ToInt32(count) > 0)
            {
                return false;
            }
            else
                return true;
        }

        internal static DataTable GetVolEleArcDtlByVolEleArcID(string p)
        {
            string sql = "select [EleArcVol_ID]  from [VolEleArc] where [ArchiveId]='{0}'";
            sql = string.Format(sql, p);
            DbHelper.SetProvider(MyDBType.Sql);
            return  DbHelper.ExecuteTable(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
        }
    }
}