using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace LINGYUN.Abp.IdentityModel
{
    [DependsOn(
        typeof(AbpIdentityModelModule),
        typeof(AbpSecurityModule))]
    public class AbpCachedIdentityModelModule : AbpModule
    {
    }
}
