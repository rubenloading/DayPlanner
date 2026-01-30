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

            while(true)
            {
            string Date = DateTime.Now.ToString("dd.MM.yyyy");
            int decision = 0;
            Console.WriteLine("What do you wanna do ?");
            Console.WriteLine("Press 1 for seeing your tasks for today");
            Console.WriteLine("Press 2 for adding Tasks for a specific date, Format: Task tt.mm.jj");
            Console.WriteLine("Press 3 to remove something from the list");

            string input = Console.ReadLine();
            int.TryParse(input, out decision);

            switch (decision)
            {
                case 1:
                    ShowTodaysTasks(filePath, Date);
                    break; 
                case 2:
                    AddTask(filePath);
                    break;
                case 3:
                    RemoveTask(filePath);
                    break;
                default:
                    Console.WriteLine("Bitte 1-3 eingeben!");
                    break;
            }
            }
            
        }

        static void ShowTodaysTasks(string filePath, string todayDate)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Keine Aufgaben gefunden (Datei existiert nicht).");
                return;
            }

            string[] tasks = File.ReadAllLines(filePath);
            bool found = false;

            Console.WriteLine($"Deine Aufgaben für heute ({todayDate}): ");

            foreach (string task in tasks)
            {
                if (task.Contains(todayDate))
                {
                    Console.WriteLine(task);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Keine Aufgaben für heute :)!");
            }
        }

        public static void AddTask(string filePath)
        {
            Console.WriteLine("Gib die Aufgabe ein (Format: Aufgabe tt.mm.jj): ");
            string task = Console.ReadLine();
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

            
          /*  if (!string.IsNullOrWhiteSpace(task))
            {
                File.AppendAllText(filePath, task + Environment.NewLine);
                Console.WriteLine("Aufgabe gespeichert!");
            }*/
        }

        static void RemoveTask(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Keine Aufgaben gefunden");
                return;
            }

            string[] tasks = File.ReadAllLines(filePath);
            if (tasks.Length == 0)
            {
                Console.WriteLine("Keine Aufgabe zum Entfernen!");
                return;
            }

            for (int i = 0; i < tasks.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {tasks[i]}");
            }

            Console.WriteLine("Welche Aufgabe willst du Entfernen ? Gib die Nummer ein: ");
            string input2 = Console.ReadLine();

            if (int.TryParse(input2, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
            {
                var taskList = tasks.ToList();
                taskList.RemoveAt(taskNumber - 1);

                File.WriteAllLines(filePath, taskList);
                Console.WriteLine("Aufgabe Gelöscht!");
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe!");
            }
        }
    }
}
