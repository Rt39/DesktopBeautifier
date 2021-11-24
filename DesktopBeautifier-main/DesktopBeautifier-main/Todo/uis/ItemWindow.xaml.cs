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

namespace Todo
{
    /// <summary>
    /// MdfWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ItemWindow : Window
    {
        public ItemWindow()
        {
            InitializeComponent();
        }

        public void SetTextBox(string title, string content)
        {
            tbxTitle.Text = title;
            tbxContent.Text = content;
        }

        private void btnCan_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCfm_Click(object sender, RoutedEventArgs e)
        {
            string title = tbxTitle.Text;
            string content = tbxContent.Text;

            if (title.Equals(""))
            {
                MessageBox.Show("请输入标题！", "提示");
                return;
            }

            MainWindow m = this.Owner as MainWindow;
            m.SetTodoItem(title, content);

            this.DialogResult = true;
            this.Close();
        }
    }
}
