using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WallPaper.Clawer
{
    class WallPaperClawer
    {
        public string Website { get; set; }
        #region download
        public void ChooseWeb(int selectindex,string category)
        {
            if (selectindex == 0)
            {
                Website = $"https://wall.alphacoders.com/popular.php"; //热门推荐
            }
            else
            {
                Website = $"https://wall.alphacoders.com/search.php?search={category}"; //搜索壁纸
            }

        }
        public void ClawerWeb()
        {
            string website = Website;
            var page = 1;//抓取的页数
            //抓取网页资源
            for (int i = 1; i <= page; i++)
            {
                //地址
                string str = GetHtmlStr(website, "UTF8");
                //匹配图片的正则表达式,表达式还应加入png格式，[jpg|png]错误
                string regstr = "https://images[0-9]{0,1}.alphacoders.com/[0-9]{3}/thumbbig-[0-9]{0,}.jpg";
                foreach (Match match in Regex.Matches(str, regstr))
                //使用正则表达式解析网页文本，获得图片地址
                {
                    //下载图片
                    SaveAsWebImg(match.Value);
                }
                //Console.ReadKey();
                //Console.WriteLine("已执行结束，按任意键退出！");
                //Console.ReadKey();
                #endregion download
            }
        }

        public void TranslateCategory(string category)
        { 

        }

        /// <summary>  
        /// 获取网页的HTML码  
        /// </summary>  
        /// <param name="url">链接地址</param>  
        /// <param name="encoding">编码类型</param>  
        /// <returns></returns>  
        public static string GetHtmlStr(string url, string encoding)
        {
            string htmlStr = "";
            if (!String.IsNullOrEmpty(url))
            {
                WebRequest request = WebRequest.Create(url);            //实例化WebRequest对象  
                WebResponse response = request.GetResponse();           //创建WebResponse对象  
                Stream datastream = response.GetResponseStream();       //创建流对象  
                Encoding ec = Encoding.Default;
                if (encoding == "UTF8")
                {
                    ec = Encoding.UTF8;
                }
                else if (encoding == "Default")
                {
                    ec = Encoding.Default;
                }
                StreamReader reader = new StreamReader(datastream, ec);
                htmlStr = reader.ReadToEnd();                           //读取数据  
                reader.Close();
                datastream.Close();
                response.Close();
            }
            return htmlStr;
        }

        /// <summary> 
        /// 下载网站图片 
        /// </summary> 
        /// <param name="picUrl"></param> 
        /// <returns></returns> 
        public static string SaveAsWebImg(string picUrl)
        {
            string result = "";
            //设置保存目录
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/File/";
            Console.WriteLine("路径为：" + path);
            //不存在目录则创建
            if (!Directory.Exists(path))
            {
                //创建目录
                Directory.CreateDirectory(path);
            }
            try
            {
                //判断图片是否为空或者null
                if (!String.IsNullOrEmpty(picUrl))
                {
                    //伪随机数生成器
                    Random rd = new Random();
                    //获取当前日期时间
                    DateTime nowTime = DateTime.Now;
                    //获取URL扩展名
                    var Extension = Path.GetExtension(picUrl);
                    //自定义文件名
                    string fileName = nowTime.Month.ToString() + nowTime.Day.ToString() + nowTime.Hour.ToString() + nowTime.Minute.ToString() + nowTime.Second.ToString() + rd.Next(1000, 1000000) + Extension;
                    WebClient webClient = new WebClient();
                    //下载url链接文件，并指定到本地的文件夹路径和文件名称
                    webClient.DownloadFile(picUrl, path + fileName);
                    //返回结果
                    result = fileName;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
    }
}




