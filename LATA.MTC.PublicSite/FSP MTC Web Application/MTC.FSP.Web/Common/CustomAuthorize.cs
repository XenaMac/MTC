using System.Linq;
using System.Web.Mvc;

namespace MTC.FSP.Web.Common
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/_Unauthorized.cshtml"
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true) ||
                                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true);

            if (!skipAuthorization)
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    // The user is not authenticated
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    if (this.Roles.Any())
                    {
                        if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
                        {
                            // The user is not in any of the listed roles => 
                            // show the unauthorized view
                            filterContext.Result = new ViewResult
                            {
                                ViewName = "~/Views/Shared/_Unauthorized.cshtml"
                            };
                        }
                    }
                }
            }
        }
    }
}