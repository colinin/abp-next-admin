using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
/// <summary>
/// 客户群opengid转换请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94822" />
/// </remarks>
public class WeChatWorkOpengIdToChatIdRequest : WeChatWorkRequest
{
    /// <summary>
    /// 小程序在微信获取到的群ID
    /// </summary>
    [NotNull]
    [JsonProperty("opengid")]
    [JsonPropertyName("opengid")]
    public string OpengId { get; }
    public WeChatWorkOpengIdToChatIdRequest(string opengId)
    {
        Check.NotNullOrWhiteSpace(opengId, nameof(opengId));

        OpengId = opengId;
    }
}
