using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Models;
public class WeChatWorkMessage
{
    /// <summary>
    /// 企业微信加密签名，
    /// msg_signature计算结合了企业填写的token、请求中的timestamp、nonce、加密的消息体
    /// </summary>
    /// <remarks>
    /// 签名计算方法参考: https://developer.work.weixin.qq.com/document/path/90930#12976/%E6%B6%88%E6%81%AF%E4%BD%93%E7%AD%BE%E5%90%8D%E6%A0%A1%E9%AA%8C
    /// </remarks>
    [JsonPropertyName("msg_signature")]
    public string Msg_Signature { get; set; }
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
