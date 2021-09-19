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
            Console.WriteLine("\nРабота с JSON\nВведите любое имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите название компании:");
            SerialazeJSON(name, age, Console.ReadLine());
            DeserialazeJSON();
        }

        public static void SerialazeJSON(string name, int age, string company)
        {
            using (StreamWriter file = File.CreateText("temp.json"))
            {
                User us = new User() { Name = name, Age = age, Company = company };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, us);
                Console.WriteLine("Информация сохранена в файл");
            }
        }
        public static void DeserialazeJSON()
        {
            using (StreamReader file = File.OpenText("temp.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                User user1 = (User)serializer.Deserialize(file, typeof(User));
                Console.WriteLine($"Имя: {user1.Name}  Возраст: {user1.Age} Компания: {user1.Company}");
            }
        }
    }
}
