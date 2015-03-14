using System;

namespace Slight.FileDB.Server {

    public static class Startup {

        public static void Main(string[] args) {

            using(OwinConfiguation.CreateOwin()) {

                Console.WriteLine("Owin started.");
                Console.ReadLine();
            }
        }
    }
}
