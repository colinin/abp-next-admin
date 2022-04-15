using LINGYUN.Abp.Authorization.OrganizationUnits;
using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement.OrganizationUnits;

[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpPermissionManagementDomainModule),
    typeof(AbpAuthorizationOrganizationUnitsModule)
    )]
public class AbpPermissionManagementDomainOrganizationUnitsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<OrganizationUnitPermissionManagementProvider>();

            options.ProviderPolicies[OrganizationUnitPermissionValueProvider.ProviderName] = "AbpIdentity.OrganizationUnits.ManagePermissions";
        });
    }
}
