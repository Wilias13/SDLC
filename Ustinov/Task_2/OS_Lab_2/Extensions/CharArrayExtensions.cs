using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OS_Lab_2.Extensions
{
    public static class CharArrayExtensions
    {
        private static readonly SHA256 Sha256 = SHA256.Create();

        public static void Next(this char[] source, char[] allowedSymbols)
        {
            for (int end = source.Length - 1; end >= 0; end--)
            {
                if (source[end] == allowedSymbols.Last())
                    source[end] = allowedSymbols[0];
                else
                {
                    int index = Array.LastIndexOf(allowedSymbols, source[end]);
                    source[end] = allowedSymbols[index + 1];
                    break;
                }
            }
        }
        
        public static string ComputeSha256Hash(this char[] source) 
        {
            Encoding enc = Encoding.ASCII;
            byte[] result = Sha256.ComputeHash(enc.GetBytes(source));
            return Convert.ToHexString(result).ToLower();
        }

        public static int CountLoopIterations(this char[] source, int start, int end)
        {
            int sum = 0,
                len = source.Length;
            for (; end >= start; end--)
            {
                sum += (int) Math.Pow(len, end);
            }

            return sum;
        }
    }
}