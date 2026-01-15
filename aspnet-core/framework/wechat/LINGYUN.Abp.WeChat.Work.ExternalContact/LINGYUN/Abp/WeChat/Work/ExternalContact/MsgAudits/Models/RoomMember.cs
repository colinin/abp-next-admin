using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Models;
/// <summary>
/// 群聊成员
/// </summary>
public class RoomMember
{
    /// <summary>
    /// roomid群成员的id，userid
    /// </summary>
    [NotNull]
    [JsonProperty("memberid")]
    [JsonPropertyName("memberid")]
    public string MemberId { get; set; }
    /// <summary>
    /// roomid群成员的入群时间
    /// </summary>
    [NotNull]
    [JsonProperty("jointime")]
    [JsonPropertyName("jointime")]
    public long JoinTime { get; set; }
}
