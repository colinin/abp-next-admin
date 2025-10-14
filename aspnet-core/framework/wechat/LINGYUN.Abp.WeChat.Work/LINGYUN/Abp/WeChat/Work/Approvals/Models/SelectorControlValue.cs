using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 单选/多选控件值
/// </summary>
public class SelectorControlValue : ControlValue
{
    /// <summary>
    /// 单选/多选内容
    /// </summary>
    [NotNull]
    [JsonProperty("selector")]
    [JsonPropertyName("selector")]
    public SelectorValue Selector { get; set; }
    public SelectorControlValue()
    {

    }

    public SelectorControlValue(SelectorValue selector)
    {
        Selector = selector;
    }
}

public class SelectorValue
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
    public List<SelectorValueOption> Options { get; set; }
    public SelectorValue()
    {

    }

    private SelectorValue(string type, List<SelectorValueOption> options)
    {
        Type = type;
        Options = options;
    }
    /// <summary>
    /// 创建一个单选选项
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static SelectorValue Single(SelectorValueOption option)
    {
        return new SelectorValue("single", new List<SelectorValueOption> { option });
    }

    /// <summary>
    /// 创建一个多选选项
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static SelectorValue Multiple(List<SelectorValueOption> options)
    {
        return new SelectorValue("multi", options);
    }
}

public class SelectorValueOption
{
    /// <summary>
    /// 选项key，可通过“获取审批模板详情”接口获得
    /// </summary>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    public SelectorValueOption()
    {

    }

    public SelectorValueOption(string key)
    {
        Key = key;
    }
}