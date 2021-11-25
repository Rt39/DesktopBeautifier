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
        private NamedPipeServerStream _pipeServer;  // 管道服务端
        private Thread _pipeListenThread; // 监听管道连接
        private System.Timers.Timer _processMonitorTimer;   // 进程监视计时器
        private static readonly string _folder = Path.Combine(Definitions.SettingFolder, "ProcessMonitor");
        private static readonly string _logPath = Path.Combine(_folder, "monitor.log");

        // 管道安全策略，设置为非管理员程序也可以连接
        private static readonly PipeSecurity _ps;
        static ProcessMonitorService() {
            _ps = new PipeSecurity();
            SecurityIdentifier sid = new SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            _ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.Read, System.Security.AccessControl.AccessControlType.Allow));
        }

        // 全部的应用程序信息
        private HashSet<ApplicationInfo> _applicationInfos = new HashSet<ApplicationInfo>();
        // 上一次统计在运行的应用，为了计算用户是否重新打开该程序
        private HashSet<ApplicationBasic> _processContinuous = new HashSet<ApplicationBasic>();

        public ProcessMonitorService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            // 创建目录
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
            WriteLog("服务启动");

            // 启动管道服务
            _pipeListenThread = new Thread(ServerThread);
            _pipeListenThread.Start();
            // 启动进程监视器，定时检测本机进程资源
            _processMonitorTimer = new System.Timers.Timer(ApplicationInfo.CHECK_INTERVAL);
            _processMonitorTimer.Elapsed += CheckProcess;
            _processMonitorTimer.Start();
        }

        // 监视进程
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
                var t = _applicationInfos.FirstOrDefault(info => info.ApplicationPath == path.ApplicationPath);
                if (t == null) {
                    _applicationInfos.Add(new ApplicationInfo(path));
                    t = _applicationInfos.FirstOrDefault(info => info.ApplicationPath == path.ApplicationPath);
                }
                if (_processContinuous.Contains(path)) t.IncreaseRunInterval();
                else { t.IncreaseRunInterval(); t.IncreaseClick(); }
            }
            _processContinuous = processPaths;
        }

        protected override void OnStop() {
            WriteLog("服务关闭");
        }

        private void ServerThread() {
            while (true) {
                try {
                    using (_pipeServer = new NamedPipeServerStream("processmonitor", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 4096, 4096, _ps)) {
                        _pipeServer.WaitForConnection();
                        WriteLog("连接成功");

                        using (StreamWriter writer = new StreamWriter(_pipeServer))
                        using (JsonWriter jw = new JsonTextWriter(writer)) {
                            new JsonSerializer().Serialize(jw, _applicationInfos);
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
            using (FileStream stream = new FileStream(_logPath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream)) {
                writer.WriteLine($"{DateTime.Now}, {s}");
            }
        }
    }
}
