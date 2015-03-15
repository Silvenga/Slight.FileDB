using System;
using System.Web.Http;

using Microsoft.Owin.Hosting;

using Owin;

using Slight.FileDB.Server.Actors.Filters;

namespace Slight.FileDB.Server {

    public class OwinConfiguation {

        public void Configuration(IAppBuilder app) {

            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "Api0",
                routeTemplate: "api/{controller}/{id}/{action}/{version}"
                );

            config.Routes.MapHttpRoute(
                name: "Api1",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new {
                    id = RouteParameter.Optional
                }
                );

            config.Routes.MapHttpRoute(
                name: "Api2",
                routeTemplate: "api/{controller}/{action}"
                );

            config.Filters.Add(new LogActionFilter());

            app.UseWebApi(config);
        }

        public static IDisposable CreateOwin(string baseAddress = "http://localhost:9000/") {

            return WebApp.Start<OwinConfiguation>(baseAddress);
        }

    }
}
