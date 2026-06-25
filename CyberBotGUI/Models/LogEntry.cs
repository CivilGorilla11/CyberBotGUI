using System;
using System.Collections.Generic;
using System.Text;

namespace CyberBotGUI.Models
{
   public class LogEntry
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Action { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Display => $"[{TimeStamp:HH:mm:ss}] [{Category}] {Action} - {Description}";


    }
}
