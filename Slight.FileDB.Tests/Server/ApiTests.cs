using System.Net;

using RestSharp;

using Slight.FileDB.Server.Models;
using Slight.FileDB.Tests.Models;

using Xunit;

namespace Slight.FileDB.Tests.Server {

    public class ApiTests : TestServer {

        [Fact]
        public void TestMethod1() {

            const string id = "test";

            var client = new RestClient(ServerEndpoint);

            var request = new RestRequest("api/file/{id}/latestmeta", Method.GET);
            request.AddUrlSegment("id", id);

            var response = client.Execute<Asset>(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.IsValid);
            Assert.Equal(response.Data.Id, id);
            Assert.True(response.Data.Version.CompareTo("1.0.0.0") >= 0);
        }
    }
}
