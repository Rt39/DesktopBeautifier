using System;
using WallPaper;
using System.Windows;
using Alarm;
namespace Mascot
{
    public class Utils
    {
        public static bool flag = false;
        /// <summary>
        /// 最近打开应用的情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Process_Click(object sender, EventArgs e)
        {
            Forms.Process processForm = new Forms.Process();
            processForm.Show();
        }
        /// <summary>
        /// 快速路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RecentFile_Click(object sender, EventArgs e)
        {
            if (flag) return;
            flag = true;
            DesktopFileWatcher desktopFileWatcher = new DesktopFileWatcher();
            Forms.File fileForm = new Forms.File(ref desktopFileWatcher,"无");
            fileForm.WindowStartupLocation = WindowStartupLocation.Manual;
            fileForm.Top = 350;
            fileForm.Left = 1100;
            fileForm.ShowDialog();
        }
        /// <summary>
        /// 壁纸功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Wallpaper_Clicked(object sender, EventArgs e) {
            SelectionBar selectionBar = new SelectionBar();
            selectionBar.Show();
        }
        /// <summary>
        /// 闹钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Alarm_Clicked(object sender, EventArgs e) {
            Alarm.MainWindow mainWindow = new Alarm.MainWindow();
            mainWindow.ShowDialog();
        }
        /// <summary>
        /// 笔记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Note_Clicked(object sender,EventArgs e) {
            Note.MainWindow mainWindow = new Note.MainWindow();
            mainWindow.Show();
        }
       /// <summary>
       /// 备忘录
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        public static void Todo_Clicked(object sender, EventArgs e) {
            Todo.MainWindow mainWindow = new Todo.MainWindow();
            mainWindow.Show();
        }
        /// <summary>
        /// 图片编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ImgEdit_Clicked(object sender, EventArgs e) {
            ImgEditLiteWPF.MainWindow mainWindow = new ImgEditLiteWPF.MainWindow();
            mainWindow.Show();
        }
        /// <summary>
        /// 翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Translate_Clicked(object sender, EventArgs e) {
            TranslatorWPF.MainWindow mainWindow = new TranslatorWPF.MainWindow();
            mainWindow.Show();
        }
    }
}
