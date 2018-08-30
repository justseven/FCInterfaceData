using System.Collections.Generic;
using Web4BDC.FC.Models;
using Web4BDC.Models.BDCModel;

namespace Web4BDC.Models.FCDAModel
{
    public class BDCDAModel
    {
        public ArchiveIndex archiveIndex { get; set; }
        public List<HouseInfo> houses { get; set; }
        public List<Person> persons { get; set; }
        public List<Certificate> certificates { get; set; }
        public List<HouseArchiveRelation> houseArchiveRelations { get; set; }
        public List<PropArchiveRelation> propArchiveRelations { get; set; }
        public List<VolEleArc> volEleArcs { get; set; }
        public List<VolEleArcDtl> volEleArcDtls { get; set; }

        public List<DJ_XGDJGL> FSLBH_List { get; set; }
    }
}