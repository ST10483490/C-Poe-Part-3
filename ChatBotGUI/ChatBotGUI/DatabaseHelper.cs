using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient; // Updated to use the installed package

namespace ChatBotGUI
{
    public class CyberTaskModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsCompleted { get; set; }
    }

    public static class DatabaseHelper
    {
        // Connection string pointing to your SQL Server Express instance and database
        private static string connectionString = "Server=LabVM2049939\\SQLEXPRESS;Database=CyberSecurityDB;Integrated Security=True;TrustServerCertificate=True;";

        public static void AddTask(string title, string description, string reminder)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO CyberTasks (Title, Description, ReminderTimeframe) VALUES (@title, @desc, @remind)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@remind", string.IsNullOrEmpty(reminder) ? "None" : reminder);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<CyberTaskModel> GetAllTasks()
        {
            List<CyberTaskModel> tasks = new List<CyberTaskModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TaskId, Title, Description, ReminderTimeframe, IsCompleted FROM CyberTasks";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new CyberTaskModel
                            {
                                TaskId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Reminder = reader.IsDBNull(3) ? "None" : reader.GetString(3),
                                IsCompleted = reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }
            return tasks;
        }
    }
}