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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImgEditLiteWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //定义属性和字段
        private string fName;                              //当前打开的文件名
        private Bitmap img;                                //当前的像素图
        private Stack stack = Stack.getStack();            //用于撤销恢复的栈

        private int imgWidth = 760;                        //imgImg原宽度
        private int imgHeight = 480;                       //imgImg原高度

        private int imgLeft = 0;                           //imgImg原Left
        private int imgTop = 0;                            //imgImg原Top

        private int minWidth = 100;                        //imgImg最小宽度
        private int minHeight = 100;                       //imgImg最小高度

        //构造函数
        public MainWindow()
        {
            InitializeComponent();

            //读入测试图片
            fName = @".\img\classroom.jpg";
            //将图片保存和显示
            img = (Bitmap)System.Drawing.Image.FromFile(fName);
            imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //将图片入栈
            stack.push(new StackImg(
                Convert.ToInt32(lblLDVal.Content),
                Convert.ToInt32(lblDBDVal.Content),
                Convert.ToInt32(lblBHDVal.Content),
                Convert.ToInt32(lblSWVal.Content),
                this.img));
        }

        //应用图片
        public void ApplyImg(Bitmap img)
        {
            //将图片保存和显示
            this.img = img;
            imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //将图片入栈
            stack.push(new StackImg(
                Convert.ToInt32(lblLDVal.Content),
                Convert.ToInt32(lblDBDVal.Content),
                Convert.ToInt32(lblBHDVal.Content),
                Convert.ToInt32(lblSWVal.Content),
                this.img));
        }

        //设置初始调色值
        public void InitToningValue()
        {
            //设置初始调色值
            sldLD.Value = 0;
            lblLDVal.Content = "0";

            sldDBD.Value = 0;
            lblDBDVal.Content = "0";

            sldBHD.Value = 0;
            lblBHDVal.Content = "0";

            sldSW.Value = 0;
            lblSWVal.Content = "0";
        }

        /// <summary>
        /// 菜单控件事件
        /// </summary>
        //打开
        private void mitOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "所有支持的图片格式|*.jpg;*jpeg;*.jpe;*.bmp;*.gif;*.png;*.raw;*.arw;*.nef;*.crw;*.mrw;*.raf;*.erf;*.3fr;*.dcr;*.dng;*.pef;*.cr2;*.st2;*.mef;*.orf;*.psd" + "|" +
                "JPG 格式|*.jpg;*.jpeg;*.jpe|BMP 格式|*.bmp|GIF 格式|*.gif|PNG 格式|*.png";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //读入图片
                this.fName = openFileDialog.FileName;
                //将图片保存和显示
                try
                {
                    //使用Image.FromFile创建图像对象
                    this.img = (Bitmap)System.Drawing.Image.FromFile(fName);
                }
                catch (Exception exp)
                {
                    System.Windows.MessageBox.Show(exp.Message);
                }
                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                //设置初始调色值
                InitToningValue();
                //将图片入栈
                stack.push(new StackImg(
                    Convert.ToInt32(lblLDVal.Content),
                    Convert.ToInt32(lblDBDVal.Content),
                    Convert.ToInt32(lblBHDVal.Content),
                    Convert.ToInt32(lblSWVal.Content),
                    this.img));
            }
        }
        //保存
        private void mitSave_Click(object sender, RoutedEventArgs e)
        {
            //获取扩展名
            string ext = System.IO.Path.GetExtension(this.fName);
            //生成保存路径
            string fName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\IMG_" + string.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ext;
            //保存图片
            switch (ext)
            {
                case ".gif":
                    this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".jpg":
                    this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".bmp":
                    this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                default:
                    this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }
        }
        //另存为
        private void mitSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "JPG 格式|*.jpg|PNG 格式|*.png|BMP 格式|*.bmp";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获取文件路径
                string fName = saveFileDialog.FileName;
                //获取扩展名
                string ext = System.IO.Path.GetExtension(fName);
                //保存图片
                switch (ext)
                {
                    case ".gif":
                        this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case ".jpg":
                        this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        this.img.Save(fName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
            }
        }
        //撤销
        private void mitUndo_Click(object sender, RoutedEventArgs e)
        {
            if (stack.isUndoable())
            {
                StackImg s = stack.pop();

                //将图片保存和显示
                this.img = s.img;
                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                
                sldLD.Value = s.LD;
                lblLDVal.Content = Convert.ToString(s.LD);

                sldDBD.Value = s.DBD;
                lblDBDVal.Content = Convert.ToString(s.DBD);

                sldBHD.Value = s.BHD;
                lblBHDVal.Content = Convert.ToString(s.BHD);

                sldSW.Value = s.SW;
                lblSWVal.Content = Convert.ToString(s.SW);
            }
        }
        //恢复
        private void mitRedo_Click(object sender, RoutedEventArgs e)
        {
            if (stack.isRedoable())
            {
                StackImg s = stack.redo();

                //将图片保存和显示
                this.img = s.img;
                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                sldLD.Value = s.LD;
                lblLDVal.Content = Convert.ToString(s.LD);

                sldDBD.Value = s.DBD;
                lblDBDVal.Content = Convert.ToString(s.DBD);

                sldBHD.Value = s.BHD;
                lblBHDVal.Content = Convert.ToString(s.BHD);

                sldSW.Value = s.SW;
                lblSWVal.Content = Convert.ToString(s.SW);
            }
        }
        //修改尺寸
        private void mitResize_Click(object sender, RoutedEventArgs e)
        {
            ResizeWindow resizeWindow = new ResizeWindow();
            resizeWindow.SetImg(this.img);
            resizeWindow.Owner = this;
            resizeWindow.ShowDialog();
        }
        //关于
        private void mitAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// 滑块滑动事件
        /// </summary>
        //亮度滑块
        private void sldLD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblLDVal.Content = sldLD.Value.ToString();
        }
        //对比度滑块
        private void sldDBD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblDBDVal.Content = sldDBD.Value.ToString();
        }
        //饱和度滑块
        private void sldBHD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblBHDVal.Content = sldBHD.Value.ToString();
        }
        //色温滑块
        private void sldSW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblSWVal.Content = sldSW.Value.ToString();
        }
        //编辑大小滑块
        private void sldESize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblESizeVal.Content = sldESize.Value.ToString() + "%";
        }

        /// <summary>
        /// 图片控件事件
        /// </summary>
        //鼠标滚轮事件
        private void imgImg_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int i = e.Delta * SystemInformation.MouseWheelScrollLines / 5;
            if ((imgImg.Width > this.minWidth && imgImg.Height > this.minHeight) || (i > 0))
            {
                imgImg.Width = imgImg.Width + i;                                                            //增加Image的宽度
                imgImg.Height = imgImg.Height + i;
                imgImg.Margin = new Thickness(imgImg.Margin.Left - i / 2, imgImg.Margin.Top - i / 2, 0, 0); //使Image的中心位于窗体的中心
            }
        }
        //图片控件大小改变事件
        private void imgImg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int n = Convert.ToInt32(imgImg.Width * 100 / this.imgWidth);
            if (n > 500)
            {
                sldESize.Value = 500;
            }
            else
            {
                sldESize.Value = n;
            }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        //自适应
        private void btnESelfAdp_Click(object sender, RoutedEventArgs e)
        {
            imgImg.Width = this.imgWidth;
            imgImg.Height = this.imgHeight;
            imgImg.Margin = new Thickness(this.imgLeft, this.imgTop, 0, 0);
        }

        /// <summary>
        /// 调色
        /// </summary>
        //滑块鼠标弹起事件
        private void Slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider)
            {
                Slider slider = sender as Slider;

                Toning toning = ToningFactory.getToning(slider.Uid);
                this.img = toning.DoToning(this.img, (int)slider.Value);

                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                
                stack.push(new StackImg(
                    Convert.ToInt32(lblLDVal.Content),
                    Convert.ToInt32(lblDBDVal.Content),
                    Convert.ToInt32(lblBHDVal.Content),
                    Convert.ToInt32(lblSWVal.Content),
                    this.img));
            }
        }
        //滑块拖动完成事件
        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (sender is Slider)
            {
                Slider slider = sender as Slider;

                Toning toning = ToningFactory.getToning(slider.Uid);
                this.img = toning.DoToning(this.img, (int)slider.Value);

                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                stack.push(new StackImg(
                    Convert.ToInt32(lblLDVal.Content),
                    Convert.ToInt32(lblDBDVal.Content),
                    Convert.ToInt32(lblBHDVal.Content),
                    Convert.ToInt32(lblSWVal.Content),
                    this.img));
            }
        }

        /// <summary>
        /// 滤镜
        /// </summary>
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.RadioButton)
            {
                System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;

                Filter filter = FilterFactory.getFilter(radioButton.Content.ToString());
                this.img = filter.DoFilter(this.img);

                imgImg.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(this.img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                stack.push(new StackImg(
                    Convert.ToInt32(lblLDVal.Content),
                    Convert.ToInt32(lblDBDVal.Content),
                    Convert.ToInt32(lblBHDVal.Content),
                    Convert.ToInt32(lblSWVal.Content),
                    this.img));
            }
        }
    }
}
