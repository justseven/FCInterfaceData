using Config;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web4BDC.FC.Models;
using Web4BDC.Models.DAModels;

namespace Web4BDC.Dal.XZFCDA.DAL
{
    public class FCDA_NEW_DAL
    {
        DbHelper dbHelper = new DbHelper();
        DbTransaction trans = null;
        public FCDA_NEW_DAL()
        {
            dbHelper.SetProvider(MyDBType.Sql);
            if(null==dbHelper.Conn)
            {
                 dbHelper.CreateConn();
            }
            trans = dbHelper.Conn.BeginTransaction();
        }
        
        internal void Insert(FCDA da)
        {
            Insert(da.archiveInfo);
            Insert(da.CerList);
            Insert(da.HouseList);
            Insert(da.HRList);
            Insert(da.PersonList);
            Insert(da.PropArchList);
            Insert(da.VolList);
        }

        private void Insert(List<VolEleArc> volList)
        {
            foreach (VolEleArc volEleArc in volList)
            {
                string sql = "insert into VolEleArc(EleArcVol_ID,ArchiveId,Ordinal,EleArcName,IsShow) values(@EleArcVol_ID,@ArchiveId,@Ordinal,@EleArcName,@IsShow)";
                List<DbParameter> list = new List<DbParameter>();
                ListAdd(list, "@EleArcVol_ID", volEleArc.EleArcVol_ID);
                ListAdd(list, "@ArchiveId", volEleArc.ArchiveId);
                ListAdd(list, "@Ordinal", volEleArc.Ordinal);
                ListAdd(list, "@EleArcName", volEleArc.EleArcName);
                ListAdd(list, "@IsShow", volEleArc.IsShow);


                
                int i = dbHelper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());
            }

           


        }

        private void Insert(List<PropArchiveRelation> propArchList)
        {
            if(null!=propArchList && propArchList.Count>0)
            {
                foreach (var propArchiveRelation in propArchList)
                {
                    string sql = "insert into PropArchiveRelation(RelationID,ArchiveId,CertificateID) " +
                "values(@RelationID,@ArchiveId,@CertificateID)";
                    List<DbParameter> list = new List<DbParameter>();
                    ListAdd(list, "@RelationID", propArchiveRelation.RelationID);
                    ListAdd(list, "@ArchiveId", propArchiveRelation.ArchiveId);
                    ListAdd(list, "@CertificateID", propArchiveRelation.CertificateID);


                   
                    int i = dbHelper.ExecuteNonQuery(MyDBType.Sql, trans,System.Data.CommandType.Text, sql, list.ToArray());
                }
            }
        }

