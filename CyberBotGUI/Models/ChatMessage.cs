using System;
using System.Collections.Generic;
using System.Text;

namespace CyberBotGUI.Models
{
    public enum MessageSender { User, Bot }
    public class ChatMessage
    {
        public string Text { get; set; }
        public MessageSender Sender { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public ChatMessage(string text, MessageSender sender)
        {
            Text = text;
            Sender = sender;
        }
    }
}
