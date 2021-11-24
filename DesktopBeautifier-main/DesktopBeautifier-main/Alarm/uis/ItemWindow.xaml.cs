using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Alarm
{
    /// <summary>
    /// ItemWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ItemWindow : Window
    {
        public ItemWindow()
        {
            InitializeComponent();

            dpkDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }

        public void SetItemValue(DateTime time, string msg, bool state)
        {
            dpkDate.Text = time.ToString("yyyy/MM/dd");
            tpkTime.Text = time.ToString("HH:mm:ss");
            tbxMsg.Text = msg;
            rbtOn.IsChecked = state;
            rbtOff.IsChecked = !state;
        }

        private void btnCan_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCfm_Click(object sender, RoutedEventArgs e)
        {
            DateTime time = Convert.ToDateTime(dpkDate.Text + " " + tpkTime.Text);
            string msg = tbxMsg.Text;
            bool state = (bool)rbtOn.IsChecked;

            if (msg.Equals(""))
            {
                MessageBox.Show("请输入闹钟名！", "提示");
                return;
            }

            MainWindow m = this.Owner as MainWindow;
            m.SetAlarmItem(time, msg, state);

            this.DialogResult = true;
            this.Close();
        }
    }
}
