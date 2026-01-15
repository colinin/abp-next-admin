using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
/// <summary>
/// 获取会话内容存档内部群信息响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91782" />
/// </remarks>
public class WeChatWorkGetGroupChatResponse : WeChatWorkResponse
{
    /// <summary>
    /// roomid对应的群名称
    /// </summary>
    [NotNull]
    [JsonProperty("roomname")]
    [JsonPropertyName("roomname")]
    public string RoomName { get; set; }
    /// <summary>
    /// roomid对应的群创建者，userid
    /// </summary>
    [NotNull]
    [JsonProperty("creator")]
    [JsonPropertyName("creator")]
    public string Creator { get; set; }
    /// <summary>
    /// roomid对应的群创建时间
    /// </summary>
    [NotNull]
    [JsonProperty("room_create_time")]
    [JsonPropertyName("room_create_time")]
    public long RoomCreateTime { get; set; }
    /// <summary>
    /// roomid对应的群公告
    /// </summary>
    [CanBeNull]
    [JsonProperty("notice")]
    [JsonPropertyName("notice")]
    public string? Notice { get; set; }
    /// <summary>
    /// roomid对应的群成员列表
    /// </summary>
    [NotNull]
    [JsonProperty("members")]
    [JsonPropertyName("members")]
    public RoomMember[] Members { get; set; }
}
