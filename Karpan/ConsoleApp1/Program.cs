using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogicalInfo.Run();
            WorkWithFile.Run();
            WorkWithXML.Run();
            WorkWithJson.Run();
        }
    }
}
