using Newtonsoft.Json;
using System;
using System.IO;

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
        public static void Run()
        {
            Console.WriteLine("\n  Работа с JSON\nВведите имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите название компании:");
            SerialazeJSON(name, age, Console.ReadLine());
            Console.WriteLine("Содержимое JSON-файла:");
            DeserialazeJSON();
        }

        public static void SerialazeJSON(string name, int age, string company)
        {
            using StreamWriter file = File.CreateText("temp.json");
            User us = new() { Name = name, Age = age, Company = company };
            JsonSerializer serializer = new();
            serializer.Serialize(file, us);
            Console.WriteLine("Информация сохранена в файл.");
        }
        public static void DeserialazeJSON()
        {
            using StreamReader file = File.OpenText("temp.json");
            JsonSerializer serializer = new();
            User user1 = (User)serializer.Deserialize(file, typeof(User));
            Console.WriteLine($"Имя: {user1.Name} \t Возраст: {user1.Age} \t Компания: {user1.Company}");
        }
    }
}
