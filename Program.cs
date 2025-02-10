using System.Text.Json;
using MyProject.Models;
using MyProject.Services;
class Program
{
    private static readonly string[] VALID_ACTIONS = { "help", "exit", "list", "add", "delete", "mark-in-progress", "mark-done" };

    private static readonly Dictionary<string, string> HELP_COMMANDS = new()
{
    { "help", "Show this help menu" },
    { "list", "Show all tasks" },
    { "list done", "Show completed tasks" },
    { "list todo", "Show pending tasks" },
    { "list in-progress", "Show tasks in progress" },
    { "add [task]", "Add a new task" },
    { "delete [task id]", "Remove a task" },
    { "mark-in-progress [task id]", "Mark a task as in progress" },
    { "mark-done [task id]", "Mark a task as done" },
    { "exit", "Close the application"}
};

    private static List<ToDoTask> tasks = TaskService.LoadTasks();

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

        string[] parts = command.ToLowerInvariant().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string action = parts[0];
        string[] arguments = parts.Skip(1).ToArray();

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

        switch (action)
        {
            case "add":
                RunAddCommand(arguments);
                break;
            case "delete":
                RunDeleteCommand(arguments);
                break;
            case "mark-in-progress":
                RunMarkCommand(arguments, State.InProgress);
                break;
            case "mark-done":
                RunMarkCommand(arguments, State.Done);
                break;
            default:
                Console.WriteLine($"ERROR: Unknown command '{command}'.");
                return;
        }
    }

    private static void RunMarkCommand(string[] arguments, State state)
    {
        if (arguments.Length == 0 || arguments.Length > 1 || !int.TryParse(arguments[0], out int result))
        {
            if (state == State.InProgress)
            {
                Console.WriteLine("ERROR: Mark Command needs to be in the form: mark-in-progress <task-id>\n");
            }
            else
            {
                Console.WriteLine("ERROR: Mark Command needs to be in the form: mark-done <task-id>\n");
            }
            return;
        }

        tasks = TaskService.LoadTasks();

        if (result > tasks.Count || result <= 0)
        {
            Console.WriteLine($"ERROR: No task with id {result}.\n");
            return;
        }

        tasks[result - 1].State = state;
        TaskService.SaveTasks(tasks);
        Console.WriteLine($"Task {result} marked successfully!\n");
        RunListCommand();
    }

    private static void RunDeleteCommand(string[] arguments)
    {
        if (arguments.Length == 0 || arguments.Length > 1 || !int.TryParse(arguments[0], out int result))
        {
            Console.WriteLine("ERROR: Delete Command needs to be in the form: delete <task-id>\n");
            return;
        }

        tasks = TaskService.LoadTasks();

        if (result > tasks.Count || result <= 0)
        {
            Console.WriteLine($"ERROR: No task with id {result}.\n");
            return;
        }

        tasks.RemoveAt(result - 1);

        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Id = i + 1; // Reset IDs from 1, 2, ..., n
        }

        TaskService.SaveTasks(tasks);

        Console.WriteLine("Task deleted successfully!\n");
        RunListCommand();
    }
    private static void RunAddCommand(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            Console.WriteLine("ERROR: Task description cannot be empty.\n");
            return;
        }

        int id = tasks.Count() == 0 ? 1 : tasks[tasks.Count() - 1].Id + 1;

        ToDoTask task = new ToDoTask
        {
            Id = id,
            Description = string.Join(" ", arguments),
            State = State.Todo
        };

        tasks = TaskService.LoadTasks();
        tasks.Add(task);
        TaskService.SaveTasks(tasks);

        Console.WriteLine("Task added successfully!\n");
        RunListCommand();
    }

    private static void RunListCommand(string option = "")
    {
        tasks = TaskService.LoadTasks();
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
    private static void RunHelpCommand()
    {
        Console.WriteLine("\n===== Available Commands =====");
        foreach (var command in HELP_COMMANDS)
        {
            Console.WriteLine($"{command.Key.PadRight(20)} - {command.Value}");
        }
        Console.WriteLine("==============================\n");
    }

    private static void ExitApplication()
    {
        Console.WriteLine("Exiting Task Tracker... Goodbye!");
        Environment.Exit(0);
    }

    private static string? ReadCommand()
    {
        while (true)
        {
            string? command = Console.ReadLine();
            if (IsValid(command))
            {
                return command;
            }
            Console.WriteLine($"'{command}' is not a valid command\n");
        }
    }

    private static bool IsValid(string? command)
    {
        return !string.IsNullOrWhiteSpace(command) &&
            VALID_ACTIONS.Contains(command.Split(" ")[0], StringComparer.OrdinalIgnoreCase);
    }
}