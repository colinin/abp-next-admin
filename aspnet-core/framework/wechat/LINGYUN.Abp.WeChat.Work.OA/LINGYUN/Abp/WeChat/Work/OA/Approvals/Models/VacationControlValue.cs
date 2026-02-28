using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 假勤组件-请假组件值
/// </summary>
public class VacationControlValue : ControlValue
{
    /// <summary>
    /// 请假内容，即申请人在此组件内选择的请假信息
    /// </summary>
    [NotNull]
    [JsonProperty("vacation")]
    [JsonPropertyName("vacation")]
    public VacationValue Vacation { get; set; }
    public VacationControlValue()
    {

    }

    public VacationControlValue(VacationValue vacation)
    {
        Vacation = vacation;
    }
}

public class VacationValue
{
    /// <summary>
    /// 请假类型，所选选项与假期管理关联，为假期管理中的假期类型
    /// </summary>
    [NotNull]
    [JsonProperty("selector")]
    [JsonPropertyName("selector")]
    public VacationSelector Selector { get; set; }
    /// <summary>
    /// 假勤组件
    /// </summary>
    [NotNull]
    [JsonProperty("attendance")]
    [JsonPropertyName("attendance")]
    public AttendanceValue Attendance { get; set; }
    public VacationValue()
    {

    }

    public VacationValue(VacationSelector selector, AttendanceValue attendance)
    {
        Selector = selector;
        Attendance = attendance;
    }
}

public class VacationSelector
{
    /// <summary>
    /// 选择方式：single-单选；multi-多选，在假勤控件中固定为单选
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// 用户所选选项
    /// </summary>
    [NotNull]
    [JsonProperty("options")]
    [JsonPropertyName("options")]
    public List<VacationSelectorOption> Options { get; set; }
    public VacationSelector()
    {

    }

    public VacationSelector(List<VacationSelectorOption> options)
    {
        Type = "single";
        Options = options;
    }
}

public class VacationSelectorOption
{
    /// <summary>
    /// 选项key，选项的唯一id，可通过“获取审批模板详情”接口获得vacation_list中item的id值
    /// </summary>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    /// <summary>
    /// 选项值，若配置了多语言则会包含中英文的选项值
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public VacationSelectorOptionValue Value { get; set; }
    public VacationSelectorOption()
    {

    }

    public VacationSelectorOption(string key, VacationSelectorOptionValue value)
    {
        Key = key;
        Value = value;
    }
}

public class VacationSelectorOptionValue
{
    /// <summary>
    /// 选项值
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 多语言名称
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public VacationSelectorOptionValue()
    {

    }

    public VacationSelectorOptionValue(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}

