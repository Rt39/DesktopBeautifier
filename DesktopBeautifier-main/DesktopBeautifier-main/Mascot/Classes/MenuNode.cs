using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Mascot
{
    [Serializable]
    public class MenuNode
    {
        public string NodeName;
        public List<int> ChildMenu;
        public static void Init(ref List<MenuNode> list)
        {
            MenuNode node = new MenuNode();
            node.NodeName = "Start";
            node.ChildMenu = new List<int>();
            list.Add(node);
            MenuNode.Insert("设置", "Start", ref list);
        }
        public bool HasChild()
        {
            return ChildMenu.Count != 0;
        }
        public static void Insert(string name,string preName,ref List<MenuNode> list)
        {
            int index = list.Count;
            for(int i=0;i<list.Count;i++)
            {
                if(list[i].NodeName==preName)
                {
                    list[i].ChildMenu.Add(index);
                    MenuNode node = new MenuNode();
                    node.NodeName = name;
                    node.ChildMenu = new List<int>();
                    list.Add(node);
                    break;
                }
            }   
        }
        public static void Delete(ref List<MenuNode> list)
        {
            list = new List<MenuNode>();
            MenuNode node = new MenuNode();
            node.NodeName = "Start";
            node.ChildMenu = new List<int>();
            list.Add(node);
            MenuNode.Insert("设置", "Start", ref list);
        }
        public static void SaveNode(ref List<MenuNode> list,string Path = "Menu.gra")
        {
            using (FileStream fileStream = new FileStream(Path, FileMode.Create))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, list);
            }
        }
        public static void LoadMenu(ref List<MenuNode> list,string Path = "Menu.gra")
        {
            using (FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter b = new BinaryFormatter();
                list = (List<MenuNode>)b.Deserialize(fileStream);
            }
        }
        public static bool FindNode(string name,List<MenuNode> list)
        {
            int index = list.Count;
            for (int i = 0; i < index; i++)
                if (list[i].NodeName == name) return true;
            return false;
        }
    }
}
