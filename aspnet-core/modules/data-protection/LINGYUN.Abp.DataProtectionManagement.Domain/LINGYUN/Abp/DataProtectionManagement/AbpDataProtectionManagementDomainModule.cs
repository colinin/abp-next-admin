using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtectionManagement;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDataProtectionModule),
    typeof(AbpDataProtectionManagementDomainSharedModule)
 )]
public class AbpDataProtectionManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpDataProtectionManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<DataProtectionManagementDomainMappingProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<EntityTypeInfo, EntityTypeInfoEto>();
            options.EtoMappings.Add<RoleEntityRule, RoleEntityRuleEto>();
            options.EtoMappings.Add<OrganizationUnitEntityRule, OrganizationUnitEntityRuleEto>();

            options.AutoEventSelectors.Add<EntityTypeInfo>();
            options.AutoEventSelectors.Add<RoleEntityRule>();
            options.AutoEventSelectors.Add<OrganizationUnitEntityRule>();
        });

        context.Services.AddHostedService<ProtectedEntitiesSaverService>();
    }
}
