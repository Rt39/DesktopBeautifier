using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorWPF
{
    //语言类
    public class Lang
    {
        //中文转英文字典
        public Dictionary<string, string> zh2en;

        //英文转中文字典
        public Dictionary<string, string> en2zh;

        //语言列表
        public List<LangList> langList;

        //单例对象
        private static Lang lang = new Lang();

        //构造函数
        private Lang()
        {
            //初始化字典
            zh2en = new Dictionary<string, string>();
            en2zh = new Dictionary<string, string>();

            //添加元素
            zh2en.Add("自动检测", "auto");
            zh2en.Add("中文", "zh");
            zh2en.Add("英语", "en");
            zh2en.Add("粤语", "yue");
            zh2en.Add("文言文", "wyw");
            zh2en.Add("日语", "jp");
            zh2en.Add("韩语", "kor");
            zh2en.Add("法语", "fra");
            zh2en.Add("西班牙语", "spa");
            zh2en.Add("泰语", "th");
            zh2en.Add("阿拉伯语", "ara");
            zh2en.Add("俄语", "ru");
            zh2en.Add("葡萄牙语", "pt");
            zh2en.Add("德语", "de");
            zh2en.Add("意大利语", "it");
            zh2en.Add("希腊语", "el");
            zh2en.Add("荷兰语", "nl");
            zh2en.Add("波兰语", "pl");
            zh2en.Add("保加利亚语", "bul");
            zh2en.Add("爱沙尼亚语", "est");
            zh2en.Add("丹麦语", "dan");
            zh2en.Add("芬兰语", "fin");
            zh2en.Add("捷克语", "cs");
            zh2en.Add("罗马尼亚语", "rom");
            zh2en.Add("斯洛文尼亚语", "slo");
            zh2en.Add("瑞典语", "swe");
            zh2en.Add("匈牙利语", "hu");
            zh2en.Add("繁体中文", "cht");
            zh2en.Add("越南语", "vie");

            en2zh.Add("auto", "自动检测");
            en2zh.Add("zh", "中文");
            en2zh.Add("en", "英语");
            en2zh.Add("yue", "粤语");
            en2zh.Add("wyw", "文言文");
            en2zh.Add("jp", "日语");
            en2zh.Add("kor", "韩语");
            en2zh.Add("fra", "法语");
            en2zh.Add("spa", "西班牙语");
            en2zh.Add("th", "泰语");
            en2zh.Add("ara", "阿拉伯语");
            en2zh.Add("ru", "俄语");
            en2zh.Add("pt", "葡萄牙语");
            en2zh.Add("de", "德语");
            en2zh.Add("it", "意大利语");
            en2zh.Add("el", "希腊语");
            en2zh.Add("nl", "荷兰语");
            en2zh.Add("pl", "波兰语");
            en2zh.Add("bul", "保加利亚语");
            en2zh.Add("est", "爱沙尼亚语");
            en2zh.Add("dan", "丹麦语");
            en2zh.Add("fin", "芬兰语");
            en2zh.Add("cs", "捷克语");
            en2zh.Add("rom", "罗马尼亚语");
            en2zh.Add("slo", "斯洛文尼亚语");
            en2zh.Add("swe", "瑞典语");
            en2zh.Add("hu", "匈牙利语");
            en2zh.Add("cht", "繁体中文");
            en2zh.Add("vie", "越南语");

            //初始化列表
            langList = new List<LangList>();

            //添加元素
            langList.Add(new LangList { EN = "auto", ZH = "自动检测" });
            langList.Add(new LangList { EN = "zh", ZH = "中文" });
            langList.Add(new LangList { EN = "en", ZH = "英语" });
            langList.Add(new LangList { EN = "yue", ZH = "粤语" });
            langList.Add(new LangList { EN = "wyw", ZH = "文言文" });
            langList.Add(new LangList { EN = "jp", ZH = "日语" });
            langList.Add(new LangList { EN = "kor", ZH = "韩语" });
            langList.Add(new LangList { EN = "fra", ZH = "法语" });
            langList.Add(new LangList { EN = "spa", ZH = "西班牙语" });
            langList.Add(new LangList { EN = "th", ZH = "泰语" });
            langList.Add(new LangList { EN = "ara", ZH = "阿拉伯语" });
            langList.Add(new LangList { EN = "ru", ZH = "俄语" });
            langList.Add(new LangList { EN = "pt", ZH = "葡萄牙语" });
            langList.Add(new LangList { EN = "de", ZH = "德语" });
            langList.Add(new LangList { EN = "it", ZH = "意大利语" });
            langList.Add(new LangList { EN = "el", ZH = "希腊语" });
            langList.Add(new LangList { EN = "nl", ZH = "荷兰语" });
            langList.Add(new LangList { EN = "pl", ZH = "波兰语" });
            langList.Add(new LangList { EN = "bul", ZH = "保加利亚语" });
            langList.Add(new LangList { EN = "est", ZH = "爱沙尼亚语" });
            langList.Add(new LangList { EN = "dan", ZH = "丹麦语" });
            langList.Add(new LangList { EN = "fin", ZH = "芬兰语" });
            langList.Add(new LangList { EN = "cs", ZH = "捷克语" });
            langList.Add(new LangList { EN = "rom", ZH = "罗马尼亚语" });
            langList.Add(new LangList { EN = "slo", ZH = "斯洛文尼亚语" });
            langList.Add(new LangList { EN = "swe", ZH = "瑞典语" });
            langList.Add(new LangList { EN = "hu", ZH = "匈牙利语" });
            langList.Add(new LangList { EN = "cht", ZH = "繁体中文" });
            langList.Add(new LangList { EN = "vie", ZH = "越南语" });
        }

        //获取单例对象
        public static Lang getLang()
        {
            return lang;
        }
    }

    public class LangList
    {
        public string EN { get; set; }
        public string ZH { get; set; }
    }

    enum LangEnum
    {
        auto, //自动检测
        zh,   //中文
        en,   //英语
        yue,  //粤语
        wyw,  //文言文
        jp,   //日语
        kor,  //韩语
        fra,  //法语
        spa,  //西班牙语
        th,   //泰语
        ara,  //阿拉伯语
        ru,   //俄语
        pt,   //葡萄牙语
        de,   //德语
        it,   //意大利语
        el,   //希腊语
        nl,   //荷兰语
        pl,   //波兰语
        bul,  //保加利亚语
        est,  //爱沙尼亚语
        dan,  //丹麦语
        fin,  //芬兰语
        cs,   //捷克语
        rom,  //罗马尼亚语
        slo,  //斯洛文尼亚语
        swe,  //瑞典语
        hu,   //匈牙利语
        cht,  //繁体中文
        vie   //越南语
    }
}
