using cab301_assignment3;


Console.WriteLine("╔════════════════════════════════╗");
Console.WriteLine("║     Welcome to TaskTracker     ║");
Console.WriteLine("╚════════════════════════════════╝");
Console.WriteLine();

TaskManager taskManager = new TaskManager();

string filePath = "";

bool exit = false;

while (!exit)
{
    DisplayMenu();
    int choice;
    if (int.TryParse(Console.ReadLine(), out choice))
    {
        switch (choice)
        {
            case 1:
                InitialiseFile(taskManager, ref filePath);
                break;
            case 2:
                AddTask(taskManager);
                break;
            case 3:
                RemoveTask(taskManager);
                break;
            case 4:
                UpdateTaskTime(taskManager);
                break;
            case 5:
                SaveTasksToFile(taskManager, filePath);
                break;
            case 6:
                FindAndSaveTaskSequence(taskManager, @"C:\Users\benro\OneDrive\Desktop\cab301-assignment3\cab301-assignment3\sequence.txt");
                break;
            case 8:
                taskManager.PrintTasks();
                break;
            case 9:
                exit = true;
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a number.");
    }

    Console.WriteLine();
}

void DisplayMenu()
{
    Console.WriteLine("Menu:");
    Console.WriteLine("(1) Load Project from File");
    Console.WriteLine("(2) Add Task");
    Console.WriteLine("(3) Remove Task");
    Console.WriteLine("(4) Update Task Time");
    Console.WriteLine("(5) Save Project to File");
    Console.WriteLine("(6) Find Task Sequence");
    Console.WriteLine("(7) Find Earliest Times");
    Console.WriteLine("(8) Print tasks");
    Console.WriteLine("(9) Exit");
    Console.Write("Enter your choice: ");
}

static void InitialiseFile(TaskManager taskManager, ref string filePath)
{
    Console.Write("Enter full path of the text file (with extension): ");
    // string filePath = Console.ReadLine();
    filePath = @"C:\Users\benro\OneDrive\Desktop\cab301-assignment3\cab301-assignment3\tasks.txt";

    if (!File.Exists(filePath))
    {
        Console.WriteLine("File '{0}' does not exist", filePath);
    }
    else if (Path.GetExtension(filePath) != ".txt")
    {
        Console.WriteLine("File '{0}' is not a .txt file", filePath);
    }
    else
    {
        Console.WriteLine("File '{0}' loaded successfully!", filePath);
        taskManager.ReadTasksFromFile(filePath);
    }
}

static void AddTask(TaskManager taskManager)
{
    Console.Write("Enter task ID: ");
    string taskId = Console.ReadLine();

    Console.Write("Enter time needed to complete the task: ");
    if (!int.TryParse(Console.ReadLine(), out int timeNeeded))
    {
        Console.WriteLine("Invalid time input. Please enter a valid integer value.");
        return;
    }

    Console.WriteLine("Enter dependencies (comma-separated, leave empty if there are no dependencies): ");
    string dependenciesInput = Console.ReadLine();
    List<string> dependencies = new List<string>();

    if (!string.IsNullOrEmpty(dependenciesInput))
    {
        dependencies = dependenciesInput.Split(',').Select(d => d.Trim()).ToList();
    }

    taskManager.AddTask(taskId, timeNeeded, dependencies);
}

static void RemoveTask(TaskManager taskManager)
{
    Console.Write("Enter task ID to remove: ");
    string taskId = Console.ReadLine();

    taskManager.RemoveTask(taskId);
}

static void UpdateTaskTime(TaskManager taskManager)
{
    Console.Write("Enter task ID: ");
    string taskId = Console.ReadLine();

    Console.Write("Enter new time needed to complete the task: ");
    if (!int.TryParse(Console.ReadLine(), out int newTimeNeeded))
    {
        Console.WriteLine("Invalid time input. Please enter a valid integer value.");
        return;
    }

    taskManager.UpdateTaskTime(taskId, newTimeNeeded);
}

static void SaveTasksToFile(TaskManager taskManager, string filePath)
{
    taskManager.SaveTasksToFile(filePath);
}

static void FindAndSaveTaskSequence(TaskManager taskManager, string filePath)
{
    List<string> taskSequence = taskManager.TopologicalSort();

    if (taskSequence != null)
    {
        string sequenceString = string.Join(", ", taskSequence);

        File.WriteAllText(filePath, sequenceString);

        Console.WriteLine(sequenceString);        
        Console.WriteLine("Task sequence saved to file: " + filePath);
    }
}