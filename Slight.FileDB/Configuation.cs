using System.Web.Http;

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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {
                    id = RouteParameter.Optional
                }
                
            );

            Filters.Add(new LogActionFilter());
        }

    }
}
