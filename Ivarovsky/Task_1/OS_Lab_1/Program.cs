using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.IO.Compression;

namespace OS_Lab_1
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            while (true)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("------------------Menu------------------");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("1.Drive|2.File|3.JSON|4.XML|5.ZIP|6.Exit");
                Console.WriteLine("----------------------------------------");
                Console.Write("Enter number: ");
                try 
                {
                    n = Convert.ToInt32(Console.ReadLine());
                    switch (n)
                    {
                        case 1: Console.WriteLine(); Drive_Work(); break;
                        case 2: Console.WriteLine(); File_Work(); break;
                        case 3: Console.WriteLine(); JSON_Work(); break;
                        case 4: Console.WriteLine(); XML_Work(); break;
                        case 5: Console.WriteLine(); ZIP_Work(); break;
                        case 6: ; Console.WriteLine(); break;
                        default: throw new ArgumentException("Illegal operation code.");
                    }
                }
                catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
                if (n == 6) { break; }
            }
        }
        public static void Drive_Work()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    Console.WriteLine($"Name: {drive.Name}");
                    Console.WriteLine($"Type: {drive.DriveType}");
                    if (drive.IsReady)
                    {
                        Console.WriteLine($"Total: {drive.TotalSize}");
                        Console.WriteLine($"Free: {drive.TotalFreeSpace}");
                        Console.WriteLine($"Label: {drive.VolumeLabel}");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
        }

        public static void File_Work()
        {
            try
            {
                string path = @"C:\test.txt";
                if (File.Exists(path)) { File.Delete(path); }
                Console.Write("Enter string: ");
                string text = Console.ReadLine();
                Console.WriteLine();
                using (FileStream fs = File.Create(path))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fs.Write(array, 0, array.Length);
                    Console.WriteLine("Written succesfully.");
                }
                Console.WriteLine();
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine($"Entered string: {textFromFile}");
                }
                Console.WriteLine();
                Console.WriteLine("Press Enter to delete...");
                Console.ReadLine();
                if (File.Exists(path)) { File.Delete(path); }
                Console.WriteLine("Deleted Succesfully.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
            catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
        }

        public static void JSON_Work()
        {
            try
            {
                string path = @"C:\test.json";
                if (File.Exists(path)) { File.Delete(path); }
                Console.Write("Enter person's name: ");
                string n = Console.ReadLine();
                int a;
                Console.Write("Enter person's age: ");
                a = Convert.ToInt32(Console.ReadLine());
                Person p = new Person() { Name = n, Age = a };
                string json = JsonSerializer.Serialize<Person>(p);
                File.WriteAllText(path, json);
                Console.WriteLine();
                Console.WriteLine("Written succesfully.");
                Console.WriteLine();
                json = File.ReadAllText(path);
                Person restoredPerson = JsonSerializer.Deserialize<Person>(json);
                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
                Console.WriteLine();
                Console.WriteLine("Press Enter to delete...");
                Console.ReadLine();
                if (File.Exists(path)) { File.Delete(path); }
                Console.WriteLine("Deleted Succesfully.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
            catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
        }

        public static void XML_Work()
        {
            try
            {
                string path = @"C:\test.xml";
                List<Person> Persons = new List<Person>();
                if (File.Exists(path)) { File.Delete(path); }
                Console.Write("Enter 1 person's name: ");
                string n = Console.ReadLine();
                Console.Write("Enter 1 person's age: ");
                int a = Convert.ToInt32(Console.ReadLine());
                Person p1 = new Person() { Name = n, Age = a };
                Persons.Add(p1);
                Console.Write("Enter 2 person's name: ");
                n = Console.ReadLine();
                Console.Write("Enter 2 person's age: ");
                a = Convert.ToInt32(Console.ReadLine());
                Person p2 = new Person() { Name = n, Age = a };
                Persons.Add(p2);
                var serializer = new XmlSerializer(typeof(List<Person>));
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate)) { serializer.Serialize(fs, Persons); }
                Console.WriteLine();
                Console.WriteLine("Written succesfully.");
                Console.WriteLine();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    List<Person> restoredPersons = (List<Person>)serializer.Deserialize(fs);
                    foreach (Person i in restoredPersons) { Console.WriteLine($"Name: {i.Name}  Age: {i.Age}"); }
                }
                Console.WriteLine();
                Console.WriteLine("Press Enter to delete...");
                Console.ReadLine();
                if (File.Exists(path)) { File.Delete(path); }
                Console.WriteLine("Deleted Succesfully.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
            catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
        }

        public static void ZIP_Work()
        {
            try
            {
                string sourcepath = @"C:\test";
                string targetpath = @"C:\test2";
                DirectoryInfo dirInfo = new DirectoryInfo(sourcepath);
                if (!dirInfo.Exists) { dirInfo.Create(); }
                DirectoryInfo dirInfo2 = new DirectoryInfo(targetpath);
                if (!dirInfo.Exists) { dirInfo.Create(); }
                using (File.Create(sourcepath + @"\test.txt")) { } ;
                string sourceFolder = "C://test";
                string zip = "C://test.zip";
                string targetFolder = "C://test2";
                ZipFile.CreateFromDirectory(sourceFolder, zip);
                Console.WriteLine($"Папка {sourceFolder} архивирована в файл {zip}");
                ZipFile.ExtractToDirectory(zip, targetFolder);
                Console.WriteLine($"Файл {zip} распакован в папку {targetFolder}");
                Console.WriteLine();
                Console.WriteLine("Press Enter to delete...");
                Console.ReadLine();
                Directory.Delete(sourcepath, true);
                Directory.Delete(targetpath, true);
                File.Delete(zip);
                Console.WriteLine("Deleted Succesfully.");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
            catch (Exception x) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(); Console.WriteLine(x.Message); Console.WriteLine(); Console.ResetColor(); Console.WriteLine("Press Enter to continue..."); Console.ReadLine(); }
        }
    }
}
