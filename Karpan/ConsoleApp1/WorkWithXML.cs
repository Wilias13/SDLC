using System;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }

        public Person()
        {
        }

        public Person(string name, int age, string company)
        {
            Name = name;
            Age = age;
            Company = company;
        }
    }
    class WorkWithXML
    {
        public static void Run()
        {
            Console.WriteLine("\n  Работа с XML\nВведите имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите название компании:");
            SerealizeXML(name,age,Console.ReadLine());
            Console.WriteLine("Создан XML-файл. \nСодержимое XML-файла:");
            DeserealizeXML();
            FileInfo FD = new("temp.xml");
            FD.Delete();
        }
        public static void SerealizeXML(string name3, int age3, string company3)
        {
            Person person1 = new("Frank", 20, "Apple");
            Person person2 = new("Magnus", 30, "Microsoft");
            Person person3 = new(name3, age3, company3);
            Person[] people = new Person[] { person1, person2, person3 };

            XmlSerializer formatter = new(typeof(Person[]));

            using FileStream fs = new("temp.xml", FileMode.OpenOrCreate);
            formatter.Serialize(fs, people);
        }
        public static void DeserealizeXML()
        {
            XmlSerializer formatter = new(typeof(Person[]));
            using FileStream fs = new("temp.xml", FileMode.OpenOrCreate);
            Person[] newpeople = (Person[])formatter.Deserialize(fs);

            for (int i = 0; i < newpeople.Length; i++)
            {
                Person p = newpeople[i];
                Console.WriteLine($"Имя: {p.Name} \t Возраст: {p.Age} \t Компания: {p.Company}");
            }
        }
    }
}
