using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 选择控件配置
/// </summary>
public class SelectorControlConfig : ControlConfig
{
    /// <summary>
    /// 选择控件内容，即申请人在此控件选择的选项内容
    /// </summary>
    [NotNull]
    [JsonProperty("selector")]
    [JsonPropertyName("selector")]
    public SelectorConfig Selector { get; set; }
    public SelectorControlConfig()
    {

    }

    public SelectorControlConfig(SelectorConfig selector)
    {
        Selector = selector;
    }
}

public class SelectorConfig
{
    /// <summary>
    /// 选择方式：single-单选；multi-多选
    /// </summary>
    [NotNull]
    [JsonProperty("type")]
    [JsonPropertyName("type")]
    public string Type { get; set; }
    /// <summary>
    /// 多选选项，多选属性的选择控件允许输入多个
    /// </summary>
    [NotNull]
    [JsonProperty("options")]
    [JsonPropertyName("options")]
    public List<SelectorOption> Options {  get; set; }
    public SelectorConfig()
    {

    }

    private SelectorConfig(string type, List<SelectorOption> options)
    {
        Type = type;
        Options = options;
    }

    public static SelectorConfig Single(List<SelectorOption> options)
    {
        return new SelectorConfig("single", options);
    }

    public static SelectorConfig Multiple(List<SelectorOption> options)
    {
        return new SelectorConfig("multi", options);
    }
}

public class SelectorOption
{
    /// <summary>
    /// 时间展示类型：day-日期；hour-日期+时间 ，和对应模板控件属性一致
    /// </summary>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    /// <summary>
    /// 选项说明
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public List<SelectorOptionValue> Value { get; set; }
    public SelectorOption()
    {

    }

    public SelectorOption(string key, List<SelectorOptionValue> value)
    {
        Key = key;
        Value = value;
    }
}

public class SelectorOptionValue
{
    /// <summary>
    /// 名称
    /// </summary>
    [NotNull]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 显示语言
    /// </summary>
    [NotNull]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public SelectorOptionValue()
    {

    }

    public SelectorOptionValue(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}