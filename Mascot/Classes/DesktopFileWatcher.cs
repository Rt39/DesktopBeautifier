using Mascot.UtilClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot.Classes {
    public class DesktopFileWatcher : IDisposable {
        private static readonly string _folder = Path.Combine(Utils.Definitions.SettingFolder, "FileWatch");
        private static readonly string _settingFilePath = Path.Combine(_folder, "setting.json");

        private readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        public FileWatchSettings FileWatchSettings { get; set; }

        public event EventHandler<FileSystemEventArgs> NewFile;

        public DesktopFileWatcher() {
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
            if (File.Exists(_settingFilePath)) {
                string setting;
                using (StreamReader sr = new StreamReader(_settingFilePath))
                    setting = sr.ReadToEnd();
                FileWatchSettings = JsonConvert.DeserializeObject<FileWatchSettings>(setting);
            }
            else {
                FileWatchSettings = new FileWatchSettings
                {
                    ExtentionExceptions = ".lnk"
                };
                Serialize();
            }
            FileWatchSettings.IgnoreFolderChanged += ChangeIgnoreFolder;

            _fileSystemWatcher.NotifyFilter = FileWatchSettings.IsIgnoreFolder ? NotifyFilters.FileName : NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Changed += OnCreate;
        }

        // 过滤不必要的事件
        private void OnCreate(object sender, FileSystemEventArgs e) {
            string extention = Path.GetExtension(e.FullPath);
            if (FileWatchSettings.ExtentionExceptionList.Contains(extention)) return;
            NewFile(this, e);
        }

        // 修改监听文件夹
        private void ChangeIgnoreFolder(object sender, EventArgs e) {
            _fileSystemWatcher.NotifyFilter = FileWatchSettings.IsIgnoreFolder ? NotifyFilters.FileName : NotifyFilters.FileName | NotifyFilters.DirectoryName;
        }

        public void MoveToFolder(FileSystemEventArgs e, string destDirPath) {
            Directory.Move(e.FullPath, Path.Combine(destDirPath, e.Name));
        }

        private void Serialize() {
            using (FileStream fs = new FileStream(_settingFilePath, FileMode.OpenOrCreate | FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter writer = new JsonTextWriter(sw))
                new JsonSerializer().Serialize(writer, FileWatchSettings);
        }

        public void Dispose() {
            _fileSystemWatcher.Dispose();
            Serialize();
        }
    }
}
