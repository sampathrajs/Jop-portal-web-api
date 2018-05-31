using jobportal.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace jobportal
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static readonly Log4NetWrap _log = new Log4NetWrap(_fileNm);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string corsOverride = ConfigurationManager.AppSettings["CORSOverride"];

            if (string.IsNullOrWhiteSpace(corsOverride))
            {
                return;
            }

            var originHeader = Request.Headers.Get("Origin");
            originHeader = (!string.IsNullOrWhiteSpace(originHeader)) ? originHeader : "*";

            string originHeaderAllowed = ConfigurationManager.AppSettings["Access-Control-Allow-Origin"];
            originHeaderAllowed = !(string.IsNullOrWhiteSpace(originHeaderAllowed)) ? originHeaderAllowed : "*";

            if ((originHeaderAllowed.Trim() == "*") || originHeaderAllowed.ToLower().Contains(originHeader.ToLower()))
            {
                Response.Headers.Remove("Access-Control-Allow-Origin");
                Response.AddHeader("Access-Control-Allow-Origin", originHeader);

                Response.Headers.Remove("Access-Control-Allow-Credentials");
                Response.AddHeader("Access-Control-Allow-Credentials", ConfigurationManager.AppSettings["Access-Control-Allow-Credentials"]);

                Response.Headers.Remove("Access-Control-Allow-Methods");
                Response.AddHeader("Access-Control-Allow-Methods", ConfigurationManager.AppSettings["Access-Control-Allow-Methods"]);

                Response.Headers.Remove("Access-Control-Allow-Headers");
                Response.AddHeader("Access-Control-Allow-Headers", ConfigurationManager.AppSettings["Access-Control-Allow-Headers"]);

                Response.Headers.Remove("Access-Control-Expose-Headers");
                Response.AddHeader("Access-Control-Expose-Headers", ConfigurationManager.AppSettings["Access-Control-Expose-Headers"]);

                if (!string.Equals(Request.HttpMethod, "OPTIONS", StringComparison.CurrentCultureIgnoreCase)) return;
                Response.Flush();
                Response.End();
            }
        }
    }
}
