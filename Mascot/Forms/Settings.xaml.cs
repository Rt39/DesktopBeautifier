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

namespace Mascot.Forms
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {
        public Notification Notification = new Notification();
        List<MenuNode> list = new List<MenuNode>();
        public Settings()
        {
            InitializeComponent();
            MenuNode.LoadMenu(ref list);
            CheckBox1.IsChecked = (MenuNode.FindNode(CheckBox1.Content.ToString(), list));
            CheckBox2.IsChecked = (MenuNode.FindNode(CheckBox2.Content.ToString(), list));
            CheckBox3.IsChecked = (MenuNode.FindNode(CheckBox3.Content.ToString(), list));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuNode.Delete(ref list);
            if (CheckBox1.IsChecked.Value)
                MenuNode.Insert(CheckBox1.Content.ToString(), "Start",ref list);

            if (CheckBox2.IsChecked.Value)
                MenuNode.Insert(CheckBox2.Content.ToString(), "Start", ref list);

            if (CheckBox3.IsChecked.Value)
                MenuNode.Insert(CheckBox3.Content.ToString(), "Start", ref list);
            //保存
            MenuNode.SaveNode(ref list);
            Notification.OnUpdateEvent(this, new EventArgs());
            //Test test = new Test();test.Show();
            this.Close();

        }
    }
}
