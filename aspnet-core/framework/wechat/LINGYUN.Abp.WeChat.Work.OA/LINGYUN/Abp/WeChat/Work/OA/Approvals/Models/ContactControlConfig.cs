using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 成员控件配置
/// </summary>
public class ContactControlConfig : ControlConfig
{
    /// <summary>
    /// 成员、部门控件
    /// </summary>
    [NotNull]
    [JsonProperty("contact")]
    [JsonPropertyName("contact")]
    public ContactConfig Contact { get; set; }
    public ContactControlConfig()
    {

    }

    public ContactControlConfig(ContactConfig contact)
    {
        Contact = contact;
    }
}

public class ContactConfig
{
    /// <summary>
    /// single-单选、multi-多选
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// user-成员、department-部门
    /// </summary>
    [NotNull]
    [JsonProperty("mode")]
    [JsonPropertyName("mode")]
    public string Mode { get; set; }
    public ContactConfig()
    {

    }

    private ContactConfig(string type, string mode)
    {
        Type = type;
        Mode = mode;
    }

    public static ContactConfig SingleUser()
    {
        return new ContactConfig("single", "user");
    }

    public static ContactConfig SingleDepartment()
    {
        return new ContactConfig("single", "department");
    }

    public static ContactConfig MultipleUser()
    {
        return new ContactConfig("multi", "user");
    }

    public static ContactConfig MultipleDepartment()
    {
        return new ContactConfig("multi", "department");
    }
}
