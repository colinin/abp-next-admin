using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;

namespace LINGYUN.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpDddApplicationModule))]
public class AbpOpenIddictApplicationModule : AbpModule
{
}
