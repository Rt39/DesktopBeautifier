using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Documents;
using System.IO;

namespace Mascot {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public Angent angent = new Angent("Test");
        Notification Note = new Notification();
        BaiduUnit BaiduUnit = new BaiduUnit();//百度机器人API
        PutInTray Tray;   //托盘
        private Timer timer = new Timer();//播放图片
        private Timer dialogtimer = new Timer();
        private DateTime sTime = DateTime.Now;
        private uint askTimes = 0; //询问次数
        private int n = 0;//图片次序
        private int dialogn = 0;
        private Dialog d;
        private DesktopFileWatcher FileWatcher = new DesktopFileWatcher();//桌面文件监视
        private List<string> tips = new List<string>();
        Random r = new Random(10); //随机得到XMl文件中设置好的对话
        public MainWindow() {
            InitializeComponent();
        }
        private void Main_Load(object sender, RoutedEventArgs e) {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 1100;
            this.Top = 700;
            Note.MsgEvent += new EventHandler(Notification);
            BaiduUnit.MsgEvent += new EventHandler(Notification);
            FileWatcher.NewFile += new EventHandler<FileSystemEventArgs>(FileWatcher_NewFile);
            timer.Interval = 300;
            timer.Tick += new EventHandler(timer_Tick);
            dialogtimer.Interval = 300;
            dialogtimer.Tick += new EventHandler(dialogtimer_Tick);
            //设置序列化存储路径
            if (!Directory.Exists(Definitions.SettingFolder))
                Directory.CreateDirectory(Definitions.SettingFolder);
            Tray = new PutInTray(this);//托盘
            Tray.Init();
            GetTips();
            timer.Start();
            dialogtimer.Start();
        }
        /// <summary>
        /// 监听得到新文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileWatcher_NewFile(object sender, FileSystemEventArgs e) {
            if (Utils.flag) return;
            Utils.flag = true;
            this.Dispatcher.Invoke(new Action(() => {
                Notification("新文件被发现了哦~", new EventArgs());
                Forms.File fileForm = new Forms.File(ref FileWatcher, e.Name);
                fileForm.WindowStartupLocation = WindowStartupLocation.Manual;
                fileForm.Top = 350;
                fileForm.Left = 1100;
                fileForm.ShowDialog();
            }));
        }
        //获得XML中设置的对话
        private void GetTips() {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.Tips);
            XmlNode root = xml.SelectSingleNode("/tips");
            XmlNodeList tipList = root.ChildNodes;
            foreach (XmlElement i in tipList) {
                if (i.HasChildNodes) {
                    XmlNodeList list = i.ChildNodes;
                    foreach (XmlElement j in list) {
                        tips.Add(j.InnerText);
                    }
                }
            }
        }
        private void timer_Tick(object sender, EventArgs e) {
            n = (n) % 61 + 1;
            var bitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/frame/shime{n}.png"));
            this.backImage.Source = bitmap;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            if (n % 20 == 0 || n == 1) timer.Stop();
        }
        private void dialogtimer_Tick(object sender, EventArgs e) {
            dialogn += 1;
            if (dialogn % 15 == 0 && d != null) { d.Close(); d = null; }
        }

        private void MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e) {
            this.DragMove();
            if (d != null) {
                d.Left = this.Left - 310;
                d.Top = this.Top - 290;
            }
        }

        private void MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e) {
            timer.Start();
            dialogtimer.Start();
            if (d == null) {
                d = new Dialog(this.Left-310, this.Top-290, tips[r.Next(tips.Count)]);
                d.Show();
            }
        }
        private void MouseRightButtonDown_1(object sender, MouseButtonEventArgs e) {
            //timer.Stop();
            if (d == null) {
                d = new Dialog(this.Left-310, this.Top-290, "");
                d.Show();
            }
            dialogtimer.Stop();
            dialogn = 0;
            d.QueryBox.Visibility = Visibility.Visible;
            askTimes++;
            string s = string.Empty;
            if (askTimes % 10 == 0) s = angent.ChangeStatus(true);
            Notification("有什么问题，问我吧! " + s, new EventArgs());
        }
        private void Notification(object sender, EventArgs e) {
            string s = sender as string;
            if (d == null) {
                this.Dispatcher.Invoke(new Action(() => {
                    n = 0;
                    d = new Dialog(this.Left-310, this.Top-290, "");
                    d.Show();
                }));
            }
            d.Dispatcher.Invoke(() => {
                System.Windows.Documents.FlowDocument doc = d.TextBox.Document;
                doc.Blocks.Clear();//清空对话框
                d.TextBox.AppendText(s);
            });
        }
    }
}
