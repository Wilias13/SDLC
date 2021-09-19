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
            char a;
            do
            {
                Console.WriteLine("\n1) Create XML and write string in it\n2) Read XML\n3) Deleted XML \n4) Main menu");
                a = Console.ReadKey().KeyChar;
                switch (a)
                {
                    case '1':
                        WriteInXML();
                        break;
                    case '2':
                        DeserializeXML();
                        break;
                    case '3':
                        FileInfo FI = new FileInfo("temp.xml");
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
        public static void WriteInXML()
        {
            Console.WriteLine("\nName of person:");
            string name = Console.ReadLine();
            Console.WriteLine("Age:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Company:");
            Person person = new Person(name, age, Console.ReadLine());
            XmlSerializer formatter = new XmlSerializer(typeof(Person));
            using (FileStream fs = new FileStream("temp.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, person);
            }
        }
        public static void DeserializeXML()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Person));
            using (FileStream fs = new FileStream("temp.xml", FileMode.OpenOrCreate))
            {
                Person newPerson = (Person)formatter.Deserialize(fs);
                Console.WriteLine($"\nName: {newPerson.Name} --- Age: {newPerson.Age} --- Company: {newPerson.Company}");
            }
        }
    }
}
