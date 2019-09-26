using KuuhakuFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.Security.Cryptography
{
    public static class Cryptor
    {
        public static byte[] EncryptBytes(byte[] input, byte[] phrase)
        {
            using (var TDESAlgorithm = new TripleDESCryptoServiceProvider())
            {
                TDESAlgorithm.Key = phrase;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                using (var cryptor = TDESAlgorithm.CreateEncryptor())
                {
                    return cryptor.TransformFinalBlock(input, 0, input.Length);
                }
            }
        }

        public static byte[] DecryptBytes(byte[] input, byte[] phrase)
        {
            using (var TDESAlgorithm = new TripleDESCryptoServiceProvider())
            {
                TDESAlgorithm.Key = phrase;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                using (var cryptor = TDESAlgorithm.CreateDecryptor())
                {
                    return cryptor.TransformFinalBlock(input, 0, input.Length);
                }
            }
        }

        public static string Encrypt(string message, string phrase)
        {
            return EncryptBytes(message.FromUTF8(), Phraser.GenerateHash(phrase)).ToBase64();
        }

        public static string Decrypt(string message, string phrase)
        {
            return DecryptBytes(message.FromBase64(), Phraser.GenerateHash(phrase)).ToUTF8();
        }
    }
}
