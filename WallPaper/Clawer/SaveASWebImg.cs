using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WallPaper.Clawer {
    class SaveASWebImg {
        /// <summary> 
        /// 下载网站图片 
        /// </summary> 
        /// <param name="picUrl"></param> 
        /// <returns></returns> 
        public string Download(string picUrl, string randomnum) {
            string result = "";
            //伪随机数生成器
            Random rd = new Random();
            //获取当前日期时间
            DateTime nowTime = DateTime.Now;
            //设置保存目录
            string RandomNum = randomnum;
            string path = "C:\\Windows\\Temp\\" + RandomNum;
            //不存在目录则创建
            if (!Directory.Exists(path)) {
                //创建目录
                Directory.CreateDirectory(path);
            }
            try {
                //判断图片是否为空或者null
                if (!String.IsNullOrEmpty(picUrl)) {
                    //获取URL扩展名
                    var Extension = Path.GetExtension(picUrl);
                    //自定义文件名
                    string fileName = nowTime.Month.ToString() + nowTime.Day.ToString() + nowTime.Hour.ToString() + nowTime.Minute.ToString() + nowTime.Second.ToString() + rd.Next(1000, 1000000) + Extension;
                    WebClient webClient = new WebClient();
                    //下载url链接文件，并指定到本地的文件夹路径和文件名称
                    webClient.DownloadFile(picUrl, path + "\\" + fileName);
                    //返回结果
                    result = fileName;

                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}
