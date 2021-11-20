﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Mascot.Classes {
    public static class PipeClient {
        public static HashSet<ApplicationInfo> GetApplicationInfos() {
            var pipeClient =
                    new NamedPipeClientStream(".", "processmonitor",
                        PipeDirection.In, PipeOptions.None,
                        System.Security.Principal.TokenImpersonationLevel.Impersonation);
            pipeClient.Connect(100);
            HashSet<ApplicationInfo> applicationInfos;
            using (StreamReader reader = new StreamReader(pipeClient)) {
                string s = reader.ReadToEnd();
                //Console.WriteLine(s);
                applicationInfos = JsonConvert.DeserializeObject<HashSet<ApplicationInfo>>(s);
            }
            return applicationInfos;
        }
    }
}
