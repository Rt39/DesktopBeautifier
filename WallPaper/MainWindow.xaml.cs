using System;
using System.Collections.Generic;
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

namespace WallPaper {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    /*TODO: 拆分功能，将不同的功能放在不同的类中*/
    /*TODO: 完善爬虫功能*/
    /*TODO: 完善通知功能*/
    /*建议：使用MVVM模式，充分利用wpf的绑定机制*/
    public partial class MainWindow : Window {
        /*TODO: 补全public和private*/
        string fileName;
        string filePath;
        string keyWord;
        string folderFullName;
        BitmapImage bitmapImage;

        DirectoryInfo directory;
        ArrayList filesArray;
        ChangeWallPaper cwp;
        int fileIndex;
        bool fitToScreen = true;
        double rotateAngle = 0;


        public MainWindow() {
            InitializeComponent();

            //爬虫爬取、下载
            //WallPaperClawer wallPaperClawer = new WallPaperClawer();
            //keyWord = "Klose";
            //wallPaperClawer.ChooseWeb(1, keyWord);
            //wallPaperClawer.ClawerWeb();

            //读取文件、show第一张图片
            /*TODO: 添加文件夹为空的异常处理*/
            folderFullName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/File/";
            directory = new DirectoryInfo(folderFullName);
            filesArray = null;
            filesArray = new ArrayList();
            if (directory.GetFiles("*.jpg") != null) {
                foreach (FileInfo fileinfo in directory.GetFiles("*.jpg")) {
                    filesArray.Add(fileinfo);
                }
            }

            if (directory.GetFiles("*.png") != null) {
                foreach (FileInfo fileinfo in directory.GetFiles("*.png")) {
                    filesArray.Add(fileinfo);
                }
            }
            fileIndex = 0;//自动打开第一个图片的index为1

            this.fileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "File\\" + ((FileInfo)(filesArray[(fileIndex) % (filesArray.Count)])).Name;
            filePath = fileName.Substring(0, fileName.LastIndexOf('\\'));



            showPicture(this.fileName);

            //this.border1.Visibility = Visibility.Hidden;
            this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
        }

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (fileName != null) {
                showPicture(fileName);
            }
        }
        private void SetFilesArray() {

        }

        private void OpenFileClick(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图像文件(*.jpg;*.png)|*.jpg;*.png";
            if ((bool)dlg.ShowDialog(this)) {
                fileName = dlg.FileName;
                showPicture(fileName);


                filePath = fileName.Substring(0, fileName.LastIndexOf('\\'));//打开的文件路径

                /*
                DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                    filesArray.Add(NextFile.Name);
                */
                directory = new DirectoryInfo(filePath);
                filesArray = null;
                filesArray = new ArrayList();

                /*
                if (directory.GetFiles("*.jpg") != null&&directory.GetFiles("*.png") != null)
                {
                    foreach (FileInfo fileinfo in directory.GetFiles())
                    {
                        filesArray.Add(fileinfo);
                    }
                }
                */

                if (directory.GetFiles("*.jpg") != null) {
                    foreach (FileInfo fileinfo in directory.GetFiles("*.jpg")) {
                        filesArray.Add(fileinfo);
                    }
                }

                if (directory.GetFiles("*.png") != null) {
                    foreach (FileInfo fileinfo in directory.GetFiles("*.png")) {
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

        /*建议：去除旋转功能*/
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
                    this.img.Height = scrollViewer.ViewportHeight - 5;
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
            //imgsource = null;
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

        private void OnRotate90Click(object sender, RoutedEventArgs e) {
            rotateAngle += 90;
            rotateAngle = rotateAngle % 360;
            doRotate();
        }

        private void doRotate() {
            //TransformedBitmap tb = new TransformedBitmap();
            //tb.BeginInit();
            //tb.Source = bitmapImage;
            //RotateTransform transform;
            //if (rotateAngle==0)
            //{
            //    transform = new RotateTransform(0);
            //    tb.Transform = transform;
            //}
            //else if (rotateAngle == 90)
            //{
            //    transform = new RotateTransform(90);
            //    tb.Transform = transform;
            //}
            //else if (rotateAngle == 180)
            //{
            //    transform = new RotateTransform(180);
            //    tb.Transform = transform;
            //}
            //else if (rotateAngle == 270)
            //{
            //    transform = new RotateTransform(270);
            //    tb.Transform = transform;
            //}
            //tb.EndInit();
            //this.img.Height = tb.Height;
            //this.img.Width = tb.Width;
            //this.img.Source = tb;
            showPicture(fileName);
        }


        private void btnRotatecounterclockwise90_Click(object sender, RoutedEventArgs e) {
            rotateAngle -= 90;
            rotateAngle += 360;
            rotateAngle = rotateAngle % 360;
            doRotate();
        }

        private void btnRotateclockwise90_Click(object sender, RoutedEventArgs e) {
            rotateAngle += 90;
            rotateAngle = rotateAngle % 360;
            doRotate();
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
    }
}
