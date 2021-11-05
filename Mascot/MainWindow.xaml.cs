using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Xml;
using System.Collections.Generic;

/*TODO: 使用enum状态切换*/

namespace Mascot {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        /*TODO: 将不同类别的字段分类以增加可读性*/
        /*TODO: 去除公有字段，改为公有属性*/
        public Angent angent = new Angent("Test");
        System.Windows.Controls.TextBox tb1;
        Notification Note = new Notification();
        PutInTray Tray;
        private Timer timer = new Timer();
        /*TODO: 给变量有意义的名称*/
        private int n = 0;
        private Dialog d;
        XmlDocument xml = new XmlDocument();
        private List<string> tips = new List<string>();
        Random r = new Random(0);
        public MainWindow() {
            InitializeComponent();
        }

        private void Main_Load(object sender, RoutedEventArgs e) {
            var desktopWorkingArea = Screen.PrimaryScreen.WorkingArea;
            /*TODO: 修改显示位置*/
            //this.Left = desktopWorkingArea.Right - this.Width+350;
            //this.Top = desktopWorkingArea.Bottom - this.Height+300;
            Note.WallPaperEvent += new EventHandler(Notification);
            GetTips();
            timer.Interval = 300;
            timer.Tick += new EventHandler(timer_Tick);
            Tray = new PutInTray(this);//托盘
            Tray.Init();
            timer.Start();
        }
        private void GetTips() {
            /*TODO: 修改资源文件使用方式*/
            xml.Load("../../Resources/Tips.xml");
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
            //nsole.WriteLine(tips[0]);
        }

        /*TODO: 给事件有意义的名称（可以使用F12重命名变量）*/
        private void timer_Tick(object sender, EventArgs e) {
            /*TODO: 更换图片显示框架，可以使用XamlAnimatedGif库*/
            n = (n) % 61 + 1;
            var bitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/frame/shime{n}.png"));
            this.backImage.Source = bitmap;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            if (n % 20 == 0 || n == 1) timer.Stop();
            if (n % 15 == 0) { d.Close(); d = null; }
        }

        private void MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e) {
            this.DragMove();
            Note.test();
            if (d != null) {
                d.Left = this.Left - 310;
                d.Top = this.Top - 290;
            }
        }

        private void MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e) {
            timer.Start();
            if (d == null) {
                d = new Dialog(this.Left, this.Top, tips[r.Next(tips.Count)]);
                d.Show();
            }
        }

        private void MouseRightButtonDown_1(object sender, MouseButtonEventArgs e) {
            tb1 = new System.Windows.Controls.TextBox();
            tb1.Name = "myTextBox1";
            tb1.Text = "";
            tb1.Width = 100;
            tb1.Height = 30;
            //tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.VerticalAlignment = VerticalAlignment.Top;
            tb1.Margin = new Thickness(100.0, 100.0, 0.0, 0.0);
            Grid.Children.Add(tb1);
        }

        private async void EnterKey_Down(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter && tb1 != null && !string.IsNullOrEmpty(tb1.Text)) {
                await BaiduUnit.unit_utterance(tb1.Text);
                tb1 = null;
            }
        }
        private void Notification(object sender, EventArgs e) {
            string s = sender as string;
            d.Text.Content = s;
        }
    }
}
