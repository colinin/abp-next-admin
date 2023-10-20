using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.Location.Baidu.Utils
{
    public class BaiduAKSNCaculater
    {
        private static string MD5(string password)
        {
            try
            {
                System.Security.Cryptography.HashAlgorithm hash = System.Security.Cryptography.MD5.Create();
                byte[] hash_out = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var md5_str = BitConverter.ToString(hash_out).Replace("-", "");
                return md5_str.ToLower();
            }
            catch
            {
                throw;
            }
        }

        private static string HttpBuildQuery(IDictionary<string, string> querystring_arrays)
        {
            List<string> list = new List<string>(querystring_arrays.Count);
            foreach (var item in querystring_arrays)
            {
                list.Add($"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value)}");
            }

            return string.Join("&", list);
        }

        public static string CaculateAKSN(string sk, string url, IDictionary<string, string> querystring_arrays)
        {
            var queryString = HttpBuildQuery(querystring_arrays);

            var str = Uri.EscapeDataString(url + "?" + queryString + sk);

            return MD5(str);
        }
    }
}
