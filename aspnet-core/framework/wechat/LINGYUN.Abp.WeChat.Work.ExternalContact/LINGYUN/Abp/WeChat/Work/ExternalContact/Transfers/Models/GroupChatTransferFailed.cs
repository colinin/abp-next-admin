using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Models;
/// <summary>
/// 没能成功继承的群详情
/// </summary>
public class GroupChatTransferFailed
{
    /// <summary>
    /// 没能成功继承的群ID
    /// </summary>
    [NotNull]
    [JsonProperty("chat_id")]
    [JsonPropertyName("chat_id")]
    public string ChatId { get; set; }
    /// <summary>
    /// 没能成功继承的群，错误码
    /// </summary>
    [NotNull]
    [JsonProperty("errcode")]
    [JsonPropertyName("errcode")]
    public int ErrCode { get; set; }
    /// <summary>
    /// 没能成功继承的群，错误描述
    /// </summary>
    [NotNull]
    [JsonProperty("errmsg")]
    [JsonPropertyName("errmsg")]
    public string ErrMsg { get; set; }
}
