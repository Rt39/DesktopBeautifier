using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Todo
{
    [Serializable]
    class TodoItem
    {
        public DateTime Date { get; set; }     //日期
        public string Title { get; set; }      //标题
        public string Content { get; set; }    //内容

        public TodoItem()
        {
            Date = DateTime.Now.Date;
            Title = "请输入标题。";
            Content = "请输入内容。";
        }

        public TodoItem(DateTime date, string title, string content)
        {
            Date = date;
            Title = title;
            Content = content;
        }

        public TodoItem(TodoItem todoItem)
        {
            Date = todoItem.Date;
            Title = todoItem.Title;
            Content = todoItem.Content;
        }

        public static List<TodoItem> SearchByDate(List<TodoItem> todoItems, DateTime dateTime)
        {
            var todoItemsRlt =
                from item in todoItems
                where item.Date == dateTime
                select item;

            return todoItemsRlt.ToList();
        }

        public static string getMemory(object o) // 获取引用类型的内存地址方法    
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }

        public static void SaveAsBin(List<TodoItem> todoItems, string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, todoItems);
            stream.Close();
        }

        public static List<TodoItem> OpenAsBin(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            List<TodoItem> todoItems = (List<TodoItem>)formatter.Deserialize(stream);
            stream.Close();

            return todoItems;
        }

        public static void SaveAsJson(List<TodoItem> todoItems, string fileName)
        {
            string json = JsonConvert.SerializeObject(todoItems);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(json);
            }
        }

        public static List<TodoItem> OpenAsJson(string fileName)
        {
            string line;
            string json = String.Empty;

            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    json += line;
                }
            }

            return JsonConvert.DeserializeObject<List<TodoItem>>(json);
        }
    }
}
