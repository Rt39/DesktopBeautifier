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
        public Angent(string Name)
        {
            this.Name = Name;
            Status = 0;
            resDir = $"../../Resources/{Name}/frame";
        }
        public string GetDir()
        {
            return this.resDir;
        }
        public string GetStatus()
        {
            List<string> list;
            using (FileStream fileStream = new FileStream("Status.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter b = new BinaryFormatter();
                list = (List<string>)b.Deserialize(fileStream);
            }
            return list[Status];
        }
        public static void SaveStatus()
        {
            List<string> list = new List<string>() ;
            list.Add("开心^_^");
            list.Add("一般。");
            list.Add("失落~");
            list.Add("生气！");
            list.Add("兴奋~");
            using (FileStream fileStream = new FileStream("Status.xml", FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, list);
            }
        }
    }
}
