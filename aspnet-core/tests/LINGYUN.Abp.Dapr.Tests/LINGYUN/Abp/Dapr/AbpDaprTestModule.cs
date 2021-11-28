using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule))]
    public class AbpDaprTestModule : AbpModule
    {

    }
}
