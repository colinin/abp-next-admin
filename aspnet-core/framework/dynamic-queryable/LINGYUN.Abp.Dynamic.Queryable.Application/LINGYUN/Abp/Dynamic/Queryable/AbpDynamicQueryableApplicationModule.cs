using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dynamic.Queryable;

[DependsOn(
    typeof(AbpDynamicQueryableApplicationContractsModule),
    typeof(AbpDddApplicationModule))]
public class AbpDynamicQueryableApplicationModule : AbpModule
{
}
