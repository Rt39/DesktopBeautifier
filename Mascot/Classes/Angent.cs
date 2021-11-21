using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mascot
{
    public class Angent
    {
        public string Name { set; get; }
        private int Status;
        private string resDir;
        private List<string> list;
        private static string StatusPath = Path.Combine(Definitions.SettingFolder, "Status.log");
        public Angent(string Name)
        {
            this.Name = Name;
            Status = 0;
            resDir = $"../../Resources/{Name}/frame";
            if (!File.Exists(StatusPath)) SaveStatus();
            using (FileStream fileStream = new FileStream(StatusPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter b = new BinaryFormatter();
                list = (List<string>)b.Deserialize(fileStream);
            }
        }
        public string GetDir()
        {
            return this.resDir;
        }
        public string GetStatus()
        {
            return list[Status];
        }
        public string ChangeStatus(bool negative)
        {
            if (negative && Status < 4) Status++;
            else if (!negative && Status > 1) Status--;
            switch (Status)
            {
                case 1:return "我很开心！";
                case 2:return "不过我心情一般~";
                default:return "不过我心情很不好，我想休息一会";
            }
        }
        public static void SaveStatus()
        {
            List<string> list = new List<string>() ;
            list.Add("开心^_^");
            list.Add("一般。");
            list.Add("失落~");
            list.Add("生气！");
            list.Add("兴奋~");
            using (FileStream fileStream = new FileStream(StatusPath, FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, list);
            }
        }
    }
}
