using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MTC.FSP.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
//#if(TOLGAPC || Debug)
            //automatically update target database.
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MTCDbContext, MTC.FSP.Web.Migrations.MTCDbContext.Configuration>());
//#endif
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var backupEmailNotificationTimer = new BackupEmailNotificationTimer();
        }
    }
}