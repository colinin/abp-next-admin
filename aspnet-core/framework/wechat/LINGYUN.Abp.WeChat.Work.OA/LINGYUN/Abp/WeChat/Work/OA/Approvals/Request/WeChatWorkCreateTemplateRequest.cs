using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
/// <summary>
/// 创建审批模板请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/97437"/>
/// </remarks>
public class WeChatWorkCreateTemplateRequest : WeChatWorkRequest
{
    /// <summary>
    /// 模版名称数组
    /// </summary>
    [NotNull]
    [JsonProperty("template_name")]
    [JsonPropertyName("template_name")]
    public List<TemplateName> TemplateName {  get; set; }
    /// <summary>
    /// 审批模版控件设置，由多个表单控件及其内容组成，其中包含需要对控件赋值的信息
    /// </summary>
    [NotNull]
    [JsonProperty("template_content")]
    [JsonPropertyName("template_content")]
    public TemplateContent TemplateContent { get; set; }

    public WeChatWorkCreateTemplateRequest(List<TemplateName> templateName, TemplateContent templateContent)
    {
        TemplateName = templateName;
        TemplateContent = templateContent;
    }
}
