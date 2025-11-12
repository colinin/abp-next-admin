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
    /// <summary>
    /// 关联控件
    /// </summary>
    [NotNull]
    [JsonProperty("op_relations")]
    [JsonPropertyName("op_relations")]
    public List<SelectorOptionRelation> OptionRelations { get; set; }
    /// <summary>
    /// 关联外部选项
    /// </summary>
    [NotNull]
    [JsonProperty("external_option")]
    [JsonPropertyName("external_option")]
    public SelectorOptionExternal ExternalOption { get; set; }
    public SelectorConfig()
    {

    }

    private SelectorConfig(
        string type,
        List<SelectorOption> options,
        List<SelectorOptionRelation> optionRelations = null,
        SelectorOptionExternal optionExternal = null)
    {
        Type = type;
        Options = options;
        OptionRelations = optionRelations;
        ExternalOption = optionExternal;
    }

    public static SelectorConfig Single(
        List<SelectorOption> options,
        List<SelectorOptionRelation> optionRelations = null, 
        SelectorOptionExternal optionExternal = null)
    {
        return new SelectorConfig("single", options, optionRelations, optionExternal);
    }

    public static SelectorConfig Multiple(
        List<SelectorOption> options,
        List<SelectorOptionRelation> optionRelations = null,
        SelectorOptionExternal optionExternal = null)
    {
        return new SelectorConfig("multi", options, optionRelations, optionExternal);
    }
}

public class SelectorOption
{
    /// <summary>
    /// 选项key
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

public class SelectorOptionRelation
{
    /// <summary>
    /// 选项key
    /// </summary>
    [NotNull]
    [JsonProperty("key")]
    [JsonPropertyName("key")]
    public string Key { get; set; }
    /// <summary>
    /// 关联控件列表
    /// </summary>
    [NotNull]
    [JsonProperty("relation_list")]
    [JsonPropertyName("relation_list")]
    public List<SelectorRelation> Relations { get; set; }
    public SelectorOptionRelation()
    {

    }

    public SelectorOptionRelation(string key, List<SelectorRelation> relations)
    {
        Key = key;
        Relations = relations;
    }
}

public class SelectorRelation
{
    /// <summary>
    /// 控件Id
    /// </summary>
    [NotNull]
    [JsonProperty("related_control_id")]
    [JsonPropertyName("related_control_id")]
    public string ControlId { get; set; }
    /// <summary>
    /// 操作方法
    /// </summary>
    /// <remarks>
    /// 1 - 显示对应控件
    /// </remarks>
    [NotNull]
    [JsonProperty("action")]
    [JsonPropertyName("action")]
    public int Action { get; set; }
    public SelectorRelation()
    {

    }

    public SelectorRelation(string controlId, int action = 1)
    {
        ControlId = controlId;
        Action = action;
    }
}

public class SelectorOptionExternal
{
    /// <summary>
    /// 关联外部选项
    /// </summary>
    [NotNull]
    [JsonProperty("use_external_option")]
    [JsonPropertyName("use_external_option")]
    public bool UseExternalOption { get; set; }
    /// <summary>
    /// 外部选项页面地址
    /// </summary>
    [NotNull]
    [JsonProperty("external_url")]
    [JsonPropertyName("external_url")]
    public string ExternalUrl { get; set; }
    public SelectorOptionExternal()
    {

    }

    public SelectorOptionExternal(string externalUrl)
    {
        ExternalUrl = externalUrl;
        UseExternalOption = true;
    }
}