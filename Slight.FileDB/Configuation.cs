﻿using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Slight.FileDB.Actors.Filters;

namespace Slight.FileDB {
    class Configuation : HttpConfiguration {

        public Configuation() {
            ConfigureRoutes();
        }

        private void ConfigureRoutes() {

            Routes.MapHttpRoute(
                name: "Api1",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {
                    id = RouteParameter.Optional
                }
                );

            Routes.MapHttpRoute(
                name: "Api2",
                routeTemplate: "api/{controller}/{action}"
                );

            Filters.Add(new LogActionFilter());
        }

    }
}
