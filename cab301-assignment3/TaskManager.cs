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

        public void ReadTasksFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

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
            Dictionary<string, int> inDegree = new Dictionary<string, int>();

            // Initialize in-degree for all tasks
            foreach (var task in adjacencyList)
            {
                string taskId = task.Key;
                inDegree[taskId] = 0;
            }

            // Calculate in-degree for each task
            foreach (var task in adjacencyList)
            {
                string taskId = task.Key;
                List<string> dependencies = task.Value.Skip(1).ToList();

                foreach (string dependency in dependencies)
                {
                    inDegree[dependency]++;
                }
            }

            // Enqueue tasks with in-degree 0
            Queue<string> queue = new Queue<string>();
            foreach (var task in inDegree)
            {
                if (task.Value == 0)
                {
                    queue.Enqueue(task.Key);
                }
            }

            // Perform topological sorting
            while (queue.Count > 0)
            {
                string task = queue.Dequeue();
                sortedTasks.Add(task);

                if (adjacencyList.ContainsKey(task))
                {
                    List<string> dependencies = adjacencyList[task].Skip(1).ToList();

                    foreach (string dependency in dependencies)
                    {
                        inDegree[dependency]--;

                        if (inDegree[dependency] == 0)
                        {
                            queue.Enqueue(dependency);
                        }
                    }
                }
            }

            return sortedTasks;
        }
    }
}
