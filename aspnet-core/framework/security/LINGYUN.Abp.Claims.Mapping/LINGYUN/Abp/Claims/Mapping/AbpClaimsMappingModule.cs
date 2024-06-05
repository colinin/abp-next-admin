using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace LINGYUN.Abp.Claims.Mapping;

[DependsOn(typeof(AbpSecurityModule))]
public class AbpClaimsMappingModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        JwtClaimTypesMapping.MapAbpClaimTypes();
    }
}
