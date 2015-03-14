using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slight.FileDB.Server;

namespace Slight.FileDB.Tests.Models {
    public class TestServer : IDisposable {

        private Random _random = new Random();

        public string ServerEndpoint {
            get;
            private set;
        }

        public IDisposable Server {
            get;
            private set;
        }

        public TestServer(string serverEndpoint = "http://localhost:{0}/") {

            ServerEndpoint = string.Format(serverEndpoint, _random.Next(49152, 65535));
            Server = OwinConfiguation.CreateOwin(ServerEndpoint);
        }

        public void Dispose() {

            Server.Dispose();
        }

    }
}
