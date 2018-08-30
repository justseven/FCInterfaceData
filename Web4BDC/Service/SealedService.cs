using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Models;

namespace Web4BDC.Service
{
    [ModelBinder(typeof(SealedServiceBinder))]
    public interface ISealedService
    {
        DataTable GetSealedInfoFromWS(SealedQueryForm form);
    }
    public class SealedService : ISealedService
    {
        /// <summary>
        /// 从webservice中获取查封的信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSealedInfoFromWS(SealedQueryForm form)
        {
            DataSet ds = new DataSet();
            DataTable dt=new DataTable();
            dt.Columns.Add("HID");
            dt.Columns.Add("QLRXM");
            dt.Columns.Add("ZJHM");
            dt.Columns.Add("FWZL");
            DataRow row1= dt.NewRow();
            row1["HID"]=Guid.NewGuid();
            row1["QLRXM"]="葛文超";
            row1["ZJHM"] = "32068217457845214";
            row1["FWZL"]="睢宁";
            dt.Rows.Add(row1);
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }
    }

    public class SealedServiceBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return new SealedService();
        }
    }
}