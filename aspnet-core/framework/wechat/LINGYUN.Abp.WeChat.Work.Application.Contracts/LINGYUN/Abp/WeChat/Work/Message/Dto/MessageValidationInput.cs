using LINGYUN.Abp.WeChat.Work.Models;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class MessageValidationInput : WeChatWorkMessage
{
    /// <summary>
    /// 加密的字符串。需要解密得到消息内容明文，解密后有random、msg_len、msg、receiveid四个字段，其中msg即为消息内容明文
    /// </summary>
    [JsonPropertyName("echostr")]
    public string EchoStr { get; set; }
}
