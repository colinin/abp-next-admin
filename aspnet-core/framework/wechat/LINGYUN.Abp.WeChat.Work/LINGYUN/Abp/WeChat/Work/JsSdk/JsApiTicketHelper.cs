using System;
using System.Security.Cryptography;
using System.Text;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;
public static class JsApiTicketHelper
{
    private static string[] _randomChars = new string[]
    {
        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
    };

    public static string GenerateNonce()
    {
        var r = new Random();
        var sb = new StringBuilder();
        var length = _randomChars.Length;
        for (var i = 0; i < 15; i++)
        {
            sb.Append(_randomChars[r.Next(length - 1)]);
        }
        return sb.ToString();
    }

    public static long GenerateTimestamp()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
    }

    private static string ToSha1(string str)
    {
        using (var sha = SHA1.Create())
        {
            var data = sha.ComputeHash(Encoding.UTF8.GetBytes(str));

            var sb = new StringBuilder();
            foreach (var d in data)
            {
                sb.Append(d.ToString("x2"));
            }
            return sb.ToString();
        }
    }
    /// <summary>
    /// 生成JS-SDK签名
    /// See: https://developer.work.weixin.qq.com/document/path/90506
    /// </summary>
    /// <param name="jsapiTicket"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GenerateSignature(
        string jsapiTicket, 
        string nonce,
        string timestamp,
        string url)
    {
        var sb = new StringBuilder();
        sb.Append("jsapi_ticket=").Append(jsapiTicket).Append("&")
          .Append("noncestr=").Append(nonce).Append("&")
          .Append("timestamp=").Append(timestamp).Append("&")
          .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
        return ToSha1(sb.ToString());
    }
}
