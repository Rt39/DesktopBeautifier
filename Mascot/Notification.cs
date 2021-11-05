using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mascot
{
    public  class Notification
    {
        protected string NoteStr="cask";
        private static EventHandler OnNoteEvent;
        public event EventHandler WallPaperEvent
        {
            add { OnNoteEvent += new EventHandler(value); }
            remove { OnNoteEvent -= new EventHandler(value); }
        }
        public event EventHandler OtherEvent;
        public void test()
        {
            OnNoteEvent(NoteStr, new EventArgs());
        }
    }
}
