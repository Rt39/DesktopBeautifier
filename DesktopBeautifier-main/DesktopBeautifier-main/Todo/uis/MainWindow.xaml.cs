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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Todo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TodoItem> todoItems = new List<TodoItem>();                //全局代办事项
        List<TodoItem> curItems = new List<TodoItem>();                 //当前绑定列表

        DateTime selDate = DateTime.Now.Date;                           //选中日历日期

        string jsonFile = @"data.json";                                 //Json文件路径

        TodoItem todoItem = new TodoItem();                             //用于添加和修改

        public MainWindow()
        {
            InitializeComponent();

            //读取列表Json
            this.todoItems = TodoItem.OpenAsJson(this.jsonFile);

            //更新绑定
            UpdateBinding();
        }

        //设置TodoItem
        public void SetTodoItem(string title, string content)
        {
            this.todoItem = new TodoItem(this.selDate, title, content);
        }

        //更新绑定
        private void UpdateBinding()
        {
            //更新选中日期
            if (cldMain.SelectedDate == null)
                this.selDate = DateTime.Now.Date;
            else
                this.selDate = (DateTime)cldMain.SelectedDate;

            //绑定数据源
            this.curItems = TodoItem.SearchByDate(this.todoItems, this.selDate);
            dgdItem.ItemsSource = this.curItems;
        }

        //更改日期
        private void cldMain_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //更新绑定
            UpdateBinding();
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
                this.todoItems.Add(new TodoItem(this.todoItem));

                //保存到Json
                TodoItem.SaveAsJson(this.todoItems, this.jsonFile);

                //更新绑定
                UpdateBinding();
            }
        }

        //点击修改按钮
        private void btnMdf_Click(object sender, RoutedEventArgs e)
        {
            TodoItem selItem = (TodoItem)dgdItem.SelectedItem;

            if (selItem == null)
            {
                MessageBox.Show("请选择一项！", "提示");
                return;
            }

            ItemWindow itemWindow = new ItemWindow();
            itemWindow.Title = "Modify";
            itemWindow.Owner = this;
            itemWindow.SetTextBox(selItem.Title, selItem.Content);
            
            if (itemWindow.ShowDialog() == true)
            {
                //移除原Item
                this.todoItems.Remove(selItem);

                //添加Item到全局对象
                this.todoItems.Add(new TodoItem(this.todoItem));

                //保存到Json
                TodoItem.SaveAsJson(this.todoItems, this.jsonFile);

                //更新选中日期并绑定
                UpdateBinding();
            }
        }

        //点击删除按钮
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            TodoItem selItem = (TodoItem)dgdItem.SelectedItem;

            if (selItem == null)
            {
                MessageBox.Show("请选择一项！", "提示");
                return;
            }

            //移除Item
            this.todoItems.Remove(selItem);

            //保存到Json
            TodoItem.SaveAsJson(this.todoItems, this.jsonFile);

            //更新选中日期并绑定
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
    }
}
