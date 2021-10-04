using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latypov2
{
    class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 26, a => {// работающий в параллельном режиме цикл, перебор значений а от 0 до 26, тем самым перебирается весь алфавит
                byte[] password = new byte[5];//в этом массиве будут перебираться пароли
                byte[] hash;//переменная, хранящая хешированный пароль для его сравнения с заданными
                byte[] one = StringHashToByteArray("1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad");
                byte[] two = StringHashToByteArray("3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b");
                byte[] three = StringHashToByteArray("74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
                password[0] = (byte)(97 + a);// первый символ из пароля, перебирается от а до z
                var sha = System.Security.Cryptography.SHA256.Create();// создаёт экземпляр класса SHA256
                for (password[1] = 97; password[1] < 123; password[1]++)//полный перебор пятисимвольного пароля
                    for (password[2] = 97; password[2] < 123; password[2]++)
                        for (password[3] = 97; password[3] < 123; password[3]++)
                            for (password[4] = 97; password[4] < 123; password[4]++)
                            {
                                hash = sha.ComputeHash(password);// хэширование пароля для дальнейшего сравнения с заданными хешами
                                if (matches(one, hash) || matches(two, hash) || matches(three, hash))
                                    Console.WriteLine(Encoding.ASCII.GetString(password) + " == " + BitConverter.ToString(hash).ToLower().Replace("-", ""));
                            }
            });
        }
        static byte[] StringHashToByteArray(string s)//перевод заданной строки с хэшем в массив байтов
        {
            return Enumerable.Range(0, s.Length / 2).Select(i => (byte)Convert.ToInt16(s.Substring(i * 2, 2), 16)).ToArray();
        }
        static bool matches(byte[] a, byte[] b)// проверяет полное соответствие заданного и подобранного нами хэша
        {
            for (int i = 0; i < 32; i++)
                if (a[i] != b[i])
                    return false;
            return true;
            
        }
    }
}
