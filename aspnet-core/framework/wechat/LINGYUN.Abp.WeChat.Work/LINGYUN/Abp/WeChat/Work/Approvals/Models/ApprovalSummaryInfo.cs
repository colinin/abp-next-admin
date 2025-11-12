using JetBrains.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 摘要信息
/// </summary>
public class ApprovalSummaryInfo
{
    /// <summary>
    /// 摘要行显示文字，用于记录列表和消息通知的显示，不要超过20个字符
    /// </summary>
    [NotNull]
    [StringLength(20)]
    [JsonProperty("text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }
    /// <summary>
    /// 摘要行显示语言，中文：zh_CN（注意不是zh-CN），英文：en。
    /// </summary>
    [NotNull]
    [StringLength(30)]
    [JsonProperty("lang")]
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    public ApprovalSummaryInfo()
    {

    }

    public ApprovalSummaryInfo(string text, string lang = "zh_CN")
    {
        Text = text;
        Lang = lang;
    }
}
