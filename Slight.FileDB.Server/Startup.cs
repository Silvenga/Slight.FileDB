using System;

using Microsoft.Owin.Hosting;

using Owin;

namespace Slight.FileDB.Server {

    class Startup {

        static void Main(string[] args) {

            const string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using(WebApp.Start<Startup>(url: baseAddress)) {

                Console.WriteLine("Owin started.");
                Console.ReadLine();
            }
        }

        public void Configuration(IAppBuilder app) {

            var config = new Configuation();
            app.UseWebApi(config);
        }
    }
}
