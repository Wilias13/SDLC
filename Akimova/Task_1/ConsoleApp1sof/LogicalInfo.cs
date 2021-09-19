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
                Console.WriteLine("\nDevice {0}", d.Name);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Type: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Total:           {0, 15} bytes ",
                        d.TotalSize);
                    Console.WriteLine(
                        "  Free:            {0, 15} bytes ",
                        d.TotalFreeSpace);
                    Console.WriteLine(
                        "  Used:            {0, 15} bytes ",
                        d.TotalSize-d.TotalFreeSpace);
                }
            }
        }
    }
}
