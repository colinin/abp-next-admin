using Elsa.Common.Features;
using Elsa.Common.Multitenancy;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Tenants.Extensions;
using Elsa.Workflows;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Multitenancy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.ElsaNext;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpFeaturesModule),
    typeof(AbpThreadingModule),
    typeof(AbpJsonModule))]
public class AbpElsaNextModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<ITenantResolver, AbpTenantResolver>();

        var elsaModule = context.Services.GetPreConfigureActions<IModule>();

        context.Services.AddElsa(elsa =>
        {
            elsa
             .AddActivitiesFrom<AbpElsaNextModule>()
             .AddWorkflowsFrom<AbpElsaNextModule>()
             .UseTenants(tenants =>
             {
                 tenants.ConfigureMultitenancy(options =>
                 {
                     options.TenantResolverPipelineBuilder.Append<AbpTenantResolver>();
                 });
             });

            elsa.Configure<MultitenancyFeature>(feature =>
            {
                feature.UseTenantsProvider<AbpTenantsProvider>();
            });

            elsaModule.Configure(elsa);
        });

        context.Services.Replace(
            ServiceDescriptor.Singleton<IIdentityGenerator, AbpElsaIdentityGenerator>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<ElsaNextResource>("en");
        });
    }
}
