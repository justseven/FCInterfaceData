using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Models;
using Web4BDC.Service;

namespace Web4BDC.Controllers
{
    public class SealedController : Controller
    {
        //
        // GET: /Sealed/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetHouseInfo(SealedQueryForm form,ISealedService sealedService)
        {
            try
            {
                DataTable data = sealedService.GetSealedInfoFromWS(form);
                string ret = JsonConvert.SerializeObject(data);
                return Content(ret, "application/json");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            } 
        }
    }
}
