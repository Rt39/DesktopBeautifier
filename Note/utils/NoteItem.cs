using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Note
{
    [Serializable]
    class NoteItem
    {
        public string Title { get; set; }      //标题
        public string Content { get; set; }    //内容

        public NoteItem()
        {
            Title = "请输入标题。";
            Content = "请输入内容。";
        }

        public NoteItem(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public NoteItem(NoteItem noteItem)
        {
            Title = noteItem.Title;
            Content = noteItem.Content;
        }

        public static void SaveAsJson(List<NoteItem> noteItems, string fileName)
        {
            string json = JsonConvert.SerializeObject(noteItems);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(json);
            }
        }

        public static List<NoteItem> OpenAsJson(string fileName)
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

            return JsonConvert.DeserializeObject<List<NoteItem>>(json);
        }
    }
}
