using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace DayPlanner
{
    
    
    class Program
    {
        
        static void Main(string[] args)
        {
            string filePath = "tasks.txt";

            // Clean up old completed tasks on startup
            CleanUpCompletedTasks(filePath);
            
            // Get today's date in dd.MM.yyyy format
            string Date = DateTime.Now.ToString("dd.MM.yyyy");
            int decision = -1;
            
            
            while (decision != 0)
            {
                Console.WriteLine("What do you want to do ?");
                Console.WriteLine("Press 1 for seeing your tasks for today");
                Console.WriteLine("Press 2 for adding Tasks for a specific date, Format: Task tt.mm.jj");
                Console.WriteLine("Press 3 to remove or complete something from the list");
                Console.WriteLine("Press 0 to exit");

                string input = Console.ReadLine() ?? string.Empty;
                int.TryParse(input, out decision);

                
                switch (decision)
                {
                    case 1:
                    
                        ShowTodaysTasks(filePath, Date);
                        Console.WriteLine("\n Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case 2:
                        AddTask(filePath);
                        Console.WriteLine("\n Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case 3:
                        RemoveorCompleteTask(filePath);
                        Console.WriteLine("\n Press any key to continue...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Bye!");
                        break;
                }

            }
            
        }


        // Removes completed tasks that have passed their due date
        static void CleanUpCompletedTasks(string filePath)
        {
        
            if (!File.Exists(filePath))
            {
                return;
            }

            // Read all tasks from file
            string[] tasks = File.ReadAllLines(filePath);
            var updatedTasks = new List<string>();
            DateTime today = DateTime.Now.Date; 

            // Iterate through all tasks and filter out old completed ones
            foreach (string task in tasks)
            {
                // Check if task is marked as completed
                if (task.StartsWith("[COMPLETED] "))
                {
                    // Extract task content and date
                    string[] parts = task.Substring("[COMPLETED] ".Length).Split(',');
                    if (parts.Length == 2)
                    {
                        string dateInput = parts[1].Trim();
                        // Parse the task date and keep only tasks with future or today's date
                        if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                        {
                            // Keep tasks that are today or in the future
                            if (taskDate.Date >= today)
                            {
                                updatedTasks.Add(task);
                            }
                        }
                        else
                        {
                            // Invalid date format - keep the task to avoid data loss
                            updatedTasks.Add(task); 
                        }
                    }
                    else
                    {
                        // Malformed completed task - keep to avoid data loss
                        updatedTasks.Add(task); 
                    }
                }
                else
                {
                    // Non-completed tasks are always kept
                    updatedTasks.Add(task); 
                }
            }

            // Write updated task list back to file
            File.WriteAllLines(filePath, updatedTasks);
        }

        // Displays tasks for today and optionally shows all tasks
        static void ShowTodaysTasks(string filePath, string todayDate)
        {
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No task found (File not found)");
                return;
            }

            
            string[] tasks = File.ReadAllLines(filePath);
            bool found = false;
            
            Console.WriteLine($"Tasks for today ({todayDate}): ");
            // Display tasks matching today's date
            foreach(string task in tasks)
            {
                if(task.Contains(todayDate))
                {
                    Console.WriteLine(task);
                    found = true;
                }
            }
            

            if(found)
            {
                Console.WriteLine("Do you wanna see the full list of tasks? (y/n)");

                string response = Console.ReadLine() ?? string.Empty;
                if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    foreach(string task in tasks)
                    {
                        Console.WriteLine(task);
                    }

                }
            }
            
            if (!found)
            {
                Console.WriteLine("No tasks for today:)!");
                Console.WriteLine("Do you wanna see the full list of tasks? (y/n)");

                string response1 = Console.ReadLine() ?? string.Empty;
                if (response1.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    foreach(string task in tasks)
                    {
                        Console.WriteLine(task);
                    }

                }
            }
        }
        // Allows user to add a task with a specific date
        private static void AddTask(string filePath)
        {
            Console.WriteLine("Type in task (Format: Task, dd.mm.yyyy): ");
            string task = Console.ReadLine() ?? string.Empty;
            // Split input by comma to separate task and date
            string[] parts = task.Split(',');
            // Validate input format
            if(parts.Length != 2)
            {
                Console.WriteLine("Wrong format!");
                return;
            }
            string taskText = parts [0].Trim();
            string dateInput = parts[1].Trim();

            
            if(!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
            {
                    Console.WriteLine("Invalid date format!");
                    return; 
            }
            // Append task to file with date in correct format
            string dateString = taskDate.ToString("dd.MM.yyyy");
            File.AppendAllText(filePath, $"{taskText},{dateString}{Environment.NewLine}");
            Console.WriteLine("Task added!");

            
         
        }

    
        static void RemoveorCompleteTask(string filePath)
        {
            Console.WriteLine("Do you want to remove a task or mark it as completed? (type r or c)");
            string ressssponse = Console.ReadLine() ?? string.Empty;
            
            if (ressssponse.Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                // Validate file and tasks exist
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No tasks found");
                    return;
                }
                string[] tasks = File.ReadAllLines(filePath);
                if (tasks.Length == 0)
                {
                    Console.WriteLine("No tasks to complete!");
                    return;
                }
                // Display all tasks with numbers for selection
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {tasks[i]}");
                }
                Console.WriteLine("Which task do you want to mark as completed? Please type in the Number:  ");
                string input1 = Console.ReadLine() ?? string.Empty;
                // Validate and mark selected task as completed
                if (int.TryParse(input1, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
                {
                    var taskList = tasks.ToList();
                    // Add [COMPLETED] prefix to the task
                    taskList[taskNumber - 1] = "[COMPLETED] " + taskList[taskNumber - 1];
                    File.WriteAllLines(filePath, taskList);
                    Console.WriteLine("Task marked as completed!");
                }
                else
                {
                    Console.WriteLine("Invalid task number!");
                }
            }
            // Handle 'remove' option
            if (ressssponse.Equals("r", StringComparison.OrdinalIgnoreCase))
            {
                
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No tasks found");
                    return;
                }
                string[] tasks = File.ReadAllLines(filePath); 
                if (tasks.Length == 0)
                {
                    Console.WriteLine("No tasks to remove!");
                    return;
                }
                // Display all tasks with numbers for selection
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {tasks[i]}");
                }
                Console.WriteLine("Do you want to remove all tasks? (y/n)");
                string response = Console.ReadLine() ?? string.Empty;
                // Clear all tasks if confirmed
                if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    File.WriteAllText(filePath, string.Empty);
                    Console.WriteLine("All tasks removed!");
                    return;
                }
                // Validate user input
                if (!response.Equals("n", StringComparison.OrdinalIgnoreCase) && !response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Invalid input! Please enter y or n.");
                    return;
                }
                // Remove single task if user chooses "no" to removing all
                if (response.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Which task do you want to remove? Please type in the Number:  ");
                    string input2 = Console.ReadLine() ?? string.Empty;
                    // Validate and remove selected task
                    if (int.TryParse(input2, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
                    {
                        var taskList = tasks.ToList();
                        taskList.RemoveAt(taskNumber - 1);
                        File.WriteAllLines(filePath, taskList);
                        Console.WriteLine("Task removed");
                    }
                    else
                    {
                        Console.WriteLine("No Task found at specific number");
                    }
                }
            }
           
        }

        /* ==================== FUTURE ENHANCEMENT IDEAS ====================
         * TODO: Display today's tasks first when showing the full task list
         * TODO: Add ability to add details to tasks/switch task dates
         * TODO: Implement search functionality
         * TODO: Sort tasks by date
         * TODO: Add export functionality
         * TODO: Implement undo/redo functionality
         * TODO: Add multi-user support
         * TODO: Create graphical user interface (GUI)
         * TODO: Add confirmation dialog before deleting tasks
         * TODO: Add task priority levels
         * ================================================================== */ 
        

    }

}
