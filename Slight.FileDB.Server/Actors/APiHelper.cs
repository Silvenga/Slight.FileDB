using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Slight.FileDB.Server.Actors {
    public static class ApiHelper {

        public static string MapPath(params string[] paths) {

            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            Debug.Assert(basePath != null, "basePath != null");
            var path = Path.Combine(new Uri(basePath).LocalPath, Path.Combine(paths));

            return path;
        }

        public static string Md5HashFile(string filename) {

            using(var md5 = MD5.Create()) {
                using(var stream = File.OpenRead(filename)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }
    }
}
