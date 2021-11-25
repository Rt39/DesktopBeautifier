using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WallPaper.Clawer
{
    class WallPaperClawer
    {
        public string Website { get; set; }
        public void ChooseWeb(string category)
        {
            Website = $"https://wall.alphacoders.com/search.php?search={category}"; //搜索壁纸
        }
        public async Task<bool> ClawerWeb(string random)
        {
            return await Task.Run(() => {
                string website = Website;
                var page = 1;//抓取的页数
                             //抓取网页资源
                for (int i = 1; i <= page; i++) {
                    //地址
                    string str = GetHtmlStr(website, "UTF8");
                    //匹配图片的正则表达式,表达式还应加入png格式，[jpg|png]错误-->[jpg|png]$
                    string regstr = "https://images[0-9]{0,1}.alphacoders.com/[0-9]{3}/thumbbig-[0-9]{0,}.jpg";
                    SaveASWebImg saveAS = new SaveASWebImg();
                    foreach (Match match in Regex.Matches(str, regstr))
                    //使用正则表达式解析网页文本，获得图片地址
                    {
                        //下载图片
                        saveAS.Download(match.Value, random);
                    }
                }
                return true;
            });
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
    }
}