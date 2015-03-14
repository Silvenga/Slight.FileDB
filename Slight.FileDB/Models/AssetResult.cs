using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Slight.FileDB.Models {
    public class AssetResult : IHttpActionResult {

        public const string DefaultMimeType = "application/octet-stream";

        public string Filepath {
            get;
            private set;
        }

        public string Id {
            get;
            set;
        }

        public AssetResult(string filePath, string id) {

            if(!File.Exists(filePath)) {
                throw new ArgumentException("File must exist.");
            }

            Filepath = filePath;
            Id = id;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {

            return Task.Run(
                delegate {

                    var response = new HttpResponseMessage(HttpStatusCode.OK) {
                        Content = new StreamContent(File.OpenRead(Filepath))
                    };

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(DefaultMimeType);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(string.Format("attachment")) {
                        FileName = Id
                    };

                    return response;

                },
                cancellationToken);
        }
    }
}
