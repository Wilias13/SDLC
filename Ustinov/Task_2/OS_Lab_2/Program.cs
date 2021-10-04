using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OS_Lab_2.Extensions;

namespace OS_Lab_2
{
    class Program
    {
        private static void Sha256BruteForce(
            string[] hashes,
            int numOfThreads)
        {
            List<byte[]> bytesList = hashes.Select(x => StringHashToByteArray(x)).ToList();
            Parallel.For(
                0,
                26,
                new ParallelOptions() {MaxDegreeOfParallelism = numOfThreads},
                i =>
            {
                byte[] password = new byte[5];
                byte[] hash;
                password[0] = (byte) (97 + i);
                var sha = System.Security.Cryptography.SHA256.Create();
                for (password[1] = 97; password[1] < 123; password[1]++)
                for (password[2] = 97; password[2] < 123; password[2]++)
                for (password[3] = 97; password[3] < 123; password[3]++)
                for (password[4] = 97; password[4] < 123; password[4]++)
                {
                    hash = sha.ComputeHash(password);
                    for (int j = 0; j < bytesList.Count; j++)
                    {
                        if (Matches(bytesList[j], hash))
                            Console.WriteLine(Encoding.ASCII.GetString(password) + " => "
                                + BitConverter.ToString(hash).ToLower()
                                    .Replace("-", ""));
                    }
                }
            });
        }
        
        static byte[] StringHashToByteArray(string s)
        {
            return Enumerable.Range(0, s.Length / 2).Select(i => (byte)Convert.ToInt16(s.Substring(i * 2, 2), 16)).ToArray();
        }
        static bool Matches(byte[] a, byte[] b)
        {
            for (int i = 0; i < 32; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }

        static string[] ReadHashesFromFile()
        {
            string pathToHashes = Path.Combine(Environment.CurrentDirectory,"hashes.txt");

            using (StreamReader sr = File.OpenText(pathToHashes))
            {
                return sr.ReadToEnd().Split("\n");
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Choose number of threads (1-10): ");
            if(!Int32.TryParse(Console.ReadLine(), out int numOfThreads)
               || numOfThreads <= 0
               || numOfThreads > 10)
            {
                Console.WriteLine("Wrong data!");
                return;
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var hashes = ReadHashesFromFile();
            Sha256BruteForce(hashes,numOfThreads);
            timer.Stop();
            
            Console.WriteLine("Elapsed time: {0}s",timer.Elapsed.TotalSeconds);
        }
    }
}