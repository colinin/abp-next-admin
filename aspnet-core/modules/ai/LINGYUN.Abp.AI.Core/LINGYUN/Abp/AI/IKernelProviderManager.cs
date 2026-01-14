using System.Collections.Generic;

namespace LINGYUN.Abp.AI;
public interface IKernelProviderManager
{
    List<IKernelProvider> Providers { get; }
}
