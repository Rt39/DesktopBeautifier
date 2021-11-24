using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WallPaper.utils {
    class SaveAs {
        public void PictureSaveAs(string fileName) {
            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            sf.Title = "请选择要保存的文件路径";
            //设置文件类型
            sf.Filter = "图像文件(*.jpg)|*.jpg";
            //saveFileDialog1.FilterIndex = 1;//设置文件类型显示
            DateTime now = DateTime.Now;
            string RandomNum = now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            sf.FileName = "WallPaper" + RandomNum;//设置默认文件名
            sf.DefaultExt = ".jpg";
            sf.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            sf.CheckPathExists = true;//检查目录
            Nullable<bool> result = sf.ShowDialog();
            if (result == true) {
                //显示成功标签
                string bmpPath = sf.FileName.ToString();
                bmpPath = bmpPath.Substring(0, bmpPath.LastIndexOf("."));
                bmpPath = bmpPath + @".bmp";//新图片要存储的位置

                System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);
                using (var bmp = new Bitmap(img.Width, img.Height)) {
                    bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                    using (var g = Graphics.FromImage(bmp)) {
                        g.Clear(System.Drawing.Color.White);
                        g.DrawImageUnscaled(img, 0, 0);
                    }
                    bmp.Save(bmpPath, ImageFormat.Bmp);
                }
            }
            else {
                //显示失败
            }
        }
    }
}
