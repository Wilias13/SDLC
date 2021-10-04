using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.IO.Compression;

namespace OS_LAB_1
{
    class Program
    {
        private static void DisksInfo()
        {
            var logicalDisks = DriveInfo.GetDrives();
            foreach (var disk in logicalDisks)
                Console.WriteLine("Name: {0}\n\tTotal size: {1} bytes\n\tTotal free space: {2} bytes\n\tAvailable free space: {3} bytes\n\tFile system: {4}",
                                  disk.Name,
                                  disk.TotalSize,
                                  disk.TotalFreeSpace,
                                  disk.AvailableFreeSpace,
                                  disk.DriveFormat);
        }

        private static void WriteReadDeleteTextFile()
        {
            string txtPath = Path.Combine(Environment.CurrentDirectory, "temp.txt");

            Console.WriteLine("Enter some info for file {0}:", txtPath);
            using (StreamWriter sw = File.CreateText(txtPath))
            {
                sw.Write(Console.ReadLine());
            }
            using (StreamReader sr = File.OpenText(txtPath))
            {
                Console.WriteLine("You entered: {0}", sr.ReadToEnd());
            }
            
            File.Delete(txtPath);
        }

        private static void WriteReadDeleteJson()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, "temp.json");

            Console.WriteLine("Enter some info for file {0}:", jsonPath);
            using (StreamWriter sw = File.CreateText(jsonPath))
            {
                sw.Write(JsonSerializer.Serialize(Console.ReadLine()));
            }
            using (StreamReader sr = File.OpenText(jsonPath))
            {
                Console.WriteLine("You entered: {0}", JsonSerializer.Deserialize<string>(sr.ReadToEnd()));
            }
            
            File.Delete(jsonPath);
        }

        private static void WriteReadDeleteXml()
        {
            string xmlPath = Path.Combine(Environment.CurrentDirectory, "temp.xml");

            Console.WriteLine("Enter some info for file {0}:", xmlPath);
            using (StreamWriter sw = File.CreateText(xmlPath))
            {
                using(XmlWriter xmlWriter = XmlWriter.Create(sw))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("userString");
                    xmlWriter.WriteString(Console.ReadLine());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                }
                    
            }
            using (StreamReader sr = File.OpenText(xmlPath))
            {
                using(XmlReader xmlReader = XmlReader.Create(sr))
                {
                    xmlReader.MoveToContent();
                    Console.WriteLine("You entered: {0}", xmlReader.ReadElementContentAsString());
                }
                    
            }
            
            File.Delete(xmlPath);
        }

        private static void ZipFileDeleteZip()
        {
            string zipPath = Path.Combine(Environment.CurrentDirectory, "temp.zip");
            string txtToZipPath = Path.Combine(Environment.CurrentDirectory, "temp.txt");

            Console.WriteLine("Enter some info for file to zip {0}:", txtToZipPath);
            using (StreamWriter sw = File.CreateText(txtToZipPath))
            {
                sw.Write(Console.ReadLine());
            }
            using (StreamReader sr = File.OpenText(txtToZipPath))
            {
                Console.WriteLine("You entered: {0}", sr.ReadToEnd());
            }

            var fileInfo = new FileInfo(txtToZipPath);
            
            using (var stream = File.OpenWrite(zipPath))
            {
                using(ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name, CompressionLevel.Optimal);
                }
            }
            
            File.Delete(txtToZipPath);
            File.Delete(zipPath);
        }

        static void Main(string[] args)
        {
            DisksInfo();
            WriteReadDeleteTextFile();
            WriteReadDeleteJson();
            WriteReadDeleteXml();
            ZipFileDeleteZip();
        }
    }
}
