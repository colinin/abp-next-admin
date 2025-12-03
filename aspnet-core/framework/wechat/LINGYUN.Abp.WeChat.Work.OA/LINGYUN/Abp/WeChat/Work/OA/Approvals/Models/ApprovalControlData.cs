using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 审批申请单控件数据
/// </summary>
public class ApprovalControlData
{
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
    /// Formula-公式<br />
    /// DateRange-时长
    /// </remarks>
    [NotNull]
    [JsonProperty("control")]
    [JsonPropertyName("control")]
    public string Control { get; set; }
    /// <summary>
    /// 控件id：控件的唯一id，可通过“获取审批模板详情”接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }
    /// <summary>
    /// 控件值 ，需在此为申请人在各个控件中填写内容不同控件有不同的赋值参数
    /// </summary>
    /// <remarks>
    /// 模板配置的控件属性为必填时，对应value值需要有值。
    /// </remarks>
    [NotNull]
    [JsonProperty("value")]
    [JsonPropertyName("value")]
    public ApprovalControlValue Value { get; set; }
    /// <summary>
    /// 控件隐藏标识，为1表示控件被隐藏
    /// </summary>
    [CanBeNull]
    [JsonProperty("display")]
    [JsonPropertyName("display")]
    public int? Display { get; set; }
    /// <summary>
    /// 控件必输标识，为1表示控件必输
    /// </summary>
    [CanBeNull]
    [JsonProperty("require")]
    [JsonPropertyName("require")]
    public int? Require { get; set; }
}
