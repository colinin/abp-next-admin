using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Models;
/// <summary>
/// 小程序类型属性
/// </summary>
public class ExternalMiniProgramAttribute : ExternalAttribute
{
    /// <summary>
    /// 小程序
    /// </summary>
    [NotNull]
    [JsonProperty("miniprogram")]
    [JsonPropertyName("miniprogram")]
    public ExternalMiniProgramModel MiniProgram { get; set; }
}

public class ExternalMiniProgramModel
{
    /// <summary>
    /// 小程序appid，必须是有在本企业安装授权的小程序，否则会被忽略
    /// </summary>
    [NotNull]
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }
    /// <summary>
    /// 小程序的展示标题，长度限制12个UTF8字符
    /// </summary>
    [NotNull]
    [JsonProperty("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    /// <summary>
    /// 小程序的页面路径
    /// </summary>
    [NotNull]
    [JsonProperty("pagepath")]
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
}
