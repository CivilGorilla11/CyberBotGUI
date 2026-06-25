using System;
using System.Collections.Generic;
using System.Text;

namespace CyberBotGUI.Models
{
    public class CyberTask
    {
        public int Id { get; set; }
        public string  Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete{ get; set; }

        public DateTime CreatedAt { get; set; } 
        public DateTime ? ReminderTime
        {
            get; set;

        }

        public string ReminderMessage => ReminderTime.HasValue
            ? ReminderTime.Value.ToString("dd MMM yyyy HH:mm")
            : "No reminder set";

        public string StatusDisplay => IsComplete ? "Completed" : "Pending";


    }
}
