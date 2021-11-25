using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot.UtilClass
{
    /// <summary>
    /// 判断是否是与功能相关的询问
    /// </summary>
    public static class JudgeUtil
    {
        public static int isRelative(string sentence)
        {
            if (sentence.Contains("最近使用"))return 1;
            else if (sentence.Contains("壁纸")) return 2;
            else if (sentence.Contains("提醒")) return 3;
            else if (sentence.Contains("最近文件"))return 4;
            return -1;
        }
    }
}
