﻿using System;
using System.Web.Http;

using Microsoft.Owin.Hosting;

using Owin;

using Slight.FileDB.Server.Actors.Filters;

namespace Slight.FileDB.Server.Models {

    public class OwinConfiguation {

        public void Configuration(IAppBuilder app) {

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Filters.Add(new LogActionFilter());

            app.UseWebApi(config);
        }

        public static IDisposable CreateOwin(string baseAddress, string baseDirectory) {

            Shared.BasePath = baseDirectory;

            return WebApp.Start<OwinConfiguation>(baseAddress);
        }
    }
}
