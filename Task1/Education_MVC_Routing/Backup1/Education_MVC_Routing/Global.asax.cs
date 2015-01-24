using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Education_MVC_Routing
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Homepage",
                url: "",
                defaults: new { controller = "Account", action = "LogOn" }
            );

            routes.MapRoute(
                "LogOff",
                "logoff",
                new { controller = "Account", action = "LogOff"}
            );

            routes.MapRoute(
                "LogOn",
                "logon",
                new { controller = "Account", action = "LogOn" }
            );

            routes.MapRoute(
                "Subcategories",
                "Category/{id}/subcategories",
                new { controller = "Categories", id = UrlParameter.Optional, action = "SubCategories" }
            );

            routes.MapRoute(
                "SubCategory",
                "Category/{categoryName}/{subcategoryName}",
                new { controller = "Categories", action = "SubCategory", categoryName = UrlParameter.Optional, subcategoryName = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Categories",
                "Categories/{action}/{id}",
                new { controller = "Categories", action = "List", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}