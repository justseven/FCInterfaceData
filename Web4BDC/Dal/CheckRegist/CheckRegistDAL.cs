using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web4BDC.Dal.CheckRegist
{
    public class CheckRegistDAL
    {
        public int HasQSOrYGRegister(string HouseInfoId, string slbh)
        {
            string sql = string.Format("Select Count(1) From dj_tsgl where nvl2(LifeCycle,LifeCycle,0)=0 and SLBH not like '{0}%' and TSTYBM='{1}' and (DJZL='权属' or DJZL='预告')", slbh, HouseInfoId);
            return DBHelper.GetScalar(sql);
        }
    }
}