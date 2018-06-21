using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CapitalSoftWebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
      //  public static string cultureName;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    HttpCookie cultureCookie = Request.Cookies["lang"];
        //    if (cultureCookie != null)
        //        cultureName = cultureCookie.Value;
        //    else
        //        cultureName = "en";
        //    List<string> cultures = new List<string>() { "ru", "en", "am" };
        //    if (!cultures.Contains(cultureName))
        //    {
        //        cultureName = "en";
        //    }
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
        //    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        //}
    }
}
