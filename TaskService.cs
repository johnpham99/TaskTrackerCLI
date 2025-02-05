using MyProject.Models;
using System.Text.Json;

namespace MyProject.Services
{
    public static class TaskService
    {
        private static readonly string filePath = "tasks.json";

        public static List<ToDoTask> LoadTasks()
        {
            if (!File.Exists(filePath)) return new List<ToDoTask>();

            string jsonString = File.ReadAllText(filePath);
            return string.IsNullOrWhiteSpace(jsonString)
                ? new List<ToDoTask>()
                : JsonSerializer.Deserialize<List<ToDoTask>>(jsonString) ?? new List<ToDoTask>();
        }

        public static void SaveTasks(List<ToDoTask> tasks)
        {
            string updatedJson = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
