using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using Web4BDC.Dal.XZFCDA.DAL;
using Web4BDC.FC.Models;
using Web4BDC.Models;
using Web4BDC.Models.DAModels;
using Web4BDC.Models.FCDAModel;

namespace Web4BDC.Bll.XZFCDA.BLL
{
    public class FCDA_NEW_BLL
    {
        /// <summary>
        /// 插入产权处档案库
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns>1:插入成功；0：宗地业务；异常信息：失败</returns>
        public static BDCFilterResult Insert_FCDA(PageParams pageParams)
        {
            BDCFilterResult res = null;
            if (CheckYW(pageParams))
            {
                List<FCDA> fcda = GetFCDA(pageParams.PrjId);//CreateFCDA(pageParams.PrjId);
                List<BDCDA> bdcda = GetBDCDA(pageParams.PrjId);// CreateBDCDA(pageParams.PrjId);

                try
                {
                    PushFCDA(fcda);
                }
                catch(Exception ex)
                {

                }

                try
                {
                    PushBDCDA(bdcda);
                }
                catch (Exception ex)
                { }

            }
            return res;
        }

        private static List<BDCDA> GetBDCDA(string prjId)
        {
            DataTable dt = GetAllSlbhFrmSJD(prjId);
            if (null != dt && dt.Rows.Count > 0)
            {
                List<BDCDA> list = new List<BDCDA>();
                foreach (DataRow row in dt.Rows)
                {
                    BDCDA da = CreateBDCDA(row["slbh"].ToString());
                    list.Add(da);
                }
                return list;
            }
            return null;
        }

        private static List<FCDA> GetFCDA(string prjId)
        {
            DataTable dt = GetAllSlbhFrmSJD(prjId);
            if(null!=dt && dt.Rows.Count>0)
            {
                List<FCDA> list = new List<FCDA>();
                foreach (DataRow row in dt.Rows)
                {
                    FCDA da = CreateFCDA(row["slbh"].ToString());
                    list.Add(da);
                }
                return list;
            }
            return null;
        }

        private static DataTable GetAllSlbhFrmSJD(string prjId)
        {
            throw new NotImplementedException();
        }

        private static void PushBDCDA(List<BDCDA> fcda)
        {
            if (null != fcda && fcda.Count > 0)
            {
                foreach (BDCDA da in fcda)
                {
                    InserIntoBDCDB(da);
                }
            }
        }

        private static void InserIntoBDCDB(BDCDA da)
        {
            throw new NotImplementedException();
        }

        private static void PushFCDA(List<FCDA> fcda)
        {
            if(null!=fcda && fcda.Count>0)
            {
                foreach (FCDA da in fcda)
                {
                    InserIntoFCDB(da);
                   
                }
            }
        }
        

        private static void InserIntoFCDB(FCDA da)
        {
            FCDA_NEW_DAL dal = new FCDA_NEW_DAL();
            dal.Insert(da);
        }

        private static BDCDA CreateBDCDA(string prjId)
        {
            throw new NotImplementedException();
        }

        private static FCDA CreateFCDA(string prjId)
        {
            FCDA da = new FCDA();
            da.archiveInfo = CreateArchiveIndex(prjId);
            da.CerList = CreateCerList(prjId);
            da.HouseList = CreateHouseList(prjId);
            da.PersonList = CreatePersonList(prjId);
            da.VolList = CreateVolList(prjId);

            //da.PropArchList = CreatePropArchList(da);
            //da.HRList = CreateHRList(da);

            return da;
        }

        private static List<VolEleArc> CreateVolList(string prjId)
        {
            throw new NotImplementedException();
        }

        private static List<PropArchiveRelation> CreatePropArchList(FCDA da)
        {
            if (null != da.CerList && da.CerList.Count > 0 && null != da.archiveInfo)
            {
                List<PropArchiveRelation> list = new List<PropArchiveRelation>();
                foreach (Certificate cer in da.CerList)
                {
                    PropArchiveRelation cr = new PropArchiveRelation();
                    cr.RelationID = CreateGuid(32);
                    cr.ArchiveId = da.archiveInfo.ArchiveId;
                    cr.CertificateID = cer.CertificateID;
                    list.Add(cr);
                }
                return list;
            }
            return null;
        }

        private static List<Person> CreatePersonList(string prjId)
        {
            throw new NotImplementedException();
        }

        private static List<HouseArchiveRelation> CreateHRList(FCDA da)
        {
            if(null!=da.HouseList&& da.HouseList.Count>0 && null!=da.archiveInfo)
            {
                List<HouseArchiveRelation> list = new List<HouseArchiveRelation>();
                foreach (HouseInfo h in da.HouseList)
                {
                    HouseArchiveRelation hr = new HouseArchiveRelation();
                    hr.ArchiveId = da.archiveInfo.ArchiveId;
                    hr.BusiNO = da.archiveInfo.BusiNO;
                    hr.HouseInfo_ID = h.HouseInfo_ID;
                    hr.RelationID = CreateGuid(32);
                    list.Add(hr);
                }
                return list;
            }
            return null;
        }
        private static Guid CreateGuid(int length)
        {
            //string str = Guid.NewGuid().ToString().Substring(3);
            //return new Guid("BDC" + str);
            return Guid.NewGuid();
        }

        private static List<HouseInfo> CreateHouseList(string prjId)
        {
            throw new NotImplementedException();
        }

        private static List<Certificate> CreateCerList(string prjId)
        {
            throw new NotImplementedException();
        }

        private static ArchiveIndex CreateArchiveIndex(string prjId)
        {
            throw new NotImplementedException();
        }

        private static bool CheckYW(PageParams pageParams)
        {
            //检查SLBH长度
            if(!CheckSLBH(pageParams.PrjId))
            {
                return false;
            }
            //检查业务流程是否可归档
            if(!CheckCanGD(pageParams))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查流程是否满足归档条件
        /// </summary>
        /// <param name="pageParams"></param>
        /// <returns></returns>
        private static bool CheckCanGD(PageParams pageParams)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 检查SLBH长度等是否符合
        /// </summary>
        /// <param name="prjId"></param>
        /// <returns></returns>
        private static bool CheckSLBH(string prjId)
        {
            throw new NotImplementedException();
        }
    }
}