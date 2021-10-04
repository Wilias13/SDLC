using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HelloApp
{
    class Program
    {
        class Person
        {
            public string Name { get; set; }
        }

        public static void WriteAndReadTextFile()
        {
            Console.WriteLine("\nВведите название файла (text): ");
            string path = Console.ReadLine();

            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            // запись в файл
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
            }

            // чтение из файла
            using (FileStream fstream = File.OpenRead(path))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }

            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                fileInf.Delete();
                // альтернатива с помощью класса File
                // File.Delete(path);
            }
        }

        public static async Task CreateAndWriteAndReadAndDeleteJson()
        {
            // сохранение данных
            Console.WriteLine("\nВведите название файла для создания json файла: ");
            string path = Console.ReadLine();

            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            using (FileStream fs = new FileStream(path + ".json", FileMode.OpenOrCreate))
            {
                Person tom = new Person() { Name = text };
                await JsonSerializer.SerializeAsync<Person>(fs, tom);
            }

            // чтение данных
            using (FileStream fs = new FileStream(path + ".json", FileMode.OpenOrCreate))
            {
                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
                Console.WriteLine($"Текст из файла : {restoredPerson.Name}");
            }
            File.Delete(path+".json");
        }

        public static void CreateAndWriteXML()
        {
            XDocument xdoc = new XDocument();
            Console.WriteLine("\nВведите название файла для создания xml файла: ");
            string path = Console.ReadLine();
            // создаем первый элемент
            XElement Laba_1 = new XElement(path+".xml");
            // создаем атрибут
            XAttribute NameAttr = new XAttribute("name", "Andrew");
            XElement familiElem = new XElement("famili", "Nikiforov");
            XElement groupElem = new XElement("group", "BSBO-01-18");
            // добавляем атрибут и элементы в первый элемент
            Laba_1.Add(NameAttr);
            Laba_1.Add(familiElem);
            Laba_1.Add(groupElem);

            // добавляем корневой элемент в документ
            xdoc.Add(Laba_1);
            //сохраняем документ
            xdoc.Save(path+".xml");

            var doc = new XmlDocument();
            // Загружаем данные из файла.
            doc.Load(path + ".xml");
            // Получаем корневой элемент документа.
            var root = doc.DocumentElement;
            // Используем метод для рекурсивного обхода документа.
            PrintItem(root);
            File.Delete(path+".xml");
        }

        /// <param name="item"> Элемент Xml. </param>
        /// <param name="indent"> Количество отступов от начала строки. </param>
        private static void PrintItem(XmlElement item, int indent = 0)
        {
            // Выводим имя самого элемента.
            // new string('\t', indent) - создает строку состоящую из indent табов.
            // Это нужно для смещения вправо.
            // Пробел справа нужен чтобы атрибуты не прилипали к имени.
            Console.Write($"{new string('\t', indent)}{item.LocalName} ");

            // Если у элемента есть атрибуты, 
            // то выводим их поочередно, каждый в квадратных скобках.
            foreach (XmlAttribute attr in item.Attributes)
            {
                Console.Write($"[{attr.InnerText}]");
            }

            // Если у элемента есть зависимые элементы, то выводим.
            foreach (var child in item.ChildNodes)
            {
                if (child is XmlElement node)
                {
                    // Если зависимый элемент тоже элемент,
                    // то переходим на новую строку 
                    // и рекурсивно вызываем метод.
                    // Следующий элемент будет смещен на один отступ вправо.
                    Console.WriteLine();
                    PrintItem(node, indent + 1);
                }

                if (child is XmlText text)
                {
                    // Если зависимый элемент текст,
                    // то выводим его через тире.
                    Console.Write($"- {text.InnerText}");
                }
            }
        }

        public static void achive()
        {
            // путь к создаваемому архиву
            const string archivePath = @"C:\Users\nikia\Desktop\ConsoleApp2\ConsoleApp2\bin\Debug\archive.zip";

            // создание нового архива
            using (ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                // путь к добавляемому файлу
                const string pathFileToAdd = @"C:\test\test.txt";
                // имя добавляемого файла
                const string nameFileToAdd = "test.txt";

                // вызов метода для добавления файла в архив
                zipArchive.CreateEntryFromFile(pathFileToAdd, nameFileToAdd);
            }

            // открытие архива в режиме чтения
            using (ZipArchive zipArchive = ZipFile.OpenRead(archivePath))
            {
                // имя извлекаемого файла
                const string nameExtractFile = "test.txt";
                // путь, куда необходимо извлечь файл
                const string pathExtractFile = @"C:\Users\nikia\Desktop\ConsoleApp2\ConsoleApp2\bin\Debug\test.txt";

                // поиск необходимого файла в архиве
                // если он есть, то будет вызван метод, который его извлечёт
                zipArchive.Entries.FirstOrDefault(x => x.Name == nameExtractFile)?.
                    ExtractToFile(pathExtractFile);
            }
        }

        static void Main(string[] args)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                }
                Console.WriteLine();
            }

            WriteAndReadTextFile(); // Запись в файл информации и последующее чтение
            CreateAndWriteAndReadAndDeleteJson(); // Создание, запись и чтение информации и удаление json файла
            CreateAndWriteXML(); // Создание и запись в xml файл
            achive();
        }


    }
}