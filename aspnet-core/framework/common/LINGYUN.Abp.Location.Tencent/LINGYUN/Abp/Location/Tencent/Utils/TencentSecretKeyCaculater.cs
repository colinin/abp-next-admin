using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.Location.Tencent.Utils
{
    public class TencentSecretKeyCaculater
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

            StringBuilder sb = new StringBuilder();
            foreach (var item in querystring_arrays)
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(item.Value);
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static string CalcSecretKey(string url, string secretKey, IDictionary<string, string> querystring_arrays)
        {
            var queryString = HttpBuildQuery(querystring_arrays);

            return MD5(url + "?" + queryString + secretKey);
        }
    }
}
