using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 部门成员
/// </summary>
public class DepartmentMember
{
    /// <summary>
    /// 成员UserID。对应管理端的账号
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员名称，代开发自建应用需要管理员授权才返回
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 成员所属部门列表。列表项为部门ID，32位整型
    /// </summary>
    [CanBeNull]
    [JsonProperty("department")]
    [JsonPropertyName("department")]
    public int[]? Department { get; set; }
    /// <summary>
    /// 全局唯一
    /// </summary>
    [NotNull]
    [JsonProperty("open_userid")]
    [JsonPropertyName("open_userid")]
    public string OpenUserId { get; set; }
}
