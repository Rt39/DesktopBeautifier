using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils {
    [Serializable]
    public struct ApplicationBasic {
        public string ApplicationName;
        public string ApplicationPath;
    }

    [Serializable]
    public class ApplicationInfo {
        [JsonIgnore]
        public const uint CHECK_INTERVAL = 30_000;  //单位：毫秒
        [JsonProperty]
        private readonly ApplicationBasic _applicationBasic;
        [JsonIgnore]
        public string ApplicationName { get { return _applicationBasic.ApplicationName; } }
        [JsonIgnore]
        public string ApplicationPath { get { return _applicationBasic.ApplicationPath; } }
        public uint ApplicationRunIntervals { get; set; } = 0;
        [JsonProperty]
        private uint _applicationClicks = 0;
        [JsonIgnore]
        public uint ApplicationClicks { get { return _applicationClicks; } }

        public void IncreaseRunInterval() { ++ApplicationRunIntervals; }
        public void IncreaseClick() { ++_applicationClicks; }

        public ApplicationInfo(ApplicationBasic applicationBasic) {
            _applicationBasic = applicationBasic;
        }
    }
}
