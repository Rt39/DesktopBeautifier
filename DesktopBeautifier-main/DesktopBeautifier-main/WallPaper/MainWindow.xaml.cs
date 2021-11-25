using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using WallPaper.Clawer;
using WallPaper.utils;

namespace WallPaper {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    /*TODO: 完善通知功能*/
    public partial class MainWindow : Window {
        private string fileName;
        private string filePath;
        private string folderFullName;
        BitmapImage bitmapImage;

        DirectoryInfo directory;
        ArrayList filesArray;
        ChangeWallPaper cwp;
        private int fileIndex;
        bool fitToScreen = true;
        private double rotateAngle = 0;

        public MainWindow(string randomnum) {
            InitializeComponent();
            string RandomNum = randomnum;

            //读取文件、显示第一张图片
            /*TODO: 添加文件夹为空的异常处理*/
            folderFullName = "C:\\Windows\\Temp\\" + RandomNum;
            directory = new DirectoryInfo(folderFullName);
            filesArray = null;
            filesArray = new ArrayList();
            if (directory.GetFiles("*.jpg") != null && directory.GetFiles("*.png") != null) {
                foreach (FileInfo fileinfo in directory.GetFiles()) {
                    filesArray.Add(fileinfo);
                }
            }
            fileIndex = 0;//自动打开第一个图片的index为1
            this.fileName = "C:\\Windows\\Temp\\" + RandomNum + "\\" + ((FileInfo)(filesArray[(fileIndex) % (filesArray.Count)])).Name;
            filePath = fileName.Substring(0, fileName.LastIndexOf('\\'));
            showPicture(this.fileName);

            this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
        }

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (fileName != null) {
                showPicture(fileName);
            }
        }

        private void OpenFileClick(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图像文件(*.jpg;*.png)|*.jpg;*.png";
            if ((bool)dlg.ShowDialog(this)) {
                fileName = dlg.FileName;
                showPicture(fileName);

                filePath = fileName.Substring(0, fileName.LastIndexOf('\\'));//打开的文件路径
                
                directory = new DirectoryInfo(filePath);
                filesArray = null;
                filesArray = new ArrayList();
                                
                if (directory.GetFiles("*.jpg") != null&&directory.GetFiles("*.png") != null)
                {
                    foreach (FileInfo fileinfo in directory.GetFiles())
                    {
                        filesArray.Add(fileinfo);
                    }
                }       

                //找到当前文件的序号
                int i = 0;
                foreach (FileInfo file in filesArray) {
                    if (file.Name == fileName) {
                        fileIndex = i;
                        break;
                    }
                    i++;
                }
            }
            this.border1.Visibility = Visibility.Visible;

        }

        private void showPicture(string fileName) {
            bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(fileName);

            if (rotateAngle == 0) {
                bitmapImage.Rotation = Rotation.Rotate0;
            }
            else if (rotateAngle == 90) {
                bitmapImage.Rotation = Rotation.Rotate90;
            }
            else if (rotateAngle == 180) {
                bitmapImage.Rotation = Rotation.Rotate180;
            }
            else if (rotateAngle == 270) {
                bitmapImage.Rotation = Rotation.Rotate270;
            }
            bitmapImage.EndInit();
            if (!fitToScreen) {
                this.img.Height = bitmapImage.Height;
                this.img.Width = bitmapImage.Width;
                this.border1.Height = this.img.Height;
                this.border1.Width = this.img.Width;

                this.img.Source = bitmapImage;
                return;
            }
            else {
                double scale = bitmapImage.Height / bitmapImage.Width;
                if (scale >= 1) //以竖直方向为标准
                {
                    this.img.Height = 500;
                    //this.img.Height = scrollViewer.ViewportHeight - 5;
                    this.img.Width = this.img.Height / scale;
                }
                else {
                    //Width待修改
                    this.img.Width = 700;
                    //this.img.Width = scrollViewer.ViewportWidth - 20;
                    this.img.Height = this.img.Width * scale;
                }
                this.border1.Height = this.img.Height;
                this.border1.Width = this.img.Width;

                this.img.Source = bitmapImage;
                return;
            }
        }

        private void CloseFileClick(object sender, RoutedEventArgs e) {
            bitmapImage = null;
            this.img.Width = this.scrollViewer.Width;
            this.img.Height = this.scrollViewer.Height;
            this.img.Source = null;
            this.border1.Visibility = Visibility.Hidden;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e) {
            this.fileName = filePath + "\\" + ((FileInfo)(filesArray[(fileIndex++) % (filesArray.Count)])).Name;
            showPicture(this.fileName);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e) {
            if (fileIndex == 0) {
                fileIndex = filesArray.Count - 1;
            }
            else {
                fileIndex--;
            }
            this.fileName = filePath + "\\" + ((FileInfo)(filesArray[(fileIndex) % (filesArray.Count)])).Name;
            showPicture(this.fileName);
        }

        private void btnFitSwitch_Click(object sender, RoutedEventArgs e) {
            fitToScreen = !fitToScreen;
            showPicture(fileName);
        }

        private void OnQuitClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void OnAboutMe(object sender, RoutedEventArgs e) {
            //AboutMe aboutDialog = new AboutMe();
            //aboutDialog.ShowDialog();
        }

        private void btnChangeWallPaper_Click(object sender, RoutedEventArgs e) {
            cwp = new ChangeWallPaper();
            cwp.Change(fileName);
        }

        private void Window_Closed(object sender, EventArgs e) {
            //跳转
            SelectionBar selection = new SelectionBar();
            this.Close();
            selection.Show();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            if (img.Source != null) {
                SaveAs sa = new SaveAs();
                sa.PictureSaveAs(fileName);
            }
            else {
                //显示未选中图片
            }
        }
    }
}
