using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                string[] lines = File.ReadAllLines(tasksFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length >= 2)
                    {
                        string taskId = parts[0].Trim();
                        int timeNeeded;

                        if (int.TryParse(parts[1].Trim(), out timeNeeded))
                        {
                            List<string> dependencies = new List<string>();

                            for (int i = 2; i < parts.Length; i++)
                            {
                                dependencies.Add(parts[i].Trim());
                            }

                            AddTask(taskId, timeNeeded, dependencies);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid time needed for task '{taskId}'");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line: {line}");
                    }
                }

                Console.WriteLine("Tasks and dependencies loaded from file successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading the file: " + e.Message);
            }
        }

        public void SaveTasksToFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    Console.WriteLine("Trying to write to file: " + filePath);

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var task in adjacencyList)
                        {
                            StringBuilder line = new StringBuilder();
                            line.Append(task.Key).Append(", ").Append(task.Value[0]);

                            for (int i = 1; i < task.Value.Count; i++)
                            {
                                line.Append(", ").Append(task.Value[i]);
                            }

                            writer.WriteLine(line.ToString());
                        }
                    }

                    Console.WriteLine("Tasks saved to file successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while saving the file: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("No file has been loaded. Please load a file first.");
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

        public void AddTask(string taskId, int timeNeeded, List<string> dependencies)
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

                // Remove this task from the dependencies of other tasks
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
            Console.WriteLine("Tasks in the graph:");
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
                    Console.WriteLine("No information available for this task");
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
            HashSet<string> visited = new HashSet<string>();
            Stack<string> stack = new Stack<string>();

            foreach (var task in adjacencyList)
            {
                string taskId = task.Key;
                if (!visited.Contains(taskId))
                {
                    DepthFirstSearch(taskId, visited, stack);
                }
            }

            while (stack.Count > 0)
            {
                // Insert at the beginning of the list
                sortedTasks.Insert(0, stack.Pop());
            }

            return sortedTasks;
        }

        private void DepthFirstSearch(string taskId, HashSet<string> visited, Stack<string> stack)
        {
            visited.Add(taskId);

            if (adjacencyList.ContainsKey(taskId))
            {
                foreach (string dependency in adjacencyList[taskId].Skip(1))
                {
                    if (!visited.Contains(dependency))
                    {
                        DepthFirstSearch(dependency, visited, stack);
                    }
                }
            }

            stack.Push(taskId);
        }
    }
}
