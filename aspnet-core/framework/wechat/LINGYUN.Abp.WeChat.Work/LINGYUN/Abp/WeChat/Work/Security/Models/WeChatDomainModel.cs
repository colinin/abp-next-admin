using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Security.Models;
public class WeChatDomainModel
{
    /// <summary>
    /// 域名
    /// </summary>
    [JsonProperty("domain")]
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = default!;
    /// <summary>
    /// 泛域名
    /// </summary>
    [JsonProperty("universal_domian")]
    [JsonPropertyName("universal_domian")]
    public string UniversalDomian { get; set; }
    /// <summary>
    /// 协议 如TCP UDP
    /// </summary>
    [JsonProperty("protocol")]
    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = default!;
    /// <summary>
    /// 端口号列表
    /// </summary>
    [JsonProperty("port")]
    [JsonPropertyName("port")]
    public List<int> Port { get; set; } = new List<int>();
    /// <summary>
    /// 是否必要，0-否 1-是， 如果必要的域名或IP被拦截，将导致企业微信的功能出现异常
    /// </summary>
    [JsonProperty("is_necessary")]
    [JsonPropertyName("is_necessary")]
    public int IsNecessary { get; set; } = default!;
    /// <summary>
    /// IP涉及到的功能的描述信息
    /// </summary>
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
