using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogicalInfo LI = new LogicalInfo();
            WorkWithFile WWF = new WorkWithFile();
            WorkWithXML WWXML = new WorkWithXML();
            WorkWithJson WWJ = new WorkWithJson();
            WorkWithZIP WWZIP = new WorkWithZIP();
            char a;
            do
            {
                Console.WriteLine("\n1) Info about disk \n2) Working with a file \n3) Working with JSON \n4) Working with XML \n5) Working with ZIP \n6) Exit");
                a = Console.ReadKey().KeyChar;
                switch (a)
                {
                    case '1':
                        LI.Run();
                        break;
                    case '2':
                        WWF.Run();
                        break;
                    case '3':
                        WWJ.Run();
                        break;
                    case '4':
                        WWXML.Run();
                        break;
                    case '5':
                        WWZIP.Run();
                        break;
                    case '6':
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
            while (a != '6');
        }
    }
}
