using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cab301_assignment3
{
    internal class TaskManager
    {
        private Dictionary<string, List<string>> adjacencyList;

        public TaskManager()
        {
            adjacencyList = new Dictionary<string, List<string>>();
        }

        public void ReadTasksFromFile(string tasksFilePath)
        {
            try
            {
                // Read lines from the file
                string[] lines = File.ReadAllLines(tasksFilePath);

                foreach (string line in lines)
                {
                    // Split the lines based on comma delimeter
                    string[] parts = line.Split(',');

                    if (parts.Length < 2)
                    {
                        // Invalid line that doesn't fit the desired format
                        Console.WriteLine($"Invalid line: {line}");
                        continue;
                    }

                    // Get the task id
                    string taskId = parts[0].Trim();

                    // Get the time needed 
                    if (!uint.TryParse(parts[1].Trim(), out uint timeNeeded))
                    {
                        Console.WriteLine($"Invalid time needed for task '{taskId}'");
                        continue;
                    }

                    List<string> dependencies = new List<string>();

                    // Get the dependencies 
                    for (int i = 2; i < parts.Length; i++)
                    {
                        dependencies.Add(parts[i].Trim());
                    }

                    // Add task, and dependencies to the graph
                    AddTask(taskId, timeNeeded, dependencies);
                }

                Console.WriteLine("Tasks and dependencies loaded from file successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading file: " + e.Message);
            }
        }

        public void SaveTasksToFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file has been loaded. Please load a file first.");
                return;
            }

            try
            {
                Console.WriteLine("Trying to write to file: " + filePath);

                // Create stream writer to write output to tasks file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var task in adjacencyList)
                    {
                        // Construct line for task
                        string taskLine = task.Key + ", " + string.Join(", ", task.Value);

                        // Write to the file
                        writer.WriteLine(taskLine);
                    }
                }

                Console.WriteLine("Tasks saved to file successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while saving the file: " + e.Message);
            }
        }

        public void SaveTaskSequenceToFile(List<string> taskSequence, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string task in taskSequence)
                    {
                        writer.WriteLine(task);
                    }
                }

                Console.WriteLine("Task sequence saved to file successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while saving the task sequence: " + e.Message);
            }
        }

        public void AddTask(string taskId, uint timeNeeded, List<string> dependencies)
        {
            if (!adjacencyList.ContainsKey(taskId))
            {
                List<string> taskDetails = new List<string>();

                taskDetails.Add(timeNeeded.ToString());
                
                taskDetails.AddRange(dependencies);
                
                adjacencyList.Add(taskId, taskDetails);
            }
            else
            {
                Console.WriteLine($"Task '{taskId}' already exists in the graph.");
            }
        }

        public void RemoveTask(string taskId)
        {
            if (adjacencyList.ContainsKey(taskId))
            {
                adjacencyList.Remove(taskId);

                // Remove task from the dependencies of other tasks
                foreach (var dependencies in adjacencyList.Values)
                {
                    dependencies.Remove(taskId);
                }
                Console.WriteLine($"Task '{taskId}' removed successfully!");
            }
            else
            {
                Console.WriteLine($"Task '{taskId}' does not exist in the graph.");
            }
        }

        public void PrintTasks()
        {
            Console.WriteLine("All Tasks:");

            foreach (var task in adjacencyList.Keys)
            {
                Console.WriteLine($"Task: {task}");

                List<string> taskDetails = adjacencyList[task];

                if (taskDetails.Count > 0)
                {
                    Console.WriteLine($"Time Needed: {taskDetails[0]}");

                    if (taskDetails.Count > 1)
                    {
                        List<string> dependencies = taskDetails.GetRange(1, taskDetails.Count - 1);

                        Console.WriteLine($"Dependencies: {string.Join(", ", dependencies)}");
                    }
                    else
                    {
                        Console.WriteLine("No dependencies");
                    }
                }
                else
                {
                    Console.WriteLine("Task details unavailable");
                }

                Console.WriteLine();
            }
        }

        public void UpdateTaskTime(string taskId, int newExecutionTime)
        {
            if (adjacencyList.ContainsKey(taskId))
            {
                adjacencyList[taskId][0] = newExecutionTime.ToString();

                Console.WriteLine($"Time needed for task '{taskId}' updated successfully.");
            }
            else
            {
                Console.WriteLine($"Task '{taskId}' does not exist in the graph.");
            }
        }

        public List<string> TopologicalSort()
        {
            List<string> sortedTasks = new List<string>();
            HashSet<string> processedTasks = new HashSet<string>();
            Stack<string> taskStack = new Stack<string>();

            // Do a depth-first search on each of the unprocessed tasks in adjacencyList
            foreach (var task in adjacencyList)
            {
                string taskId = task.Key;

                if (!processedTasks.Contains(taskId))
                {
                    // Recursively perform depth-first search on the task
                    DepthFirstSearch(taskId, processedTasks, taskStack);
                }
            }

            // Get sorted tasks by popping elements from the stack
            while (taskStack.Count > 0)
            {
                // Insert at the beginning of the list
                sortedTasks.Insert(0, taskStack.Pop());
            }

            return sortedTasks;
        }

        private void DepthFirstSearch(string taskId, HashSet<string> processedTasks, Stack<string> taskStack)
        {
            // Mark this current task as processed
            processedTasks.Add(taskId);

            // Do a depth-first search on each of the unprocessed dependencies of the current task
            if (adjacencyList.ContainsKey(taskId))
            {
                // Get the dependencies
                foreach (string dependency in adjacencyList[taskId].Skip(1))
                {
                    if (!processedTasks.Contains(dependency))
                    {
                        DepthFirstSearch(dependency, processedTasks, taskStack);
                    }
                }
            }

            // Push this task onto the stack
            taskStack.Push(taskId);
        }
    }
}
