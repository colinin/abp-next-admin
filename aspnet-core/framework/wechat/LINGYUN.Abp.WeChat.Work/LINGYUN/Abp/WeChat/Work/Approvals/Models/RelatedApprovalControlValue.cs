using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Approvals.Models;
/// <summary>
/// 关联审批单控件值
/// </summary>
public class RelatedApprovalControlValue : ControlValue
{
    /// <summary>
    /// 关联审批单
    /// </summary>
    [NotNull]
    [JsonProperty("related_approval")]
    [JsonPropertyName("related_approval")]
    public RelatedApprovalValue RelatedApproval { get; set; }
    public RelatedApprovalControlValue()
    {

    }

    public RelatedApprovalControlValue(RelatedApprovalValue relatedApproval)
    {
        RelatedApproval = relatedApproval;
    }
}


public class RelatedApprovalValue
{
    /// <summary>
    /// 关联审批单的审批单号
    /// </summary>
    [NotNull]
    [JsonProperty("sp_no")]
    [JsonPropertyName("sp_no")]
    public string SpNo { get; set; }
    public RelatedApprovalValue()
    {

    }

    public RelatedApprovalValue(string spNo)
    {
        SpNo = spNo;
    }
}