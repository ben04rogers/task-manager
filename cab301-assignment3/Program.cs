Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔════════════════════════════════════╗");
Console.WriteLine("║     Welcome to TaskMinder Pro      ║");
Console.WriteLine("╚════════════════════════════════════╝");
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.White;

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
                InitialiseFile();
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
    Console.WriteLine("1. Load Project from File");
    Console.WriteLine("2. Add Job");
    Console.WriteLine("3. Remove Job");
    Console.WriteLine("4. Change Job Time");
    Console.WriteLine("5. Save Project to File");
    Console.WriteLine("6. Find Task Sequence");
    Console.WriteLine("7. Find Earliest Times");
    Console.WriteLine("8. Exit");
    Console.Write("Enter your choice: ");
}

void InitialiseFile()
{
    Console.Write("Enter the path of the text file: ");
    filePath = Console.ReadLine();
    Console.WriteLine("File loaded successfully!");
}

try
{
    //Pass the filepath and filename to the StreamWriter Constructor
    StreamWriter sw = new StreamWriter(filePath);
    //Write a line of text
    sw.WriteLine("Hello World!!");
    //Write a second line of text
    sw.WriteLine("From the StreamWriter class");
    //Close the file
    sw.Close();
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e.Message);
}
