using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranslatorWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Translator trans = new Translator();
        Lang lang = Lang.getLang();

        List<LangList> srcList;
        List<LangList> dstList;

        bool uitbxHasText = false;

        bool isSwapable = false;

        //定义委托
        public delegate void DoSomeCallBack();

        //声明回调 
        DoSomeCallBack doSomeCallBack;

        //定义Timer
        System.Timers.Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            //初始化列表
            this.srcList = new List<LangList>(lang.langList);
            this.dstList = new List<LangList>(lang.langList);
            this.dstList.RemoveRange(0, 1);

            //下拉列表框数据绑定
            cbxSrc.ItemsSource = this.srcList;
            cbxSrc.SelectedValuePath = "EN";
            cbxSrc.DisplayMemberPath = "ZH";

            cbxDst.ItemsSource = this.dstList;
            cbxDst.SelectedValuePath = "EN";
            cbxDst.DisplayMemberPath = "ZH";

            //下拉框默认值
            cbxSrc.SelectedIndex = 0;
            cbxDst.SelectedIndex = 0;

            //设置回调
            this.doSomeCallBack = new DoSomeCallBack(TranslateAsync);

            //设置定时器
            SetTimer();
        }

        //翻译文本
        private string Translate(string q, string from, string to)
        {
            return trans.Translate(q, from, to);
        }

        //设置定时器
        private void SetTimer()
        {
            this.timer = new System.Timers.Timer();

            this.timer.Interval = 1500;
            this.timer.Elapsed += delegate {
                if (!this.Dispatcher.CheckAccess())
                {
                    this.Dispatcher.Invoke(doSomeCallBack);
                }
                else
                {
                    TranslateAsync();
                };
            };

            this.timer.Start();
        }

        //异步任务
        private async Task<string> TranslateTask()
        {
            string q = tbxSrc.Text;
            string from = cbxSrc.SelectedValue.ToString();
            string to = cbxDst.SelectedValue.ToString();

            Task<string> task = new Task<string>(
                () => { return Translate(q, from, to); }
            );

            task.Start();

            //等待异步执行完毕
            string dst = await task;

            return dst;
        }

        //异步方法
        private async void TranslateAsync()
        {
            string dst = await TranslateTask();
            tbxDst.Text = dst;
        }

        //点击清空按钮
        private void btnClr_Click(object sender, RoutedEventArgs e)
        {
            //清空文本框
            tbxSrc.Clear();
            tbxDst.Clear();

            //初始化文本框
            tbxSrc.Text = "请输入文本进行翻译。";
            tbxSrc.Foreground = new SolidColorBrush(Colors.DimGray);
            this.uitbxHasText = false;
        }

        //点击交换按钮
        private void btnSwap_Click(object sender, RoutedEventArgs e)
        {
            //交互语言
            int temp = cbxSrc.SelectedIndex;
            cbxSrc.SelectedIndex = cbxDst.SelectedIndex + 1;
            cbxDst.SelectedIndex = temp - 1;

            //交换文本
            string text = tbxSrc.Text;
            tbxSrc.Text = tbxDst.Text;
            tbxDst.Text = text;
        }

        //改变源语言
        private void cbxSrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSrc.SelectedIndex == 0)
                this.isSwapable = false;
            else
                this.isSwapable = true;

            btnSwap.IsEnabled = this.isSwapable;
        }

        //下拉列表框获得焦点
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox uitbx = sender as System.Windows.Controls.TextBox;

            if (this.uitbxHasText == false)
                uitbx.Text = "";

            uitbx.Foreground = new SolidColorBrush(Color.FromArgb(255, 48, 48, 48));
        }

        //下拉列表框失去焦点
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox uitbx = sender as System.Windows.Controls.TextBox;

            if (uitbx.Text == "")
            {
                uitbx.Text = "请输入文本进行翻译。";
                uitbx.Foreground = new SolidColorBrush(Colors.DimGray);
                this.uitbxHasText = false;
            }
            else
                this.uitbxHasText = true;
        }
    }
}
