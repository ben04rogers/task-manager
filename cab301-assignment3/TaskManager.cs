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
                // Clear the existing graph
                adjacencyList.Clear(); 

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

                    // Remove the existing task if it already exists
                    if (adjacencyList.ContainsKey(taskId))
                    {
                        adjacencyList.Remove(taskId);
                        Console.WriteLine($"Task '{taskId}' already exists in the graph. Overwriting...");

                    }

                    // Add task and dependencies to the graph
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

        public void AddTask(string taskId, uint timeNeeded, List<string> taskDependencies)
        {
            if (CheckCircularDependency(taskId, taskDependencies))
            {
                throw new InvalidOperationException("Circular dependency found. Please fix before proceeding.");
            }

            if (!adjacencyList.ContainsKey(taskId))
            {
                List<string> taskDetails = new List<string>();

                taskDetails.Add(timeNeeded.ToString());
                taskDetails.AddRange(taskDependencies);

                adjacencyList.Add(taskId, taskDetails);
            }
            else
            {
                Console.WriteLine($"Task '{taskId}' already exists in the graph.");
            }
        }

        public bool CheckCircularDependency(string taskId, List<string> dependencies)
        {
            HashSet<string> visited = new HashSet<string>();

            visited.Add(taskId);

            foreach (string dependency in dependencies)
            {
                if (HasCircularDependency(dependency, visited))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasCircularDependency(string taskId, HashSet<string> visitedTasks)
        {
            if (visitedTasks.Contains(taskId))
            {
                // Found circular dependency
                return true;
            }

            visitedTasks.Add(taskId);

            // Perform a DFS traversal to detect circular depdencies

            if (adjacencyList.ContainsKey(taskId))
            {
                List<string> dependencies = adjacencyList[taskId].Skip(1).ToList();

                foreach (string dependency in dependencies)
                {
                    if (HasCircularDependency(dependency, visitedTasks))
                    {
                        // Found circular dependency
                        return true;
                    }
                }
            }

            visitedTasks.Remove(taskId);

            return false;
        }

        public void RemoveTask(string taskId)
        {
            if (adjacencyList.ContainsKey(taskId))
            {
                adjacencyList.Remove(taskId);

                // Remove this task as a dependency from other tasks
                foreach (var dependencies in adjacencyList.Values)
                {
                    dependencies.Remove(taskId);
                }

                Console.WriteLine($"Task '{taskId} removed successfully!");
            }
            else
            {
                Console.WriteLine($"Task '{taskId}' does not exist in the graph.");
            }
        }

        public void PrintTasks()
        {
            if (adjacencyList.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

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

                Console.WriteLine($"Time needed for task '{taskId}' updated successfully!");
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

        public Dictionary<string, uint> FindEarliestCommencementTimes()
        {
            Dictionary<string, uint> commencementTimes = new Dictionary<string, uint>();

            Console.WriteLine("Earliest commencement times:");

            // Calculate earliest commencement time for each task
            foreach (var task in adjacencyList)
            {
                string taskId = task.Key;

                // Check if the task has already been processed
                if (!commencementTimes.ContainsKey(taskId))
                {
                    CalculateCommencementTime(taskId, commencementTimes);
                }
            }

            return commencementTimes;
        }

        private uint CalculateCommencementTime(string taskId, Dictionary<string, uint> commencementTimes)
        {
            if (commencementTimes.TryGetValue(taskId, out uint commencementTime))
            {
                return commencementTime;
            }

            uint maxDependencyTime = 0;

            List<string> dependencies = adjacencyList[taskId].Skip(1).ToList();

            foreach (string dependency in dependencies)
            {
                uint dependencyTime = CalculateCommencementTime(dependency, commencementTimes);

                // Calculate max depedency time among the dependencies
                maxDependencyTime = Math.Max(maxDependencyTime, dependencyTime + uint.Parse(adjacencyList[dependency][0]));
            }

            // Calculate commencement time for task
            commencementTime = maxDependencyTime;

            // Store in dictionary
            commencementTimes.Add(taskId, commencementTime);

            Console.WriteLine($"{taskId}, {maxDependencyTime}");

            return commencementTime;
        }
    }
}
