using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainSharedModule),
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpCachingModule))]
public class AbpTextTemplatingDomainModule : AbpModule
{

}
