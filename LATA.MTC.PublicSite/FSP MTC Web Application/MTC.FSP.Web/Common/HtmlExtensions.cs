using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MTC.FSP.Web.Common
{    
    public static class HtmlExtensions
    {
        public static MvcHtmlString HtmlMenu(this HtmlHelper helper)
        {
            var sb = new StringBuilder();
            var applicationPath = HttpContext.Current.Request.ApplicationPath;
            //var applicationPath = "http://" + HttpContext.Current.Request.Url.Authority;

#if(TOLGAPC)
            applicationPath = "http://" + HttpContext.Current.Request.Url.Authority;
#endif

            var currentControllerName = (string) helper.ViewContext.RouteData.Values["controller"];
            var currentActionName = (string) helper.ViewContext.RouteData.Values["action"];


            if (!HttpContext.Current.User.Identity.IsAuthenticated) return MvcHtmlString.Create(sb.ToString());

            #region Monitoring

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("Monitoring <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");

                sb.Append("<li><a href='" + applicationPath + "/Map/Index'>Live Map</a></li>");

                if (!HttpContext.Current.User.IsInRole("DataConsultant") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/LiveIncidents/Index'>Live Incidents</a></li>");

                if (!HttpContext.Current.User.IsInRole("DataConsultant") &&
                    !HttpContext.Current.User.IsInRole("TowContractor") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/TruckStatus/Index'>Truck Status</a></li>");

                if (!HttpContext.Current.User.IsInRole("DataConsultant"))
                    sb.Append("<li><a href='" + applicationPath + "/DailySchedule/Index'>Daily Schedule</a></li>");

                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region Operations

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                !HttpContext.Current.User.IsInRole("DataConsultant") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("Operations <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");

                if (!HttpContext.Current.User.IsInRole("TowContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/Alerts/Index'>Alarms Management</a></li>");

                if (!HttpContext.Current.User.IsInRole("CHPOfficer"))
                    sb.Append("<li><a href='" + applicationPath + "/BackupTrucks/Index'>Back-ups</a></li>");

                if (!HttpContext.Current.User.IsInRole("TowContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/DriverMessages/Index'>Messaging</a></li>");

                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region CHP

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                !HttpContext.Current.User.IsInRole("TowContractor") &&
                !HttpContext.Current.User.IsInRole("DataConsultant") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("CHP <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");
                sb.Append("<li><a href='" + applicationPath + "/CHPInspections/Index'>Inspections</a></li>");
                sb.Append("<li><a href='" + applicationPath + "/Investigations/Index'>Investigations</a></li>");
                sb.Append("<li><a href='" + applicationPath +
                          "/DriverInteractions/Index'>Driver Contacts</a></li>");
                sb.Append("<li><a href='" + applicationPath + "/Dispatching/Index'>Dispatching</a></li>");
                sb.Append("<li><a href='" + applicationPath + "/OverTimeActivities/Index'>Overtime</a></li>");
                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region Fleet Management

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("Fleet Management <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");

                if (!HttpContext.Current.User.IsInRole("TowContractor") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/Violations/Index'>Violations</a></li>");


                if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("TowContractor") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/Invoice/Index'>Invoices</a></li>");


                if (!HttpContext.Current.User.IsInRole("CHPOfficer"))
                    sb.Append("<li><a href='" + applicationPath +
                              "/TroubleTickets/Index'>Trouble Tickets</a></li>");

                if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("DataConsultant"))
                    sb.Append("<li><a href='" + applicationPath + "/Appeals/Index'>Appeals</a></li>");


                if (!HttpContext.Current.User.IsInRole("MTC") &&
                    !HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/Merchandise/Order'>Order Supplies</a></li>");

                if (!HttpContext.Current.User.IsInRole("FSPPartners") &&
                    !HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath +
                              "/Merchandise/OrderHistory'>Order History</a></li>");

                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region Data

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("Data <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");

                //if (!HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                //    !HttpContext.Current.User.IsInRole("DataConsultant"))
                //{
                //    sb.Append("<li><a href='" + applicationPath + "/Home/Temp'>Historical map</a></li>");
                //}

                //if (!HttpContext.Current.User.IsInRole("TowContractor") &&
                //    !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                //    !HttpContext.Current.User.IsInRole("DataConsultant"))
                //{
                //    sb.Append("<li><a href='" + applicationPath + "/Database/Index'>Database</a></li>");
                //}

                sb.Append("<li><a href='" + applicationPath + "/Reports/Index'>Reports</a></li>");

                if (!HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + Utilities.GetApplicationSettingValue("MotoristSurveyUrl") +
                              "'>Motorists Surveys</a></li>");

                //if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                //    !HttpContext.Current.User.IsInRole("TowContractor") &&
                //    !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                //    !HttpContext.Current.User.IsInRole("DataConsultant"))
                //{
                //    sb.Append("<li><a href='" + applicationPath + "/Home/Temp'>Cellular Surveys</a></li>");
                //}

                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region Administration

            if (!HttpContext.Current.User.IsInRole("GeneralUser") &&
                !HttpContext.Current.User.IsInRole("Guest"))
            {
                sb.Append("<li class='dropdown'>");
                sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
                sb.Append("Administration <span class='caret'></span>");
                sb.Append("</a>");
                sb.Append("<ul class='dropdown-menu'>");

                if (!HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/ShiftEntry/Index'>Incident Entry</a></li>");

                if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor"))
                    sb.Append("<li><a href='" + applicationPath + "/Scheduling/Index'>Scheduling</a></li>");

                if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("TowContractor") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                    !HttpContext.Current.User.IsInRole("DataConsultant"))
                    if (!string.IsNullOrEmpty(Utilities.GetApplicationSettingValue("DashboardUrl")))
                        sb.Append("<li><a href='" + Utilities.GetApplicationSettingValue("DashboardUrl") +
                                  "'>Dashboard</a></li>");

                if (!HttpContext.Current.User.IsInRole("CHPOfficer") &&
                    !HttpContext.Current.User.IsInRole("TowContractor") &&
                    !HttpContext.Current.User.IsInRole("InVehicleContractor") &&
                    !HttpContext.Current.User.IsInRole("DataConsultant"))
                    sb.Append("<li><a href='" + applicationPath + "/Tables/Index'>Tables</a></li>");

                //if (HttpContext.Current.User.IsInRole("Admin"))
                //{
                //    sb.Append("<li><a href='" + applicationPath + "/Home/Temp'>Management</a></li>");
                //}

                if (HttpContext.Current.User.IsInRole("Admin") ||
                    HttpContext.Current.User.IsInRole("DataConsultant"))
                    sb.Append("<li><a href='" + applicationPath +
                              "/AssetStatusLocations/Index'>Asset Tracking</a></li>");

                if (!HttpContext.Current.User.IsInRole("FSPPartner"))
                    sb.Append("<li><a href='" + applicationPath +
                              "/Administration/AboutReport'>About/Report a Bug</a></li>");

                sb.Append("</ul>");
                sb.Append("</li>");
            }

            #endregion

            #region FAQ

            sb.Append("<li class='dropdown'>");
            sb.Append("<a class='dropdown-toggle' data-toggle='dropdown' href='#'>");
            sb.Append("FAQ <span class='caret'></span>");
            sb.Append("</a>");
            sb.Append("<ul class='dropdown-menu'>");
            sb.Append("<li><a href='" + applicationPath + "/FAQs/Index'>FAQs</a></li>");
            sb.Append("<li><a href='" + applicationPath + "/FAQs/TrainingMedia'>Training Media</a></li>");
            sb.Append("</ul>");
            sb.Append("</li>");

            #endregion

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}