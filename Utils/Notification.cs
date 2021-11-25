using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot {
    public class Notification {
        public static EventHandler OnNoteEvent;
        public static EventHandler OnUpdateEvent;
        public event EventHandler MsgEvent {
            add { OnNoteEvent += new EventHandler(value); }
            remove { OnNoteEvent -= new EventHandler(value); }
        }
        public event EventHandler UpdateEvent {
            add { OnUpdateEvent += new EventHandler(value); }
            remove { OnUpdateEvent -= new EventHandler(value); }
        }
    }
}