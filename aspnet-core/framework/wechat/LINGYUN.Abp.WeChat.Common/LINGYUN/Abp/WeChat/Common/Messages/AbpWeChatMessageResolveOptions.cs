using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public class AbpWeChatMessageResolveOptions
{
    public List<IMessageResolveContributor> MessageResolvers { get; }

    public AbpWeChatMessageResolveOptions()
    {
        MessageResolvers = new List<IMessageResolveContributor>();
    }
}
