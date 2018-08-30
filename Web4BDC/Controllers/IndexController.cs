using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Bll;

namespace Web4BDC.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult BdcInfoExport()
        {
            BDCInfo4WWW bll = new BDCInfo4WWW();
            string fileName = Server.MapPath("~/Files/BdcStepInfo.xls");
            bll.GetBdcStepInfo(fileName);
            return File(fileName, "application/ms-excel", string.Format("不动产办件步骤信息[{0}].xls", DateTime.Now.ToString("u")));
        }


        public ActionResult BdcInfoExportT()
        {
            BDCInfo4WWW bll = new BDCInfo4WWW();
            string fileName = Server.MapPath("~/Files/BdcStepInfo.xls");
            bll.GetBdcStepInfoT(fileName);
            return File(fileName, "application/ms-excel", string.Format("不动产办件步骤信息[{0}].xls", DateTime.Now.ToString("u")));
        }
    }
}
