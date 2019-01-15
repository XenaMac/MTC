using System.Web;
using System.Web.Optimization;

namespace MTC.FSP.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                   "~/Scripts/knockout-{version}.js",
                   "~/Scripts/moment.js",
                   "~/Scripts/toastr.js",
                   "~/MyScripts/app.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/appAngular").Include(
                  "~/Scripts/angular.min.js",
                  "~/Scripts/angular-sanitize.min.js",
                  "~/Scripts/angular-route.min.js",
                  "~/Scripts/angular-ui/ui-bootstrap.js",
                  "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                  "~/Scripts/googleMap/jquery.minicolors.min.js",
                  "~/Scripts/googleMap/angular-minicolors.js",
                  "~/app/app.js",
                  "~/app/filters/mtcAppFilters.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/liveIncidents").Include(
               "~/MyScripts/LiveIncidents/main.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/driverMessages").Include(
            "~/MyScripts/DriverMessages/main.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-mtc-3.3.0.css",
                      "~/Content/font-awesome.css",
                      "~/Content/toastr.css",
                      "~/Content/mtc.css"
                      ));

            

            BundleTable.EnableOptimizations = false;

        }
    }
}
