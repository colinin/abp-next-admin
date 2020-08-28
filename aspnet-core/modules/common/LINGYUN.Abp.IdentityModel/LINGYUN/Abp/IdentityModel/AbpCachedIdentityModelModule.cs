using Volo.Abp.Caching;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityModel
{
    [DependsOn(
        typeof(AbpIdentityModelModule),
        typeof(AbpCachingModule))]
    public class AbpCachedIdentityModelModule : AbpModule
    {
    }
}
