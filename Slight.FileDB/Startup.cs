using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Owin.Hosting;

using Owin;

using Slight.FileDB.Actors;

namespace Slight.FileDB {

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
