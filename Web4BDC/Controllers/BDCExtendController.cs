using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Bll;
using Web4BDC.Easyui;
using Web4BDC.Models;

namespace Web4BDC.Controllers
{
    /// <summary>
    /// 不动产拓展
    /// </summary>
    public class BDCExtendController : Controller
    {
        //
        // GET: /BDCExtend/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FilesUpdateControl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SetCreateTime(string CID,string slbh,string CreateTime)
        {
            if (string.IsNullOrEmpty(CID) || string.IsNullOrEmpty(slbh) || string.IsNullOrEmpty(CreateTime))
            { 
                return Json(new{
                    Ret=false,
                    Message = "CID或slbh或CreateTime为空"
                });
            }
            DateTime time;
            if (DateTime.TryParse(CreateTime, out time))
            {
                BDCExtendBLL bll = new BDCExtendBLL();
                if (bll.ResetUploadFileCreateTime(time, slbh, CID) > 0)
                {
                    return Json(new
                    {
                        Ret = true
                    });
                }
                else {
                    return Json(new
                    {
                        Ret = false,
                        Message = "未找到需要更新的数据"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Ret = false,
                    Message = "Createtime输入不标准"
                });
            }
        }

        public ActionResult GetAttachLst(FileUploadQueryForm form, EasyUIGridSetting gridSetting)
        {
            if (form == null || string.IsNullOrEmpty(form.Slbh))
            {
                return Content("[]", "application/json");
            }
            BDCExtendBLL bll = new BDCExtendBLL();
            EasyUIGridModel result = bll.GetAttachLst(form, gridSetting);
            return Content(result.ToJson(), "application/json");
        }
    }
}
