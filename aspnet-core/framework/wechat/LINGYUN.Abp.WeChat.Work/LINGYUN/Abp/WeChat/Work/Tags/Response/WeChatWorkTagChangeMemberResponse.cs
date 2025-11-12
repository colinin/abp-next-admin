using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Tags.Response;
/// <summary>
/// 标签成员变更响应参数
/// </summary>
public class WeChatWorkTagChangeMemberResponse : WeChatWorkResponse
{
    /// <summary>
    /// 若部分userid非法，则返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("invalidlist")]
    [JsonPropertyName("invalidlist")]
    public string InvalidList { get; set; }
    /// <summary>
    /// 若部分partylist非法，则返回
    /// </summary>
    [CanBeNull]
    [JsonProperty("invalidparty")]
    [JsonPropertyName("invalidparty")]
    public List<int> InvalidPart { get; set; }
}
