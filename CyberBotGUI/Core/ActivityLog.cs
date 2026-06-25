using System;
using System.Collections.Generic;
using System.Text;
using CyberBotGUI.Models;

namespace CyberBotGUI.Core
{
    public static class ActivityLog
    {
        private static readonly List<LogEntry> _entries = new List<LogEntry>(); 

        static ActivityLog() 
        {
            var seed = new[]
            {
               new LogEntry{ TimeStamp = DateTime.Now.AddMinutes(-45), Action = "Bot Started", Category = "System", Description = "The CyberBot has launched successfully." },
               new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-44), Category = "VOICE",   Action = "Voice Greeting",      Description = "Male voice welcome message played on startup" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-43), Category = "DB",      Action = "Database Initialised",Description = "SQLite task database created and seeded with 5 default tasks" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-40), Category = "USER",    Action = "Name Captured",       Description = "User introduced themselves — name stored in MemoryStore" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-35), Category = "KEYWORD", Action = "Keyword Matched",     Description = "User asked about phishing — response delivered from ResponseLibrary" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-30), Category = "TASK",    Action = "Tasks Displayed",     Description = "User requested task list — 5 tasks retrieved from database" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-20), Category = "TASK",    Action = "Task Completed",      Description = "User marked 'Enable 2FA' as complete" },
                new LogEntry { TimeStamp = DateTime.Now.AddMinutes(-10), Category = "QUIZ",    Action = "Quiz Started",        Description = "User launched the cybersecurity awareness quiz" },
           };
            _entries.AddRange(seed);
        }

        public static void Add(string action, string description, string category = "SYSTEM")
        {
            _entries.Add(new LogEntry
            {
                TimeStamp = DateTime.Now,
                Action = action,
                Description = description,
                Category = category
            });

        } 

        public static List<LogEntry> GetAll() => 
            _entries.OrderByDescending(e => e.TimeStamp).ToList();
    public static string FormatForChat()
        {
            var entries = GetAll().Take(15).ToList();
            var lines = new System.Text.StringBuilder();
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

