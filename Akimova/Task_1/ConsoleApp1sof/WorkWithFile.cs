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
            char a;
            string path = "temp.txt";
            do
            {
                Console.WriteLine("\n1) Create file and write string in it\n2) Read file\n3) Deleted file \n4) Main menu");
                a = Console.ReadKey().KeyChar;
                switch (a)
                {
                    case '1':
                        CreateFile(path);
                        break;
                    case '2':
                        ReadFile(path);
                        break;
                    case '3':
                        FileInfo FI = new FileInfo(path);
                        FI.Delete();
                        break;
                    case '4':
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            while (a != '4');
        }
        public void CreateFile(string path)
        {
            FileInfo FI = new FileInfo(path);
            Console.WriteLine("\nWrite string:");
            using (FileStream FS = FI.Create())
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(Console.ReadLine());
                FS.Write(array, 0, array.Length);
            }
        }
        public void ReadFile(string path)
        {
            FileInfo FI = new FileInfo(path);
            using (FileStream FS = FI.OpenRead())
            {
                byte[] array = new byte[FS.Length];
                FS.Read(array, 0, array.Length);
                Console.WriteLine($"\nString from file: {System.Text.Encoding.Default.GetString(array)}");
            }
        }
    }
}
