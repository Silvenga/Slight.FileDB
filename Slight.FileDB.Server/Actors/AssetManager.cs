using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

                    return asset.Valid;
                });
        }

        public async Task<bool> Exists(string version, string md5Hash) {

            return await Task.Run(
                delegate {
                    var asset = new Asset {
                        Id = Id,
                        Version = version,
                        Md5Hash = md5Hash
                    };

                    return asset.Exits;
                });
        }

        public async Task<Asset> Create(Stream file, string version) {

            if(string.IsNullOrWhiteSpace(version)) {
                throw ApiHelper.Error("'Version' is required.");
            }

            var asset = new Asset {
                Id = Id,
                Version = version.EscapeString()
            };

            var contentPath = ApiHelper.MapPath(Config.BasePath);
            var parrentPath = Path.Combine(contentPath, asset.Folder);

            if(!Directory.Exists(parrentPath)) {
                Directory.CreateDirectory(parrentPath);
            }

            var tmpPath = Path.Combine(parrentPath, ".tmp");

            if(File.Exists(tmpPath)) {
                File.Delete(tmpPath);
            }

            using(var fileStream = File.OpenWrite(tmpPath)) {

                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(fileStream);
            }

            asset.Md5Hash = ApiHelper.Md5HashFile(tmpPath);

            if(asset.Exits) {
                File.Delete(tmpPath);
                throw ApiHelper.Error("File version exists.");
            }

            File.Move(tmpPath, Path.Combine(contentPath, asset.Filename));

            return asset;
        }

        public async Task<Asset> Latest() {

            return await Task.Run(
                delegate {
                    return Files().OrderBy(x => x.Version).FirstOrDefault();
                });
        }

        public async Task<Asset> Version(string version) {

            return await Task.Run(
                delegate {
                    return Files().FirstOrDefault(x => x.Version == version);
                });
        }

        private IEnumerable<Asset> Files() {

            var parrentPath = ApiHelper.MapPath(Config.BasePath, Id);

            if(!Directory.Exists(parrentPath)) {
                throw ApiHelper.Error("File has no candidates.", HttpStatusCode.NotFound);
            }

            var assetsPaths = Directory.EnumerateFiles(parrentPath);

            return assetsPaths.Select(Asset.Read);
        }

        public void Dispose() {

            IsLocked = false;
        }

    }
}
