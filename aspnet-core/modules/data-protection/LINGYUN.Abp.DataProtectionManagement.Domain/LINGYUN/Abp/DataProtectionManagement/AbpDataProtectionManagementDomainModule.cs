using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtectionManagement;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDataProtectionModule),
    typeof(AbpDataProtectionManagementDomainSharedModule)
 )]
public class AbpDataProtectionManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpDataProtectionManagementDomainModule>();

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<EntityTypeInfo, EntityTypeInfoEto>(typeof(AbpDataProtectionManagementDomainModule));
            options.EtoMappings.Add<RoleEntityRule, RoleEntityRuleEto>(typeof(AbpDataProtectionManagementDomainModule));
            options.EtoMappings.Add<OrganizationUnitEntityRule, OrganizationUnitEntityRuleEto>(typeof(AbpDataProtectionManagementDomainModule));

            options.AutoEventSelectors.Add<EntityTypeInfo>();
            options.AutoEventSelectors.Add<RoleEntityRule>();
            options.AutoEventSelectors.Add<OrganizationUnitEntityRule>();
        });

        context.Services.AddHostedService<ProtectedEntitiesSaverService>();
    }
}
