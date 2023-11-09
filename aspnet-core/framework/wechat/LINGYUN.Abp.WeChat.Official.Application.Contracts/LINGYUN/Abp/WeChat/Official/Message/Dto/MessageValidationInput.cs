using LINGYUN.Abp.WeChat.Official.Models;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Message;
public class MessageValidationInput : WeChatMessage
{
    /// <summary>
    /// 加密的字符串。需要解密得到消息内容明文，解密后有random、msg_len、msg、receiveid四个字段，其中msg即为消息内容明文
    /// </summary>
    [JsonPropertyName("echostr")]
    public string EchoStr { get; set; }
}
