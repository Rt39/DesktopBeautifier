using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Mascot.Forms
{
    /// <summary>
    /// File.xaml 的交互逻辑
    /// </summary>
    public partial class File : Window
    {
        private string newRealPath;
        private List<FastPath> fastPaths;
        private DesktopFileWatcher FileWatcher;
        public File(ref DesktopFileWatcher _fileWatcher,string filename)
        {
            InitializeComponent();
            FileWatcher = _fileWatcher;
            _fileWatcher.Dispose();
            this.Dispatcher.Invoke(new Action(() =>
            {
                fileLabel.Content = filename;
                RemoveBtn.Visibility = Visibility.Visible;
            }));
            fastPaths = FastPath.getPathList(_fileWatcher.FileWatchSettings.ArchiveDirectory);
            this.pathListview.Dispatcher.Invoke(new Action(() => { pathListview.ItemsSource = fastPaths; }));
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            newRealPath = m_Dir;
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(fastPathbox.Text))
            {
                FastPath fastPath = new FastPath(fastPathbox.Text, newRealPath);
                fastPaths.Add(fastPath);
                newRealPath = null;
                this.pathListview.Dispatcher.Invoke(new Action(() 
                    => {fastPathbox.Text=string.Empty; pathListview.ItemsSource=null; pathListview.ItemsSource = fastPaths; }));
            }
        }

        private void SelectPath_Changed(object sender, SelectionChangedEventArgs e)
        {
            var path = pathListview.SelectedItem as FastPath;
            //选择路径并进行转移
            if(path!=null && path is FastPath)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileLabel.Content as string));
                fileInfo.MoveTo(Path.Combine(path.realPath, fileInfo.Name));
                this.Dispatcher.Invoke(new Action(() =>
                {
                    fileLabel.Content = null;
                    RemoveBtn.Visibility = Visibility.Hidden;
                }));
            }
        }

        private void Delete_Right_Click(object sender, RoutedEventArgs e)
        {
            var path = pathListview.SelectedItem as FastPath;
            if (path != null && path is FastPath)
            {
                foreach(var i in fastPaths)
                {
                    if(i.filePath==path.filePath)
                    {
                        fastPaths.Remove(i);break;
                    }
                }
                this.pathListview.Dispatcher.Invoke(new Action(() =>
                { pathListview.ItemsSource = null; pathListview.ItemsSource = fastPaths; }));
            }
        }

        private void SavePath_Closing(object sender, CancelEventArgs e)
        {
            FileWatcher.FileWatchSettings.ArchiveDirectory = FastPath.getPathDic(fastPaths);
            FileWatcher.Dispose();
            Utils.flag = false;
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                fileLabel.Content = null;
                RemoveBtn.Visibility = Visibility.Hidden;
            }));
        }
    }
}
