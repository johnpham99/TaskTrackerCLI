using System.Text.Json;
class Program
{
    private static readonly string[] VALID_ACTIONS = { "help", "exit", "list" };

    private static readonly Dictionary<string, string> HELP_COMMANDS = new()
{
    { "help", "Show this help menu" },
    { "list", "Show all tasks" },
    { "list done", "Show completed tasks" },
    { "list todo", "Show pending tasks" },
    { "list in-progress", "Show tasks in progress" },
    { "add [task]", "Add a new task" },
    { "remove [task]", "Remove a task" },
    { "complete [task]", "Mark a task as done" },
    { "exit", "Close the application"}
};

    private static List<ToDoTask> tasks = new List<ToDoTask>();

    static void Main(string[] args)
    {
        tasks = ReadTasksFile();

        Console.WriteLine(" --- Task Tracker Launched! --- ");
        Console.WriteLine("'help' - print available commands");

        while (true)
        {
            string? command = ReadCommand();
            RunCommand(command);
        }
    }
    private static List<ToDoTask> ReadTasksFile()
    {
        List<ToDoTask> tasks = new List<ToDoTask>();

        string filePath = "tasks.json";
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                tasks = JsonSerializer.Deserialize<List<ToDoTask>>(jsonString) ?? new List<ToDoTask>();
            }
        }

        return tasks;
    }

    private static void RunCommand(string? command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            Console.WriteLine("ERROR: Attempted to run NULL or empty command.");
            return;
        }

        command = command.ToLowerInvariant();

        switch (command)
        {
            case "help":
                RunHelpCommand();
                return;
            case "list":
                RunListCommand();
                return;
            case "exit":
                ExitApplication();
                return;
        }

        string[] parts = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string action = parts[0];

        switch (action)
        {
            case "add":
                break;
            default:
                Console.WriteLine($"ERROR: Unknown command '{command}'.");
                return;
        }
    }

    private static void RunListCommand(string option = "")
    {
        Console.WriteLine("\n===== To Do List =====");
        if (option == "")
        {
            foreach (ToDoTask task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        Console.WriteLine("==============================\n");
    }

    private static void ExitApplication()
    {
        Console.WriteLine("Exiting Task Tracker... Goodbye!");
        Environment.Exit(0);
    }


    private static void RunHelpCommand()
    {
        Console.WriteLine("\n===== Available Commands =====");
        foreach (var command in HELP_COMMANDS)
        {
            Console.WriteLine($"{command.Key.PadRight(20)} - {command.Value}");
        }
        Console.WriteLine("==============================\n");
    }

    private static string? ReadCommand()
    {
        bool validCommand = false;
        string? command = "";

        while (!validCommand)
        {
            command = Console.ReadLine();
            validCommand = IsValid(command);

            if (!validCommand)
            {
                Console.WriteLine($"'{command}' is not a valid command");
            }
        }

        return command;
    }

    private static bool IsValid(string? command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return false;
        }

        string[] parts = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        return VALID_ACTIONS.Contains(parts[0], StringComparer.OrdinalIgnoreCase);
    }
}