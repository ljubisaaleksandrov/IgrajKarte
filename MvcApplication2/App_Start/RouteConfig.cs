﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IgrajKarte.Filters;

namespace IgrajKarte
{
    public class RouteConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            /*
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogonAuthorize());
            filters.Add(new VerifyFilter());
            filters.Add(new TermsFilter());
            filters.Add(new ProfileFilter());
            filters.Add(new IdentityFilter());
            filters.Add(new SecureFilter());
            filters.Add(new ViewBagAttribute());
            */
        }


        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}