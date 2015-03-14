using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slight.FileDB.Actors.Exceptions {
    public class LockedException : Exception {

        public LockedException()
            : base("An attempt to lock failed.") {

        }
    }
}
