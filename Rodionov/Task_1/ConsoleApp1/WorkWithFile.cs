using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class WorkWithFile
    {
        public void Run()
        {
            FileInfo FI = new FileInfo("temp.txt");
            Console.WriteLine("\nРабота с файлом\nНапишите любой текст:");
            CreateFile(FI);
            ReadFile(FI);

            Console.WriteLine("\nРабота с ZIP");
            WorkWithZIP WWZIP = new WorkWithZIP();
            WWZIP.Run("temp.txt");

            FI.Delete();
        }
        public void CreateFile(FileInfo fileInfo)
        {
            using (FileStream FS = fileInfo.Create())
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(Console.ReadLine());
                FS.Write(array, 0, array.Length);
            }
        }
        public void ReadFile(FileInfo fileInfo)
        {
            using (FileStream FS = fileInfo.OpenRead())
            {
                byte[] array = new byte[FS.Length];
                FS.Read(array, 0, array.Length);
                Console.WriteLine($"Полученный текст: {System.Text.Encoding.Default.GetString(array)}");
            }
        }
    }
}
