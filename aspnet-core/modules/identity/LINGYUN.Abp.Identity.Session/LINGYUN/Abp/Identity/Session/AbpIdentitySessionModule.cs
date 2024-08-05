using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.Session;

[DependsOn(typeof(AbpCachingModule))]
public class AbpIdentitySessionModule : AbpModule
{
}
