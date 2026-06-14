using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CybersecurityChatbot.WinFormsApp
{
    public class DatabaseService
    {
        private readonly string serverConnectionString =
            "server=127.0.0.1;port=3306;user=root;password=CyberPass123!;SslMode=Disabled;AllowPublicKeyRetrieval=True;";

        private readonly string databaseConnectionString =
            "server=127.0.0.1;port=3306;database=cybersecurity_chatbot_db;user=root;password=CyberPass123!;SslMode=Disabled;AllowPublicKeyRetrieval=True;";
        public DatabaseService()
        {
            EnsureDatabaseAndTablesExist();
        }

        private void EnsureDatabaseAndTablesExist()
        {
            using (var connection = new MySqlConnection(serverConnectionString))
            {
                connection.Open();

                string createDatabaseQuery = "CREATE DATABASE IF NOT EXISTS cybersecurity_chatbot_db;";

                using var command = new MySqlCommand(createDatabaseQuery, connection);
                command.ExecuteNonQuery();
            }

            using (var connection = new MySqlConnection(databaseConnectionString))
            {
                connection.Open();

                string createTasksTableQuery = @"
                    CREATE TABLE IF NOT EXISTS CyberTasks (
                        TaskId INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT,
                        ReminderText VARCHAR(255),
                        IsCompleted BOOLEAN DEFAULT FALSE,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                using var createTasksCommand = new MySqlCommand(createTasksTableQuery, connection);
                createTasksCommand.ExecuteNonQuery();

                string createLogsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ActivityLogs (
                        LogId INT AUTO_INCREMENT PRIMARY KEY,
                        ActionDescription TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                using var createLogsCommand = new MySqlCommand(createLogsTableQuery, connection);
                createLogsCommand.ExecuteNonQuery();
            }
        }

        public List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();

            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = @"
                SELECT TaskId, Title, Description, ReminderText, IsCompleted
                FROM CyberTasks
                ORDER BY TaskId DESC;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    TaskId = reader.GetInt32("TaskId"),
                    Title = reader.GetString("Title"),
                    Description = reader["Description"]?.ToString() ?? "",
                    ReminderText = reader["ReminderText"]?.ToString() ?? "No reminder set",
                    IsCompleted = reader.GetBoolean("IsCompleted")
                });
            }

            return tasks;
        }

        public int AddTask(CyberTask task)
        {
            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = @"
                INSERT INTO CyberTasks (Title, Description, ReminderText, IsCompleted)
                VALUES (@Title, @Description, @ReminderText, @IsCompleted);
                SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@ReminderText", task.ReminderText);
            command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public void CompleteTask(int taskId)
        {
            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = "UPDATE CyberTasks SET IsCompleted = TRUE WHERE TaskId = @TaskId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.ExecuteNonQuery();
        }

        public void DeleteTask(int taskId)
        {
            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = "DELETE FROM CyberTasks WHERE TaskId = @TaskId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.ExecuteNonQuery();
        }

        public void AddActivityLog(string actionDescription)
        {
            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = "INSERT INTO ActivityLogs (ActionDescription) VALUES (@ActionDescription);";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ActionDescription", actionDescription);
            command.ExecuteNonQuery();
        }

        public List<string> GetRecentActivityLogs()
        {
            var logs = new List<string>();

            using var connection = new MySqlConnection(databaseConnectionString);
            connection.Open();

            string query = @"
                SELECT ActionDescription, CreatedAt
                FROM ActivityLogs
                ORDER BY LogId DESC
                LIMIT 10;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string action = reader.GetString("ActionDescription");
                DateTime createdAt = reader.GetDateTime("CreatedAt");

                logs.Add($"{createdAt:yyyy-MM-dd HH:mm} - {action}");
            }

            logs.Reverse();
            return logs;
        }
    }
}