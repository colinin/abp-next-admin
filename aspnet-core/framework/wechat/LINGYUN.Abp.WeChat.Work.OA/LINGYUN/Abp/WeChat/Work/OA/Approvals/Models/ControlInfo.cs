using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 模板控件属性
/// </summary>
public class ControlInfo
{
    /// <summary>
    /// 控件id。1-模版内控件id必须唯一；2-控件id格式：control-数字，如"Text-01"
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    /// <summary>
    /// 控件类型
    /// </summary>
    /// <remarks>
    /// Text-文本<br />
    /// Textarea-多行文本<br />
    /// Number-数字<br />
    /// Money-金额<br />
    /// Date-日期/日期+时间<br />
    /// Selector-单选/多选<br />
    /// Contact-成员/部门<br />
    /// Tips-说明文字<br />
    /// File-附件<br />
    /// Table-明细<br />
    /// Location-位置<br />
    /// RelatedApproval-关联审批单<br />
    /// DateRange-时长<br />
    /// PhoneNumber-电话号码<br />
    /// Vacation-假期<br />
    /// Attendance-外出/出差/加班<br />
    /// BankAccount-收款账户<br />
    /// 以上为目前可支持的控件类型
    /// </remarks>
    [NotNull]
    [JsonProperty("control")]
    [JsonPropertyName("control")]
    public string Control { get; set; }
    /// <summary>
    /// 控件名称
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public List<ControlTtile> Title { get; set; }
    /// <summary>
    /// 控件说明，假勤组件（Vacation、Attendance）暂不支持设置
    /// </summary>
    [CanBeNull]
    [JsonProperty("placeholder")]
    [JsonPropertyName("placeholder")]
    public List<ControlPlaceholder> Placeholder { get; set; }
    /// <summary>
    /// 控件是否必填。0-非必填；1-必填；默认为0;假勤组件（Vacation、Attendance）不支持设置非必填
    /// </summary>
    [CanBeNull]
    [JsonProperty("require")]
    [JsonPropertyName("require")]
    public byte Require { get; set; }
    /// <summary>
    /// 控件是否可打印。0-可打印；1-不可打印；默认为0；假勤组件（Vacation、Attendance）不支持设置不可打印
    /// </summary>
    [CanBeNull]
    [JsonProperty("un_print")]
    [JsonPropertyName("un_print")]
    public byte UnPrint { get; set; }
    /// <summary>
    /// un_replace
    /// </summary>
    [CanBeNull]
    [JsonProperty("un_replace")]
    [JsonPropertyName("un_replace")]
    public byte UnReplace { get; set; }
    /// <summary>
    /// inner_id
    /// </summary>
    [CanBeNull]
    [JsonProperty("inner_id")]
    [JsonPropertyName("inner_id")]
    public string InnerId { get; set; }
    /// <summary>
    /// 控件是否隐藏
    /// </summary>
    [CanBeNull]
    [JsonProperty("display")]
    [JsonPropertyName("display")]
    public byte Display { get; set; }
}
