using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Text;
using System.IO;
using System.Threading.Tasks; 

namespace DayPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "tasks.txt"; 
            int decision = 0; 
            Console.WriteLine("What do you wanna do ?");
            Console.WriteLine("Press 1 for seeing your tasks for today");
            Console.WriteLine("Press 2 for adding Tasks for a specific date");
            Console.WriteLine("Press 3 to remove something from the list");
            string input = Console.ReadLine();
            Int32.TryParse(input, out decision); 
                Console.WriteLine(decision);
                if(decision != 1 && decision != 2 && decision != 3)
                {
                    Console.WriteLine("Try again");
                }
                if (decision == 1)
                {
                    if(File.Exists(filePath))
                    {
                    string tasks = File.ReadAllText(filePath);
                    Console.WriteLine("Deine Aufgaben: ");
                    Console.WriteLine(tasks);
                    }
                    else {Console.WriteLine("Keine Aufgaben gefunden");}
                }
                else if (decision == 2)
                {
                    Console.WriteLine("Gib die Aufgabe ein: ");
                    string task = Console.ReadLine();
                    File.AppendAllText(filePath, task + Environment.NewLine);
                    Console.WriteLine("Aufgabe gespeichert!");
                }
                else if (decision == 3)
                {
                    if(File.Exists(filePath))
                    {
                        string [] tasks = File.ReadAllLines(filePath);

                        if (tasks.Length == 0)
                             {
                                 Console.WriteLine("Keine Aufgabe zum Löschen!");
                             }
                            else
                            {
                                Console.WriteLine("Deine Aufgaben: ");
                                for(int i = 0; i < tasks.Length; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                                }

                                Console.WriteLine("Welche Aufgabe willst du löschen? Gib die Nummer ein: ");
                                string input2 = Console.ReadLine();
                                if (int.TryParse(input2, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
                                {
                                    var taskList = tasks.ToList();
                                    taskList.RemoveAt(taskNumber -1);

                                    File.WriteAllLines(filePath, taskList);
                                    Console.WriteLine("Aufgabe Geöscht!");
                                }
                                else
                                {
                                    Console.WriteLine("Keine Aufgabe gefunden!");
                                }
                            }
                    }

                }

        }
    }
}
