using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanteenAutomationSystem.Controllers
{
    [Authorize]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["USERNAME"] == null || HttpContext.Current.Session["USERNAME"].ToString() == "")
            {
                filterContext.Result = new RedirectResult("~/UserLogin/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}