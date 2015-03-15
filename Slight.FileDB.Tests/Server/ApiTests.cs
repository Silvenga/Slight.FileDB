using System.IO;
using System.Linq;
using System.Net;

using RestSharp;

using Slight.FileDB.Server.Actors;
using Slight.FileDB.Server.Models;
using Slight.FileDB.Tests.Models;

using Xunit;

namespace Slight.FileDB.Tests.Server {

    public class ApiTests : TestServer {

        private const string UploadTest = "upload.test";
        private const string UploadTestFile = @"TestFiles\upload.test";

        private const string NoVersions = "noVersions";
        private const string NoFile = "noExist";
        private const string PreExists = "preExists";

        public ApiTests() {

            var uploadDirectory = ApiHelper.MapPath(Config.BasePath, UploadTest);
            if(Directory.Exists(uploadDirectory)) {
                Directory.Delete(uploadDirectory, true);
            }

            var noVersionDirectory = ApiHelper.MapPath(Config.BasePath, NoVersions);
            if(!Directory.Exists(noVersionDirectory)) {
                Directory.CreateDirectory(noVersionDirectory);
            }
        }

        [Fact]
        public void LatestMeta() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest/meta", Method.GET);
            request.AddUrlSegment("id", PreExists);

            var response = client.Execute<Asset>(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.Equal(response.Data.Id, PreExists);
            Assert.True(response.Data.Version.CompareTo("1.0.0.0") >= 0);
        }

        [Fact]
        public void Latest() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", PreExists);

            var response = client.Execute(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Hello, World!", response.Content);
            Assert.Equal("application/octet-stream", response.ContentType);
            Assert.Equal("attachment; filename=" + PreExists, response.Headers.First(x => x.Name.Equals("Content-Disposition")).Value);
        }

        [Fact]
        public void VersionMeta() {

            const string version = "1.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}/meta", Method.GET);
            request.AddUrlSegment("id", PreExists);
            request.AddUrlSegment("version", version);

            var response = client.Execute<Asset>(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.Equal(response.Data.Id, PreExists);
            Assert.True(response.Data.Version.CompareTo("1.0.0.0") == 0);
        }

        [Fact]
        public void Version() {

            const string version = "1.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}", Method.GET);
            request.AddUrlSegment("id", PreExists);
            request.AddUrlSegment("version", version);

            var response = client.Execute(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Hello, World!", response.Content);
            Assert.Equal("application/octet-stream", response.ContentType);
            Assert.Equal("attachment; filename=" + PreExists, response.Headers.First(x => x.Name.Equals("Content-Disposition")).Value);
        }

        [Theory]
        [InlineData("test", "notexists")]
        [InlineData("notexists", "1.0.0.0")]
        [InlineData("notexists", "notexists")]
        public void VersionNotFound(string id, string version) {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}", Method.GET);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("version", version);

            var response = client.Execute(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void LatestNotFound() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", NoFile);

            var response = client.Execute(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void LatestNoVersions() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", NoVersions);

            var response = client.Execute(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void Upload() {

            const string version = "2.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/upload/{version}", Method.POST);
            request.AddUrlSegment("id", UploadTest);
            request.AddUrlSegment("version", version);
            request.AddFile("file", UploadTestFile);

            var response = client.Execute<Asset>(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.Equal(response.Data.Id, UploadTest);
            Assert.True(response.Data.Version.CompareTo(version) == 0);


            var request2 = new RestRequest("api/file/{id}/latest/meta", Method.GET);
            request2.AddUrlSegment("id", UploadTest);

            var response2 = client.Execute<Asset>(request2);

            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.True(response2.Data.Valid);
            Assert.Equal(response2.Data.Id, UploadTest);
            Assert.True(response2.Data.Version.CompareTo(version) == 0);

            var request3 = new RestRequest("api/file/{id}/version/{version}/meta", Method.GET);
            request3.AddUrlSegment("id", UploadTest);
            request3.AddUrlSegment("version", version);

            var response3 = client.Execute<Asset>(request3);

            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            Assert.True(response3.Data.Valid);
            Assert.Equal(response3.Data.Id, UploadTest);
            Assert.True(response3.Data.Version.CompareTo(version) == 0);
        }

        [Fact]
        public void UploadDuplicate() {

            const string version = "2.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/upload/{version}", Method.POST);
            request.AddUrlSegment("id", UploadTest);
            request.AddUrlSegment("version", version);
            request.AddFile("file", UploadTestFile);

            client.Execute<Asset>(request);
            var response = client.Execute<Asset>(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
