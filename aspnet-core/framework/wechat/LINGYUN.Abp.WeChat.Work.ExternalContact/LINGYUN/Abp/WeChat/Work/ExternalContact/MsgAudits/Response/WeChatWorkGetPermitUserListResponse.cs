using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
/// <summary>
/// 获取会话内容存档开启成员列表响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91614" />
/// </remarks>
public class WeChatWorkGetPermitUserListResponse : WeChatWorkResponse
{
    /// <summary>
    /// 设置在开启范围内的成员的userid列表
    /// </summary>
    [NotNull]
    [JsonProperty("ids")]
    [JsonPropertyName("ids")]
    public string[] Ids { get; set; }
}
