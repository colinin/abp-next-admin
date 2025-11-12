using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 部门控件值
/// </summary>
public class DepartmentControlValue : ContactControlValue
{
    /// <summary>
    /// 所选部门内容，即申请人在此控件选择的部门，多选模式下可能有多个
    /// </summary>
    [NotNull]
    [JsonProperty("departments")]
    [JsonPropertyName("departments")]
    public List<DepartmentValue> Departments { get; set; }
    public DepartmentControlValue()
    {

    }

    private DepartmentControlValue(List<DepartmentValue> departments)
    {
        Departments = departments;
    }

    public static DepartmentControlValue Single(DepartmentValue department)
    {
        return new DepartmentControlValue(new List<DepartmentValue> { department });
    }

    public static DepartmentControlValue Multiple(List<DepartmentValue> departments)
    {
        return new DepartmentControlValue(departments);
    }
}

public class DepartmentValue
{
    /// <summary>
    /// 所选部门id
    /// </summary>
    [NotNull]
    [JsonProperty("openapi_id")]
    [JsonPropertyName("openapi_id")]
    public string DepartmentId { get; set; }
    /// <summary>
    /// 所选部门名
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    public DepartmentValue()
    {

    }

    public DepartmentValue(string departmentId, string name)
    {
        DepartmentId = departmentId;
        Name = name;
    }
}
