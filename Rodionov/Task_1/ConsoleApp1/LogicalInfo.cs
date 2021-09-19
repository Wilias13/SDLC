using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class LogicalInfo
    {
        public void Run()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Диск {0}", d.Name);
                Console.WriteLine("  Тип: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Метка: {0}", d.VolumeLabel);
                    Console.WriteLine("  Файловая система: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Размер диска:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }
    }
}
