using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

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
            var bitmap = new BitmapImage(new Uri($"pack://application:,,,/Resources/tip.png"));
            this.backImage.Source = bitmap;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = Left-2000;
            this.Top = Top-1000;
            this.Msg = msg;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            RenderText3();
            this.Text.Content = Msg;
            //zSystem.Windows.MessageBox.Show(this.Text.Content.ToString());
        }
        private void RenderText3()
        {
            IntPtr DesktopHandle = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(DesktopHandle);
            Font font = System.Drawing.SystemFonts.CaptionFont;
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            System.Windows.Point location = this.Text.PointToScreen(new System.Windows.Point());
            System.Drawing.Point location1=new System.Drawing.Point((int)location.X, (int)location.Y);
            g.DrawString(this.Msg, font, Brushes.White, (float)location.X,(float)location.Y);
        }
    }
}
