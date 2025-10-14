using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 成员控件值
/// </summary>
public class MemberControlValue : ContactControlValue
{
    /// <summary>
    /// 所选成员内容，即申请人在此控件选择的成员，多选模式下可以有多个
    /// </summary>
    [NotNull]
    [JsonProperty("members")]
    [JsonPropertyName("members")]
    public List<MemberValue> Members { get; set; }
    public MemberControlValue()
    {

    }

    private MemberControlValue(List<MemberValue> members)
    {
        Members = members;
    }

    public static MemberControlValue Single(MemberValue member)
    {
        return new MemberControlValue(new List<MemberValue> { member });
    }

    public static MemberControlValue Multiple(List<MemberValue> members)
    {
        return new MemberControlValue(members);
    }
}

public class MemberValue
{
    /// <summary>
    /// 所选成员的userid
    /// </summary>
    [NotNull]
    [JsonProperty("userid")]
    [JsonPropertyName("userid")]
    public string UserId { get; set; }
    /// <summary>
    /// 成员名
    /// </summary>
    [NotNull]
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    public MemberValue()
    {

    }

    public MemberValue(string userId, string name)
    {
        UserId = userId;
        Name = name;
    }
}
