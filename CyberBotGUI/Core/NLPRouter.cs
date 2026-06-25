using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace CyberBotGUI.Core
{
    public enum Intent
    {
        ShowTasks, AddTask, CompleteTask, DeleteTask, SetReminder, StartQuiz, 
        ShowLog, Greeting, NameIntro, Exit,
        CyberKeyword
    }

    public static class NLPRouter
    {
        private static readonly Dictionary<Intent, List<string>>
            _phrases = new Dictionary<Intent, List<string>>
            {
                [Intent.ShowTasks] = new List<string>
                {
                    "show my tasks", "list tasks", "what are my tasks", "display tasks",
                    "pending tasks", "show pending tasks", "what tasks do I have", "see tasks"
                },
                [Intent.AddTask] = new List<string>
                {
                     "add task", "new task", "create task", "add a task",
                "remind me to", "i need to", "set a task", "schedule task",
                "add reminder", "create a task", "task to add"
                },
                [Intent.CompleteTask] = new List<string>
                {
                   "complete task", "mark done", "finish task", "task done",
                "mark complete", "completed task", "done with task",
                "mark as done", "i finished", "mark it done"
                },
                [Intent.DeleteTask] = new List<string>
                {
                   "delete task", "remove task", "cancel task",
                "get rid of task", "drop task", "remove reminder"
            },
                [Intent.SetReminder] = new List<string>
                {
                    "set reminder", "remind me", "create reminder",
                "schedule reminder", "add reminder", "remind me to"
                },
                [Intent.StartQuiz] = new List<string>
                {
                    "start quiz", "begin quiz", "take quiz",
                "quiz time", "start the quiz", "launch quiz"
                },
                [Intent.ShowLog] = new List<string>
                {
                    "show log", "display log", "view log",
                "see log", "open log", "log history"
                },
                [Intent.Greeting] = new List<string>
                {
                    "hello", "hi", "hey", "greetings",
                "good morning", "good afternoon", "good evening"
                },
                [Intent.Exit] = new List<string>
                {
                    "exit", "quit", "close", "bye",
                "goodbye", "see you later"
                },
                [Intent.NameIntro] = new List<string>
                {
                    "my name is", "i am", "call me", "call me",

            },
            };

        public static Intent Detect(string input)
        {
            string lower = input.ToLower().Trim();

            lower = lower
                .Replace("i've", "i have")
                .Replace("i'd", "i would")
                .Replace("can't", "cannot")
                .Replace("don't", "do not")
                .Replace("what's", "what is");

            foreach (var (intent, phrases) in _phrases)
            {
                foreach (var phrase in phrases)
                {
                    if (Regex.IsMatch(lower, $@"\b{Regex.Escape(phrase)}\b"))
                    {
                        return intent;
                    }

                }
            }
            if (lower.Split(" ").Length <= 3)
            {
                foreach (var phrase in _phrases[Intent.Greeting])
                {
                    if (Regex.IsMatch(lower, $@"\b{Regex.Escape(phrase)}\b"))
                    {
                        return Intent.Greeting;
                    }
                }


            }
            return Intent.CyberKeyword;

        }
        public static string ExtractTaskTitle(string input)
        {
            string lower = input.ToLower();
            string[] removals = {
                "add task", "new task", "create task", "add a task",
                "remind me to", "i need to", "set a task", "schedule task",
                "add reminder", "create a task", "task to add" 
            };
            foreach (var removal in removals)
            {
                lower = lower.Replace(removal, "").Trim();
            }
                if(lower.Length < 3)
                {
                    return string.Empty;
                }
                return char.ToUpper(lower[0]) + lower.Substring(1).TrimEnd('.', '!', '?');
            }



        public static int ExtractTaskId(string input)
        {
            var tokens = input.Split(' ');
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out int id))
                {
                    return id;
                }
            }
            return -1;
        }
    }
}
