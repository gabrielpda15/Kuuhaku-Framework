using KuuhakuFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.Security.Cryptography
{
    public static class Phraser
    {
        public static byte[] GenerateBytePhrase(int length = 16, string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmnnOoPpQqRrSsTtUuVvWwXxYyZz0123456789")
        {
            var phrase = GeneratePhrase(length, chars);
            return GenerateHash(phrase);
        }

        public static string GeneratePhrase(int length = 16, string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmnnOoPpQqRrSsTtUuVvWwXxYyZz0123456789")
        {
            var rnd = new Random(new Random().Next());
            var temp = "";

            for (int i = 0; i < length; i++)
            {
                temp += chars[rnd.Next(chars.Length)].ToString();
            }

            return temp;
        }

        public static byte[] GenerateHash(byte[] value)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                return hashProvider.ComputeHash(value);
            }
        }

        public static byte[] GenerateHash(string value)
        {
            return GenerateHash(value.FromUTF8());
        }
    }
}
