Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔════════════════════════════════════╗");
Console.WriteLine("║     Welcome to TaskMinder Pro      ║");
Console.WriteLine("╚════════════════════════════════════╝");
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.White;

string fileName = "tasks.txt";
string filePath = @"C:\Users\benro\OneDrive\Desktop\cab301-assignment3\cab301-assignment3\tasks.txt";

Console.WriteLine(filePath);

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
finally
{
    Console.WriteLine("Executing finally block.");
}