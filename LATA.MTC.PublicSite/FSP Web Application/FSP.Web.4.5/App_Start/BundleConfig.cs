using System.Web;
using System.Web.Optimization;

namespace FSP.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-{version}"));


            //jqueryVal
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            //jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //jqueryUI
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            //bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-tooltip.js",
                      "~/Scripts/bootstrap-popover.js"
                //"~/Scripts/bootstrap-datepicker.js"
                      ));

            //app
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/sortable.js"
                ));


            //KO + signalR
            bundles.Add(new ScriptBundle("~/bundles/KOAndSignarlR").Include(
                    "~/Scripts/knockout-{version}.js",
                    "~/Scripts/jquery.signalR-{version}.js"
                    ));

            //KO
            bundles.Add(new ScriptBundle("~/bundles/KO").Include(
                    "~/Scripts/knockout-{version}.js"
                    ));

            //dispatch
            bundles.Add(new ScriptBundle("~/bundles/fspDispatch").Include(
                "~/MyScripts/fsp.constructor.js",
                "~/MyScripts/fsp.dispatchViewModel.js",
                "~/MyScripts/fsp.truckCollection.js"
                ));

            //dispatch
            bundles.Add(new ScriptBundle("~/bundles/fspAlertMessages").Include(
                "~/MyScripts/fsp.constructor.js",
                "~/MyScripts/fsp.alertMessagesViewModel.js",
                "~/MyScripts/fsp.truckCollection.js"
                ));

            //dispatch
            bundles.Add(new ScriptBundle("~/bundles/fspAlarmHistory").Include(
                "~/Scripts/jquery.ui-{version}.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/moment.js",
                "~/MyScripts/alarmHistory.js"
                ));

            //dispatch
            bundles.Add(new ScriptBundle("~/bundles/fspDriversAlertComments").Include(
                "~/Scripts/jquery.ui-{version}.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/moment.js",
                "~/MyScripts/driversAlertComments.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/alertsViewModel").Include(
              "~/Scripts/moment.js",
              "~/MyScripts/fsp.constructor.js",
              "~/MyScripts/fsp.alertsViewModel.js"
              ));

            //truckList
            bundles.Add(new ScriptBundle("~/bundles/fspTruckList").Include(
                "~/MyScripts/fsp.constructor.js",
                "~/MyScripts/fsp.gridViewModel.js",
                "~/MyScripts/fsp.truckCollection.js"
                ));



            //assit
            bundles.Add(new ScriptBundle("~/bundles/fspAssist").Include(
                "~/MyScripts/fsp.constructor.js",
                "~/MyScripts/fsp.assistViewModel.js"
                ));

            //assit
            bundles.Add(new ScriptBundle("~/bundles/fspIncident").Include(
                "~/MyScripts/fsp.constructor.js",
                "~/MyScripts/fsp.incidentViewModel.js"
                ));

            //map
            bundles.Add(new ScriptBundle("~/bundles/fspMap").Include(
             "~/Scripts/markerwithlabel.js",
             "~/Scripts/RichMarker.js",
             "~/Sprites/Map/TruckMarkerOverlay.js",
                //"~/Scripts/jQueryRotateCompressed.2.1.js",
             "~/Scripts/Tooltip.js",
             "~/MyScripts/fsp.map.LabelInFront.js",
             "~/MyScripts/fsp.constructor.js",
             "~/MyScripts/fsp.mapViewModel.js",
             "~/MyScripts/fsp.truckCollection.js"
            ));


            //styles

            //bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
            //   "~/Content/bootstrap204/bootstrap.css",
            //   "~/Content/bootstrap204/bootstrap-responsive.css"
            //   ));


            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
               "~/Content/bootstrap232/css/bootstrap.css",
               "~/Content/bootstrap232/css/bootstrap-responsive.css"
               ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/FSP.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jqueryUI").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}