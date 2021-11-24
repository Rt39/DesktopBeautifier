using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mascot {
    [Serializable]
    public class FileWatchSettings {
        // 需要忽略的扩展名
        public string ExtentionExceptions { get; set; }
        [JsonIgnore]
        // 需要忽略的扩展名（数组）
        public string[] ExtentionExceptionList {
            get { return Array.ConvertAll(ExtentionExceptions.Split(';'), p => p.Trim()); }
        }
        // 是否忽略文件夹
        public event EventHandler<EventArgs> IgnoreFolderChanged;
        [JsonProperty]
        private bool _isIgnoreFoler;
        [JsonIgnore]
        public bool IsIgnoreFolder {
            get { return _isIgnoreFoler; }
            set {
                if (_isIgnoreFoler == value) return;
                _isIgnoreFoler = value;
                IgnoreFolderChanged(this, new EventArgs());
            }
        }
        // 保存的目录
        public Dictionary<string, string> ArchiveDirectory { get; set; } = new Dictionary<string, string>();
    }
}