using System;
using System.Collections.Generic;
using System.Text;

namespace CyberBotGUI.Core
{
    public static  class MemoryStore
    {
        private static readonly Dictionary<string, string> _memory = new Dictionary<string, string>();

        public static int MessageCount { get; private set; } = 0;
        public static string LastTopic { get; private set; } = "";
        public static string UserName { get; private set; } = "";

        public static void IncrementMessage()
        {
            MessageCount++;
        }

        public static void SetName(string name)
        {
            UserName = name;
            _memory["name"] = name;
        }

        public static  void SetLastTopic(string topic)
        {
            LastTopic = topic;
            _memory["lastTopic"] = topic;
        }

        public static string Get(string key)
            => _memory.ContainsKey(key) ? _memory[key] : "";

        public static bool HasName()
            => !string.IsNullOrWhiteSpace(UserName);

        public static string GetGreeting()
        {
            if (!HasName()) return "Hello I am CyberBot. What is your name?";
            if (MessageCount == 1) return $"Welcome back, {UserName}! What cybersecurity related topic can I assist with today";
            return $"Good to have you back in the chat, {UserName}!";  
        }

      
            }
}
