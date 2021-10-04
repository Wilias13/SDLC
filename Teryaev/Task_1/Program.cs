using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;
using System.IO.Compression;

namespace Latypov_1
{
    class Program
    {
        static void Main(string[] args)
        {
            task_0();
            //task_1();
            //task_2();
            //task_3();
            //task_4();
        }
        static void task_0()
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

        static void task_1()
        {
            Console.WriteLine("Задание №1.");
            // создаем каталог для файла
            string path = @"C:\SomeDir2";
            string filename = "note.txt";
            
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            // запись в файл
            writeToFile(path, filename, text);
            // чтение из файла
            string redData = readFromFile(path, filename);
            // удаление
            deleteFile(path, filename);

            Console.ReadLine();
        }

        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        static void task_2()
        {
            Console.WriteLine("Задание №2.");
            // сохранение данных
            string path = @"C:\SomeDir2";
            string filename = "user.json";

            Person tom = new Person { Name = "Tom", Age = 35 };
            string data = JsonSerializer.Serialize<Person>(tom);

            writeToFile(path, filename, data);

            string textFromFile = readFromFile(path, filename);
            Person restoredPerson = JsonSerializer.Deserialize<Person>(textFromFile);
            Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");

            deleteFile(path, filename);
        }

        class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Company { get; set; }
        }

        static void task_3()
        {

            User user1 = new User { Name = "Bill Gates", Age = 48};
            User user2 = new User { Name = "Larry Page", Age = 42};
            List<User> users = new List<User> { user1, user2 };

            XmlWriter xmlWriter = XmlWriter.Create(@"C:\SomeDir2\users.xml");
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("users");
            foreach(User user in users)
            {
                xmlWriter.WriteStartElement("user");
                xmlWriter.WriteAttributeString("age", user.Age.ToString());
                xmlWriter.WriteString(user.Name);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            users = new List<User>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\SomeDir2\users.xml");
            XmlNodeList userNodes = xDoc.SelectNodes("//users/user");
            foreach (XmlNode userNode in userNodes)
            {
                User user = new User();
                user.Age = int.Parse(userNode.Attributes["age"].Value);
                user.Name = userNode.InnerText;
                users.Add(user);
            }

            foreach (User u in users)
            {
                Console.WriteLine($"{u.Name} - {u.Age}");
            }
            Console.Read();

            deleteFile(@"C:\SomeDir2", "users.xml");
        }

        static void task_4()
        {
            string sourceFile = @"C:\SomeDir2\book.pdf"; // исходный файл
            string compressedFile = @"C:\SomeDir2\book.gz"; // сжатый файл
            string targetFile = @"C:\SomeDir2\book_new.pdf"; // восстановленный файл

            // создание сжатого файла
            Compress(sourceFile, compressedFile);
            // чтение из сжатого файла
            Decompress(compressedFile, targetFile);

            deleteFile(@"C:\SomeDir2", "book.gz");
            deleteFile(@"C:\SomeDir2", "book_new.pdf");

            Console.ReadLine();
        }

        public static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
        }

        static void writeToFile(string path, string filename, string data)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            using (FileStream fstream = new FileStream($"{path}\\{filename}", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(data);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }

        static string readFromFile(string path, string filename)
        {
            using (FileStream fstream = File.OpenRead($"{path}\\{filename}"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
                return textFromFile;
            }
        }

        static void deleteFile(string path, string filename)
        {
            FileInfo fileInf = new FileInfo($"{path}\\{filename}");
            if (fileInf.Exists)
            {
                fileInf.Delete();
                Console.WriteLine($"Файл: {path}\\{filename} удален.");
            }
        }
    }
}
