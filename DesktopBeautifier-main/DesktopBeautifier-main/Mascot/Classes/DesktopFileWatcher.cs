using Mascot.UtilClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot
{
    public class DesktopFileWatcher : IDisposable
    {
        private static readonly string _folder = Path.Combine(Definitions.SettingFolder, "FileWatch");
        private static readonly string _settingFilePath = Path.Combine(_folder, "setting.json");

        private readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        public FileWatchSettings FileWatchSettings { get; set; }

        public event EventHandler<FileSystemEventArgs> NewFile;

        public DesktopFileWatcher()
        {
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
            if (File.Exists(_settingFilePath))
            {
                string setting;
                using (StreamReader sr = new StreamReader(_settingFilePath))
                    setting = sr.ReadToEnd();
                FileWatchSettings = JsonConvert.DeserializeObject<FileWatchSettings>(setting);
                if (FileWatchSettings.ArchiveDirectory == null)
                {
                    FileWatchSettings.ArchiveDirectory = new Dictionary<string, string>();
                    Serialize();
                }
            }
            else
            {
                FileWatchSettings = new FileWatchSettings
                {
                    ExtentionExceptions = ".lnk"
                };
                Serialize();
            }
            FileWatchSettings.IgnoreFolderChanged += ChangeIgnoreFolder;

            _fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes| NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            //_fileSystemWatcher.NotifyFilter = FileWatchSettings.IsIgnoreFolder ? NotifyFilters.FileName : NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Created += new FileSystemEventHandler(OnCreate);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        // 过滤不必要的事件
        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            string extention = Path.GetExtension(e.FullPath);
            if (FileWatchSettings.ExtentionExceptionList.Contains(extention)) return;
            NewFile(this, e);
        }

        // 修改监听文件夹
        private void ChangeIgnoreFolder(object sender, EventArgs e)
        {
            _fileSystemWatcher.NotifyFilter = FileWatchSettings.IsIgnoreFolder ? NotifyFilters.FileName : NotifyFilters.FileName | NotifyFilters.DirectoryName;
        }

        private void Serialize()
        {
            using (FileStream fs = new FileStream(_settingFilePath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter writer = new JsonTextWriter(sw))
                new JsonSerializer().Serialize(writer, FileWatchSettings);
        }
        public void Dispose()
        {
            //_fileSystemWatcher.Dispose();
            Serialize();
        }
    }
}