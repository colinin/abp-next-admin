using System.Collections.Generic;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolProviderManager
{
    List<IAIToolProvider> Providers { get; }
}
