using System.Text.Json.Serialization;

namespace MyProject.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public State State { get; set; }

        public override string ToString()
        {
            return $"{Id}. {Description} ({State})";
        }
    }

    public enum State
    {
        Todo,
        InProgress,
        Done
    }
}
