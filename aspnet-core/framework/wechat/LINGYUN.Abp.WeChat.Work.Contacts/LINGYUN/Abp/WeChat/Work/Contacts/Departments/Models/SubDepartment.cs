using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Models;
/// <summary>
/// 子部门
/// </summary>
public class SubDepartment
{
    /// <summary>
    /// 部门id
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    /// <summary>
    /// 父部门id。根部门为1。
    /// </summary>
    [NotNull]
    [JsonProperty("parentid")]
    [JsonPropertyName("parentid")]
    public int ParentId { get; set; }
    /// <summary>
    /// 在父部门中的次序值。order值大的排序靠前。值范围是[0, 2^32)。
    /// </summary>
    [NotNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int Order { get; set; }
}
