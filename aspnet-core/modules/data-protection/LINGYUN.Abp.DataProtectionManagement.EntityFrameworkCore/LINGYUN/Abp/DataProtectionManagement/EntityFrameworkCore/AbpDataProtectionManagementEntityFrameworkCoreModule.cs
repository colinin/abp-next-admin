using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataProtectionManagementDomainModule),
    typeof(AbpDataProtectionEntityFrameworkCoreModule)
)]
public class AbpDataProtectionManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AbpDataProtectionManagementDbContext>(options =>
        {
            options.AddRepository<EntityTypeInfo, EfCoreEntityTypeInfoRepository>();

            options.AddRepository<RoleEntityRule, EfCoreRoleEntityRuleRepository>();
            options.AddRepository<OrganizationUnitEntityRule, EfCoreOrganizationUnitEntityRuleRepository>();

            options.AddDefaultRepositories();
        });
    }
}
