using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PostmarkInboundWebhookMailParser;

namespace Sample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static PostmarkIncomingMessage LastMailMessage { get; set; }
        public static string LastMailMessageJson { get; set; }
        public static DateTime LastMailMessageDate { get; set; }

        public static Exception LastError { get; set; }
        public static DateTime LastErrorDate { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
