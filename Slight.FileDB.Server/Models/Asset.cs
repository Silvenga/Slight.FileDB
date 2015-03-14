using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Slight.FileDB.Server.Actors;

namespace Slight.FileDB.Server.Models {
    public class Asset {

        public string Version {
            get;
            set;
        }

        public string Id {
            get;
            set;
        }

        public string Md5Hash {
            get;
            set;
        }

        public bool IsValid {
            get {
                var path = ApiHelper.MapPath(Config.BasePath, Filename);
                return File.Exists(path) && ApiHelper.Md5HashFile(path).Equals(Md5Hash);
            }
        }

        public string Folder {
            get {
                return Id;
            }
        }

        public string Filename {
            get {
                return string.Format("{0}{1}{2}{3}{4}", Folder, Path.DirectorySeparatorChar, Version, Config.FileDelimiter, Md5Hash);
            }
        }

        public static Asset Read(string path) {

            if(!File.Exists(path)) {
                throw new Exception("File does not exist.");
            }

            var parrentDir = Directory.GetParent(path);
            var id = parrentDir.Name;

            var rawName = Path.GetFileName(path);

            Debug.Assert(rawName != null, "rawName != null");
            var list = rawName.Split(
                    new[] {
                    Config.FileDelimiter
                },
                    StringSplitOptions.RemoveEmptyEntries);

            var version = list.First();
            var md5Hash = list.Last();

            return new Asset {
                Id = id,
                Md5Hash = md5Hash,
                Version = version
            };
        }

        public AssetResult ToResult() {

            return new AssetResult(ApiHelper.MapPath(Config.BasePath, Filename), Id);
        }
    }
}
