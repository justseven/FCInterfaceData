using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web4BDC.FC.Models;

namespace Web4BDC.Models.DAModels
{
    public class FCDA
    {
        public ArchiveIndex archiveInfo { get; set; }
        public List<Certificate> CerList { get; set; }

        public List<PropArchiveRelation> PropArchList { get {
                if (null != CerList && CerList.Count > 0 && null != archiveInfo)
                {
                    List<PropArchiveRelation> list = new List<PropArchiveRelation>();
                    foreach (Certificate cer in CerList)
                    {
                        PropArchiveRelation cr = new PropArchiveRelation();
                        cr.RelationID = CreateGuid(32);
                        cr.ArchiveId = archiveInfo.ArchiveId;
                        cr.CertificateID = cer.CertificateID;
                        list.Add(cr);
                    }
                    return list;
                }
                return null;
            }
        }
        public List<HouseArchiveRelation> HRList
        {
            get
            {
                if (null != HouseList && HouseList.Count > 0 && null != archiveInfo)
                {
                    List<HouseArchiveRelation> list = new List<HouseArchiveRelation>();
                    foreach (HouseInfo h in HouseList)
                    {
                        HouseArchiveRelation hr = new HouseArchiveRelation();
                        hr.ArchiveId = archiveInfo.ArchiveId;
                        hr.BusiNO = archiveInfo.BusiNO;
                        hr.HouseInfo_ID = h.HouseInfo_ID;
                        hr.RelationID = CreateGuid(32);
                        list.Add(hr);
                    }
                    return list;
                }
                return null;
            }
        }
        public List<HouseInfo> HouseList { get; set; }

        public List<Person> PersonList { get; set; }

        public List<VolEleArc> VolList { get; set; }

        private Guid CreateGuid(int length)
        {
            //string str = Guid.NewGuid().ToString().Substring(3);
            //return new Guid("BDC" + str);
            return Guid.NewGuid();
        }

    }
}