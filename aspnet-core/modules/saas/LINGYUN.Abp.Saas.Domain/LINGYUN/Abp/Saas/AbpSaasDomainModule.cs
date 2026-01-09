using LINGYUN.Abp.MultiTenancy.Editions;
using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.ObjectExtending;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Saas;

[DependsOn(typeof(AbpSaasDomainSharedModule))]
[DependsOn(typeof(AbpMapperlyModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpMultiTenancyEditionsModule))]
public class AbpSaasDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpSaasDomainModule>();

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Edition, EditionEto>(typeof(AbpSaasDomainModule));
            options.EtoMappings.Add<Tenant, TenantEto>(typeof(AbpSaasDomainModule));

            options.AutoEventSelectors.Add<Edition>();
            options.AutoEventSelectors.Add<Tenant>();
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                SaasModuleExtensionConsts.ModuleName,
                SaasModuleExtensionConsts.EntityNames.Edition,
                typeof(Edition)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                SaasModuleExtensionConsts.ModuleName,
                SaasModuleExtensionConsts.EntityNames.Tenant,
                typeof(Tenant)
            );
        });
    }
}
