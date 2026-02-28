using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 公式控件值
/// </summary>
public class FormulaControlValue : ControlValue
{
    /// <summary>
    /// 公式
    /// </summary>
    [NotNull]
    [JsonProperty("formula")]
    [JsonPropertyName("formula")]
    public FormulaValue Formula { get; set; }
    public FormulaControlValue()
    {

    }

    public FormulaControlValue(FormulaValue formula)
    {
        Formula = formula;
    }
}

public class FormulaValue
{
    /// <summary>
    /// 公式的值，提交表单时无需填写，后台自动计算
    /// </summary>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public string Value { get; set; }
    public FormulaValue()
    {

    }

    public FormulaValue(string value)
    {
        Value = value;
    }
}