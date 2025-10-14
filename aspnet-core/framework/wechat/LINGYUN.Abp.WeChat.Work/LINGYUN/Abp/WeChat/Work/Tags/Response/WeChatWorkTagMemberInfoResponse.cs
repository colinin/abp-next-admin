using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Tags.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Tags.Response;
/// <summary>
/// 标签成员响应参数
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/90213
/// </remarks>
public class WeChatWorkTagMemberInfoResponse : WeChatWorkResponse
{
    /// <summary>
    /// 标签名
    /// </summary>
    [NotNull]
    [JsonProperty("tagname")]
    [JsonPropertyName("tagname")]
    public string TagName { get; set; }
    /// <summary>
    /// 标签中包含的成员列表
    /// </summary>
    [NotNull]
    [JsonProperty("userlist")]
    [JsonPropertyName("userlist")]
    public List<TagUserInfo> Users { get; set; }
    /// <summary>
    /// 标签中包含的部门id列表
    /// </summary>
    [NotNull]
    [JsonProperty("partylist")]
    [JsonPropertyName("partylist")]
    public List<int> Parts { get; set; }
}
