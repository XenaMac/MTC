using System.Web.Mvc;
using System.Web.Routing;

namespace FSP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();      

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "Truck", action = "Grid", id = UrlParameter.Optional }
            );
        }
    }
}