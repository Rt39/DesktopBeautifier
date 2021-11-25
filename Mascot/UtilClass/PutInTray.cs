using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Mascot
{
    class PutInTray
    {
        MainWindow mainwindow;
        NotifyIcon notifyIcon = new NotifyIcon();
        private DateTime BeginTime;
        private DateTime EndTime;
        private DispatcherTimer notifystate = new DispatcherTimer();
        private List<MenuNode> list;
        //菜单设置文件
        private string path = Path.Combine(Definitions.SettingFolder, "Menu.gra");
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
            notifyIcon.Text = mainwindow.angent.GetStatus();
            notifyIcon.Icon = Properties.Resources.icon;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(100);
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
            //菜单
            LoadMenu();
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { LoadNode(0), exit });
            //动态托盘状态
            notifystate.Tick += new EventHandler(notifystateupdate);
            notifystate.Interval = new TimeSpan(0, 0, 30);
            notifystate.Start();
        }
        //设置完成触发更新菜单事件
        private void UpdateMenu(object sender,EventArgs e)
        {
            LoadMenu();
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { LoadNode(0), exit });
        }
        /// <summary>
        /// 加载菜单
        /// </summary>
        private void LoadMenu()
        {
            if (!File.Exists(path))
            {
                SaveMenu();
            }
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter b = new BinaryFormatter();
                list = (List<MenuNode>)b.Deserialize(fileStream);
            }
        }
        private MenuItem LoadNode(int idx)
        {
            if (list[idx].HasChild())
            {
                int cont = list[idx].ChildMenu.Count;
                MenuItem[] items = new MenuItem[cont];
                for(int i=0;i<cont;i++)
                {
                    int id = list[idx].ChildMenu[i];
                    //递归得到子菜单节点
                    items[i] = LoadNode(id);
                }
                return new MenuItem($"{list[idx].NodeName}", items);
            }
            else
            {
                MenuItem i= new MenuItem($"{list[idx].NodeName}");
                //绑定处理事件
                EventBinding(ref i);
                return i;
            }
        }
        /// <summary>
        /// 保存菜单
        /// </summary>
        private void SaveMenu()
        { 
            string Dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DestopBeautifer");
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }
            if (!File.Exists(path))
            {
                list = new List<MenuNode>();
                MenuNode.Init(ref list);
            }
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, list);
            }
        }
        private void notifystateupdate(object sender, EventArgs e)
        {
            notifyIcon.Text = mainwindow.angent.GetStatus();
        }

        private void loadstate(object sender, EventArgs e)
        {
            EndTime = DateTime.Now;
            TimeSpan RunTime = new TimeSpan();
            RunTime = EndTime.Subtract(BeginTime);
            string msg = "开启了"+Convert.ToString(RunTime)+"\r\n";
            msg += "精灵状态：" + mainwindow.angent.GetStatus() + "\r\n";
            System.Windows.MessageBox.Show(msg);
        }
        /// <summary>
        /// 绑定菜单和功能的处理函数
        /// </summary>
        /// <param name="item"></param>
        private void EventBinding(ref MenuItem item)
        {
            string util = item.Text;
            switch (util)
            {
                case "设置":item.Click += new EventHandler(set_Click);break;
                case "最近使用":item.Click += new EventHandler(Utils.Process_Click);break;
                case "最近文件":item.Click += new EventHandler(Utils.RecentFile_Click);break;
                case "壁纸":item.Click += new EventHandler(Utils.Wallpaper_Clicked);break;
                case "闹钟":item.Click += new EventHandler(Utils.Alarm_Clicked);break;
                case "笔记": item.Click += new EventHandler(Utils.Note_Clicked); break;
                case "备忘录": item.Click += new EventHandler(Utils.Todo_Clicked); break;
                case "翻译器": item.Click += new EventHandler(Utils.Translate_Clicked); break;
                case "图片编辑": item.Click += new EventHandler(Utils.ImgEdit_Clicked); break;
                default: break;
            }
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_Click(object sender, EventArgs e)
        {
            Forms.Settings settings = new Forms.Settings();
            //订阅更新事件
            settings.Notification.UpdateEvent += new EventHandler(UpdateMenu);
            settings.Show();
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
