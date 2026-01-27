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
            string input = Console.ReadLine();
            Int32.TryParse(input, out decision); 
                Console.WriteLine(decision);
                if(decision != 1 && decision != 2)
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
                    File.AppendAllText(filePath, task + "/n");
                    Console.WriteLine("Aufgabe gespeichert!");
                }

        }
    }
}
