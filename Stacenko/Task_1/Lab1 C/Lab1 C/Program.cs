using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
namespace Lab1_C
{
    class Program
    { 
        static void Main()
        {
            try
            {
                Console.WriteLine("Выберите номер задания:\n1. Файловая система.\n2. Работа с файлом.\n3. Работа с JSON\n4. Работа с XML\n5. Создание архива\n0. Выход из программы\n");
                var value = Convert.ToInt32(Console.ReadLine());
                switch (value)
                {
                    case 0:
                        Process.GetCurrentProcess().Kill();
                        break;
                    case 1:
                        first();
                        break;
                    case 2:
                        file();
                        break;
                    case 3:
                        json();
                        break;
                    case 5:
                        zip();
                        break;

                   default:
                        Console.WriteLine("Неверно введено значение");
                        Main();
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Вы ввели не цифру");
                Main();
            }
        
   
        
            void first()
            {
                var driver = DriveInfo.GetDrives();
                Console.WriteLine("Количество дисков: " + DriveInfo.GetDrives().Length);
                Console.WriteLine("Выберете диск:");

                string info = "";
                int i = 0;
                foreach (var drive in DriveInfo.GetDrives())
                {
                    i++;
                    Console.WriteLine(i + ") " + drive.Name);
                }
                Console.WriteLine("Чтобы выйти в главное меню нажмите 0");
                try
                {
                    int value = Convert.ToInt32(Console.ReadLine());
                    if (value <= DriveInfo.GetDrives().Length)
                    {
                        Disc(driver, value);
                    }
                    else
                    {
                        Console.WriteLine("Такого диска не существует");
                    }
                }

                catch
                {
                    Console.WriteLine("Вы ввели не цифру");
                }

            }

            void Disc(DriveInfo[] driver, int value)
            {   
                if(value == 0)
                {
                    Main();
                }
                var count = 0;
                foreach (var disk in driver)
                {
                    count++;
                    if (count == value)
                    {
                        Console.WriteLine("Имя диска: " + disk.Name);
                        Console.WriteLine("Файловая система: " + disk.DriveFormat);
                        Console.WriteLine("Тип диска: " + disk.DriveType);
                        Console.WriteLine("Объем доступного свободного места (в байтах): " + disk.AvailableFreeSpace);
                        Console.WriteLine("Готов ли диск: " + disk.IsReady);
                        Console.WriteLine("Корневой каталог диска: " + disk.RootDirectory);
                        Console.WriteLine("Общий объем свободного места, доступного на диске (в байтах): " + disk.TotalFreeSpace);
                        Console.WriteLine("Размер диска (в байтах): " + disk.TotalSize);
                        Console.WriteLine("Метка тома диска: " + disk.VolumeLabel);
                        Console.WriteLine("Чтобы вернутся назад нажмите 0");
                    }
                }
                int x = Convert.ToInt32(Console.ReadLine()); 
                if(x == 0)
                {
                    first();
                }
            }
            void file()
            {
                Console.WriteLine("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь");
                string value = Console.ReadLine();
                Console.WriteLine("Введите текст");
                string info = Console.ReadLine();

                using (StreamWriter w = File.AppendText(value))
                {
                    w.WriteLine(info);
                    w.Close();
                }
                using (StreamReader r= new StreamReader(value))
                {
                    string loop = "";
                    Console.WriteLine("Данные из файла:\n" );
                    while (!r.EndOfStream)
                    {
                        loop = r.ReadLine();
                        Console.WriteLine(loop);
                    }
                    r.Close();
                }
                Console.WriteLine("Чтобы удалить файл: 1\nДля выхода в главное меню: 0");
                try
                {
                    int choose = Convert.ToInt32(Console.ReadLine());
                    switch (choose)
                    {
                        case 1:
                            System.IO.File.Delete(value);
                            Console.WriteLine($"Файл {value} удален");
                            Main();
                            break;
                        case 0:
                            Main();
                            break;
                        default:
                            Console.WriteLine("Такого выбора нет");
                            Main();
                            break;

                    }
                }
                catch
                {
                    Console.WriteLine("Вы ввели не число");
                    Main();
                } 
            }

            void json()
            {
                Console.WriteLine("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь");
                string value = Console.ReadLine();
                Console.WriteLine("Введите текст");
                string info = Console.ReadLine()+",";
                File.AppendAllText(value, info);
                
                using (StreamReader r = new StreamReader(value))
                {
                    string loop = "";
                    Console.WriteLine("Данные из файла:\n");
                    while (!r.EndOfStream)
                    {
                        loop = r.ReadLine();
                        Console.WriteLine(loop);
                    }
                    r.Close();
                }
                Console.WriteLine("Чтобы удалить файл: 1\nДля выхода в главное меню: 0");
                try
                {
                    int choose = Convert.ToInt32(Console.ReadLine());
                    switch (choose)
                    {
                        case 1:
                            System.IO.File.Delete(value);
                            Console.WriteLine($"Файл {value} удален");
                            Main();
                            break;
                        case 0:
                            Main();
                            break;
                        default:
                            Console.WriteLine("Такого выбора нет");
                            Main();
                            break;

                    }
                }
                catch
                {
                    Console.WriteLine("Вы ввели не число");
                    Main();
                }
            }
            void zip()
            {
                Console.WriteLine("Напишите название архива, если хотите чтобы он остался в этой же директории или укажите полный путь");
                string value = Console.ReadLine();
                Console.WriteLine("Напишите название файла, если хотите чтобы он остался в этой же директории или укажите полный путь");
                string file = Console.ReadLine();
                Console.WriteLine("Укажите куда поместить разархевированный файл");
                string info = Console.ReadLine();
                Compress(file, value);
                Decompress(value, info);
                
                Console.WriteLine("Чтобы удалить файлы: 1\nДля выхода в главное меню: 0");
                try
                {
                    int choose = Convert.ToInt32(Console.ReadLine());
                    switch (choose)
                    {
                        case 1:
                            System.IO.File.Delete(value);
                            System.IO.File.Delete(file);
                            System.IO.File.Delete(info);
                            Console.WriteLine($"Файлы удалены");
                            Main();
                            break;
                        case 0:
                            Main();
                            break;
                        default:
                            Console.WriteLine("Такого выбора нет");
                            Main();
                            break;

                    }
                }
                catch
                {
                    Console.WriteLine("Вы ввели не число");
                    Main();
                }
            }
            void Compress(string sourceFile, string compressedFile)
            {
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                        }
                    }
                }
            }
            void Decompress(string compressedFile, string targetFile)
            {
                // поток для чтения из сжатого файла
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                            Console.WriteLine("Восстановлен файл: {0}", targetFile);
                        }
                    }
                }
            }
        }
    }
}


