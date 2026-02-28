using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Response;
/// <summary>
/// 审批模板响应参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91982"/>
/// </remarks>
public class WeChatWorkTemplateResponse : WeChatWorkResponse
{
    /// <summary>
    /// 模版名称数组
    /// </summary>
    [NotNull]
    [JsonProperty("template_names")]
    [JsonPropertyName("template_names")]
    public List<TemplateName> TemplateNames { get; set; }
    /// <summary>
    /// 审批模版控件设置，由多个表单控件及其内容组成，其中包含需要对控件赋值的信息
    /// </summary>
    [NotNull]
    [JsonProperty("template_content")]
    [JsonPropertyName("template_content")]
    public TemplateContent TemplateContent { get; set; }
    public WeChatWorkTemplateResponse()
    {

    }

    public WeChatWorkTemplateResponse(List<TemplateName> templateNames, TemplateContent templateContent)
    {
        TemplateNames = templateNames;
        TemplateContent = templateContent;
    }
}
