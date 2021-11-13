using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot.UtilClass {
    [Serializable]
    public struct FileWatchSettings {
        // 需要忽略的扩展名
        public string ExtentionExceptions { get; set; }
        [JsonIgnore]
        // 需要忽略的扩展名（数组）
        public string[] ExtentionExceptionList {
            get { return Array.ConvertAll(ExtentionExceptions.Split(';'), p => p.Trim()); }
        }
        // 保存的目录
        public Dictionary<string, string> ArchiveDirectory { get; set; }
    }
}
