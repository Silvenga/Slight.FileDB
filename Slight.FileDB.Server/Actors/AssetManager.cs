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

        public async Task Create(Stream file, string version, string md5Hash) {

            version = version.EscapeString();
        }

        public async Task<Asset> Latest() {

            return Files().OrderBy(x => x.Version).FirstOrDefault();
        }

        private IEnumerable<Asset> Files() {

            var parrentPath = ApiHelper.MapPath(Config.BasePath, Id);
            var assetsPaths = Directory.EnumerateFiles(parrentPath);

            return assetsPaths.Select(Asset.Create);
        }

        public void Dispose() {

            IsLocked = false;
        }

    }
}
