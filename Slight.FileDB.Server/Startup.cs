using System;
using System.Linq;

namespace Slight.FileDB.Server {

    public static class Startup {

        public static void Main(string[] args) {

            var host = "http://localhost:9000/";

            if(args.Any() && !string.IsNullOrWhiteSpace(args.First())) {
                host = args.First();
            }

            using(OwinConfiguation.CreateOwin(host)) {

                Console.WriteLine("Slight.FileDB started on {0}.", host);
                Console.WriteLine("Any key to exit.");
                Console.ReadKey();
            }

            Console.WriteLine("Exiting...");
        }
    }
}
