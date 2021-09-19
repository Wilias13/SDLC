using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
        { }

        public Person(string name, int age, string company)
        {
            Name = name;
            Age = age;
            Company = company;
        }
    }
    class WorkWithXML
    {
        public void Run()
        {
            Console.WriteLine("\nРабота с XML\nВведите любое имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите возраст:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите название компании:");
            SerealizeXML(name,age,Console.ReadLine());
            Console.WriteLine("Создан XML-файл. \nСодержимое XML-файла:");
            DeserealizeXML();
            FileInfo FD = new FileInfo("temp.xml");
            FD.Delete();
        }
        public static void SerealizeXML(string name3, int age3, string company3)
        {
            Person person1 = new Person("Tom", 29, "Microsoft");
            Person person2 = new Person("Bill", 25, "Apple");
            Person person3 = new Person(name3, age3, company3);
            Person[] people = new Person[] { person1, person2, person3 };

            XmlSerializer formatter = new XmlSerializer(typeof(Person[]));

            using (FileStream fs = new FileStream("temp.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, people);
            }
        }
        public static void DeserealizeXML()
        {
            //первый способ десериализации
            XmlSerializer formatter = new XmlSerializer(typeof(Person[]));
            using (FileStream fs = new FileStream("temp.xml", FileMode.OpenOrCreate))
            {
                Person[] newpeople = (Person[])formatter.Deserialize(fs);

                foreach (Person p in newpeople)
                {
                    Console.WriteLine($"Имя: {p.Name} --- Возраст: {p.Age} --- Компания: {p.Company}");
                }
            }

            //второй способ десериализации
            /*List<Person> persons = new List<Person>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("temp.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                Person person = new Person();
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "Name")
                        person.Name = childnode.InnerText;

                    if (childnode.Name == "Company")
                        person.Company = childnode.InnerText;

                    if (childnode.Name == "Age")
                        person.Age = Int32.Parse(childnode.InnerText);
                }
                persons.Add(person);
            }
            foreach (Person p in persons)
                Console.WriteLine($"{p.Name} ({p.Company}) - {p.Age}");*/
        }
    }
}
