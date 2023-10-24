using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Work.Security;
/// <summary>
/// 企业微信加解密配置
/// </summary>
public class WeChatWorkCryptoConfiguration : Dictionary<string, string>
{
    /// <summary>
    /// 用于生成签名的Token
    /// </summary>
    public string Token {
        get => this.GetOrDefault(nameof(Token));
        set => this[nameof(Token)] = value;
    }

    /// <summary>
    /// 用于消息加密的密钥
    /// </summary>
    public string EncodingAESKey {
        get => this.GetOrDefault(nameof(EncodingAESKey));
        set => this[nameof(EncodingAESKey)] = value;
    }

    public WeChatWorkCryptoConfiguration()
    {

    }

    public WeChatWorkCryptoConfiguration(string token, string encodingAESKey)
    {
        this[nameof(Token)] = token;
        this[nameof(EncodingAESKey)] = encodingAESKey;
    }
}
