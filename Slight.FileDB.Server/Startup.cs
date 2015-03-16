using System;
using System.IO;
using System.Linq;

using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Server {

    public static class Startup {

        public static void Main(string[] args) {

            var host = "http://localhost:9000/";
            var content = Directory.GetCurrentDirectory();

            if(args.Length == 2) {
                host = args[0];
                content = args[1];
            }

            using(OwinConfiguation.CreateOwin(host, content)) {

                Console.WriteLine("Slight.FileDB started on {0}.", host);
                Console.WriteLine("Using {0} as the content directory.", Shared.BasePath);
                Console.WriteLine("Any key to exit.");
                Console.ReadKey();
            }

            Console.WriteLine("Exiting...");
        }
    }
}
