using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mascot;

namespace WallPaper.Clawer {
    class NewWPC {
        public List<string> data = new List<string>();
        public List<string> Picture_Url = new List<string>();
        public string RandomNum;
        public string Txt_Path;
        public void Judge(int instruction) {
            if (IsConnectInternet()) {
                Excute(instruction);
            }
            else {
                //网络未连接，不推送
            }
        }
        public void Excute(int instruct) {
            //完善文件夹分类
            DateTime now = DateTime.Now;
            RandomNum = "NewWallPaper" + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            Txt_Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"PicId.txt";
            
            //读取，将txt中的内容读取到列表data中
            StreamReader sr = new StreamReader(Txt_Path);
            while (sr.ReadLine() != null) {
                data.Add(sr.ReadLine());
            }
            sr.Close();

            //txt放在应用内
            GetData("https://wall.alphacoders.com/popular.php", data[0], instruct);
            SaveASWebImg sa = new SaveASWebImg();
            for (int i = 0; i < Picture_Url.Count; i++) {
                sa.Download(Picture_Url[i],RandomNum);
            }
        }

        public void GetData(String address, string picture_list, int inst) {
            WebClient wc = new WebClient();
            //地址由调用时传入
            byte[] htmlData = wc.DownloadData(address);

            string html = Encoding.UTF8.GetString(htmlData);
            //使用正则表达式匹配 
            Regex reg = new Regex("https://images[0-9]{0,1}.alphacoders.com/[0-9]{3}/thumbbig-[0-9]{0,}.jpg");
            //接受所有匹配到的项
            MatchCollection result = reg.Matches(html);
            //循环输出
            string str = "\n";
            foreach (Match item in result) {
                string pic_id = "";
                pic_id = item.Value.Substring(0, item.Value.LastIndexOf('.'));
                pic_id = pic_id.Substring(pic_id.LastIndexOf('-') + 1);
                str = str + pic_id + " + ";

                if (picture_list.Contains(pic_id) == false) {
                    Picture_Url.Add(item.Value);
                }
            }
            //判断是否有更新
            if (Picture_Url.Count > 0) {
                //查看后更新
                if(inst == 1) {
                    File.WriteAllText(Txt_Path, str);
                }
                else {
                    Notification.OnNoteEvent($"{Picture_Url.Count}张新壁纸待查看",new EventArgs());
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
    }
}
