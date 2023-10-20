using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.Security;

namespace LINGYUN.Abp.WeChat.Work;
/// <summary>
/// 企业微信应用配置
/// </summary>
public class WeChatWorkApplicationConfiguration
{
    /// <summary>
    /// 应用的标识
    /// </summary>
    public string AgentId { get; set; }
    /// <summary>
    /// 应用的凭证密钥
    /// </summary>
    public string Secret { get; set; }
    /// <summary>
    /// 应用加密配置
    /// </summary>
    public WeChatWorkCryptoConfigurationDictionary CryptoKeys { get; set; }

    public WeChatWorkApplicationConfiguration()
    {
        CryptoKeys = new WeChatWorkCryptoConfigurationDictionary();
    }

    public WeChatWorkApplicationConfiguration(string agentId, string secret)
    {
        AgentId = agentId;
        Secret = secret;
        CryptoKeys = new WeChatWorkCryptoConfigurationDictionary();
    }

    [NotNull]
    public WeChatWorkCryptoConfiguration GetCryptoConfiguration(string feture)
    {
        return CryptoKeys.GetCryptoConfigurationOrNull(feture)
               ?? throw new AbpWeChatWorkException("WeChatWork:101404", $"WeChat Work crypto was not found configuration with feture '{feture}' .")
                    .WithData("AgentId", AgentId)
                    .WithData("Feture", feture);
    }
}
