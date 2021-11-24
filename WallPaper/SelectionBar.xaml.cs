using System;
using System.Collections.Generic;
using System.IO;
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
using System.Runtime.InteropServices;
using WallPaper.Clawer;
using WallPaper.utils;
 
namespace WallPaper {
    /// <summary>
    /// SelectionBar.xaml 的交互逻辑
    /// </summary>
    public partial class SelectionBar : Window {
        private string KeyWord;
        NewWPC newWPC; 
        public SelectionBar() {
            InitializeComponent();
            newWPC = new NewWPC();
            newWPC.Judge(1);
            if (newWPC.Picture_Url.Count == 0) {
                lbl_recommend.Content = "暂无新壁纸推送";
            }
            else {
                lbl_recommend.Content = $"{newWPC.Picture_Url.Count}张新壁纸待查看";
            }
            ShowTools(0);
        }

        private void btn_search_Click(object sender, RoutedEventArgs e) {
            KeyWord = rb_Landscape.IsChecked == true ? "Landscape" : null;
            KeyWord = rb_Anime.IsChecked == true ? "Anime" : KeyWord;
            KeyWord = rb_Pets.IsChecked == true ? "Pets" : KeyWord;
            KeyWord = rb_ScienceFiction.IsChecked == true ? "Science Fiction" : KeyWord;
            KeyWord = rb_Games.IsChecked == true ? "Games" : KeyWord;
            KeyWord = rb_Sports.IsChecked == true ? "Sports" : KeyWord;
            KeyWord = rb_Others.IsChecked == true ? txb_keyword.Text : KeyWord;
            if (KeyWord != null) {
                //完善文件夹分类
                DateTime now = DateTime.Now;
                string RandomNum = "WallPaper" + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
                string folderFullName = "C:\\Windows\\Temp\\" + RandomNum;
                //抛出异常：网络
                if (IsConnectInternet()) {
                    //爬虫爬取、下载
                    //显示正在爬取
                    try {
                        WallPaperClawer wallPaperClawer = new WallPaperClawer();
                        wallPaperClawer.ChooseWeb(KeyWord);
                        wallPaperClawer.ClawerWeb(RandomNum);

                        MainWindow Mn = new MainWindow(RandomNum);
                        this.Hide();
                        Mn.Show();
                    }
                    catch {
                        //显示未搜索到壁纸，请更换关键词
                        MessageBox.Show("未搜索到壁纸，请更换关键词!", "提示");
                    }
                    //未搜索到图片
                    //if (Directory.GetFiles(folderFullName).Length > 0) {  }
                }
                else {
                    //显示错误
                    MessageBox.Show("网络未连接!", "提示");
                }
            }
        }


        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);

        //用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败
        public static bool IsConnectInternet() {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }

        private void btn_interest_Click(object sender, RoutedEventArgs e) {
            ShowTools(0);
        }

        private void btn_recommend_Click(object sender, RoutedEventArgs e) {
            ShowTools(1);
        }

        private void ShowTools(int i) {
            SP_interest.Visibility = i == 0 ? Visibility.Visible : Visibility.Hidden;
            btn_search.Visibility = i == 0 ? Visibility.Visible : Visibility.Hidden;
            lbl_recommend.Visibility = i == 1 ? Visibility.Visible : Visibility.Hidden;
            btn_examine.Visibility = (newWPC.Picture_Url.Count > 0 && i == 1) ? Visibility.Visible : Visibility.Hidden;
        }

        private void btn_examine_Click(object sender, RoutedEventArgs e) {
            MainWindow MW = new MainWindow(newWPC.RandomNum);
            this.Hide();
            MW.Show();
        }
    }
}