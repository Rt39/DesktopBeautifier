using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mascot
{
    /// <summary>
    /// Dialog.xaml 的交互逻辑
    /// </summary>
    public partial class Dialog : Window
    {
        public string Msg { set; get; }
        [DllImport("User32.dll")]
        public extern static System.IntPtr GetDC(System.IntPtr hWnd);
        public Dialog(double Left,double Top,string msg)
        {
            InitializeComponent();
        //  var bitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/tip.png"));
        //  this.backImage.Source = bitmap;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Margin = new Thickness(100, 100, 0, 0);
            this.Left = Left;
            this.Top = Top;
            this.Msg = msg;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            this.Margin = new Thickness(1000, 100, 0, 0);
            this.TextBox.AppendText(Msg);
        }

        private async void EnterKey(object sender, KeyEventArgs e)
        {
            string query = QueryBox.Text;
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(query))
            {
                int r= UtilClass.JudgeUtil.isRelative(query);
                if(r==-1)
                await BaiduUnit.unit_utterance(query);
                else
                {
                    if(r==1) Utils.Process_Click(new object(), new EventArgs());
                    else if(r==4) Utils.RecentFile_Click(new object(), new EventArgs());
                }
                QueryBox.Text = "";
                QueryBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
