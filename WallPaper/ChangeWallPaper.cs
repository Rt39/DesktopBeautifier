using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace WallPaper
{
    class ChangeWallPaper
    {
        public class WinAPI
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }

        public void Change(string imagepath)
        {
            #region ChangeWallPaper
            //锁定图片picture绝对路径
            string ImagePath = imagepath;

            //string ImageFormt = ImagePath.Substring(ImagePath.LastIndexOf('.'));//获取图片格式

            string ImageName = ImagePath.Substring(0, ImagePath.LastIndexOf('.'));

            

            //转换格式为bmp，不需要改变大小
            Image img = Image.FromFile(ImagePath);

            string bmpPath = ImageName + @".bmp";//新图片要存储的位置
            using (var bmp = new Bitmap(img.Width, img.Height))
            {
                bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(img, 0, 0);
                }
                bmp.Save(bmpPath, ImageFormat.Bmp);
            }


            int nResult;
            if (File.Exists(bmpPath))
            {

                nResult = WinAPI.SystemParametersInfo(20, 1, bmpPath, 0x1 | 0x2); //更换壁纸
                if (nResult == 0)
                {
                    Console.WriteLine("没有更新成功!");

                }
                else
                {
                    RegistryKey hk = Registry.CurrentUser;
                    RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                    run.SetValue("Wallpaper", bmpPath);  //将新图片路径写入注册表
                    //MessageBox.Show("更新成功");
                }
            }
            else
            {
                Console.WriteLine("文件不存在！");

            }
            #endregion ChangeWallPaper

            //设置路径，帮助用户下载保存
        }
    }
}
