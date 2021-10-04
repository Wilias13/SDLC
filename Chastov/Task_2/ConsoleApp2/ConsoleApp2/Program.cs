using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;

class Program
{
    static object file = new object();
    public static void WriteJSON(string file, string answer, string hash)
    {
        Dictionary<string, string> pred;
        if (File.Exists(file))
        {
            string rtext = File.ReadAllText(file);
            if (rtext!="")
                pred = JsonSerializer.Deserialize<Dictionary<string, string>>(Encoding.ASCII.GetBytes(rtext));
            else
                pred = new Dictionary<string, string>();
        }
        else
        {
            pred = new Dictionary<string, string>();
        }
        pred.TryAdd(hash,answer);
        string jsonString = JsonSerializer.Serialize(pred);
        File.WriteAllText(file, jsonString);
        pred.Clear();
    }
    public static void WriteXML(string file, string answer, string hash)
    {
        const string NameMain = "answers";
        XDocument xdoc=null; XElement main = null;
        if (File.Exists(file))
        {
            xdoc = XDocument.Load(file);
            foreach (var it in xdoc.Document.Elements())
                if (it.Name.ToString() == NameMain)
                {
                    //xdoc.AddAfterSelf(str);
                    main = it;
                    break;
                }
            xdoc = null;
            File.Delete(file);
        }
        if (xdoc == null)
        {
            xdoc = new XDocument();
            if (main==null) main = new XElement(NameMain);
        }
        main.Add(new XElement("hash_" + hash, answer));
        xdoc.Add(main);
        xdoc.Save(file);

    }

    public static void WriteZIP(string file, string answer, string hash)
    {
        string fileName = file;
        const string dir = "temp"; string fileFullName = dir + "\\" + fileName;
        string archivePath = dir + ".zip";
        if (File.Exists(archivePath)) File.Delete(archivePath);
        Directory.CreateDirectory(dir);
        if (Directory.Exists(dir))
        {
            
            string tempFile =  fileFullName + ".temp";
            string previousData = "";
            if (File.Exists(fileName)) previousData = File.ReadAllText(fileName);
            File.WriteAllText(fileFullName, previousData + hash + " - " + answer + "\r\n");
            File.WriteAllText(fileName, previousData + hash + " - " + answer + "\r\n");

            ZipFile.CreateFromDirectory(dir, archivePath);
            using (ZipArchive zipArchive = ZipFile.OpenRead(archivePath))
            {
                // имя извлекаемого файла
                string nameExtractFile = fileName;

                // поиск необходимого файла в архиве
                // если он есть, то будет вызван метод, который его извлечёт
                zipArchive.Entries.FirstOrDefault(x => x.Name == nameExtractFile)?.
                    ExtractToFile(tempFile);
            }
            //outputData(tempFile);
            //FileInfo zip = new FileInfo(archivePath);
            //Console.WriteLine("Length: " + zip.Length + " bytes; creation time: " + zip.CreationTime);
            //File.Delete(archivePath);
            Directory.Delete(dir, true);
        }
    }
    static void Main(string[] args)
    {
        string[] hashes = new string[]{("1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad"), ("3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b"),
            ("74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f")};
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < 3; i++)
        {
            int c = i;
            Task task = new Task(() => { Brute(hashes[c]); });
            task.Start();
            tasks.Add(task);
        }
        for (int i=0; i<tasks.Count; i++)
        {
            tasks[i].Wait();
        }

        Console.Write("end");
    }

    static void Brute(string shash)
    {
        string result = "";
        byte[] password = new byte[5];
        byte[] hash;
        byte[] one = StringHashToByteArray(shash);
        for (int i=0; i<26; i++)
            {
                
                password[0] = (byte)(97 + i);
                var sha = System.Security.Cryptography.SHA256.Create();
                for (password[1] = 97; password[1] < 123; password[1]++)
                    for (password[2] = 97; password[2] < 123; password[2]++)
                        for (password[3] = 97; password[3] < 123; password[3]++)
                            for (password[4] = 97; password[4] < 123; password[4]++)
                            {
                                hash = sha.ComputeHash(password);
                                if (matches(one, hash))
                                {
                                    result += Encoding.ASCII.GetString(password);
                                }
                            }
                
            }//);
        lock (file)
        {
            Console.WriteLine(result);
            WriteJSON("text.json", result, shash);
            WriteXML("text.xml", result, shash);
            WriteZIP("text.txt", result, shash);
        }
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