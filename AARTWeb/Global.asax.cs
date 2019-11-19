using AARTWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AARTWeb
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
        void Session_End(object sender, EventArgs e)
        {
            // perform your logic
            BaseModel audit = new BaseModel();
            string username = Session["UserName"].ToString();
            String Ip = Session["ip"].ToString();
            audit.InsertLogoutAudit("LogOut", "Successfully Logout", "Success", username, Ip);
            Session.Abandon();

        }
    }
}
