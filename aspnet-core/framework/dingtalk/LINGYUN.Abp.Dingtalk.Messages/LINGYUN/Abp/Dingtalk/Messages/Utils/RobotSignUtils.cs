using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LINGYUN.Abp.Dingtalk.Messages.Utils;

public static class RobotSignUtils
{
    /// <summary>
    /// 计算签名
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://open.dingtalk.com/document/dingstart/customize-robot-security-settings#"/>
    /// </remarks>
    /// <param name="secret"></param>
    /// <returns></returns>
    public static (long Timestamp, string Sign) CalculateSign(string secret)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var stringToSign = timestamp + "\n" + secret;
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var signData = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
        return (timestamp, HttpUtility.UrlEncode(Convert.ToBase64String(signData)));
    }
}
