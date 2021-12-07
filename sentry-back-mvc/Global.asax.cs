using System;
using Sentry;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Sentry.AspNet;
using System.Web.Http;
using System.Web.Optimization;

namespace sentry_back_mvc
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IDisposable _sentry;

        protected void Application_Start()
        {
            // We add the query logging here so multiple DbContexts in the same project are supported
            //SentryDatabaseLogging.UseBreadcrumbs();

            // Set up the sentry SDK
            _sentry = SentrySdk.Init(o =>
            {
                // We store the DSN inside Web.config; make sure to use your own DSN!
                o.Dsn = ConfigurationManager.AppSettings["SentryDsn"];

                // When configuring for the first time, to see what the SDK is doing:
                o.Debug = true;
                // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                // We recommend adjusting this value in production.
                o.TracesSampleRate = 1.0;
            });

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

            // Global error catcher
            protected void Application_Error()
            {
                var exception = Server.GetLastError();

                // Capture unhandled exceptions
                SentrySdk.CaptureException(exception);
            }

            protected void Application_End()
            {
                // Close the Sentry SDK (flushes queued events to Sentry)
                _sentry?.Dispose();
            }

            protected void Application_BeginRequest()
            {
                // Start a transaction that encompasses the current request
                Context.StartSentryTransaction();
            }

            protected void Application_EndRequest()
            {
                Context.FinishSentryTransaction();
            }
    }
}
