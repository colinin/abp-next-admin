using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainSharedModule),
    typeof(AbpTextTemplatingCoreModule))]
public class AbpTextTemplatingDomainModule : AbpModule
{

}
