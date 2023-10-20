using LINGYUN.Abp.Authorization.OrganizationUnits;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.OrganizaztionUnits;

[DependsOn(typeof(AbpIdentityDomainModule))]
[DependsOn(typeof(AbpAuthorizationOrganizationUnitsModule))]
public class AbpIdentityOrganizaztionUnitsModule : AbpModule
{

}
