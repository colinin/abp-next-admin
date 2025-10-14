using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 模板控件
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(ControlNewtonsoftJsonConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(ControlSystemTextJsonConverter))]
public class Control
{
    /// <summary>
    /// 控件属性
    /// </summary>
    [NotNull]
    [JsonProperty("property")]
    [JsonPropertyName("property")]
    public ControlInfo Property { get; set; }
    /// <summary>
    /// 控件配置
    /// </summary>
    [CanBeNull]
    [JsonProperty("config")]
    [JsonPropertyName("config")]
    public ControlConfig Config { get; set; }
}