        private void Insert(List<Person> personList)
        {
            if(null!=personList && personList.Count>0)
            {
                foreach (var person in personList)
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


                    
                    int i = dbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());

                }
            }
        }

        private void Insert(List<HouseArchiveRelation> hRList)
        {
            if (null != hRList && hRList.Count > 0)
            {
                foreach (var houseArchiveRelation in hRList)
                {
                    string sql = "insert into HouseArchiveRelation(RelationID,ArchiveId,HouseInfo_ID,BusiNO) " +
                "values(@RelationID,@ArchiveId,@HouseInfo_ID,@BusiNO)";
                    List<DbParameter> list = new List<DbParameter>();
                    ListAdd(list, "@RelationID", houseArchiveRelation.RelationID);
                    ListAdd(list, "@ArchiveId", houseArchiveRelation.ArchiveId);
                    ListAdd(list, "@HouseInfo_ID", houseArchiveRelation.HouseInfo_ID);
                    ListAdd(list, "@BusiNO", houseArchiveRelation.BusiNO);

                    
                    int i = dbHelper.ExecuteNonQuery(MyDBType.Sql, trans, System.Data.CommandType.Text, sql, list.ToArray());

                }
            }
        }

        private void Insert(List<HouseInfo> houseList)
        {
            if(null!=houseList && houseList.Count>0)
            {
                foreach (HouseInfo houseInfo in houseList)
                {
                    string sql = "insert into HouseInfo(HouseInfo_ID,H_HoUse,HoSite,H_ConAcre,I_ItSite,I_ItName,BuNum,BuName,H_CeCode,H_RoNum,H_CurLay,H_HoStru,CHID,BusiNo) " +
                "values(@HouseInfo_ID,@H_HoUse,@HoSite,@H_ConAcre,@I_ItSite,@I_ItName,@BuNum,@BuName,@H_CeCode,@H_RoNum,@H_CurLay,@H_HoStru,@CHID,@BusiNo)";
                    List<DbParameter> list = new List<DbParameter>();
                    ListAdd(list, "@HouseInfo_ID", houseInfo.HouseInfo_ID);

                    ListAdd(list, "@HoSite", houseInfo.HoSite);
                    ListAdd(list, "@H_HoUse", houseInfo.H_HoUse);
                    ListAdd(list, "@H_CeCode", null);
                    ListAdd(list, "@H_ConAcre", houseInfo.H_ConAcre);
                    ListAdd(list, "@I_ItSite", houseInfo.I_ItSite);
                    ListAdd(list, "@I_ItName", houseInfo.I_ItName);
                    ListAdd(list, "@BuNum", houseInfo.BuNum);
                    ListAdd(list, "@BuName", houseInfo.BuName);
                    ListAdd(list, "@H_RoNum", houseInfo.H_RoNum);
                    ListAdd(list, "@H_CurLay", houseInfo.H_CurLay);
                    ListAdd(list, "@H_HoStru", houseInfo.H_HoStru);
                    ListAdd(list, "@CHID", houseInfo.CHID);
                    ListAdd(list, "@BusiNo", houseInfo.BusiNo);

                    int i = dbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());


                }
            }
        }

        private void Insert(List<Certificate> cerList)
        {
            if(null!=cerList && cerList.Count>0)
            {
                foreach (var certificate in cerList)
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


                    
                    int i = dbHelper.ExecuteNonQuery(MyDBType.Sql,trans, System.Data.CommandType.Text, sql, list.ToArray());

                }
            }
        }

        private void Insert(ArchiveIndex archiveInfo)
        {
            string sql = "insert into ArchiveIndex(ArchiveId,ArchiveType,BusiNO,IsHistoray,ArchiveDate,Source,ReqType,HousePropertyType,FmBusiNo,IsOld) values(@ArchiveId,@ArchiveType,@BusiNO,@IsHistoray,@ArchiveDate,@Source,@ReqType,@HousePropertyType,@FmBusiNo,@IsOld)";
            List<DbParameter> list = new List<DbParameter>();

            ListAdd(list, "@ArchiveId", archiveInfo.ArchiveId);
            ListAdd(list, "@ArchiveType", archiveInfo.ArchiveType);
            ListAdd(list, "@BusiNO", archiveInfo.BusiNO);
            ListAdd(list, "@IsHistoray", archiveInfo.IsHistoray);
            ListAdd(list, "@ArchiveDate", archiveInfo.ArchiveDate);

            ListAdd(list, "@Source", archiveInfo.Source);
            ListAdd(list, "@ReqType", archiveInfo.ReqType);
            ListAdd(list, "@HousePropertyType", archiveInfo.HousePropertyType);
            ListAdd(list, "@FmBusiNo", archiveInfo.FmBusiNo);
            ListAdd(list, "@IsOld", archiveInfo.IsOld);

            int i = dbHelper.ExecuteNonQuery(MyDBType.Sql, trans,System.Data.CommandType.Text, sql, list.ToArray());
        }
        internal  int GetOrdinal(string cNAME)
        {
            string sql = @"select t2.ArcVolTypeNO+t1.ReceiveVolNO from dbo.ReceiveVol t1 
inner join dbo.ArcVolType t2 on t1.ArcVolTypeID = t2.ArcVolTypeID
where t1.ReceiveVolName like '{0}%'";
            sql = string.Format(sql, cNAME);
            
                object o = dbHelper.ExecuteScalar(MyDBType.Sql, System.Data.CommandType.Text, sql, null);
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                return 2001;
            
        }

        private static void ListAdd(List<DbParameter> list, string paraName, object value)
        {
            if (null == value)
                list.Add(new SqlParameter(paraName, DBNull.Value));
            else
                list.Add(new SqlParameter(paraName, value));
        }
    }
}