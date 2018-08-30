using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web4BDC.Bll.JGCX;

namespace Web4BDC.Controllers
{
    public class JGCXController : Controller
    {
        //
        // GET: /JGCX/
        public FileResult Download(string fileName)
        {
            string filePath = Server.MapPath("~/Activex/"+fileName);//路径
            return File(filePath, "text/plain", fileName); //welcome.txt是客户端保存的名字
        }
        public ActionResult Index(string slbh)
        {
            if (!string.IsNullOrEmpty(slbh))
            {
                string cxlx = GetCXLX(slbh);
                string viewName = string.Empty;
                object source = null;
                Dictionary<string, string> map = new Dictionary<string, string>();

                switch (cxlx)
                {
                    case "不动产登记信息查询证明(家庭住房状况)":
                        source = GetQSQKSource(slbh);
                        viewName = "QSQKCX";
                        break;
                    case "不动产登记信息查询结果":
                        source = GetQSQKSource(slbh);
                        viewName = "QSQKCX";// "DJXXCX";
                        break;
                    case "不动产抵押权登记信息查询结果":
                        source = GetDYQKSource(slbh);
                        viewName = "DYQKCX";
                        break;
                    case "不动产预告登记信息查询结果":
                        source = GetYGQKSource(slbh);
                        viewName = "YGXXCX";
                        break;
                    case "不动产查封登记信息查询结果":
                        source = GetCFQKSource(slbh);
                        viewName = "CFQKCX";
                        break;
                    case "不动产异议登记信息查询结果":
                        source = GetYYQKSource(slbh);
                        viewName = "YYXXCX";
                        break;
                    case "不动产自然状况查询":
                        source = GetFWQKSource(slbh);
                        viewName = "ZRZYQKCX";
                        break;
                }

                return ReturnView(viewName, source);
            }
            return View();
        }

        private object GetFWQKSource(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetZRZYQK(slbh);
        }

        private object GetYYQKSource(string slbh)
        {
            throw new NotImplementedException();
        }

        private object GetCFQKSource(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetCFQK(slbh);
        }

        private object GetYGQKSource(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetYGQK(slbh);
        }

        private object GetDYQKSource(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetDYQK(slbh);
        }

        private object GetDJQKSource(string slbh)
        {
            throw new NotImplementedException();
        }

        private object GetQSQKSource(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetQSQK(slbh);
        }

        private string GetCXLX(string slbh)
        {
            JGCXBLL bll = new JGCXBLL();
            return bll.GetCXLX(slbh);
        }

        public ActionResult ReturnView(string viewName,object source)
        {
            return View(viewName,source);
        }

    }
}
