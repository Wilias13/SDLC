using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogicalInfo LI = new LogicalInfo();
            LI.Run();
            WorkWithFile WWF = new WorkWithFile();
            WWF.Run();
            WorkWithXML WWXML = new WorkWithXML();
            WWXML.Run();
            WorkWithJson WWJ = new WorkWithJson();
            WWJ.Run();
        }
    }
}
