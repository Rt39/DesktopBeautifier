using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace Mascot
{
    public static class PipeClient
    {
        public static void Init()
        {
            try
            {
                var pipeClient =
                            new NamedPipeClientStream(".", "processmonitor",
                                PipeDirection.In, PipeOptions.None,
                                System.Security.Principal.TokenImpersonationLevel.Impersonation);
                Console.WriteLine("Connecting to server...\n");
                pipeClient.Connect(100);
            }catch
            {
                throw new Exception("服务器连接失败");
            }
        }
        public static HashSet<ApplicationInfo> GetApplicationInfos()
        {
            try
            {
                var pipeClient =
                          new NamedPipeClientStream(".", "processmonitor",
                              PipeDirection.In, PipeOptions.None,
                              System.Security.Principal.TokenImpersonationLevel.Impersonation);
                Console.WriteLine("Connecting to server...\n");
                pipeClient.Connect(100);
                //最近使用进程哈希表
                HashSet<ApplicationInfo> applicationInfos;
                using (StreamReader reader = new StreamReader(pipeClient))
                {
                    //获取的json文件流，并反序列化
                    string s = reader.ReadToEnd();
                    applicationInfos = JsonConvert.DeserializeObject<HashSet<ApplicationInfo>>(s);
                }
                return applicationInfos;
            }
            catch
            {
                throw new Exception("服务器连接失败");
            }
        }
    }
}