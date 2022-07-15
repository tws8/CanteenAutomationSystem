using static CanteenAutomationSystem.Include.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CanteenAutomationSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            HttpContext.Current.Session["_SYS_NAME"] = gSystemName.ToString();
            HttpContext.Current.Session["_SYS_NAME_SHORT"] = gSystemName_Short.ToString();
            HttpContext.Current.Session["_USERNAME"] = "";
            HttpContext.Current.Session["_USERTYPE"] = "";
        }

        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (Request.Cookies["User"] != null)
            {
                if (!HttpContext.Current.Session["_USERNAME"].Equals(Request.Cookies["User"].Value))
                {
                    HttpContext.Current.Session["_USERNAME"] = Request.Cookies["User"].Value;
                    HttpContext.Current.Session["_USERTYPE"] = Request.Cookies["Type"].Value;
                    iniVariable(); //Assign accessible level to Variable
                }
            }
        }
    }
}
