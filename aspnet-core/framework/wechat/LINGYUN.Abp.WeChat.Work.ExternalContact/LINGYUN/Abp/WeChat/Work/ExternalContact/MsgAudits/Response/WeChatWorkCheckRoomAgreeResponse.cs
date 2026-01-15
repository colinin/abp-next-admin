using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
/// <summary>
/// 获取群聊会话同意情况响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91782" />
/// </remarks>
public class WeChatWorkCheckRoomAgreeResponse : WeChatWorkResponse
{
    /// <summary>
    /// 同意情况
    /// </summary>
    [NotNull]
    [JsonProperty("agreeinfo")]
    [JsonPropertyName("agreeinfo")]
    public RoomAgreeInfo[] AgreeInfo { get; set; }
}
