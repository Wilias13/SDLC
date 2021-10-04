using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace HelloApp
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //info();
            //fileinfo();
            //SerialazeJSON();
            //DeserialazeJSON();
            xml1();
            string sourceFile = "phones.xml";
            string compressedFile = "phones.gz";
            string targetFile = "phones_new.xml";

            Compress(sourceFile, compressedFile);
            Decompress(compressedFile, targetFile);
            //xml2();
            Console.ReadKey();

        }
        static void info()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
        }
        static void fileinfo()
        {
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            using (FileStream fstream = new FileStream("note.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }

            using (FileStream fstream = File.OpenRead("note.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }

            FileInfo fileInf = new FileInfo("note.txt");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }

        }
        public static void SerialazeJSON()
        {
            using (StreamWriter file = File.CreateText("temp.json"))
            {
                Person per = new Person() { Name = "Роаопрв", Age = 23};
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, per);
            }
        }
        public static void DeserialazeJSON()
        {
            using (StreamReader file = File.OpenText("temp.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Person person1 = (Person)serializer.Deserialize(file, typeof(Person));
                Console.WriteLine($"Имя: {person1.Name}  Возраст: {person1.Age}");
            }
            FileInfo fileInf = new FileInfo("temp.json");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
        }
        static void xml1()
        {
            XDocument xdoc = new XDocument(new XElement("phones",
                new XElement("phone",
                    new XAttribute("name", "iPhone 6"),
                    new XElement("company", "Apple"),
                    new XElement("price", "40000")),
                new XElement("phone",
                    new XAttribute("name", "Samsung Galaxy S5"),
                    new XElement("company", "Samsung"),
                    new XElement("price", "33000"))));
            xdoc.Save("phones.xml");

        }
        static void xml2()
        {
            XDocument xdoc = XDocument.Load("phones.xml");
            foreach (XElement phoneElement in xdoc.Element("phones").Elements("phone"))
            {
                XAttribute nameAttribute = phoneElement.Attribute("name");
                XElement companyElement = phoneElement.Element("company");
                XElement priceElement = phoneElement.Element("price");

                if (nameAttribute != null && companyElement != null && priceElement != null)
                {
                    Console.WriteLine($"Смартфон: {nameAttribute.Value}");
                    Console.WriteLine($"Компания: {companyElement.Value}");
                    Console.WriteLine($"Цена: {priceElement.Value}");
                }
                Console.WriteLine();
            }
           
            FileInfo fileInf = new FileInfo("phones.xml");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
        }
        public static void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
            FileInfo fileInf = new FileInfo(compressedFile);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            FileInfo fileInf1 = new FileInfo(targetFile);
            if (fileInf1.Exists)
            {
                fileInf1.Delete();
            }
        }
    }
}