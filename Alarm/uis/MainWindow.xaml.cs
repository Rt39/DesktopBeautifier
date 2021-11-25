using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alarm
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public delegate void DoSomeCallBack();                       //定义委托

        DoSomeCallBack callBackTick;                                 //声明Tick回调
        DoSomeCallBack callBackAlarm;                                //声明Alarm回调

        System.Timers.Timer tickTimer;                               //时钟走动的定时器
        System.Timers.Timer alarmTimer;                              //闹钟判断的定时器

        ObservableCollection<AlarmItem> alarmItems;                  //闹钟

        SoundPlayer sound = new SoundPlayer();                       //播放音乐

        private string _folder;
        private string xmlFile;                                //XML文件路径
        //private static readonly string _folder = System.IO.Path.Combine(Utils.Definitions.SettingFolder, "Todo");
        //private static readonly string _dataPath = System.IO.Path.Combine(_folder, "data.json");

        AlarmItem alarmItem = new AlarmItem();                       //用于添加和修改

        public MainWindow()
        {
            InitializeComponent();

            _folder = System.IO.Path.Combine(Utils.Definitions.SettingFolder, @"Alarm");
            xmlFile = System.IO.Path.Combine(_folder, @"data.xml");
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);

            //设置回调
            this.callBackTick = new DoSomeCallBack(TickTimerElapsed);
            this.callBackAlarm = new DoSomeCallBack(AlarmTimerElapsed);

            //更新并绑定
            UpdateBinding();

            //设置定时器
            SetTickTimer();
            SetAlarmTimer();


        }

        private void SetTickTimer()
        {
            //设置时钟走动的定时器
            this.tickTimer = new System.Timers.Timer();

            //设置定时器
            this.tickTimer.Interval = 100;
            this.tickTimer.Elapsed += delegate {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(callBackTick);
                }
                else
                {
                    TickTimerElapsed();
                };
            };
            this.tickTimer.Start();
        }

        private void SetAlarmTimer()
        {
            //设置闹钟判断的定时器
            this.alarmTimer = new System.Timers.Timer();

            //设置定时器
            this.alarmTimer.Interval = 1000;
            this.alarmTimer.Elapsed += delegate {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(callBackAlarm);
                }
                else
                {
                    AlarmTimerElapsed();
                };
            };
            this.alarmTimer.Start();
        }

        private void TickTimerElapsed()
        {
            int hour = DateTime.Now.Hour;

            if (hour >= 3 && hour < 6)
                lblNow.Content = "凌晨";
            else if (hour >= 6 && hour < 8)
                lblNow.Content = "早晨";
            else if (hour >= 8 && hour < 11)
                lblNow.Content = "上午";
            else if (hour >= 11 && hour < 13)
                lblNow.Content = "中午";
            else if (hour >= 13 && hour < 17)
                lblNow.Content = "下午";
            else if (hour >= 17 && hour < 19)
                lblNow.Content = "傍晚";
            else if (hour >= 19 && hour < 23)
                lblNow.Content = "晚上";
            else
                lblNow.Content = "深夜";

            lblHour.Content = DateTime.Now.Hour.ToString("D2");
            lblMin.Content = DateTime.Now.Minute.ToString("D2");
            lblSec.Content = DateTime.Now.Second.ToString("D2");
        }

        private void AlarmTimerElapsed()
        {
            TimeSpan timeSpan = new TimeSpan(999,0,0,0,0);

            foreach (var item in alarmItems)
            {
                TimeSpan ts = item.Time - DateTime.Now;

                if (ts.TotalSeconds < 0 || item.State == false)
                    continue;

                if (ts < timeSpan)
                    timeSpan = ts;

                if (ts.TotalSeconds < 1)
                {
                    try
                    {
                        if (sound.Stream == null)
                        {
                            sound.Stream = Properties.Resources.MELANCHOLY;
                        }
                        sound.PlayLooping();
                        MessageBox.Show(item.Msg);
                    }
                    catch
                    {
                        MessageBox.Show("Error");
                    }

                    //停止音乐
                    sound.Stop();

                    //设置State为false
                    item.State = false;

                    //保存到XML
                    AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

                    //更新绑定
                    UpdateBinding();
                }
            }

            if (timeSpan.Days == 999)
            {
                lblDay_.Content = "--";
                lblHour_.Content = "--";
                lblMin_.Content = "--";
            }
            else
            {
                lblDay_.Content = timeSpan.Days;
                lblHour_.Content = timeSpan.Hours;
                lblMin_.Content = timeSpan.Minutes;
            }
        }

        private void UpdateBinding()
        {
            //读取闹钟XML
            this.alarmItems = AlarmItem.OpenAsXML(this.xmlFile);
            //更新绑定
            dgdItem.ItemsSource = null;
            dgdItem.ItemsSource = this.alarmItems;
        }

        //设置AlarmItem
        public void SetAlarmItem(DateTime time, string msg, bool state)
        {
            this.alarmItem = new AlarmItem(time, msg, state);
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(alarmItems.Count);
        }

        //点击添加按钮
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ItemWindow itemWindow = new ItemWindow();
            itemWindow.Title = "Add";
            itemWindow.Owner = this;

            if (itemWindow.ShowDialog() == true)
            {
                //添加Item到全局对象
                this.alarmItems.Add(new AlarmItem(this.alarmItem));

                //保存到XML
                AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

                //更新绑定
                UpdateBinding();
            }
        }

        private void btnMdf_Click(object sender, RoutedEventArgs e)
        {
            AlarmItem selItem = (AlarmItem)dgdItem.SelectedItem;

            if (selItem == null)
            {
                MessageBox.Show("请选择一项！", "提示");
                return;
            }

            ItemWindow itemWindow = new ItemWindow();
            itemWindow.Title = "Modify";
            itemWindow.Owner = this;
            itemWindow.SetItemValue(selItem.Time, selItem.Msg, selItem.State);

            if (itemWindow.ShowDialog() == true)
            {
                //移除原Item
                this.alarmItems.Remove(selItem);

                //添加Item到全局对象
                this.alarmItems.Add(new AlarmItem(this.alarmItem));

                //保存到XML
                AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

                //更新并绑定
                UpdateBinding();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            AlarmItem selItem = (AlarmItem)dgdItem.SelectedItem;

            if (selItem == null)
            {
                MessageBox.Show("请选择一项！", "提示");
                return;
            }

            //移除Item
            this.alarmItems.Remove(selItem);

            //保存到XML
            AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

            //更新并绑定
            UpdateBinding();
        }

        private void dgdItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point aP = e.GetPosition(dgdItem);                  //返回鼠标指针相对于指定元素的位置
            IInputElement obj = dgdItem.InputHitTest(aP);       //返回坐标上的当前元素中的输入元素
            DependencyObject target = obj as DependencyObject;
            while (target != null)
            {
                if (target is DataGridRow)
                {
                    btnMdf.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                target = VisualTreeHelper.GetParent(target);
            }
        }

        private void Item_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dgdItem.CurrentCell.Column == dgdItem.Columns[2])
            {
                DataGridRow item = (DataGridRow)sender;

                AlarmItem selItem = (AlarmItem)item.Item;

                selItem.State = !selItem.State;

                //保存到XML
                AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

                //更新并绑定
                UpdateBinding();
            }
        }

        private void dgdItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (dgdItem.CurrentCell.Column == dgdItem.Columns[2])
            {
                DataGridRow item = (DataGridRow)sender;

                AlarmItem selItem = (AlarmItem)item.Item;

                selItem.State = !selItem.State;

                //保存到XML
                AlarmItem.SaveAsXML(this.alarmItems, this.xmlFile);

                //更新并绑定
                UpdateBinding();
            }
        }
    }
}
