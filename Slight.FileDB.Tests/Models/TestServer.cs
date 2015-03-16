using System;
using System.IO;

using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Tests.Models {
    public class TestServer : IDisposable {

        protected static string LocalEndpoint {
            get;
            private set;
        }

        protected static string ContentDirectory {
            get;
            private set;
        }

        private static IDisposable Server {
            get;
            set;
        }

        protected TestServer(string serverEndpoint = "http://localhost:{0}/") {

            const int port = 9090; // _random.Next(49152, 65535);
            LocalEndpoint = string.Format(serverEndpoint, port);
            ContentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Content");

            Server = Server ?? OwinConfiguation.CreateOwin(LocalEndpoint, ContentDirectory);
        }

        public void Dispose() {

            //Server.Dispose();
        }

    }
}
