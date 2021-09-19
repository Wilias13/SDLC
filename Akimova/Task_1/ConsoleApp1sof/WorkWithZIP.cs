using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class WorkWithZIP
    {
        public void Run()
        {
            string sourceFile = "temp.txt";
            string compressedFile = "temp.gz";
            string targetFile = "temp_new.txt";
            char a;
            do
            {
                Console.WriteLine("\n1) Create file and write string in it\n2) Compress file\n3) Read ZIP\n4) Deleted XML \n5) Main menu");
                a = Console.ReadKey().KeyChar;
                switch (a)
                {
                    case '1':
                        WorkWithFile CreateFile = new WorkWithFile();
                        CreateFile.CreateFile(sourceFile);
                        break;
                    case '2':
                        Compress(sourceFile, compressedFile);
                        FileInfo FD = new FileInfo(sourceFile);
                        FD.Delete();
                        break;
                    case '3':
                        Decompress(compressedFile, targetFile);
                        WorkWithFile WWF = new WorkWithFile();
                        WWF.ReadFile(targetFile);
                        FileInfo FI = new FileInfo(targetFile);
                        FI.Delete();
                        break;
                    case '4':
                        FileInfo FC = new FileInfo(compressedFile);
                        FC.Delete();
                        break;
                    case '5':
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            while (a != '5');
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
                        Console.WriteLine("\nFile {0} been compressed. Original size: {1}  Compressed size: {2}.",
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
                        Console.WriteLine("Restored file: {0}", targetFile);
                    }
                }
            }
        }
    }
}
