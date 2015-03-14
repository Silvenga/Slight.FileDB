using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Server.Actors {

    public class FileController : ApiController {

        public const string BasePath = "";

        [HttpGet]
        public async Task<Asset> LatestMeta(string id) {

            using(var manager = new AssetManager(id)) {

                return await manager.Latest();
            }
        }

        [HttpGet]
        public async Task<AssetResult> Latest(string id) {

            using(var manager = new AssetManager(id)) {

                var asset = await manager.Latest();
                return asset.ToResult();
            }
        }


        [HttpGet]
        public async Task<Asset> Version(string id) {

            using(var manager = new AssetManager(id)) {

                return await manager.Latest();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Upload(string id, [FromBody]string version, [FromBody]string md5Hash) {

            if(!Request.Content.IsMimeMultipartContent()) {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = provider.Contents.FirstOrDefault();

            if(file == null) {
                return BadRequest("No files found in the multipart upload. ");
            }

            var stream = await file.ReadAsStreamAsync();

            using(var manager = new AssetManager(id)) {

                await manager.Create(stream, version, md5Hash);
            }

            return Ok();
        }
    }
}
