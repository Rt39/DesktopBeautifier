using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Alarm
{
    [Serializable]
    public class AlarmItem
    {
        public DateTime Time { get; set; }
        public string Msg { get; set; }
        public bool State { get; set; }
        public AlarmItem()
        {
            Time = DateTime.Now;
            Msg = "闹钟";
            State = true;
        }
        public AlarmItem(DateTime time, string msg)
        {
            Time = time;
            Msg = msg;
            State = true;
        }
        public AlarmItem(DateTime time, string msg, bool state)
        {
            Time = time;
            Msg = msg;
            State = state;
        }
        public AlarmItem(AlarmItem alarmItem)
        {
            Time = alarmItem.Time;
            Msg = alarmItem.Msg;
            State = alarmItem.State;
        }
        public static void SaveAsXML(ObservableCollection<AlarmItem> alarmItems, string fileName)
        {
            IOrderedEnumerable<AlarmItem> tempOrdered = from item in alarmItems orderby item.Time select item;

            ObservableCollection<AlarmItem> temp = new ObservableCollection<AlarmItem>();

            foreach (AlarmItem item in tempOrdered)
            {
                temp.Add(item);
            }

            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<AlarmItem>));
            TextWriter tw = new StreamWriter(fileName);
            formatter.Serialize(tw, temp);
            tw.Close();
        }
        public static ObservableCollection<AlarmItem> OpenAsXML(string fileName)
        {
            ObservableCollection<AlarmItem> alarmItems;
            
            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<AlarmItem>));
            TextReader tr = new StreamReader(fileName);
            alarmItems = (ObservableCollection<AlarmItem>)formatter.Deserialize(tr);
            tr.Close();

            return alarmItems;
        }
    }
}
