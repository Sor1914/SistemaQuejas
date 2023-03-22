using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FE_QUEJAS
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

        void Session_Start(object sender, EventArgs e)
        {
            // Establecer una hora de expiración para la sesión
            Session["ExpirationTime"] = DateTime.Now.AddMinutes(1);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Limpiar la variable Session["usuario"]
            Session["usuario"] = null;
        }
    }
}
