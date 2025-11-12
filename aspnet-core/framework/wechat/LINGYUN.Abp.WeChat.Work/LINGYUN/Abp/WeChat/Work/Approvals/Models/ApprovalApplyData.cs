using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 审批申请数据
/// </summary>
public class ApprovalApplyData
{
    /// <summary>
    /// 审批申请详情，由多个表单控件及其内容组成，其中包含需要对控件赋值的信息
    /// </summary>
    [NotNull]
    [JsonProperty("contents")]
    [JsonPropertyName("contents")]
    public List<ControlData> Contents { get; set; }
    public ApprovalApplyData()
    {

    }

    public ApprovalApplyData(List<ControlData> contents)
    {
        Contents = contents;
    }
}
