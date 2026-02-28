using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 模版控件定义
/// </summary>
public class TemplateContent
{
    /// <summary>
    /// 控件数组，模版中可以设置多个控件类型，排列顺序和管理端展示的相同
    /// </summary>
    [NotNull]
    [JsonProperty("controls")]
    [JsonPropertyName("controls")]
    public List<Control> Controls { get; set; }
}
