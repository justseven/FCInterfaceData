using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web4BDC.FilterAttribute
{
    public class AllowCorsAttribute : ActionFilterAttribute
    {
        private string[] _domains;

        public AllowCorsAttribute(string domain)
        {
            _domains = new string[] { domain };
        }

        public AllowCorsAttribute(string[] domains)
        {
            _domains = domains;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            var host = context.Request.UrlReferrer?.Host;
            if (host != null && _domains.Contains(host))
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}

