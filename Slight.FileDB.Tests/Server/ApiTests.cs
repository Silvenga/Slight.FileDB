using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using RestSharp;
using Slight.FileDB.Server.Actors;
using Slight.FileDB.Server.Models;
using Slight.FileDB.Tests.Models;

namespace Slight.FileDB.Tests.Server {

    [TestFixture]
    public class ApiTests : TestServer {

        private const string UploadTest = "upload.test";
        private static readonly string UploadTestFile = Path.Combine("TestFiles", "upload.test");

        private const string NoVersions = "noVersions";
        private const string NoFile = "noExist";
        private const string PreExists = "preExists";

        public ApiTests() {

            var uploadDirectory = ApiHelper.MapPath(UploadTest);
            if(Directory.Exists(uploadDirectory)) {
                Directory.Delete(uploadDirectory, true);
            }

            var noVersionDirectory = ApiHelper.MapPath(NoVersions);
            if(!Directory.Exists(noVersionDirectory)) {
                Directory.CreateDirectory(noVersionDirectory);
            }
        }

        [Test]
        public void LatestMeta() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest/meta", Method.GET);
            request.AddUrlSegment("id", PreExists);

            var response = client.Execute<Asset>(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.AreEqual(response.Data.Id, PreExists);
            Assert.True(response.Data.Version.CompareTo("1.0.0.0") >= 0);
        }

        [Test]
        public void Latest() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", PreExists);

            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Hello, World!", response.Content);
            Assert.AreEqual("application/octet-stream", response.ContentType);
            Assert.AreEqual("attachment; filename=" + PreExists, response.Headers.First(x => x.Name.Equals("Content-Disposition")).Value);
        }

        [Test]
        public void VersionMeta() {

            const string version = "1.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}/meta", Method.GET);
            request.AddUrlSegment("id", PreExists);
            request.AddUrlSegment("version", version);

            var response = client.Execute<Asset>(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.AreEqual(response.Data.Id, PreExists);
            Assert.True(response.Data.Version.CompareTo("1.0.0.0") == 0);
        }

        [Test]
        public void Version() {

            const string version = "1.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}", Method.GET);
            request.AddUrlSegment("id", PreExists);
            request.AddUrlSegment("version", version);

            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Hello, World!", response.Content);
            Assert.AreEqual("application/octet-stream", response.ContentType);
            Assert.AreEqual("attachment; filename=" + PreExists, response.Headers.First(x => x.Name.Equals("Content-Disposition")).Value);
        }


        [TestCase("test", "notexists")]
        [TestCase("notexists", "1.0.0.0")]
        [TestCase("notexists", "notexists")]
        public void VersionNotFound(string id, string version) {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/version/{version}", Method.GET);
            request.AddUrlSegment("id", id);
            request.AddUrlSegment("version", version);

            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public void LatestNotFound() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", NoFile);

            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public void LatestNoVersions() {

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/latest", Method.GET);
            request.AddUrlSegment("id", NoVersions);

            var response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public void Upload() {

            const string version = "2.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/upload/{version}", Method.POST);
            request.AddUrlSegment("id", UploadTest);
            request.AddUrlSegment("version", version);
            request.AddFile("file", UploadTestFile);

            var response = client.Execute<Asset>(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data.Valid);
            Assert.AreEqual(response.Data.Id, UploadTest);
            Assert.True(response.Data.Version.CompareTo(version) == 0);


            var request2 = new RestRequest("api/file/{id}/latest/meta", Method.GET);
            request2.AddUrlSegment("id", UploadTest);

            var response2 = client.Execute<Asset>(request2);

            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            Assert.True(response2.Data.Valid);
            Assert.AreEqual(response2.Data.Id, UploadTest);
            Assert.True(response2.Data.Version.CompareTo(version) == 0);

            var request3 = new RestRequest("api/file/{id}/version/{version}/meta", Method.GET);
            request3.AddUrlSegment("id", UploadTest);
            request3.AddUrlSegment("version", version);

            var response3 = client.Execute<Asset>(request3);

            Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
            Assert.True(response3.Data.Valid);
            Assert.AreEqual(response3.Data.Id, UploadTest);
            Assert.True(response3.Data.Version.CompareTo(version) == 0);
        }

        [Test]
        public void UploadDuplicate() {

            const string version = "2.0.0.0";

            var client = new RestClient(LocalEndpoint);

            var request = new RestRequest("api/file/{id}/upload/{version}", Method.POST);
            request.AddUrlSegment("id", UploadTest);
            request.AddUrlSegment("version", version);
            request.AddFile("file", UploadTestFile);

            client.Execute<Asset>(request);
            var response = client.Execute<Asset>(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
