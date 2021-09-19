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
        public void Run(string path)
        {
            string sourceFile = path;
            string compressedFile = "temp.gz";
            string targetFile = "temp_new.txt";

            Compress(sourceFile, compressedFile);
            Decompress(compressedFile, targetFile);

            Console.WriteLine("Содержимое восстановленного файла:");
            WorkWithFile WWF = new WorkWithFile();
            FileInfo FI = new FileInfo(targetFile);
            WWF.ReadFile(FI);
            FI.Delete();

            FileInfo FD = new FileInfo(compressedFile);
            FD.Delete();
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
                        Console.WriteLine("Восстановленый файл: {0}", targetFile);
                    }
                }
            }
        }
    }
}
