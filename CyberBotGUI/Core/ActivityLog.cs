using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyberBotGUI.Data;
using CyberBotGUI.Models;

namespace CyberBotGUI.Core
{
    public static class ActivityLog
    {
        private static readonly List<LogEntry> _entries = new List<LogEntry>();

        static ActivityLog()
        {
            var dbLogs = DatabaseService.GetLog();
            _entries.AddRange(dbLogs);
        }

        public static void Add(string action, string description, string category = "SYSTEM")
        {
            var entry = new LogEntry
            {
                TimeStamp = DateTime.Now,
                Action = action,
                Description = description,
                Category = category
            };

            _entries.Add(entry);
            DatabaseService.AddLog(action, description, category);
        }

        public static List<LogEntry> GetAll() =>
            _entries.OrderByDescending(e => e.TimeStamp).ToList();

        public static string FormatForChat()
        {
            var entries = GetAll().Take(15).ToList();
            var lines = new StringBuilder();
            lines.AppendLine("📋 Activity Log — Recent Actions:\n");

            foreach (var entry in entries)
            {
                lines.AppendLine(entry.Display);
            }

            lines.AppendLine($"\nTotal actions recorded: {_entries.Count}");
            return lines.ToString();
        }
    }
}
