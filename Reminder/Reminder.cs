using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder
{
    public class Reminders
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string Text { get; set; }
        public bool IsShown { get; set; } 
    }

}
