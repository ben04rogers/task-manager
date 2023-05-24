using cab301_assignment3;


Console.WriteLine("╔═══════════════════════════════╗");
Console.WriteLine("║     Welcome to TaskMinder     ║");
Console.WriteLine("╚═══════════════════════════════╝");
Console.WriteLine();

Graph graph = new Graph();
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
                InitialiseFile(graph);
                break;
            case 8:
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
    Console.WriteLine("(2) Add Job");
    Console.WriteLine("(3) Remove Job");
    Console.WriteLine("(4) Change Job Time");
    Console.WriteLine("(5) Save Project to File");
    Console.WriteLine("(6) Find Task Sequence");
    Console.WriteLine("(7) Find Earliest Times");
    Console.WriteLine("(8) Exit");
    Console.Write("Enter your choice: ");
}

static void InitialiseFile(Graph graph)
{
    Console.Write("Enter full path of the text file (with extension): ");
    string filePath = Console.ReadLine();

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
        graph.ReadTasksFromFile(filePath);
        graph.PrintTasks();
    }
}
