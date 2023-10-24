using LINGYUN.Abp.Dapr;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

[DependsOn(
    typeof(AbpDaprModule),
    typeof(AbpThreadingModule),
    typeof(AbpDistributedLockingAbstractionsModule))]
public class AbpDistributedLockingDaprModule : AbpModule
{

}
