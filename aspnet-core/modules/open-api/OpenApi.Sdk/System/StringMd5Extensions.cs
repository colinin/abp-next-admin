using System.Security.Cryptography;
using System.Text;

namespace System
{
    internal static class StringMd5Extensions
    {
        public static string ToMd5(this string str)
        {
            using (MD5 mD = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] array = mD.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    stringBuilder.Append(b.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
