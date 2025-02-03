class Program
{
    private static readonly string[] VALID_ACTIONS = { "help", "exit" };

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

    static void Main(string[] args)
    {
        Console.WriteLine(" --- Task Tracker Launched! --- ");
        Console.WriteLine("'help' - print available commands");

        while (true)
        {
            string? command = ReadCommand();
            RunCommand(command);
        }
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
                break;
            case "exit":
                ExitApplication();
                break;
            default:
                Console.WriteLine($"ERROR: Unknown command '{command}'.");
                break;
        }
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