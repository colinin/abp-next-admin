using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
/// <summary>
/// 获取会话内容存档内部群信息请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92951" />
/// </remarks>
public class WeChatWorkGetGroupChatRequest : WeChatWorkRequest
{
    /// <summary>
    /// 待查询的roomid
    /// </summary>
    [NotNull]
    [JsonProperty("roomid")]
    [JsonPropertyName("roomid")]
    public string RoomId { get; }
    public WeChatWorkGetGroupChatRequest(string roomId)
    {
        RoomId = roomId;
    }
}
