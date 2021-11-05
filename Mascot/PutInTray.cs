using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Mascot
{
    class PutInTray
    {
        MainWindow mainwindow;
        NotifyIcon notifyIcon = new NotifyIcon();
        private string state;
        private DateTime BeginTime;
        private DateTime EndTime;
        private DispatcherTimer notifystate = new DispatcherTimer();
        RegistryKey rsg = null;
        Process cmd = new Process();
        public System.Windows.Forms.MenuItem picmove = new System.Windows.Forms.MenuItem("禁止移动");
        public System.Windows.Forms.MenuItem windowtop = new System.Windows.Forms.MenuItem("顶置");
        public static System.Windows.Forms.MenuItem startup = new System.Windows.Forms.MenuItem("开启");
        public static System.Windows.Forms.MenuItem startupc = new System.Windows.Forms.MenuItem("关闭");
        public PutInTray(MainWindow mainwindow)
        {
            this.mainwindow = mainwindow;
        }
        public void Init()
        {
            BeginTime = DateTime.Now;
            //设置托盘的各个属性
            notifyIcon.BalloonTipText = "来了来了";
            notifyIcon.Text = Convert.ToString(mainwindow.angent.Status);
            //notifyIcon.Icon = new Icon(System.Windows.Forms.Application.StartupPath + "/icon.ico");
            notifyIcon.Icon = new System.Drawing.Icon("../../Resources/Icon.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(100);
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
            //菜单
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);
            System.Windows.Forms.MenuItem loadxxx = new System.Windows.Forms.MenuItem("查看状态");
            loadxxx.Click += new EventHandler(loadstate);

            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { loadxxx, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //动态托盘状态
            notifystate.Tick += new EventHandler(notifystateupdate);
            notifystate.Interval = new TimeSpan(0, 0, 30);
            notifystate.Start();
        }

        private void notifystateupdate(object sender, EventArgs e)
        {
            notifyIcon.Text = mainwindow.angent.Status.ToString();
        }

        private void loadstate(object sender, EventArgs e)
        {
            EndTime = DateTime.Now;
            TimeSpan RunTime = new TimeSpan();
            RunTime = EndTime.Subtract(BeginTime);
            string msg = "开启了"+Convert.ToString(RunTime)+"\r\n";
            msg += "精灵状态：" + mainwindow.angent.Status.ToString() + "\r\n";
            System.Windows.MessageBox.Show(msg);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("真的要离开吗?",
                                               "退出",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notifyIcon.Dispose();
                //System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mainwindow.Visibility == Visibility.Visible)
                {
                    mainwindow.Visibility = Visibility.Hidden;
                }
                else
                {
                    mainwindow.Visibility = Visibility.Visible;
                    mainwindow.Activate();
                }
            }
        }
    }
}
