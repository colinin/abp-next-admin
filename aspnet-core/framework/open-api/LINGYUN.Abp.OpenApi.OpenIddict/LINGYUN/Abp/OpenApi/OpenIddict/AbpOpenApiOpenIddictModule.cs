using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;

namespace LINGYUN.Abp.OpenApi.OpenIddict;

[DependsOn(
    typeof(AbpOpenApiModule),
    typeof(AbpOpenIddictDomainModule))]
public class AbpOpenApiOpenIddictModule : AbpModule
{
}
