using System.Web.Http;

using Slight.FileDB.Server.Actors.Filters;

namespace Slight.FileDB.Server {
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
