using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Slight.FileDB.Server.Actors.Exceptions;
using Slight.FileDB.Server.Models;

namespace Slight.FileDB.Server.Actors {
    public class AssetManager : IDisposable {

        public static bool IsLocked {
            get;
            private set;
        }

        public string Id {
            get;
            set;
        }

        public AssetManager(string id) {

            if(IsLocked) {
                throw new LockedException();
            }

            Id = id;
            IsLocked = true;
        }

        public async Task<bool> ExistsAndValid(string version, string md5Hash) {

            return await Task.Run(
                delegate {
                    var asset = new Asset {
                        Id = Id,
                        Version = version,
                        Md5Hash = md5Hash
                    };

                    return asset.IsValid;
                });
        }

        public async Task Create(Stream file, string version, string md5Hash) {

            version = version.EscapeString();

            var asset = new Asset {
                Id = Id,
                Version = version,
                Md5Hash = md5Hash
            };

            var contentPath = ApiHelper.MapPath(Config.BasePath);
            var assetPath = Path.Combine(contentPath, asset.Filename);

            using(var fileStream = File.OpenWrite(assetPath)) {

                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(fileStream);
            }
        }

        public async Task<Asset> Latest() {

            return await Task.Run(
                delegate {
                    return Files().OrderBy(x => x.Version).FirstOrDefault();
                });
        }

        private IEnumerable<Asset> Files() {

            var parrentPath = ApiHelper.MapPath(Config.BasePath, Id);
            var assetsPaths = Directory.EnumerateFiles(parrentPath);

            return assetsPaths.Select(Asset.Read);
        }

        public void Dispose() {

            IsLocked = false;
        }

    }
}
