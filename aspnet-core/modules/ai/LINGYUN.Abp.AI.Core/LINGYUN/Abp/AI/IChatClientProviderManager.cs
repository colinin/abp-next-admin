using System.Collections.Generic;

namespace LINGYUN.Abp.AI;
public interface IChatClientProviderManager
{
    List<IChatClientProvider> Providers { get; }
}
