using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Homework
{

    class Program
    {
        class Person
        {
            public string Name { get; set; }
        }

        public static void WriteAndReadTextFile()
        {
            Console.WriteLine("\nВведите название файла: ");
            string path = Console.ReadLine();

            Console.WriteLine("Введите текст:");
            string text = Console.ReadLine();

            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }

            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }

            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)
                fileInf.Delete();
        }

        public static async Task CreateAndWriteAndReadAndDeleteJson()
        {
            Console.WriteLine("\nВведите название файла для создания json файла: ");
            string path = Console.ReadLine();

            Console.WriteLine("Введите текст:");
            string text = Console.ReadLine();

            using (FileStream fs = new FileStream(path + ".json", FileMode.OpenOrCreate))
            {
                Person tom = new Person() { Name = text };
                await JsonSerializer.SerializeAsync<Person>(fs, tom);
            }

            using (FileStream fs = new FileStream(path + ".json", FileMode.OpenOrCreate))
            {
                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
                Console.WriteLine($"Текст из файла : {restoredPerson.Name}");
            }
            File.Delete(path + ".json");
        }

        public static void CreateAndWriteXML()
        {
            XDocument xdoc = new XDocument();
            Console.WriteLine("\nВведите название файла для создания xml файла: ");
            string path = Console.ReadLine();
            XElement data = new XElement(path + ".xml");
            XElement Elem_2 = new XElement("Elem_2");
            XElement Elem_3 = new XElement("Elem_3");
            data.Add(Elem_2);
            data.Add(Elem_3);
            xdoc.Add(data);
            xdoc.Save(path + ".xml");
            var doc = new XmlDocument();
            doc.Load(path + ".xml");
            var root = doc.DocumentElement;
            PrintItem(root);
            File.Delete(path + ".xml");
        }

        private static void PrintItem(XmlElement item, int indent = 0)
        {
            Console.Write($"{new string('\t', indent)}{item.LocalName} ");

            foreach (XmlAttribute attr in item.Attributes)
                Console.Write($"[{attr.InnerText}]");
            foreach (var child in item.ChildNodes)
            {
                if (child is XmlElement node)
                {
                    Console.WriteLine();
                    PrintItem(node, indent + 1);
                }

                if (child is XmlText text)
                    Console.Write($"- {text.InnerText}");
            }
        }

        public static void achive()
        {
            string archivePath = Directory.GetCurrentDirectory() + "\\archive.zip";


            using (FileStream fstream = new FileStream(Directory.GetCurrentDirectory() + "\\file.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes("text");
                fstream.Write(array, 0, array.Length);
            }

            using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                string pathFileToAdd = Directory.GetCurrentDirectory() + "\\file.txt";
                const string nameFileToAdd = "file.txt";
                zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
            }

            File.Delete(Directory.GetCurrentDirectory() + "\\file.txt");

            using (ZipArchive zipArchive = ZipFile.OpenRead(archivePath))
            {
                const string nameExtractFile = "file.txt";
                string pathExtractFile = Directory.GetCurrentDirectory() + "\\file.txt";
                zipArchive.Entries.FirstOrDefault(x => x.Name == nameExtractFile)?.
                    ExtractToFile(pathExtractFile);
            }

            using (FileStream fstream = File.OpenRead(Directory.GetCurrentDirectory() + "\\file.txt"))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }
            File.Delete(Directory.GetCurrentDirectory() + "\\file.txt");
            File.Delete(Directory.GetCurrentDirectory() + "\\archive.zip");
        }

        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Метка: {drive.Name}");
                    Console.WriteLine($"Тип: {drive.DriveFormat}");
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Название: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }

            WriteAndReadTextFile();
            CreateAndWriteAndReadAndDeleteJson();
            CreateAndWriteXML();
            achive();

        }

    }
}
