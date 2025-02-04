using System.Text.Json.Serialization;

class ToDoTask
{
    public int Id { get; set; }
    public String? Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public State State { get; set; }

    public override string ToString()
    {
        return $"{Id}. {Description} ({State})";
    }
}

enum State
{
    Todo,
    InProgress,
    Done
}
