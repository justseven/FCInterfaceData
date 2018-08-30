using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Web4BDC.Dal;
using Web4BDC.Easyui;
using Web4BDC.Models;
using Web4BDC.Tools;

namespace Web4BDC.Bll
{
    public class BDCExtendBLL
    {
        public int ResetUploadFileCreateTime(DateTime createTime,string slbh,string CID)
        {
            try
            {
                BDCExtendDal bDCExtendDal = new BDCExtendDal();
                return bDCExtendDal.ResetUploadFileCreateTime(createTime,slbh,CID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EasyUIGridModel GetAttachLst(FileUploadQueryForm form, EasyUIGridSetting gridSetting)
        {
            string sql = string.Format(@"SELECT CID,PNODE as SLBH,CNAME,to_char(CREATEDATE,'yy-mm-dd hh24:mi:ss') CREATEDATE 
 FROM (Select ROWNUM AS ROWNO, T.*
      from (select CID,PNODE,CNAME,CREATEDATE from WFM_ATTACHLST where {0}) T 
      WHERE ROWNUM <= {1}) TABLE_ALIAS
WHERE TABLE_ALIAS.ROWNO >  {2}", form.GetWhere(), gridSetting.PageIndex * gridSetting.PageSize, (gridSetting.PageIndex - 1) * gridSetting.PageSize);
            string connectStr= ConfigurationManager.ConnectionStrings["bdcggkConnection"].ToString();
            DataSet ds = DBHelper.GetDataSet(connectStr,sql);
            TBToList<AttachModel> list = new TBToList<AttachModel>();
            IList<AttachModel> tags = list.ConvertToModel(ds.Tables[0]);
            string countSql = string.Format("Select count(1) from WFM_ATTACHLST where {0}", form.GetWhere());
            int count = DBHelper.GetScalar(connectStr,countSql);
            EasyUIGridModel ret = new EasyUIGridModel(gridSetting.PageIndex, count, tags);
            return ret;
        }
    }
}