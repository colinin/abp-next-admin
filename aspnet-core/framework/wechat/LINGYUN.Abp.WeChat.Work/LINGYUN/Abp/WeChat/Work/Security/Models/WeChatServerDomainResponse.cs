using Newtonsoft.Json;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Work.Security.Models;
public class WeChatServerDomainResponse : WeChatWorkResponse
{
    /// <summary>
    /// 域名列表
    /// </summary>
    [JsonProperty("domain_list")]
    public List<WeChatDomainModel> Domains { get; set; } = new List<WeChatDomainModel>();
    /// <summary>
    /// Ip列表
    /// </summary>
    [JsonProperty("ip_list")]
    public List<WeChatIpModel> Ips { get; set; } = new List<WeChatIpModel>();
    public WeChatServerDomainModel ToServerDomain()
    {
        ThrowIfNotSuccess();
        return new WeChatServerDomainModel(Domains, Ips);
    }
}
