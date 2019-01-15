using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Text;

namespace FSP.Web.Helpers
{
    public static class MenuItemHelper
    {
        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName, string area)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

            var builder = new TagBuilder("li");

            // Add selected class
            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)) // && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("active");
                        
            // Add link
            if (!String.IsNullOrEmpty(area))
                builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = area }, null).ToHtmlString();
            else
                builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName, "", "", "", new { area = "" }, null).ToHtmlString();


            // Render Tag Builder
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));

        }

    }
}