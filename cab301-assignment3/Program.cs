using cab301_assignment3;


Console.WriteLine("╔════════════════════════════════╗");
Console.WriteLine("║     Welcome to TaskTracker     ║");
Console.WriteLine("╚════════════════════════════════╝");
Console.WriteLine();

TaskManager taskManager = new TaskManager();

string tasksFilePath = "";

bool exit = false;

while (!exit)
{
    DisplayMenu();
    
    int choice;

    if (int.TryParse(Console.ReadLine(), out choice))
    {
        if (choice < 0 || choice > 9)
        {
            Console.WriteLine("Invalid input. Please enter a number (0-8).");
            continue;
        }

        switch (choice)
        {
            case 0:
                Console.Clear();
                break;
            case 1:
                tasksFilePath = InitialiseFile(taskManager);
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
                SaveTasksToFile(taskManager, tasksFilePath);
                break;
            case 6:
                FindAndSaveTaskSequence(taskManager);
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
        Console.WriteLine("Invalid input. Please enter a number (0-8).");
    }

    Console.WriteLine();
}

void DisplayMenu()
{
    Console.WriteLine("Menu:");
    Console.WriteLine("(0) Clear Screen");
    Console.WriteLine("(1) Load Project from File");
    Console.WriteLine("(2) Add Task");
    Console.WriteLine("(3) Remove Task");
    Console.WriteLine("(4) Update Task Time");
    Console.WriteLine("(5) Save Project to File");
    Console.WriteLine("(6) Find Task Sequence");
    Console.WriteLine("(7) Find Earliest Times");
    Console.WriteLine("(8) Print tasks");
    Console.WriteLine("(9) Exit");
    Console.Write("Enter your choice (0-8): ");
}

static string InitialiseFile(TaskManager taskManager)
{
    Console.WriteLine("Enter the file name (e.g. tasks.txt) to load from the documents folder.");
    Console.WriteLine();
    Console.Write("File Name (with extension): ");

    string fileName = Console.ReadLine();
    string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    string tasksFilePath = Path.Combine(documentsFolder, fileName);

    if (!File.Exists(tasksFilePath))
    {
        Console.WriteLine("File '{0}' does not exist in the documents folder.", fileName);
        return null;
    }
    else
    {
        Console.WriteLine("File '{0}' loaded successfully from the documents folder!", fileName);
        taskManager.ReadTasksFromFile(tasksFilePath);
        return tasksFilePath;
    }
}


static void AddTask(TaskManager taskManager)
{
    Console.Write("Enter task ID (beggining with 'T'): ");

    string taskId = Console.ReadLine();

    // Check if the task ID starts with "T"
    if (!taskId.StartsWith("T"))
    {
        Console.WriteLine("Invalid task ID. Task ID should start with 'T'.");
        return;
    }

    Console.Write("Enter time needed to complete the task: ");
    
    if (!uint.TryParse(Console.ReadLine(), out uint timeNeeded))
    {
        Console.WriteLine("Invalid time input. Please enter a valid integer value.");
        return;
    }

    if (timeNeeded >= uint.MaxValue)
    {
        Console.WriteLine("Time needed exceeds the maximum allowed value.");
        return;
    }

    Console.WriteLine("Enter dependencies (comma-separated, leave empty if there are no dependencies): ");
    
    string dependenciesInput = Console.ReadLine();

    // Create list of dependencies
    List<string> dependencies = string.IsNullOrEmpty(dependenciesInput)
        ? new List<string>()
        : dependenciesInput.Split(',').Select(d => d.Trim()).ToList();

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

static void FindAndSaveTaskSequence(TaskManager taskManager)
{
    string fileName = "sequence.txt";
    string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    string filePath = Path.Combine(documentsFolder, fileName);

    List<string> taskSequence = taskManager.TopologicalSort();

    if (taskSequence != null)
    {
        string sequenceString = string.Join(", ", taskSequence);

        File.WriteAllText(filePath, sequenceString);
        Console.WriteLine("Task sequence " + sequenceString);
        Console.WriteLine("Task sequence saved to file: " + filePath);
    }
}