using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using CyberBotGUI.Models;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;


namespace CyberBotGUI.Data
{
    public  class DatabaseService
    {
        private const string ConnStr = "Data Source=cyberbot.db;Version=3;";

        public static void Initialize()
        {
            using var conn = new SQLiteConnection(ConnStr);
            conn.Open();

            var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    IsComplete INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL,
                    ReminderTime TEXT
                );
                " 
            };
            cmd.ExecuteNonQuery();

            var count = new SQLiteCommand(
                "SELECT COUNT(*) FROM Tasks;", conn).ExecuteScalar();

            if(Convert.ToInt32(count) == 0)
            {
                SeedDefautTasks(conn);
            }
        }

        private static void SeedDefautTasks(SQLiteConnection conn)
        {
            var defaults = new[]
            {
                ("Enable 2FA",
                "Set up two-facor authenication on all critical accounts including email, banking and social media.",
                DateTime.Now.AddDays(1).ToString("o")),

                ("Review Privcy Settings",
                "Audit privacy settings on all social media platforms and disable data sharing where possible.",
                (string)null),

                ("Manage Passwords",
                "Audit all saved passwords, remove duplicates and ensure each account has a unique strong password.",
                DateTime.Now.AddDays(3).ToString("o")),

                ("Update Antivirus",
                "Ensure antivirus software is up to date and run a full system scan.",
                (string)null),

                ("Backup Data",
                "Create a backup of all important data and store it in a secure location.",
                DateTime.Now.AddDays(7).ToString("o")),

            };

            foreach (var (title, description, reminder) in defaults)
            {
                var cmd = new SQLiteCommand(conn)
                {
                    CommandText = @"
               INSERT INTO Tasks
                    (Title, Description, IsComplete, CreatedAt, ReminderTime)
               VALUES 
                    (@title,
                    @description, 
                    0,
                    @createdAt,
                    @reminder)"
                };
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("o"));
                cmd.Parameters.AddWithValue("@reminder", reminder ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<CyberTask> GetAll()
        {
            var list = new List<CyberTask>();
            using var conn = new SQLiteConnection(ConnStr);
            conn.Open();
            var reader = new SQLiteCommand(
                "SELECT * FROM Tasks ORDER BY CreatedAt DESC;"
                , conn).ExecuteReader();

            while (reader.Read())
            {
                var task = new CyberTask
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsComplete = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4)),

                };

                if (!reader.IsDBNull(5))
                {
                    task.ReminderTime = DateTime.Parse(reader.GetString(5));
                }
                    list.Add(task);
                }
                return list;

            }
        

            public static void AddTask(CyberTask task)
            {
                using var conn = new SQLiteConnection(ConnStr);
                conn.Open();
                var cmd = new SQLiteCommand(conn)

                {
                    CommandText = @"
                    INSERT INTO Tasks
                        (Title, Description, IsComplete, CreatedAt, ReminderTime)
                    VALUES 
                        (@title,
                        @description, 
                        0,
                        @createdAt,
                        @reminder);"


                };
                cmd.Parameters.AddWithValue("@title", task.Title);
                cmd.Parameters.AddWithValue("@description", task.Description);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("o"));
                cmd.Parameters.AddWithValue("@reminder", task.ReminderTime?.ToString("o") ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }

        public static void CompleteTask(int id)
        {
            using var conn = new SQLiteConnection(ConnStr);
            conn.Open();
            var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"
                UPDATE Tasks
                SET IsComplete = 1
                WHERE Id = @id;"
            };
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
            public static void DeleteTask(int id)
        {
            using var conn = new SQLiteConnection(ConnStr);
            conn.Open();
            var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"
                    DELETE FROM Tasks
                    WHERE Id = @id;"
            };
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        }
}

