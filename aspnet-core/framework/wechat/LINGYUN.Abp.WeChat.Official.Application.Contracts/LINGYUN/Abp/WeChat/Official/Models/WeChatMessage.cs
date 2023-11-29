using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Models;
public class WeChatMessage
{
    /// <summary>
    /// 微信加密签名，
    /// signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数
    /// </summary>
    /// <remarks>
    /// 签名计算方法参考: https://developers.weixin.qq.com/doc/oplatform/Third-party_Platforms/2.0/api/Before_Develop/Message_encryption_and_decryption.html
    /// </remarks>
    [JsonPropertyName("signature")]
    public string Signature { get; set; }
    /// <summary>
    /// 时间戳。与nonce结合使用，用于防止请求重放攻击。
    /// </summary>
    [JsonPropertyName("timestamp")]
    public int TimeStamp { get; set; }
    /// <summary>
    /// 随机数。与timestamp结合使用，用于防止请求重放攻击。
    /// </summary>
    [JsonPropertyName("nonce")]
    public string Nonce { get; set; }
}
