using System.Text.Json;
using MyProject.Models;
using MyProject.Services;
class Program
{
    private static readonly string[] VALID_ACTIONS = { "help", "exit", "list", "add", "delete", "mark-in-progress", "mark-done", "update" };

    private static readonly Dictionary<string, string> HELP_COMMANDS = new()
{
    { "help", "Show this help menu" },
    { "list", "Show all tasks" },
    { "list done", "Show completed tasks" },
    { "list todo", "Show pending tasks" },
    { "list in-progress", "Show tasks in progress" },
    { "add [task]", "Add a new task" },
    { "update [task id] [description]", "Update description of a task"},
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
            case "exit":
                ExitApplication();
                return;
        }

        switch (action)
        {
            case "add":
                RunAddCommand(arguments);
                break;
            case "list":
                RunListCommand(arguments);
                return;
            case "update":
                RunUpdateCommand(arguments);
                return;
            case "delete":
                RunDeleteCommand(arguments);
                break;
            case "mark-in-progress":
                RunMarkCommand(arguments, Status.InProgress);
                break;
            case "mark-done":
                RunMarkCommand(arguments, Status.Done);
                break;
            default:
                Console.WriteLine($"ERROR: Unknown command '{command}'.");
                return;
        }
    }

    private static void RunUpdateCommand(string[] arguments)
    {
        if (arguments.Length <= 1 || !int.TryParse(arguments[0], out int result))
        {
            Console.WriteLine("ERROR: Update Command needs to be in the form: update <task-id> <description>\n");
            return;
        }

        tasks = TaskService.LoadTasks();

        if (result > tasks.Count || result <= 0)
        {
            Console.WriteLine($"ERROR: No task with id {result}.\n");
            return;
        }

        string[] newDescription = arguments.Skip(1).ToArray();

        ToDoTask task = tasks[result - 1];
        task.description = string.Join(" ", newDescription);

        TaskService.SaveTasks(tasks);

        Console.WriteLine("Task updated successfully!\n");
        PrintTodoList();
    }

    private static void RunListCommand(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            PrintTodoList();
            return;
        }

        if (arguments.Length > 1 || (arguments[0] != "done" && arguments[0] != "todo" && arguments[0] != "in-progress"))
        {
            Console.WriteLine("ERROR: List Command needs to be in the form: list or list <done/todo/in-progress>\n");
            return;
        }

        PrintTodoList(arguments[0]);
    }

    private static void RunMarkCommand(string[] arguments, Status status)
    {
        if (arguments.Length == 0 || arguments.Length > 1 || !int.TryParse(arguments[0], out int result))
        {
            if (status == Status.InProgress)
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

        tasks[result - 1].status = status;
        TaskService.SaveTasks(tasks);
        Console.WriteLine($"Task {result} marked successfully!\n");
        PrintTodoList();
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
            tasks[i].id = i + 1; // Reset IDs from 1, 2, ..., n
        }

        TaskService.SaveTasks(tasks);

        Console.WriteLine("Task deleted successfully!\n");
        PrintTodoList();
    }
    private static void RunAddCommand(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            Console.WriteLine("ERROR: Task description cannot be empty.\n");
            return;
        }

        int id = tasks.Count() == 0 ? 1 : tasks[tasks.Count() - 1].id + 1;

        ToDoTask task = new ToDoTask
        {
            id = id,
            description = string.Join(" ", arguments),
            status = Status.Todo
        };

        tasks = TaskService.LoadTasks();
        tasks.Add(task);
        TaskService.SaveTasks(tasks);

        Console.WriteLine("Task added successfully!\n");
        PrintTodoList();
    }

    private static void PrintTodoList(string option = "")
    {
        tasks = TaskService.LoadTasks();

        IEnumerable<ToDoTask> filteredTasks = tasks;

        if (option == "")
        {
            Console.WriteLine("\n===== To Do List =====");
        }

        if (option == "done")
        {
            filteredTasks = tasks.Where(t => t.status == Status.Done);
            Console.WriteLine("\n===== To Do List (Done) =====");
        }
        else if (option == "todo")
        {
            filteredTasks = tasks.Where(t => t.status == Status.Todo);
            Console.WriteLine("\n===== To Do List (To Do) =====");
        }
        else if (option == "in-progress")
        {
            filteredTasks = tasks.Where(t => t.status == Status.InProgress);
            Console.WriteLine("\n===== To Do List (In Progress) =====");
        }

        if (!filteredTasks.Any())
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            foreach (ToDoTask task in filteredTasks)
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