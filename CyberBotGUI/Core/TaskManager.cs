using System;
using System.Text;
using CyberBotGUI.Models;
using CyberBotGUI.Data;

namespace CyberBotGUI.Core
{
    public static class TaskManager
    {
        public static string ShowAllTasks()
        {
            var tasks = DatabaseService.GetAll();

            if (tasks.Count == 0)
            {
                return "You have no tasks. Use 'add task' command followed by a title to create one.";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Your Cybersecurity Tasks:\n");

            foreach (var task in tasks)
            {
                string status = task.IsComplete ? "✅" : "⏳";
                sb.AppendLine($"{status} [{task.Id}] {task.Title}");
                sb.AppendLine($"   {task.Description}");
                sb.AppendLine($"   Reminder: {task.ReminderMessage}");
                sb.AppendLine();
            }

            ActivityLog.Add("Tasks Displayed", $"{tasks.Count} tasks retrieved from database", "TASK");
            return sb.ToString().TrimEnd();
        }

        public static string AddTask(string title, string description = "", DateTime? reminder = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Please provide a valid title for the task.";
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                description = $"CyberSecurity task: {title}.";
            }

            var task = new CyberTask
            {
                Title = title,
                Description = description,
                ReminderTime = reminder,
                CreatedAt = DateTime.Now
            };

            DatabaseService.AddTask(task);
            ActivityLog.Add("Task Added", $"Task '{title}' added to database", "TASK");

            string rem = reminder.HasValue
                ? $" Reminder set for {reminder.Value:dd MMM yyyy HH:mm}"
                : "";

            return $"Task added: {title}\n{description}.{rem}";
        }

        public static string CompleteTask(int taskId)
        {
            DatabaseService.CompleteTask(taskId);
            ActivityLog.Add("Task Completed", $"Task ID {taskId} marked as complete", "TASK");
            return $"Task {taskId} marked as complete.";
        }

        public static string DeleteTask(int taskId)
        {
            DatabaseService.DeleteTask(taskId);
            ActivityLog.Add("Task Deleted", $"Task ID {taskId} deleted from database", "TASK");
            return $"Task {taskId} has been removed.";
        }

        public static string QuickAddFromInput(string userInput)
        {
            var details = NLPRouter.ExtractTaskDetails(userInput);
            if (string.IsNullOrWhiteSpace(details.Title))
            {
                return "Say 'add task' followed by the task title to create a new task.";
            }
            return AddTask(details.Title, "", details.Reminder);
        }
    }
}

