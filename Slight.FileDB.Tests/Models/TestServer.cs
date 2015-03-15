using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slight.FileDB.Server;

namespace Slight.FileDB.Tests.Models {
    public class TestServer : IDisposable {

        private Random _random = new Random();

        protected static string LocalEndpoint {
            get;
            private set;
        }

        private static IDisposable Server {
            get;
            set;
        }

        protected TestServer(string serverEndpoint = "http://localhost:{0}/") {

            var port = 9090;// _random.Next(49152, 65535);
            LocalEndpoint = string.Format(serverEndpoint, port);
            Server = Server ?? OwinConfiguation.CreateOwin(LocalEndpoint);
        }

        public void Dispose() {

            //Server.Dispose();
        }

    }
}
