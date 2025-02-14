using System.Text.Json.Serialization;

namespace MyProject.Models
{
    public class ToDoTask
    {
        public int id { get; set; }
        public string? description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status status { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }

        public override string ToString()
        {
            return $"{id}. {description} ({status})";
        }
    }

    public enum Status
    {
        Todo,
        InProgress,
        Done
    }
}
