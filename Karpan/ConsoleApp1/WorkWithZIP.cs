using System;
using System.IO;
using System.IO.Compression;

namespace ConsoleApp1
{
    class WorkWithZIP
    {
        public static void Run(string path)
        {
            string sourceFile = path;
            string compressedFile = "temp.gz";
            string targetFile = "temp_new.txt";

            Compress(sourceFile, compressedFile);
            Decompress(compressedFile, targetFile);

            FileInfo FI = new(targetFile);
            WorkWithFile.ReadFile(FI);
            FI.Delete();

            FileInfo FD = new(compressedFile);
            FD.Delete();
        }
        public static void Compress(string sourceFile, string compressedFile)
        {
            using FileStream sourceStream = new(sourceFile, FileMode.OpenOrCreate);
            using FileStream targetStream = File.Create(compressedFile);
            using GZipStream compressionStream = new(targetStream, CompressionMode.Compress);
            sourceStream.CopyTo(compressionStream);
            Console.WriteLine("Сжатие файла {0} завершено. \nИсходный размер: {1}.  \nCжатый размер: {2}.",
                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            using FileStream sourceStream = new(compressedFile, FileMode.OpenOrCreate);
            using FileStream targetStream = File.Create(targetFile);
            using GZipStream decompressionStream = new(sourceStream, CompressionMode.Decompress);
            decompressionStream.CopyTo(targetStream);
            Console.WriteLine("Восстановленый файл: {0}", targetFile);
        }
    }
}
