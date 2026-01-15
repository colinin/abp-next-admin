using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 关联审批单控件配置
/// </summary>
public class RelatedApprovalControlConfig : ControlConfig
{
    /// <summary>
    /// 关联审批单控件
    /// </summary>
    [NotNull]
    [JsonProperty("related_approval")]
    [JsonPropertyName("related_approval")]
    public RelatedApprovalConfig RelatedApproval { get; set; }
    public RelatedApprovalControlConfig()
    {

    }

    public RelatedApprovalControlConfig(RelatedApprovalConfig relatedApproval)
    {
        RelatedApproval = relatedApproval;
    }
}

public class RelatedApprovalConfig
{
    /// <summary>
    /// 关联审批单的template_id ，不填时表示可以关联所有模版，该template_id可通过获取审批模版接口获取
    /// </summary>
    [NotNull]
    [JsonProperty("template_id")]
    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; }
    public RelatedApprovalConfig()
    {

    }

    public RelatedApprovalConfig(string templateId)
    {
        TemplateId = templateId;
    }
}
