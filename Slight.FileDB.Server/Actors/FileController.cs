using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Slight.FileDB.Server.Actors.Exceptions;
using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Server.Actors {

    public class FileController : ApiController {

        public const string BasePath = "";

        [HttpGet]
        public async Task<Asset> LatestMeta(string id) {

            using(var manager = new AssetManager(id)) {

                var asset = await manager.Latest();

                if(asset == null) {
                    throw ApiHelper.Error("No versions found.", HttpStatusCode.NotFound);
                }

                return asset;
            }
        }

        [HttpGet]
        public async Task<AssetResult> Latest(string id) {

            using(var manager = new AssetManager(id)) {

                var asset = await manager.Latest();

                if(asset == null) {
                    throw ApiHelper.Error("No versions found.", HttpStatusCode.NotFound);
                }

                return asset.ToResult();
            }
        }


        [HttpGet]
        public async Task<Asset> VersionMeta(string id, string version) {

            using(var manager = new AssetManager(id)) {

                var asset = await manager.Version(version);

                if(asset == null) {
                    throw ApiHelper.Error("No versions found.", HttpStatusCode.NotFound);
                }

                return asset;
            }
        }

        [HttpGet]
        public async Task<AssetResult> Version(string id, string version) {

            using(var manager = new AssetManager(id)) {

                var asset = await manager.Version(version);

                if(asset == null) {
                    throw ApiHelper.Error("No versions found.", HttpStatusCode.NotFound);
                }

                return asset.ToResult();
            }
        }

        [HttpPost]
        public async Task<Asset> Upload(string id, string version) {

            if(!Request.Content.IsMimeMultipartContent()) {
                throw ApiHelper.Error("Unsupported media type.", HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();

            if(file == null) {
                throw ApiHelper.Error("File is required.");
            }

            var stream = await file.ReadAsStreamAsync();

            using(var manager = new AssetManager(id)) {

                return await manager.Create(stream, version);
            }
        }
    }
}
