using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.Extensions
{
    public static class StringExtension
    {
        public static bool IsNothing(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static string ToUTF8(this byte[] bytes)
        {
            return new UTF8Encoding().GetString(bytes);
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromUTF8(this string str)
        {
            return new UTF8Encoding().GetBytes(str);
        }

        public static byte[] FromBase64(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
