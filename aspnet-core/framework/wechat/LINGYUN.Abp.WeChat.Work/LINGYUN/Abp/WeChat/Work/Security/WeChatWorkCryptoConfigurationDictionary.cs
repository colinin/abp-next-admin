using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Work.Security;
public class WeChatWorkCryptoConfigurationDictionary : Dictionary<string, WeChatWorkCryptoConfiguration>
{
    [CanBeNull]
    public WeChatWorkCryptoConfiguration GetCryptoConfigurationOrNull(string feture)
    {
        return this.GetOrDefault(feture);
    }
}
