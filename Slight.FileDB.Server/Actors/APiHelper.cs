using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Server.Actors {
    public static class ApiHelper {

        public static string MapPath(params string[] paths) {

            var path = (paths.Any()) ? Path.Combine(Shared.BasePath, Path.Combine(paths)) : Shared.BasePath;

            return path;
        }

        public static string Md5HashFile(string filename) {

            using(var md5 = MD5.Create()) {
                using(var stream = File.OpenRead(filename)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }

        public static HttpResponseException Error(string message, HttpStatusCode code = HttpStatusCode.BadRequest) {

            var resp = new HttpResponseMessage(code) {
                Content = new StringContent(message),
                ReasonPhrase = message
            };

            return new HttpResponseException(resp);
        }
    }
}
