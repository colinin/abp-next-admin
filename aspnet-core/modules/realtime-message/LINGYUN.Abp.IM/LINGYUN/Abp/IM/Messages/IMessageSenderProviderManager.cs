using System.Collections.Generic;

namespace LINGYUN.Abp.IM.Messages
{
    public interface IMessageSenderProviderManager
    {
        List<IMessageSenderProvider> Providers { get; }
    }
}
