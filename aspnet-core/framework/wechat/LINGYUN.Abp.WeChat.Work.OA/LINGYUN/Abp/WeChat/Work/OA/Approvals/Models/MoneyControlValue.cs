using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 金额控件值
/// </summary>
public class MoneyControlValue : ControlValue
{
    /// <summary>
    /// 金额内容，在此填写金额控件的输入值
    /// </summary>
    [NotNull]
    [JsonProperty("new_money")]
    [JsonPropertyName("new_money")]
    public string NewMoney { get; set; }
    public MoneyControlValue()
    {

    }

    public MoneyControlValue(decimal newMoney)
    {
        NewMoney = newMoney.ToString();
    }
}
