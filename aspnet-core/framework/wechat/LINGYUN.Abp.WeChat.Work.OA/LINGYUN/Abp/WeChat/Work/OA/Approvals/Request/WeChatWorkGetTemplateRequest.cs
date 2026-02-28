using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
/// <summary>
/// 获取审批模板请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91982"/>
/// </remarks>
public class WeChatWorkGetTemplateRequest : WeChatWorkRequest
{
    /// <summary>
    /// 模版id
    /// </summary>
    [NotNull]
    [JsonProperty("template_id")]
    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; }
    public WeChatWorkGetTemplateRequest(string templateId)
    {
        TemplateId = templateId;
    }
}
