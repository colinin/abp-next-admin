using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments.Models;
/// <summary>
/// 部门详情
/// </summary>
public class DepartmentInfo : Department
{
    /// <summary>
    /// 部门英文名称
    /// </summary>
    [CanBeNull]
    [JsonProperty("name_en")]
    [JsonPropertyName("name_en")]
    public string? NameEn { get; set; }
    /// <summary>
    /// 部门负责人的UserID，返回在应用可见范围内的部门负责人列表
    /// </summary>
    [NotNull]
    [JsonProperty("department_leader")]
    [JsonPropertyName("department_leader")]
    public string[] DepartmentLeader { get; set; }
    /// <summary>
    /// 父部门id。根部门为1
    /// </summary>
    [NotNull]
    [JsonProperty("parentid")]
    [JsonPropertyName("parentid")]
    public int ParentId { get; set; }
    /// <summary>
    /// 在父部门中的次序值。order值大的排序靠前。值范围是[0, 2^32)
    /// </summary>
    [NotNull]
    [JsonProperty("order")]
    [JsonPropertyName("order")]
    public int Order { get; set; }
}
