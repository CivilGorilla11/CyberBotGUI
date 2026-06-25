using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CyberBotGUI.Models;

namespace CyberBotGUI.Data
{
    public static class DatabaseService
    {
        private static string _connectionString =
            "Server=localhost;Database=CyberBotDB;User ID=root;Password=Ipakzo2002*;";
         
        public static void Initialize()
        {
            using var conn = new MySqlConnection(_connectionString); 
            conn.Open();  

            var cmd = conn.CreateCommand();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Tasks (
                                    Id INT AUTO_INCREMENT PRIMARY KEY,
                                    Title VARCHAR(255) NOT NULL,
                                    Description TEXT,
                                    ReminderTime DATETIME NULL,
                                    CreatedAt DATETIME NOT NULL,
                                    IsComplete BOOLEAN DEFAULT FALSE
                                );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS ActivityLog (
                                    Id INT AUTO_INCREMENT PRIMARY KEY,
                                    Action VARCHAR(255),
                                    Details TEXT,
                                    Category VARCHAR(50),
                                    Timestamp DATETIME NOT NULL
                                );";
            cmd.ExecuteNonQuery();
        }

        // ---------------- TASK METHODS ----------------
        public static List<CyberTask> GetAll()
        {
            var tasks = new List<CyberTask>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Tasks", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    ReminderTime = reader.IsDBNull(reader.GetOrdinal("ReminderTime")) 
                                   ? null 
                                   : reader.GetDateTime("ReminderTime"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    IsComplete = reader.GetBoolean("IsComplete")
                });
            }
            return tasks;
        }

        public static void AddTask(CyberTask task)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"INSERT INTO Tasks 
                (Title, Description, ReminderTime, CreatedAt, IsComplete) 
                VALUES (@title, @desc, @rem, @created, @complete)", conn);

            cmd.Parameters.AddWithValue("@title", task.Title);
            cmd.Parameters.AddWithValue("@desc", task.Description);
            cmd.Parameters.AddWithValue("@rem", task.ReminderTime.HasValue ? task.ReminderTime.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@created", task.CreatedAt);
            cmd.Parameters.AddWithValue("@complete", task.IsComplete);

            cmd.ExecuteNonQuery();
        }

        public static void CompleteTask(int taskId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("UPDATE Tasks SET IsComplete = TRUE WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", taskId);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteTask(int taskId)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM Tasks WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", taskId);
            cmd.ExecuteNonQuery();
        }

        // ---------------- ACTIVITY LOG METHODS ----------------
        public static void AddLog(string action, string details, string category)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(@"INSERT INTO ActivityLog 
                (Action, Details, Category, Timestamp) 
                VALUES (@action, @details, @category, @time)", conn);

            cmd.Parameters.AddWithValue("@action", action);
            cmd.Parameters.AddWithValue("@details", details);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@time", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public static List<LogEntry> GetLog()
        {
            var log = new List<LogEntry>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM ActivityLog ORDER BY Timestamp DESC", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                log.Add(new LogEntry
                {
                    TimeStamp = reader.GetDateTime("Timestamp"),
                    Action = reader.GetString("Action"),
                    Description = reader.GetString("Details"),
                    Category = reader.GetString("Category")
                });
            }
            return log;
        }
    }
}


