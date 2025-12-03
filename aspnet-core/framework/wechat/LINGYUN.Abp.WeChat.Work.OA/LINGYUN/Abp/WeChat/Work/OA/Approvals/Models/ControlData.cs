using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 审批申请详情
/// </summary>
public class ControlData
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
    public ControlValue Value { get; set; }
    public ControlData()
    {

    }

    private ControlData(string id, string control, ControlValue value)
    {
        Id = id;
        Control = control;
        Value = value;
    }

    /// <summary>
    /// 创建一个Text控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Text(string id, TextControlValue value)
    {
        return new ControlData(id, "Text", value);
    }
    /// <summary>
    /// 创建一个Textarea控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Textarea(string id, TextareaControlValue value)
    {
        return new ControlData(id, "Textarea", value);
    }
    /// <summary>
    /// 创建一个Number控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Number(string id, NumberControlValue value)
    {
        return new ControlData(id, "Number", value);
    }
    /// <summary>
    /// 创建一个Money控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Money(string id, MoneyControlValue value)
    {
        return new ControlData(id, "Money", value);
    }
    /// <summary>
    /// 创建一个Date控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Date(string id, DateControlValue value)
    {
        return new ControlData(id, "Date", value);
    }
    /// <summary>
    /// 创建一个Selector控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Selector(string id, SelectorControlValue value)
    {
        return new ControlData(id, "Selector", value);
    }
    /// <summary>
    /// 创建一个Contact控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Contact(string id, ContactControlValue value)
    {
        return new ControlData(id, "Contact", value);
    }
    /// <summary>
    /// 创建一个Tips控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <returns></returns>
    public static ControlData Tips(string id)
    {
        return new ControlData(id, "Tips", null);
    }
    /// <summary>
    /// 创建一个File控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData File(string id, FileControlValue value)
    {
        return new ControlData(id, "File", value);
    }
    /// <summary>
    /// 创建一个Table控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Table(string id, TableControlValue value)
    {
        return new ControlData(id, "Table", value);
    }
    /// <summary>
    /// 创建一个Location控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Location(string id, LocationControlValue value)
    {
        return new ControlData(id, "Location", value);
    }
    /// <summary>
    /// 创建一个RelatedApproval控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData RelatedApproval(string id, RelatedApprovalControlValue value)
    {
        return new ControlData(id, "RelatedApproval", value);
    }
    /// <summary>
    /// 创建一个Formula控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData Formula(string id, FormulaControlValue value)
    {
        return new ControlData(id, "Formula", value);
    }
    /// <summary>
    /// 创建一个DateRange控件
    /// </summary>
    /// <param name="id">控件Id</param>
    /// <param name="value">控件值</param>
    /// <returns></returns>
    public static ControlData DateRange(string id, DateRangeControlValue value)
    {
        return new ControlData(id, "DateRange", value);
    }
}
