using System;
using System.IO;

namespace ConsoleApp1
{
    class LogicalInfo
    {
        public static void Run()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            for (int i = 0; i < allDrives.Length; i++)
            {
                DriveInfo drivers = allDrives[i];
                Console.WriteLine("Диск {0}", drivers.Name);
                Console.WriteLine("  Тип: \t\t\t\t {0}", drivers.DriveType);
                if (drivers.IsReady == true)
                {
                    Console.WriteLine("  Метка: \t\t\t {0}", drivers.VolumeLabel);
                    Console.WriteLine("  Файловая система: \t\t {0}", drivers.DriveFormat);
                    Console.WriteLine("  Размер диска: \t {0, 15} bytes ", drivers.TotalSize);
                }
            }
        }
    }
}
