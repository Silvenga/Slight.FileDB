using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slight.FileDB.Server;

namespace Slight.FileDB.Tests.Models {
    public class TestServer : IDisposable {

        public string ServerEndpoint {
            get;
            private set;
        }

        public IDisposable Server {
            get;
            private set;
        }

        public TestServer(string serverEndpoint = "http://localhost:9000/") {

            ServerEndpoint = serverEndpoint;
            Server = OwinConfiguation.CreateOwin(ServerEndpoint);
        }

        public void Dispose() {

            Server.Dispose();
        }

    }
}
