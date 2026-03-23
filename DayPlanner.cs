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

            // Neue Zeile: Bereinige abgeschlossene Tasks mit vergangenen Daten beim Start
            CleanUpCompletedTasks(filePath);
            
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


        static void CleanUpCompletedTasks(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string[] tasks = File.ReadAllLines(filePath);
            var updatedTasks = new List<string>();
            DateTime today = DateTime.Now.Date; 

            foreach (string task in tasks)
            {
                if (task.StartsWith("[COMPLETED] "))
                {
                    
                    string[] parts = task.Substring("[COMPLETED] ".Length).Split(',');
                    if (parts.Length == 2)
                    {
                        string dateInput = parts[1].Trim();
                        if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                        {
                            if (taskDate.Date >= today)
                            {
                                updatedTasks.Add(task);
                            }
                            
                        }
                        else
                        {
                            updatedTasks.Add(task); 
                        }
                    }
                    else
                    {
                        updatedTasks.Add(task); 
                    }
                }
                else
                {
                    updatedTasks.Add(task); 
                }
            }

            File.WriteAllLines(filePath, updatedTasks);
        }

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
        private static void AddTask(string filePath)
        {
            Console.WriteLine("Type in task (Format: Task, dd.mm.yyyy): ");
            string task = Console.ReadLine() ?? string.Empty;
            string[] parts = task.Split(',');
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
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {tasks[i]}");
                }
                Console.WriteLine("Which task do you want to mark as completed? Please type in the Number:  ");
                string input1 = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(input1, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
                {
                    var taskList = tasks.ToList();
                    
                    taskList[taskNumber - 1] = "[COMPLETED] " + taskList[taskNumber - 1];
                    File.WriteAllLines(filePath, taskList);
                    Console.WriteLine("Task marked as completed!");
                }
                else
                {
                    Console.WriteLine("Invalid task number!");
                }
            }
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
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {tasks[i]}");
                }
                Console.WriteLine("Do you want to remove all tasks? (y/n)");
                string response = Console.ReadLine() ?? string.Empty;
                if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    File.WriteAllText(filePath, string.Empty);
                    Console.WriteLine("All tasks removed!");
                    return;
                }
                if (!response.Equals("n", StringComparison.OrdinalIgnoreCase) && !response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Invalid input! Please enter y or n.");
                    return;
                }
                if (response.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Which task do you want to remove? Please type in the Number:  ");
                    string input2 = Console.ReadLine() ?? string.Empty;
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


           
        //next: 
        //tasks zuerst für heute anzeigen bei ganzer liste
        //adding details to tasks/switching dates 
        //search ? 
        // task erledigen statt löschen 
        //prioritäten ? 
        //sortieren nach Datum ?
        //export ? 
        //undo redo
        //multi user 
        //GUI 
        //bestätigung vor löschen 
        

    }

}
