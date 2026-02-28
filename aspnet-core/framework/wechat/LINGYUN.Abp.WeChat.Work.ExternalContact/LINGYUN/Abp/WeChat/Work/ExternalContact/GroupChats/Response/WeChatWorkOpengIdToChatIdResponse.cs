using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Response;
/// <summary>
/// 客户群opengid转换响应结果
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94822" />
/// </remarks>
public class WeChatWorkOpengIdToChatIdResponse : WeChatWorkResponse
{
    /// <summary>
    /// 客户群ID，可以用来调用获取客户群详情
    /// </summary>
    [NotNull]
    [JsonProperty("chat_id")]
    [JsonPropertyName("chat_id")]
    public string ChatId { get; set; }
}
