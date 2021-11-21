using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot
{
    class FastPath
    {
        public string filePath { set; get; }
        public string realPath { set; get; }
        public FastPath(string fp,string rp)
        {
            filePath = fp;
            realPath = rp;
        }
        //将字典转换成list便于listview展示
        public static List<FastPath> getPathList(Dictionary<string,string> dic)
        {
            List<FastPath> fastPaths = new List<FastPath>();
            foreach(var item in dic)
            {
                fastPaths.Add(new FastPath(item.Key, item.Value));
            }
            return fastPaths;
        }
        //将list转换为字典便于保存
        public static Dictionary<string,string> getPathDic(List<FastPath> fastPaths)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach(var item in fastPaths)
            {
                dic.Add(item.filePath, item.realPath);
            }
            return dic;
        }
    }
}
