using Mascot.UtilClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot.Classes {
    public class DesktopFileWatcher : IDisposable {
        private static readonly string _settingFilePath = Path.Combine(Utils.Definitions.SettingFolder, "FileWatch", "setting.json");

        private FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        private FileWatchSettings _fileWatchSettings;

        public event EventHandler<FileSystemEventArgs> NewFile;


        public void Dispose() {
            _fileSystemWatcher.Dispose();
        }
    }
}
