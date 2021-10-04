using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;
using System.Linq;

namespace HelloApp
{
    class Program
    {

        public static void Task1()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveFormat}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
        }
        private static void outputData(string fileName)
        {
            if (File.Exists(fileName))
            {
                string result = File.ReadAllText(fileName);
                Console.WriteLine("File data: " + result);
                File.Delete(fileName);
            }
        }
        public static void Task2()
        {
            string fileName="", message="";
            Console.WriteLine("Filename: ");
            fileName = Console.ReadLine();
            Console.WriteLine("Your message: ");
            message = Console.ReadLine();
            File.WriteAllText(fileName, message);

            outputData(fileName);
        }

        class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Company { get; set; }
        }
        public static void Task3()
        {
            string fileName = "";
            Console.WriteLine("Filename: ");
            fileName = Console.ReadLine();
            User obj = new User(); obj.Age = 22; obj.Company = "Your Compny"; obj.Name = "Vadim";
            string jsonString = JsonSerializer.Serialize< User>(obj);
            File.WriteAllText(fileName, jsonString);

            outputData(fileName);
        }

        public static void Task4()
        {
             Console.WriteLine("Your file: ");
             string fileName=Console.ReadLine();

             XDocument xdoc = new XDocument();
             XElement galaxys5 = new XElement("User");

             XAttribute galaxysNameAttr = new XAttribute("name", "Vadim");
             XElement galaxysCompanyElem = new XElement("company", "Samsung");
             XElement galaxysPriceElem = new XElement("age", "22");
             galaxys5.Add(galaxysNameAttr);
             galaxys5.Add(galaxysCompanyElem);
             galaxys5.Add(galaxysPriceElem);


             xdoc.Add(galaxys5);
             xdoc.Save(fileName);
            //doc.AppendChild("dddd");
            
            outputData(fileName);

        }

        public static void Task5()
        {
            Console.WriteLine("Enter file name");
            string fileName=Console.ReadLine();
            const string dir = "temp"; string fileFullName = dir + "\\" + fileName;
            string archivePath = dir + ".zip";
            if (File.Exists(archivePath)) File.Delete(archivePath);
            Directory.CreateDirectory(dir);
            if (Directory.Exists(dir))
            {
                File.WriteAllText(fileFullName, "test message");
                ZipFile.CreateFromDirectory(dir, archivePath);
                string tempFile= fileFullName + ".temp";
                using (ZipArchive zipArchive = ZipFile.OpenRead(archivePath))
                {
                    // имя извлекаемого файла
                    string nameExtractFile = fileName;

                    // поиск необходимого файла в архиве
                    // если он есть, то будет вызван метод, который его извлечёт
                    zipArchive.Entries.FirstOrDefault(x => x.Name == nameExtractFile)?.
                        ExtractToFile(tempFile);
                }
                outputData(tempFile);
                FileInfo zip = new FileInfo(archivePath);
                Console.WriteLine("Length: "+zip.Length+" bytes; creation time: "+zip.CreationTime);
                File.Delete(archivePath);
                Directory.Delete(dir,true);
            }
        }

        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
            Task5();

            Console.ReadLine();
        }
    }
}