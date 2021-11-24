using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImgEditLiteWPF
{
    /// <summary>
    /// ResizeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ResizeWindow : Window
    {
        //定义属性和字段
        private Bitmap img;               //当前的像素图

        public ResizeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //绑定文本改变事件
            this.tbxWidth.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            this.tbxHeight.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            //初始化宽度高度
            tbxWidth.Text = img.Width.ToString();
            tbxHeight.Text = img.Height.ToString();
        }

        //设置当前像素图
        public void SetImg(Bitmap img)
        {
            this.img = img;
        }

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        //宽度文本键盘弹起事件
        private void tbxWidth_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((bool)cbxLock.IsChecked)
                    tbxHeight.Text = Convert.ToInt32(Convert.ToInt32(tbxWidth.Text) / Convert.ToDouble(img.Width) * img.Height).ToString();
                else
                    //测试转换是否会抛出异常
                    Convert.ToInt32(tbxWidth.Text);
            }
            catch (Exception)
            {
                tbxWidth.Text = "0";
                tbxHeight.Text = "0";
            }
        }
        //高度文本键盘弹起事件
        private void tbxHeight_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if ((bool)cbxLock.IsChecked)
                    tbxWidth.Text = Convert.ToInt32(Convert.ToInt32(tbxHeight.Text) / Convert.ToDouble(img.Height) * img.Width).ToString();
                else
                    //测试转换是否会抛出异常
                    Convert.ToInt32(tbxHeight.Text);
            }
            catch (Exception)
            {
                tbxWidth.Text = "0";
                tbxHeight.Text = "0";
            }
        }

        /// <summary>
        /// 文本改变事件
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //测试转换是否会抛出异常
                Convert.ToInt32(tbxWidth.Text);
                Convert.ToInt32(tbxHeight.Text);
            }
            catch (Exception)
            {
                tbxWidth.Text = "0";
                tbxHeight.Text = "0";
            }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        //应用
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            //初始信息
            int width = Convert.ToInt32(tbxWidth.Text);
            int height = Convert.ToInt32(tbxHeight.Text);

            //更改图片大小
            this.img = new Bitmap(this.img, width, height);

            //传递图片给父窗体
            MainWindow m = this.Owner as MainWindow;
            m.ApplyImg(this.img);

            //关闭窗口
            this.Close();
        }
        //取消
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭窗口
            this.Close();
        }
    }
}
