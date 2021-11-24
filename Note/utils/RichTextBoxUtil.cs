using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Note
{
    class RichTextBoxUtil
    {
        //将富文本控件的内容转换成string
        public static string RichTextBoxToString(RichTextBox box)
        {
            //创建一个流
            MemoryStream s = new MemoryStream();
            //获得富文本中的内容
            TextRange documentTextRange = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            //将富文本中的内容转换成xaml的格式，并保存到指定的流中
            documentTextRange.Save(s, DataFormats.XamlPackage);
            //将流中的内容转换成字节数组，并转换成base64的等效格式
            return Convert.ToBase64String(s.ToArray());
        }

        //将数据库中的内容转换回RichTextBox内容
        public static void StringToRichTextBox(string data, RichTextBox box)
        {
            MemoryStream s = new MemoryStream((Convert.FromBase64String(Convert.ToString(data))));
            TextRange TR = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
            TR.Load(s, DataFormats.XamlPackage);
        }
    }
}
