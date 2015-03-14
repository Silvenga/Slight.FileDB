using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slight.FileDB.Models {
    public static class Config {

        private static string _basePath = "content";
        private static string _fileDelimiter = "-";

        public static string BasePath {
            get {
                return _basePath;
            }
            set {
                _basePath = value;
            }
        }

        public static string FileDelimiter {
            get {
                return _fileDelimiter;
            }
            set {
                _fileDelimiter = value;
            }
        }

        public static string EscapeString(this string s) {

            return s.Replace(FileDelimiter, "");
        }

    }
}
