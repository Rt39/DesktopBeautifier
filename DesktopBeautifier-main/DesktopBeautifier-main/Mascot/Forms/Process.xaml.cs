using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Process.xaml 的交互逻辑
    /// </summary>
    public partial class Process : Window
    {
        private HashSet<ApplicationInfo> infos;
        private List<ApplicationInfo> applicationInfos = new List<ApplicationInfo>();
        public Process()
        {
            InitializeComponent();
            infos = PipeClient.GetApplicationInfos();
            Manage(infos);
        }
        private void Manage(HashSet<ApplicationInfo> infos)
        {
            if (infos == null) return;
            listView.Items.Clear();
            listView.BeginInit();
            foreach (var i in infos)
            {
                i.ApplicationRunIntervals /= 2;
                applicationInfos.Add(i);
            }
            MySort(1);
        }
        private void MySort(int type)
        {
            //排序
            applicationInfos.Sort(delegate (ApplicationInfo info1, ApplicationInfo info2)
            {
                if(type==1)
                    return info1.ApplicationName.CompareTo(info2.ApplicationName);
                else if(type==2)
                    return info2.ApplicationRunIntervals.CompareTo(info1.ApplicationRunIntervals);
                else
                    return info2.ApplicationClicks.CompareTo(info1.ApplicationClicks);
            });
            this.listView.Dispatcher.Invoke(new Action(() =>
            {
                listView.ItemsSource = applicationInfos;
            }));
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                if (clickedColumn != null)
                {
                    this.listView.Dispatcher.Invoke(new Action(()=> listView.ItemsSource=new List<ApplicationInfo>()));
                    string header = clickedColumn.Header.ToString();
                    if (header == "进程") MySort(1);
                    else if (header.Contains("min")) MySort(2);
                    else MySort(3);
                }
            }
        }
    }
}
