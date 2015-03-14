using System;

namespace Slight.FileDB.Server.Actors.Exceptions {
    public class LockedException : Exception {

        public LockedException()
            : base("An attempt to lock failed.") {

        }
    }
}
