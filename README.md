# TaskTrackerCLI
CLI application to track your tasks and manage your to-do list.

Completed as an excercise to practice C#.

## Requirements:
https://roadmap.sh/projects/task-tracker

## Features

- JSON "Database"
	- The program uses a JSON file to store and read information.
	- If the database/file doesn't exist, it will be created on program start.  
- A console based UI where users can input commands.
 	- ![image](/images/screenshot.png)
- CRUD functionality
	- Users can Create, Read, Update or Delete entries 
	- Entries include status and time/date stamps

## Installation and Usage
Need .NET SDK installed on your system.  
	`dotnet version`
1. Clone or download the repository.  
	`git clone https://github.com/johnpham99/TaskTrackerCLI.git`
2. Navigate to the project directory.  
	`cd TaskTrackerCLI`
3. Run the application.  
	`dotnet run`

## Challenges
- Code Structure. Unfamiliar to convention, I tried to put a lot of thought into how to structure the project files. I ended up having three separate files: Program.cs for the general application logic and entry point, Tasks.cs to define the ToDoTask structure, and TaskService.cs for the logic related to saving/loading the json file.
- String formatting. It's not very often I'm tasked with making things look pretty, especially console outputs. Generative AI came in handy for this purpose.
	
## Lessons Learned
- Use of padding to help format/align strings. 
	- `$"{command.Key.PadRight(30)} - {command.Value}"`
	- `$"{task.id,-4} | {statusSymbol,-8} | ...`
- Joining array to string
	- `string.Join(" ", newDescription);`
- Splitting a string to arary
	- `command.ToLowerInvariant().Split(" ", StringSplitOptions.RemoveEmptyEntries);`
- Skipping elements in an array
	- `parts.Skip(1).ToArray();`
- Using IEnumerable for filtering
	- `IEnumerable<ToDoTask> filteredTasks = tasks;`  
	`filteredTasks = tasks.Where(t => t.status == Status.Done);`
- Experience Using System.Text.Json.Serialization
	- `[JsonConverter(typeof(JsonStringEnumConverter))]`
	- `JsonSerializer.Deserialize<List<ToDoTask>>(jsonString) ?? new List<ToDoTask>();`
	- `JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });`

## Areas to Improve
- I want to gain more experience and understand file/project structure more. More specifically, develop an understanding of different design patterns, and gain a sense of intuition for what to apply for different scenarios. Keywords I'm seeing during my investigation are **Model-View-Controller**, **Service Layer**, **ASP.NET Core** and following **Separation of Concerns**, **Single Responsibility Principle**.
- I also want to take my CRUD application to the next level by utilizing actual databases. Perhaps starting with relational databases like **SQLite** and then advancing towards **MySQL**.