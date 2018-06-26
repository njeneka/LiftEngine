using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LiftEngine.Domain;

namespace LiftEngine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            HttpContext.Current.Application["Lift"] = new Lift(new List<Level>
            {
                new Level("G"),
                new Level("L1"),
                new Level("L2"),
                new Level("L3"),
                new Level("L4"),
                new Level("L5"),
                new Level("L6"),
                new Level("L7"),
                new Level("L8"),
                new Level("L9"),
            });
        }
    }
}
