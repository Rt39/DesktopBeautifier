﻿using System;
using WallPaper;
using System.Windows;

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
    }
}
