using System;
using System.Text;
using System.IO;

namespace ConsoleApp1
{
    class WorkWithFile
    {
        public static void Run()
        {
            FileInfo FI = new("temp.txt");
            Console.WriteLine("\n  Работа с файлом\nНапишите любой текст:");
            CreateFile(FI);
            ReadFile(FI);

            Console.WriteLine("\n  Работа с ZIP");
            WorkWithZIP.Run("temp.txt");

            FI.Delete();
        }
        public static void CreateFile(FileInfo fileInfo)
        {
            using FileStream FS = fileInfo.Create();
            byte[] array = Encoding.Default.GetBytes(Console.ReadLine());
            FS.Write(array, 0, array.Length);
            Console.WriteLine("Текст записан в файл {0}", fileInfo.Name);
        }
        public static void ReadFile(FileInfo fileInfo)
        {
            using FileStream FS = fileInfo.OpenRead();
            byte[] array = new byte[FS.Length];
            FS.Read(array, 0, array.Length);
            Console.WriteLine("Чтение из файла {0}...", fileInfo.Name);
            Console.WriteLine($"Полученный текст: {Encoding.Default.GetString(array)}");
        }
    }
}
