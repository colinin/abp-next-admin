using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 明细控件值
/// </summary>
public class TableControlValue : ControlValue
{
    /// <summary>
    /// 明细内容，一个明细控件可能包含多个子明细
    /// </summary>
    [NotNull]
    [JsonProperty("children")]
    [JsonPropertyName("children")]
    public List<TableValue> Children { get; set; }
    public TableControlValue()
    {

    }

    public TableControlValue(List<TableValue> children)
    {
        Children = children;
    }
}

public class TableValue
{
    /// <summary>
    /// 子明细列表，在此填写子明细的所有子控件的值，子控件的数据结构同一般控件。
    /// </summary>
    /// <remarks>
    /// 注意：不能为空数组，至少需要包含一个子明细，子明细中必须包括模板中设置的全部子控件，如果子明细为空，则需要将所有子控件的值设为空
    /// </remarks>
    [NotNull]
    [JsonProperty("list")]
    [JsonPropertyName("list")]
    public List<ControlData> List { get; set; }
    public TableValue()
    {

    }

    public TableValue(List<ControlData> list)
    {
        List = list;
    }
}