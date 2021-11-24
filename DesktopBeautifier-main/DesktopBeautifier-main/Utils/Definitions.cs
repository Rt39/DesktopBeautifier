using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils {
    public static class Definitions {
        public static readonly string SettingFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopBeautifer");
    }
}
