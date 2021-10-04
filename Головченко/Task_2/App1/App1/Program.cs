using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;

class Program
{
    class Person
    {
        public string HashKod { get; set; }
    }

    public static void achive()
        {
            // путь к создаваемому архиву
            const string archivePath = @"C:\Users\nikia\Desktop\lat\ConsoleApp1\ConsoleApp1\bin\Debug\netcoreapp3.1\archive.zip";

            // создание нового архива
            using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                // путь к добавляемому файлу
                const string pathFileToAdd = @"C:\Users\nikia\Desktop\lat\ConsoleApp1\ConsoleApp1\bin\Debug\netcoreapp3.1\Sample.txt";
                // имя добавляемого файла
                const string nameFileToAdd = "Sample.txt";

                // вызов метода для добавления файла в архив
                zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
            }
        }

    public static void PrintXML(string line)
    {
        XDocument xdoc = new XDocument();
        string path = "XML.xml";
        // создаем первый элемент
        XElement Laba_1 = new XElement(path);
        // создаем атрибут
        XAttribute NameAttr = new XAttribute("text", line);
        // добавляем атрибут и элементы в первый элемент
        Laba_1.Add(NameAttr);

        // добавляем корневой элемент в документ
        xdoc.Add(Laba_1);
        //сохраняем документ
        xdoc.Save(path);
    }

    static async Task WriteJson(string line)
    {
        using (FileStream fs = new FileStream("user.json", FileMode.Append))
        {
            Person name = new Person() { HashKod = line };
            await JsonSerializer.SerializeAsync<Person>(fs, name);
            //Console.WriteLine("Data has been saved to file");
        }
        
    }

    public static void WriteText(string line)
    {
        try
        {
            StreamWriter sw = new StreamWriter("Sample.txt", true);
            sw.WriteLine(line);
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    static void Main(string[] args)
    {
        string temp = "";
        Parallel.For(0, 26, a => {
            byte[] password = new byte[5];
            byte[] hash;
            byte[] one = StringHashToByteArray("1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad");
            byte[] two = StringHashToByteArray("3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b");
            byte[] three = StringHashToByteArray("74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
            password[0] = (byte)(97 + a);
            var sha = System.Security.Cryptography.SHA256.Create();
            for (password[1] = 97; password[1] < 123; password[1]++)
                for (password[2] = 97; password[2] < 123; password[2]++)
                    for (password[3] = 97; password[3] < 123; password[3]++)
                        for (password[4] = 97; password[4] < 123; password[4]++)
                        {
                            hash = sha.ComputeHash(password);
                            if (matches(one, hash) || matches(two, hash) || matches(three, hash))
                            {
                                Console.WriteLine(Encoding.ASCII.GetString(password) + " => "
                                       + BitConverter.ToString(hash).ToLower().Replace("-", ""));
                                WriteText(Encoding.ASCII.GetString(password) + " => "
        + BitConverter.ToString(hash).ToLower().Replace("-", ""));
                                temp = temp + " " + (Encoding.ASCII.GetString(password) + " => "
        + BitConverter.ToString(hash).ToLower().Replace("-", ""));
                            }
                        }
        });
        WriteJson(temp);
        PrintXML(temp);
        achive();
    }
    static byte[] StringHashToByteArray(string s)
    {
        return Enumerable.Range(0, s.Length / 2).Select(i => (byte)Convert.ToInt16(s.Substring(i * 2, 2), 16)).ToArray();
    }
    static bool matches(byte[] a, byte[] b)
    {
        for (int i = 0; i < 32; i++)
            if (a[i] != b[i])
                return false;
        return true;
    }
}