using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }
    }
    class WorkWithJson
    {
        public void Run()
        {
            char a;
            do
            {
                Console.WriteLine("\n1) Create Json and write string in it\n2) Read Json\n3) Deleted Json \n4) Main menu");
                a = Console.ReadKey().KeyChar;
                switch (a)
                {
                    case '1':
                        WriteInJson();
                        break;
                    case '2':
                        DeserialazeJSON();
                        break;
                    case '3':
                        FileInfo FI = new FileInfo("temp.json");
                        FI.Delete();
                        break;
                    case '4':
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            while (a != '4');
        }

        public static void WriteInJson()
        {
            Console.WriteLine("\nName of person:");
            string name = Console.ReadLine();
            Console.WriteLine("Age:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Company:");
            using (StreamWriter file = File.CreateText("temp.json"))
            {
                User us = new User() { Name = name, Age = age, Company = Console.ReadLine() };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, us);
            }
        }
        public static void DeserialazeJSON()
        {
            using (StreamReader file = File.OpenText("temp.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                User user1 = (User)serializer.Deserialize(file, typeof(User));
                Console.WriteLine($"\nName: {user1.Name}  Age: {user1.Age} Company: {user1.Company}");
            }
        }
    }
}
