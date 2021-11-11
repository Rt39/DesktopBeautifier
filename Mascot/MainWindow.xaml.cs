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

namespace Mascot
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Angent angent = new Angent("Test");
        Notification Note = new Notification();
        BaiduUnit BaiduUnit = new BaiduUnit();
        PutInTray Tray;
        private Timer timer=new Timer();
        private int n = 0;
        private Dialog d;
        XmlDocument xml = new XmlDocument();
        private List<string> tips = new List<string>();
        Random r = new Random(0);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = Screen.PrimaryScreen.WorkingArea;
            this.Left = desktopWorkingArea.Right - this.Width+350;
            this.Top = desktopWorkingArea.Bottom - this.Height+300;
            Note.MsgEvent += new EventHandler(Notification);
            BaiduUnit.MsgEvent += new EventHandler(Notification);
            GetTips();
            timer.Interval = 300;
            timer.Tick += new EventHandler(timer_Tick);
            if(!File.Exists("Status.xml"))Angent.SaveStatus();
            Tray = new PutInTray(this);//托盘
            Tray.Init();
            timer.Start();
        }
        private void GetTips()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Mascot.Properties.Resources.Tips);
            XmlNode root = xml.SelectSingleNode("/tips");
            XmlNodeList tipList = root.ChildNodes;
            foreach(XmlElement i in tipList)
            {
                if(i.HasChildNodes)
                {
                    XmlNodeList list = i.ChildNodes;
                    foreach(XmlElement j in list)
                    {
                        tips.Add(j.InnerText);
                    }
                }
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            n = (n) % 61 + 1;
            var bitmap= new BitmapImage(new Uri($"pack://application:,,,/Resources/frame/shime{n}.png"));
            this.backImage.Source = bitmap;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            if (n % 20 == 0||n==1) timer.Stop(); 
            if (n % 15 == 0 &&d!=null) { d.Close();d = null; }
        }

        private void MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
         // Note.test();
            if(d!=null)
            {
                d.Left = this.Left -310;
                d.Top = this.Top - 290;
            }
        }

        private void MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
            if (d == null)
            {
                d = new Dialog(this.Left, this.Top, tips[r.Next(tips.Count)]);
                d.Show();
            }
        }

        private void MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            if (d == null)
            {
                d = new Dialog(this.Left, this.Top, "");
                d.Show();
            }
            d.QueryBox.Visibility = Visibility.Visible;
            Notification("有什么问题，问我吧！", new EventArgs());
        }
        private void Notification(object sender,EventArgs e)
        {
            string s = sender as string;
            d.Dispatcher.Invoke(() =>
            {
                System.Windows.Documents.FlowDocument doc =d.TextBox.Document;
                doc.Blocks.Clear();//清空对话框
                d.TextBox.AppendText(s);
            });
        }
    }
}
