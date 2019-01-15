using FSP.Web.TowTruckServiceRef;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq;
using System.Text;
using System.Web;

namespace FSP.Web.Helpers
{
    public static class MenuItemHelper
    {
        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName, string area, string id)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

            var builder = new TagBuilder("li");

            // Add selected class
            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("active");


            // Add link
            if (!String.IsNullOrEmpty(area))
                builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = area }, new { id = id }).ToHtmlString();
            else
            {
                //if (controllerName == "AlertMessages" && actionName == "Alerts")
                //{
                //    using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                //    {
                //        if (service.GetAllAlarms().Where(p => p.SpeedingAlarm == false && p.OutOfBoundsAlarm == false && ((p.RollInAlarm == true && p.RollInAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001") || (p.RollOutAlarm == true && p.RollOutAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001") || (p.IncidentAlarm == true && p.IncidentAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001") || (p.LogOffAlarm == true && p.LogOffAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001") || (p.LogOnAlarm == true && p.LogOnAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001") || (p.GPSIssueAlarm == true && p.GPSIssueAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001") || (p.OnPatrolAlarm == true && p.OnPatrolAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))).Count() > 0)
                //        {
                //            builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = "" }, new { @style = "color:Red" }).ToHtmlString();
                //        }
                //        else
                //        {
                //            builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = "" }, null).ToHtmlString();
                //        }
                //    }
                //}
                //else
                //{
                //    builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = "" }, null).ToHtmlString();
                //}
                builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = "" }, new { id = id }).ToHtmlString();
            }


            // Render Tag Builder
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));

        }
        public static MvcHtmlString DatabaseTab(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            String applicationPath = HttpContext.Current.Request.ApplicationPath;

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole("Contractor") == false)
                {
                    sb.Append("<li><a href='" + applicationPath + "/Admin'>Database</a>");
                    sb.Append("</li>");
                }
            }
           
            return MvcHtmlString.Create(sb.ToString());

        }


        //public static MvcHtmlString MonitoringTabItemStringExtension(this HtmlHelper helper)
        //{
        //    string html = String.Empty;

        //    using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
        //    {
        //        if (service.GetAllAlarms().Where(p => p.SpeedingAlarm == false && p.OutOfBoundsAlarm == false && (p.RollInAlarm == true || p.RollOutAlarm == true || p.IncidentAlarm == true || p.LogOffAlarm == true || p.LogOnAlarm == true || p.GPSIssueAlarm == true || p.OnPatrolAlarm == true)).Count() > 0)
        //        {
        //            html = "<span style='color:Red'>Monitoring</span>";
        //        }
        //        else
        //        {
        //            html = "<span>Monitoring</span>";
        //        }
        //    }

        //    return MvcHtmlString.Create(html);

        //}
    }
}