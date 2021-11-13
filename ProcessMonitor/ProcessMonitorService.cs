using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace ProcessMonitor {
    public partial class ProcessMonitorService : ServiceBase {
        private NamedPipeServerStream pipeServer;
        private Thread thread;
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DestopBeautifer", "monitor.log");

        private static PipeSecurity ps;
        static ProcessMonitorService() {
            ps = new PipeSecurity();
            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.Read, System.Security.AccessControl.AccessControlType.Allow));
        }

        private HashSet<ApplicationInfo> applicationInfos = new HashSet<ApplicationInfo>();
        private HashSet<ApplicationBasic> processContinuous = new HashSet<ApplicationBasic>();

        public ProcessMonitorService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            thread = new Thread(ServerThread);
            thread.Start();
            System.Timers.Timer timer = new System.Timers.Timer(ApplicationInfo.CHECK_INTERVAL);
            timer.Elapsed += CheckProcess;
            timer.Start();
            WriteLog("服务启动");
        }

        private void CheckProcess(object sender, System.Timers.ElapsedEventArgs e) {
            WriteLog("检查信息");
            HashSet<ApplicationBasic> processPaths = new HashSet<ApplicationBasic>();
            Process[] processes = Process.GetProcesses();
            foreach (var p in processes) {
                string path;
                // 抑制禁止访问的异常
                try { path = p.MainModule.FileName; }
                catch (System.ComponentModel.Win32Exception) { continue; }
                // 忽略Windows目录下的程序
                string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                if (path.Substring(0, windowsPath.Length) == windowsPath) continue;

                processPaths.Add(new ApplicationBasic { ApplicationName = p.ProcessName, ApplicationPath = path });
            }
            foreach (var path in processPaths) {
                var t = applicationInfos.FirstOrDefault(info => info.ApplicationPath == path.ApplicationPath);
                if (t == null) {
                    applicationInfos.Add(new ApplicationInfo(path));
                    t = applicationInfos.FirstOrDefault(info => info.ApplicationPath == path.ApplicationPath);
                }
                if (processContinuous.Contains(path)) t.IncreaseRunInterval();
                else { t.IncreaseRunInterval(); t.IncreaseClick(); }
            }
            processContinuous = processPaths;
        }

        protected override void OnStop() {
            WriteLog("服务关闭");
        }

        private void ServerThread() {
            while (true) {
                try {
                    using (pipeServer = new NamedPipeServerStream("processmonitor", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 4096, 4096, ps)) {
                        pipeServer.WaitForConnection();
                        WriteLog("连接成功");

                        using (StreamWriter writer = new StreamWriter(pipeServer))
                        using (JsonWriter jw = new JsonTextWriter(writer)) {
                            new JsonSerializer().Serialize(jw, applicationInfos);
                        }
                    }
                }
                catch (Exception e) {
                    WriteLog($"错误：{e}");
                    return;
                }
            }
        }

        private void WriteLog(string s) {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream)) {
                writer.WriteLine($"{DateTime.Now}, {s}");
            }
        }
    }
}
