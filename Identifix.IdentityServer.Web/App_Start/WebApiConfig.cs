﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Identifix.IdentityServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Guard.IsNotNull(config, "config");
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action=RouteParameter.Optional, id = RouteParameter.Optional }
            );
        }
    }
}
