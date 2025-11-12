using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 明细控件配置
/// </summary>
public class TableControlConfig : ControlConfig
{
    /// <summary>
    /// 明细控件
    /// </summary>
    [NotNull]
    [JsonProperty("table")]
    [JsonPropertyName("table")]
    public TableConfig Table { get; set; }
    public TableControlConfig()
    {

    }

    public TableControlConfig(TableConfig table)
    {
        Table = table;
    }
}

public class TableConfig
{
    /// <summary>
    /// 打印格式；0-合并成一行打印，1-拆分成多行打印
    /// </summary>
    [NotNull]
    [JsonProperty("print_format")]
    [JsonPropertyName("print_format")]
    public byte PrintFormat { get; set; }
    /// <summary>
    /// 明细内容，一个明细控件可能包含多个子控件，每个子控件的属性和控件相同
    /// </summary>
    [NotNull]
    [JsonProperty("children")]
    [JsonPropertyName("children")]
    public List<Control> Children { get; set; }
    public TableConfig()
    {

    }

    public TableConfig(byte printFormat, List<Control> children)
    {
        PrintFormat = printFormat;
        Children = children;
    }
}