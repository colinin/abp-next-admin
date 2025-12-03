using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
/// <summary>
/// 邀请成员响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90975" />
/// </remarks>
public class WeChatWorkBulkInviteMemberResponse : WeChatWorkResponse
{
    /// <summary>
    /// 非法成员列表
    /// </summary>
    [NotNull]
    [JsonProperty("invaliduser")]
    [JsonPropertyName("invaliduser")]
    public string[] InvalidUser { get; set; }
    /// <summary>
    /// 非法部门列表
    /// </summary>
    [NotNull]
    [JsonProperty("invalidparty")]
    [JsonPropertyName("invalidparty")]
    public int[] InvalidParty { get; set; }
    /// <summary>
    /// 非法标签列表
    /// </summary>
    [NotNull]
    [JsonProperty("invalidtag")]
    [JsonPropertyName("invalidtag")]
    public int[] InvalidTag { get; set; }
}