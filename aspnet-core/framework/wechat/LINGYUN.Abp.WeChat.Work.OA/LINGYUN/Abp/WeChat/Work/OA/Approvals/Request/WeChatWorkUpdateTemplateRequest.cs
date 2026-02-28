using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
/// <summary>
/// 更新审批模板请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97438"/>
/// </remarks>
public class WeChatWorkUpdateTemplateRequest
{
    /// <summary>
    /// 模版id
    /// </summary>
    [NotNull]
    [JsonProperty("template_id")]
    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; }
    /// <summary>
    /// 模版名称数组
    /// </summary>
    [NotNull]
    [JsonProperty("template_name")]
    [JsonPropertyName("template_name")]
    public List<TemplateName> TemplateName { get; set; }
    /// <summary>
    /// 审批模版控件设置，由多个表单控件及其内容组成，其中包含需要对控件赋值的信息
    /// </summary>
    [NotNull]
    [JsonProperty("template_content")]
    [JsonPropertyName("template_content")]
    public TemplateContent TemplateContent { get; set; }

    public WeChatWorkUpdateTemplateRequest(
        string templateId,
        List<TemplateName> templateName, 
        TemplateContent templateContent)
    {
        TemplateId = templateId;
        TemplateName = templateName;
        TemplateContent = templateContent;
    }
}
